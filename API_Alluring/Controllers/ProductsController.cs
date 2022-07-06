using API_Alluring.Models;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult GetAllProducts([FromQuery]QueryParameter query)
        {
            try
            {
                //QueryParameter query = new QueryParameter(search, sortBy, from, to, page);
                return Ok(_productRepository.GetAll(query));
            }
            catch (Exception e)
            {
                return BadRequest("We cant get all product");
            }
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
        public IActionResult Add([FromBody]ProductModel product)
        {
            try
            {
                return Ok(_productRepository.Add(product));

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
                var data = _productRepository.GetById(id);
                if (data != null) return Ok(data);
                else return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        //[Authorize]
        [EnableCors("MyPolicy")]
        public IActionResult Update([FromBody]ProductVM product)
        {
            var data = _productRepository.GetById(product.ProductId);
            if (data == null) return BadRequest("Product isn't exist");
            try
            {
                _productRepository.Update(product);
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
                _productRepository.Delete(id);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
