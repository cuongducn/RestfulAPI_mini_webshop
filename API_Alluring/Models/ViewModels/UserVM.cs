namespace API_Alluring.Models.ViewModels
{
    public class UserVM
    {
        public string UserName { get; set; }

        public string? Email { get; set; }

        public string Password { get; set; }

        public DateTime? CreateAt { get; set; }

        public bool? IsEmailVerified { get; set; }
    }
}
