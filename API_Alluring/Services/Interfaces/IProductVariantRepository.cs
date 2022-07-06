using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;

namespace API_Alluring.Services.Interfaces
{
    public interface IProductVariantRepository 
    {
        List<ProductVariantVM> GetAll(QueryParameter query);

        List<ProductVariantVM> GetById(Guid id);

        ProductVariantVM Add(ProductVariantVM productVariant);

        void Update(ProductVariantVM productVariant);

        void Delete(ProductVariantVM productVariant);
    }
}
