namespace API_Alluring.Models
{
    public class ProductModel
    {
        public Guid CategoryId { get; set; }

        public Guid DiscountId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool? Gender { get; set; }

        public bool? IsShow { get; set; }

        public int? Quantity { get; set; }


    }
}
