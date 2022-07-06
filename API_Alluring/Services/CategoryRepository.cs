using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Services.Interfaces;
using System.Text;

namespace API_Alluring.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyDbContext _context;

        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public CategoryVM Add(CategoryModel category)
        {
            var _category = new Category
            {
                CategoryName = category.CategoryName
            };
            _context.Add(_category);
            _context.SaveChanges();
            
            return new CategoryVM
            {
                CategoryId = _category.CategoryId,
                CategoryName = _category.CategoryName,
            };
        }

        public void Delete(Guid id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.CategoryId.Equals(id));
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public List<CategoryVM> GetAll()
        {
            var categories = _context.Categories.Select(c => new CategoryVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
            });

            return categories.ToList();
        }

        public List<CategoryVM> GetByGender(bool gender)
        {
            var categories = from c in _context.Categories
                             join p in _context.Products on c.CategoryId equals p.CategoryId
                             where p.Gender == gender
                             select c;
            var result = categories.Select(c => new CategoryVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
            });

            var result1 = result.GroupBy(c => new { c.CategoryId, c.CategoryName })
                  .Select(g => g.First())
                  .ToList();


            return result1;
        }

        public CategoryVM GetById(Guid id)
        {
            var category = _context.Categories.SingleOrDefault(c => c.CategoryId.Equals(id));
            if (category != null)
            {
                return new CategoryVM
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                };
            }

            return null;
        }

        public void Update(CategoryVM category)
        {
            var _category = _context.Categories.SingleOrDefault(c => c.CategoryId.Equals(category.CategoryId ));
            if (_category != null)
            {
                _category.CategoryName = category.CategoryName;
                _context.SaveChanges();
            }
        }
    }
}
