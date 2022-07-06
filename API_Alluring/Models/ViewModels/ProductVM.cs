namespace API_Alluring.Models.ViewModels
{
    public class ProductVM
    {
        public Guid ProductId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? DiscountId { get; set; }

        public string CategoryName { get; set; }

        public int PercentDiscount { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool? Gender { get; set; }

        public bool? isShow { get; set; }

        public int? Quantity { get; set; }
    }
}
