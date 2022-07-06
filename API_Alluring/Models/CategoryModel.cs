using System.ComponentModel.DataAnnotations;

namespace API_Alluring.Models
{
    public class CategoryModel
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
    }
}
