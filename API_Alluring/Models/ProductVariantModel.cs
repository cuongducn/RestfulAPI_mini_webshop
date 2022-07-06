namespace API_Alluring.Models
{
    public class ProductVariantModel
    {
        public string? image { get; set; }

        public Guid? ProductId { get; set; }

        public string? color { get; set; }

        public string? colorName { get; set; }

        public int? quantity { get; set; }
    }
}
