using API_Alluring.Models;
using API_Alluring.Models.ViewModels;

namespace API_Alluring.Services.Interfaces
{
    public interface ICategoryRepository
    {
        List<CategoryVM> GetAll();

        List<CategoryVM> GetByGender(bool gender);

        CategoryVM GetById(Guid id);

        CategoryVM Add(CategoryModel category);

        void Update(CategoryVM category);

        void Delete(Guid id);
    }
}
