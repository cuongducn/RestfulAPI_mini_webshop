using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid? DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public Discount Discount { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal Price { get; set; }

        public bool isShow { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool? Gender { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        public ICollection<ProductVariant> ProductVariant { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Product()
        {
            ProductVariant = new List<ProductVariant>();
            OrderDetails = new List<OrderDetail>();
        }
    }
}
