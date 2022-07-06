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
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantRepository _productVariantRepository;

        public ProductVariantController(IProductVariantRepository productVariantRepository)
        {
            _productVariantRepository = productVariantRepository;
        }

        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult GetAll([FromQuery] QueryParameter query)
        {
            try
            {
                return Ok(_productVariantRepository.GetAll(query));
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
                var data = _productVariantRepository.GetById(id);
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
        public IActionResult Update(Guid id, ProductVariantVM productVariant)
        {
            if (id != productVariant.ProductVariantId) return BadRequest();
            try
            {
                var data = _productVariantRepository.GetById(id);
                return NoContent();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("MyPolicy")]
        public IActionResult Delete(ProductVariantVM productVariant)
        {
            try
            {
                _productVariantRepository.Delete(productVariant);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
        [Authorize]
        public IActionResult Add(ProductVariantVM productVariant)
        {
            try
            {
                return Ok(_productVariantRepository.Add(productVariant));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
