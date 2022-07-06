using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    public enum Status
    {
        Confirmation = 0, Delivering = 1, Complete = 2, Cancel = -1
    }

    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public DateTime OrderDate  { get; set; }

        [Range(0, (double)Decimal.MaxValue)]
        public decimal? TotalAmount { get; set; }

        public Status Status { get; set; }

        [MaxLength(50)]
        public string? MethodPay { get; set; }

        [MaxLength(150)]
        public string? Address { get; set; }

        [MaxLength(11)]
        public string? Phone { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}
