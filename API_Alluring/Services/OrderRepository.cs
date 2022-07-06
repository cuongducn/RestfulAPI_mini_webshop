using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext _context;

        public static int PAGE_SIZE { get; set; } = 10;

        public OrderRepository(MyDbContext context )
        {
            _context = context;
        }


        List<OrderVM> IOrderRepository.GetAll(QueryParameter query)
        {
            var allOrders = _context.Orders.Include(o => o.Customer).AsQueryable();

            //Filtering
            if (!string.IsNullOrEmpty(query.Search)) allOrders = allOrders.Where(o => o.Customer.CustomerName.Contains(query.Search));
            else if (query.From.HasValue) allOrders = allOrders.Where(o => o.TotalAmount >= query.From);
            else if (query.To.HasValue) allOrders = allOrders.Where(o => o.TotalAmount <= query.To);

            if (!string.IsNullOrEmpty(query.SortBy)) allOrders = allOrders.OrderBy(o => o.Customer.CustomerName);
            //Sorting
            switch (query.SortBy)
            {
                case "NAME_DESC": allOrders = allOrders.OrderByDescending(o => o.Customer.CustomerName); break;
                case "PRICE_ASC": allOrders.OrderBy(p => p.TotalAmount); break;
                case "PRICE_DESC": allOrders.OrderByDescending(p => p.TotalAmount); break;
                default: allOrders = allOrders.OrderBy(o => o.Customer.CustomerName); break;
            }

            //Paging

            var result = PaginatedList<Order>.Create(allOrders, (int)query.Page, PAGE_SIZE);

            return result.Select(o => new OrderVM
            {
                OrderId = o.OrderId,
                CustomerName = o.Customer.CustomerName,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = (Models.ViewModels.Status)o.Status,
                MethodPay = o.MethodPay,
                Address = o.Address,
                Phone = o.Phone

            }).ToList();
        }

        public OrderVM GetById(Guid id)
        {
            var order = _context.Orders.SingleOrDefault(c => c.OrderId.Equals(id));
            if (order != null)
            {
                return new OrderVM
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.CustomerName,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = (Models.ViewModels.Status)order.Status,
                    MethodPay = order.MethodPay,
                    Address = order.Address,
                    Phone = order.Phone
                };
            }

            return null;
        }

        public OrderVM Add(OrderModel order)
        {
            var _order = new Order
            {
                CustomerId = order.CustomerId,
                OrderDate = (DateTime)order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = (Data.Status)order.Status,
                MethodPay = order.MethodPay,
                Address = order.Address,
                Phone = order.Phone
            };
            _context.Add(_order);
            _context.SaveChanges();

            var nameCus = _context.Customers.SingleOrDefault(c => c.CustomerId == _order.CustomerId);

            return new OrderVM
            {
                OrderId = _order.OrderId,
                CustomerName= nameCus.CustomerName.ToString(),
                OrderDate = _order.OrderDate,
                TotalAmount = _order.TotalAmount,
                Status = (Models.ViewModels.Status)_order.Status,
                MethodPay = _order.MethodPay,
                Address = _order.Address,
                Phone = _order.Phone
            };
        }

        public void Update(OrderVM order)
        {
            var _order = _context.Orders.SingleOrDefault(c => c.OrderId.Equals(order.OrderId));
            if (order != null)
            {
                _order.CustomerId = order.CustomerId;
                _order.OrderDate = order.OrderDate;
                _order.TotalAmount = order.TotalAmount;
                _order.Address = order.Address;
                _order.Phone = order.Phone;
                _order.MethodPay = order.MethodPay;
                _order.Status = (Data.Status)order.Status;
                _context.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            var order = _context.Orders.SingleOrDefault(c => c.OrderId.Equals(id));
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}
