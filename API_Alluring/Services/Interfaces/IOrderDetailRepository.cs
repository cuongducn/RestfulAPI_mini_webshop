using API_Alluring.Models;
using API_Alluring.Models.ViewModels;
using API_Alluring.Models.WrapParameters;

namespace API_Alluring.Services.Interfaces
{
    public interface IOrderDetailRepository
    {
        List<OrderDetailVM> GetAll(QueryParameter query);

        List<OrderDetailVM> GetById(Guid id);

        OrderDetailVM Add(OrderDetailVM orderDetail);

        void Update(OrderDetailVM orderDetail);

        void Delete(OrderDetailVM orderDetail);
    }
}
