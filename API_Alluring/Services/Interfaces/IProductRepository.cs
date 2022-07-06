using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;

namespace API_Alluring.Services.Interfaces
{
    public interface IProductRepository
    {
        List<ProductVM> GetAll(QueryParameter query);

        ProductVM Add(ProductModel product);

        ProductVM GetById(Guid id);

        void Update(ProductVM product);

        void Delete(Guid id);
    }
}
