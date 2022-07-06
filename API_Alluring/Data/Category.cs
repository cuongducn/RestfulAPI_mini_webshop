using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        public ICollection<Product> Product { get; set; }
        public Category()
        {
            Product = new HashSet<Product>();
        }
    }
}
