using API_Alluring.Data;
using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;
using API_Alluring.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MyDbContext _context;

        public static int PAGE_SIZE { get; set; } = 10;

        public CustomerRepository(MyDbContext context)
        {
            _context = context;
        }

        public CustomerVM Add(CustomerModel customer)
        {
            var _customer = new Customer
            {
                UserId = Guid.Parse("84f23853-a0cd-40df-ce9f-08da55838467"),
                CustomerName = customer.CustomerName,
                Address = customer.Address,
                CreatedAt = customer.CreatedAt,
                Phone = customer.Phone,
                Birth = customer.Birth
            };
            _context.Add(_customer);
            _context.SaveChanges();

            return new CustomerVM
            {
                CustomerId = _customer.CustomerId,
                UserId = _customer.UserId,
                CustomerName = _customer.CustomerName,
                Address = _customer.Address,
                CreatedAt = _customer.CreatedAt,
                Phone = _customer.Phone,
                Birth = _customer.Birth
            };
        }

        public void Delete(Guid id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(id));
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public List<CustomerVM> GetAll(QueryParameter query)
        {
            var allCustomer = _context.Customers.Include(o => o.User).AsQueryable();

            //Filtering
            if (!string.IsNullOrEmpty(query.Search)) allCustomer = allCustomer.Where(o => o.CustomerName.Contains(query.Search));

            if (!string.IsNullOrEmpty(query.SortBy)) allCustomer = allCustomer.OrderBy(o => o.CustomerName);
            //Sorting
            switch (query.SortBy)
            {
                case "NAME_DESC": allCustomer = allCustomer.OrderByDescending(o => o.CustomerName); break;
                default: allCustomer = allCustomer.OrderBy(o => o.CustomerName); break;
            }

            //Paging

            var result = PaginatedList<Customer>.Create(allCustomer, (int)query.Page, PAGE_SIZE);

            return result.Select(o => new CustomerVM
            {
                CustomerId = o.CustomerId,
                UserId = o.UserId,
                CustomerName = o.CustomerName,
                Address = o.Address,
                CreatedAt = o.CreatedAt,
                Phone = o.Phone,
                Birth = o.Birth

            }).ToList();
        }

        public CustomerVM GetById(Guid id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(id));
            if (customer != null)
            {
                return new CustomerVM
                {
                    CustomerId = customer.CustomerId,
                    UserId = customer.UserId,
                    CustomerName = customer.CustomerName,
                    Address = customer.Address,
                    CreatedAt = customer.CreatedAt,
                    Phone = customer.Phone,
                    Birth = customer.Birth
                };
            }

            return null;
        }

        public void Update(CustomerVM customer)
        {
            var _customer = _context.Customers.SingleOrDefault(c => c.CustomerId.Equals(customer.CustomerId));
            if (customer != null)
            {
                _customer.CustomerId = customer.CustomerId;
                _customer.UserId = customer.UserId;
                _customer.CustomerName = customer.CustomerName;
                _customer.Address = customer.Address;
                _customer.CreatedAt = customer.CreatedAt;
                _customer.Phone = customer.Phone;
                _customer.Birth = customer.Birth;
                _context.SaveChanges();
            }
        }
    }
}
