namespace API_Alluring.Models.ViewModels
{
    public class ProductVariantVM
    {
        public Guid ProductVariantId { get; set; }

        public string? image { get; set; }

        public Guid? ProductId { get; set; }

        public string? color { get; set; }

        public string? colorName { get; set; }

        public int? quantity { get; set; }
    }
}
