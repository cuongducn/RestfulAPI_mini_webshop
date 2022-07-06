using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("Discount")]
    public class Discount
    {
        [Key]
        [Required]
        public Guid DiscountId { get; set; }

        [MaxLength(50)]
        public string? DiscountName { get; set; }

        public string? DiscountDescription { get; set; }

        [Range(0, 100)]
        public int? PercentDiscount { get; set; }

        public bool? isActive { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateStop { get; set; }

        public ICollection<Product> Product { get; set; }
        public Discount()
        {
            Product = new HashSet<Product>();
        }
    }
}
