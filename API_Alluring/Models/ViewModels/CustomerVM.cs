namespace API_Alluring.Models.ViewModels
{
    public class CustomerVM
    {
        public Guid CustomerId { get; set; }

        public Guid UserId { get; set; }

        public string CustomerName { get; set; }

        public string Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Phone { get; set; }

        public DateTime? Birth { get; set; }
    }
}
