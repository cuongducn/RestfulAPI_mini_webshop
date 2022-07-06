using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Services
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly MyDbContext _context;

        public static int PAGE_SIZE { get; set; } = 10;

        public ProductVariantRepository(MyDbContext context )
        {
            _context = context;
        }

        List<ProductVariantVM> IProductVariantRepository.GetAll(QueryParameter query)
        {
            var ProductsVariant = _context.ProductVariants.Include(o => o.Product).AsQueryable();

            //Filtering
            if (!string.IsNullOrEmpty(query.Search)) ProductsVariant = ProductsVariant.Where(o => o.Product.ProductName.Contains(query.Search));

            if (!string.IsNullOrEmpty(query.SortBy)) ProductsVariant = ProductsVariant.OrderBy(o => o.Product.ProductName);
            //Sorting
            switch (query.SortBy)
            {
                case "NAME_DESC": ProductsVariant = ProductsVariant.OrderByDescending(o => o.Product.ProductName); break;
                default: ProductsVariant = ProductsVariant.OrderBy(o => o.Product.ProductName); break;
            }

            //Paging

            var result = PaginatedList<ProductVariant>.Create(ProductsVariant, (int)query.Page, PAGE_SIZE);

            return result.Select(o => new ProductVariantVM
            {
                ProductVariantId = o.ProductVariantId,
                ProductId = o.ProductId,
                image = o.image,
                color = o.color,
                colorName = o.colorName,
                quantity = o.quantity

            }).ToList();
        }

        List<ProductVariantVM> IProductVariantRepository.GetById(Guid id)
        {
            var productVariant = _context.ProductVariants.Where(c => c.ProductVariantId.Equals(id));
            if (productVariant != null)
            {
                return productVariant.Select(o => new ProductVariantVM
                {
                    ProductVariantId = o.ProductVariantId,
                    ProductId = o.ProductId,
                    image = o.image,
                    color = o.color,
                    colorName = o.colorName,
                    quantity = o.quantity
                }).ToList();
            }

            return null;
        }

        public ProductVariantVM Add(ProductVariantVM productVariant)
        {
            var _productVariant = new ProductVariant
            {
                ProductId = (Guid)productVariant.ProductId,
                image = productVariant.image,
                color = productVariant.color,
                colorName = productVariant.colorName,
                quantity = (int)productVariant.quantity,
            };
            _context.Add(_productVariant);
            _context.SaveChanges();

            return new ProductVariantVM
            {
                ProductId = (Guid)_productVariant.ProductId,
                image = _productVariant.image,
                color = _productVariant.color,
                colorName = _productVariant.colorName,
                quantity = _productVariant.quantity,
            };
        }

        public void Update(ProductVariantVM productVariant)
        {
            var _productVariant = _context.ProductVariants.SingleOrDefault(c => c.ProductVariantId.Equals(productVariant.ProductVariantId));
            if (productVariant != null)
            {
                _productVariant.ProductId = (Guid)productVariant.ProductId;
                _productVariant.image = productVariant.image;
                _productVariant.color = productVariant.color;
                _productVariant.colorName = productVariant.colorName;
                _productVariant.quantity = (int)productVariant.quantity;
                _context.SaveChanges();
            }
        }

        public void Delete(ProductVariantVM productVariant)
        {
            var _productVariant = _context.ProductVariants.SingleOrDefault(c => c.ProductVariantId.Equals(productVariant.ProductVariantId)
                && c.ProductId.Equals(productVariant.ProductId));
            if (_productVariant != null)
            {
                _context.ProductVariants.Remove(_productVariant);
                _context.SaveChanges();
            }
        }
    }
}
