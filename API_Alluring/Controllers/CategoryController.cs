using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API_Alluring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [EnableCors("MyPolicy")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_categoryRepository.GetAll());
            }
            catch (Exception e )
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("getByGender")]
        [EnableCors("MyPolicy")]
        public IActionResult GetByGender([FromQuery]bool gender)
        {
            try
            {
                //bool gen = bool.Parse(gender);
                return Ok(_categoryRepository.GetByGender(gender));
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
                var data = _categoryRepository.GetById(id);
                if (data != null) return Ok(data);
                else return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [EnableCors("MyPolicy")]
        //[Authorize]
        public IActionResult Update(CategoryVM category)
        {
            try
            {
                _categoryRepository.Update(category);
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
                _categoryRepository.Delete(id);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [EnableCors("MyPolicy")]
        //[Authorize]
        public IActionResult Add(CategoryModel category)
        {
            try
            {
                return Ok(_categoryRepository.Add(category));

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
