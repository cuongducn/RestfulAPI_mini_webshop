using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Alluring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository orderRepository)
        {
            _customerRepository = orderRepository;
        }

        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult GetAll([FromQuery] QueryParameter query)
        {
            try
            {
                return Ok(_customerRepository.GetAll(query));
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
                var data = _customerRepository.GetById(id);
                if (data != null) return Ok(data);
                else return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        //[Authorize]
        [EnableCors("MyPolicy")]
        public IActionResult Update(Guid id, CustomerVM Customer)
        {
            if (id != Customer.CustomerId) return BadRequest();
            try
            {
                var data = _customerRepository.GetById(id);
                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _customerRepository.Delete(id);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
        //[Authorize("Admin")]
        public IActionResult Add(CustomerModel Customer)
        {
            try
            {
                return Ok(_customerRepository.Add(Customer));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
