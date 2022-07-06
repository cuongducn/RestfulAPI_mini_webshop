using API_Alluring.Data;
using API_Alluring.Helper;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Alluring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSetting _appSetting;
        private readonly AuthenService _authenService;

        public UserController(MyDbContext context, AuthenService authenService, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _authenService = authenService;
            _context = context;
            _appSetting = optionsMonitor.CurrentValue;
        }

        [HttpPost("SignUp")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> SignUp(UserVM user)
        {
            MD5_hash md5hash = new MD5_hash();

            if (_context.Users.SingleOrDefault(u => u.UserName == user.UserName) != null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Username had existed",
                    Data = null,
                });
            }

            var _user = new User
            {
                UserName = user.UserName,
                Password = md5hash.MD5Hash(user.Password),
                Email = user.Email,
                CreateAt = user.CreateAt,
                Role = "user",
                IsEmailVerified = (bool)user.IsEmailVerified,
            };

            _context.Users.Add(_user);

            var token = await _authenService.GenerateToken(_user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "SignUp success",
                Data = token,
            });
        }

        [HttpPost("SignIn")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> Validate(LoginModel user)
        {
            MD5_hash md5hash = new MD5_hash();
            var _user = _context.Users.SingleOrDefault(u => 
                u.UserName == user.UserName && u.Password == md5hash.MD5Hash(user.Password));

            if (_user == null) return Ok(new ApiResponse 
            {
                Success = false,
                Message = "Invalid Username/Password",
            });

            var token = await _authenService.GenerateToken(_user);
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authentication success",
                Data = token,
            });
        }

        [HttpPost("RenewToken")]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var hashKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // auto generate token
                ValidateIssuer = false,
                ValidateAudience = false,

                // sign the token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(hashKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false,
            };

            try
            {
                // check access valid token
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParam, out var validatedToken); ;

                // check algorithsm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok(new ApiResponse { Success = false, Message = "Invalid token" });
                    }
                }

                // check access token expire ?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse { Success = false, Message = "AccessToken hasn't expired yet" });
                }

                // check refreshToken exist in DB
                var storedToken = _context.RefreshToken.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse { Success = false, Message = "RefreshToken doesn't exist" });
                }

                // check refresh token is used/revoked
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse { Success = false, Message = "RefreshToken has been used" });
                }

                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse { Success = false, Message = "RefreshToken has been revoked" });
                }

                // AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse { Success = false, Message = "Toekn doens't match" });
                }

                // Update Token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                // create new token 
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == storedToken.UserId);
                var token = await _authenService.GenerateToken(user);

                return Ok(new ApiResponse { Success = true, Message = "Renew token success", Data = token, });
            }
            catch (Exception e)
            {

                return BadRequest(new ApiResponse { Success = false, Message = e.Message });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
