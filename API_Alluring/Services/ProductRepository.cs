using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _context;

        public static int PAGE_SIZE { get; set; } = 10;

        public ProductRepository(MyDbContext context )
        {
            _context = context;
        }


        public List<ProductVM> GetAll(QueryParameter query)
        {
            var allProducts = _context.Products.Include(p => p.Discount).Include(p => p.Category).AsQueryable();
            //if (string.IsNullOrEmpty(query.Page.ToString()) || string.IsNullOrEmpty(query.Search) && string.IsNullOrEmpty(query.SortBy)
            //    && string.IsNullOrEmpty(query.From.ToString()) && string.IsNullOrEmpty(query.To.ToString()) && query.Gender != null)
            if (query.Page == null && query.Search == null && query.SortBy == null && query.From == null && query.To == null && query.Gender == null)
                {
                var result = allProducts.Where(p => p.isShow == true).OrderBy(p => p.ProductName);
                return result.Select(p => new ProductVM
                {
                    ProductId = p.ProductId,
                    CategoryId = (Guid)p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Gender = p.Gender,
                    PercentDiscount = (int)p.Discount.PercentDiscount,
                    isShow = p.isShow,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Image = p.Image,
                }).ToList();
            } else
            {
                //Filtering
                if (!string.IsNullOrEmpty(query.Search)) allProducts = allProducts.Where(p => p.ProductName.Contains(query.Search) && p.isShow == true);
                else if (query.From.HasValue) allProducts = allProducts.Where(p => p.Price >= query.From && p.isShow == true);
                else if (query.To.HasValue) allProducts = allProducts.Where(p => p.Price <= query.To && p.isShow == true);
                else if (query.Gender != null) allProducts = allProducts.Where(p => p.Gender == query.Gender && p.isShow == true);
                if (!string.IsNullOrEmpty(query.SortBy)) allProducts = allProducts.Where(p => p.isShow == true).OrderBy(p => p.ProductName);
                //Sorting
                switch (query.SortBy)
                {
                    case "NAME_DESC": allProducts = allProducts.Where(p => p.isShow == true).OrderByDescending(p => p.ProductName); break;
                    case "PRICE_ASC": allProducts.Where(p => p.isShow == true).OrderBy(p => p.Price); break;
                    case "PRICE_DESC": allProducts.Where(p => p.isShow == true).OrderByDescending(p => p.Price); break;
                    //default: allProducts = allProducts.OrderBy(p => p.ProductName); break;
                }
                var result = new List<Product>();
                if (query.Gender != null) result = allProducts.ToList();
                else if (query.Search != null) result = allProducts.ToList();
                else result = PaginatedList<Product>.Create(allProducts, (int)query.Page == null ? 1 : (int)query.Page, PAGE_SIZE);
                //Paging
                return result.Select(p => new ProductVM {
                    ProductId = p.ProductId,
                    CategoryId = (Guid)p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    PercentDiscount = (int)p.Discount.PercentDiscount,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Gender = p.Gender,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Image = p.Image,
                }).ToList();
            }
        }

        public ProductVM Add(ProductModel product)
        {
            var _product = new Product
            {
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                DiscountId = product.DiscountId,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image,
                Gender = product.Gender,
                Quantity = (int)product.Quantity,
            };
            _context.Add(_product);
            _context.SaveChanges();

            return new ProductVM
            {
                ProductId = _product.ProductId,
                DiscountId = _product.DiscountId,
                ProductName = _product.ProductName,
                Price= _product.Price,
                Description= _product.Description,
                Image= _product.Image,
                Gender= _product.Gender,
                Quantity= _product.Quantity,
            };
        }

        public ProductVM GetById(Guid id)
        {
            var product = _context.Products.Include(c => c.Discount).Include(c => c.Category).SingleOrDefault(c => c.ProductId.Equals(id));
            if (product != null)
            {
                return new ProductVM
                {
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Description = product.Description,
                    Image = product.Image,
                    PercentDiscount = (int)product.Discount.PercentDiscount,
                    Gender = product.Gender,
                    Quantity = (int)product.Quantity,
                    isShow = product.isShow,
                };
            }

            return null;
        }

        public async void Update(ProductVM product)
        {
            var _product = _context.Products.SingleOrDefault(c => c.ProductId.Equals(product.ProductId));
            if (_product != null)
            {
                //_product.CategoryId = Guid.Parse("80dd6c6b-0749-468a-f15c-08da544074dc");
                _product.CategoryId = product.CategoryId;
                _product.ProductName = product.ProductName;
                _product.Price = product.Price;
                _product.Description = product.Description;
                _product.DiscountId = product.DiscountId;
                _product.Image = product.Image;
                _product.Gender = product.Gender;
                _product.Quantity = (int)product.Quantity;
                _product.isShow = (bool)product.isShow;
                _context.SaveChangesAsync();
            }
        }

        public void Delete(Guid id)
        {
            var product = _context.Products.SingleOrDefault(c => c.ProductId.Equals(id));
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
