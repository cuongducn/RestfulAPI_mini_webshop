using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("ProductVariant")]
    public class ProductVariant
    {
        [Key]
        public Guid ProductVariantId { get; set; }

        public string? image { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }  

        public string? color { get; set; }

        [MaxLength(50)]
        public string? colorName { get; set; }

        [Range(0, int.MaxValue)]
        public int quantity { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ProductVariant()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}
