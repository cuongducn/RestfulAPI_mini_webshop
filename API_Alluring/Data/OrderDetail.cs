using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid ProductVariantId { get; set; }

        [Range(0, (double)Decimal.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range (0, int.MaxValue)]
        public int Quantity { get; set; }


    }
}
