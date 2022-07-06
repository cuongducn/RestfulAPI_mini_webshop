namespace API_Alluring.Models.ViewModels
{
    public enum Status
    {
        Confirmation = 0, Delivering = 1, Complete = 2, Cancel = -1
    }
    public class OrderVM
    {
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public Status Status { get; set; }

        public string? MethodPay { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
    }
}
