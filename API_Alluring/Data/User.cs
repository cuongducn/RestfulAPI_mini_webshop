using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Alluring.Data
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string Password { get; set; }

        public DateTime? CreateAt { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "user";

        public bool IsEmailVerified { get; set; }
    }
}
