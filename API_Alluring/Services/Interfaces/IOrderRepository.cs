using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;

namespace API_Alluring.Services.Interfaces
{
    public interface IOrderRepository
    {
        List<OrderVM> GetAll(QueryParameter query);

        OrderVM GetById(Guid id);

        OrderVM Add(OrderModel order);

        void Update(OrderVM order);

        void Delete(Guid id);
    }
}
