using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [Required]
        public Guid CustomerId { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [MaxLength(50)]
        public string CustomerName { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Phone]
        [MaxLength(11)]
        public string Phone { get; set; }

        public DateTime? Birth { get; set; }
    }
}
