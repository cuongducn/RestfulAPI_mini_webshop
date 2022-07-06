namespace API_Alluring.Models.ViewModels
{
    public class OrderDetailVM
    {
        public Guid? OrderId { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductVariantId { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? Quantity { get; set; }
    }
}
