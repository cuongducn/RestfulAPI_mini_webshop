using API_Alluring.Data;
using API_Alluring.Helper;
using API_Alluring.Models;
using API_Alluring.Services;
using API_Alluring.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var hashKeyBytes = Encoding.UTF8.GetBytes(secretKey);
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            //policy.WithOrigins("http://example.com",
            //    "https://localhost:44398",
            //    "https://localhost:5001",
            //    "http://127.0.0.1:8000/admin/products",
            //    "http://127.0.0.1:8000")
            //        .WithMethods("PUT", "DELETE", "GET", "POST");
            policy.WithOrigins("*",
                "https://localhost:7299",
                "http://127.0.0.1:8000",
                "http://localhost:8000")
                    .WithMethods("PUT", "DELETE", "GET", "POST")
                    .AllowCredentials()
                    .AllowAnyMethod().AllowAnyHeader();
        });
});


builder.Services.AddControllers();

builder.Services.AddDbContext<MyDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration["ConnectionStrings:cns"]);
});

//Add scope
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<AuthenService>();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        // auto generate token
        ValidateIssuer = false,
        ValidateAudience = false,

        // sign the token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(hashKeyBytes),

        ClockSkew = TimeSpan.Zero
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//IConfiguration configuration1 = app.Configuration;
//IWebHostEnvironment environment1 = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyMethod());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
