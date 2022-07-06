using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;

namespace API_Alluring.Services.Interfaces
{
    public interface ICustomerRepository
    {
        List<CustomerVM> GetAll(QueryParameter query);

        CustomerVM GetById(Guid id);

        CustomerVM Add(CustomerModel customer);

        void Update(CustomerVM customer);

        void Delete(Guid id);
    }
}
