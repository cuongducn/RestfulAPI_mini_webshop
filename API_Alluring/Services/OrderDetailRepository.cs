using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Services
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly MyDbContext _context;

        public static int PAGE_SIZE { get; set; } = 10;

        public OrderDetailRepository(MyDbContext context )
        {
            _context = context;
        }

        List<OrderDetailVM> IOrderDetailRepository.GetAll(QueryParameter query)
        {
            var allDetailOrders = _context.OrderDetails.Include(o => o.Product).AsQueryable();

            //Filtering
            if (!string.IsNullOrEmpty(query.Search)) allDetailOrders = allDetailOrders.Where(o => o.Product.ProductName.Contains(query.Search));
            else if (query.From.HasValue) allDetailOrders = allDetailOrders.Where(o => o.UnitPrice >= query.From);
            else if (query.To.HasValue) allDetailOrders = allDetailOrders.Where(o => o.UnitPrice <= query.To);

            if (!string.IsNullOrEmpty(query.SortBy)) allDetailOrders = allDetailOrders.OrderBy(o => o.Product.ProductName);
            //Sorting
            switch (query.SortBy)
            {
                case "NAME_DESC": allDetailOrders = allDetailOrders.OrderByDescending(o => o.Product.ProductName); break;
                case "PRICE_ASC": allDetailOrders.OrderBy(p => p.UnitPrice); break;
                case "PRICE_DESC": allDetailOrders.OrderByDescending(p => p.UnitPrice); break;
                default: allDetailOrders = allDetailOrders.OrderBy(o => o.Product.ProductName); break;
            }

            //Paging

            var result = PaginatedList<OrderDetail>.Create(allDetailOrders, (int)query.Page, PAGE_SIZE);

            return result.Select(o => new OrderDetailVM
            {
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                ProductVariantId = o.ProductVariantId,
                UnitPrice = o.UnitPrice,
                Quantity = o.Quantity,

            }).ToList();
        }

        List<OrderDetailVM> IOrderDetailRepository.GetById(Guid id)
        {
            var orderDetail = _context.OrderDetails.Where(c => c.OrderId.Equals(id));
            if (orderDetail != null)
            {
                return orderDetail.Select(o => new OrderDetailVM
                {
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    ProductVariantId = o.ProductVariantId,
                    UnitPrice = o.UnitPrice,
                    Quantity = o.Quantity,
                }).ToList();
            }

            return null;
        }

        public OrderDetailVM Add(OrderDetailVM orderDetail)
        {
            var _order = new OrderDetail
            {
                OrderId = (Guid)orderDetail.OrderId,
                ProductVariantId = (Guid)orderDetail.ProductVariantId,
                ProductId = (Guid)orderDetail.ProductId,
                UnitPrice = (decimal)orderDetail.UnitPrice,
                Quantity = (int)orderDetail.Quantity,
            };
            _context.Add(_order);
            _context.SaveChanges();

            return new OrderDetailVM
            {
                OrderId = _order.OrderId,
                ProductVariantId = _order.ProductVariantId,
                ProductId = _order.ProductId,
                UnitPrice = _order.UnitPrice,
                Quantity = _order.Quantity,
            };
        }

        public void Update(OrderDetailVM orderDetail)
        {
            var _orderDetail = _context.OrderDetails.SingleOrDefault(c => c.OrderId.Equals(orderDetail.OrderId)
                && c.ProductId.Equals(orderDetail.ProductId) && c.ProductVariantId.Equals(orderDetail.ProductVariantId));
            if (orderDetail != null)
            {
                _orderDetail.ProductVariantId = (Guid)orderDetail.ProductVariantId;
                _orderDetail.ProductId = (Guid)orderDetail.ProductId;
                _orderDetail.UnitPrice = (decimal)orderDetail.UnitPrice;
                _orderDetail.Quantity = (int)orderDetail.Quantity;
                _context.SaveChanges();
            }
        }

        public void Delete(OrderDetailVM orderDetail)
        {
            var _orderDetail = _context.OrderDetails.SingleOrDefault(c => c.OrderId.Equals(orderDetail.OrderId)
                && c.ProductId.Equals(orderDetail.ProductId));
            if (_orderDetail != null)
            {
                _context.OrderDetails.Remove(_orderDetail);
                _context.SaveChanges();
            }
        }
    }
}
