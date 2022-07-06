using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Alluring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _OrderDetailRepository;

        public OrderDetailController(IOrderDetailRepository OrderDetailRepository)
        {
            _OrderDetailRepository = OrderDetailRepository;
        }

        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult GetAll([FromQuery] QueryParameter query)
        {
            try
            {
                return Ok(_OrderDetailRepository.GetAll(query));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [EnableCors("MyPolicy")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _OrderDetailRepository.GetById(id);
                if (data != null) return Ok(data);
                else return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [EnableCors("MyPolicy")]
        [Authorize]
        public IActionResult Update(Guid id, OrderDetailVM OrderDetail)
        {
            if (id != OrderDetail.OrderId) return BadRequest();
            try
            {
                var data = _OrderDetailRepository.GetById(id);
                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public IActionResult Delete(OrderDetailVM OrderDetail)
        {
            try
            {
                _OrderDetailRepository.Delete(OrderDetail);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        //[Authorize]
        [EnableCors("MyPolicy")]
        public IActionResult Add(OrderDetailVM OrderDetail)
        {
            try
            {
                return Ok(_OrderDetailRepository.Add(OrderDetail));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
