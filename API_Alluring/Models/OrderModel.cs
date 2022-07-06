
using API_Alluring.Models.ViewModels;

namespace API_Alluring.Models
{

    public class OrderModel
    {

        public Guid? CustomerId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public Status? Status { get; set; }

        public string? MethodPay { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }
}
