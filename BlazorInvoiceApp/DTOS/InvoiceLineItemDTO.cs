using BlazorInvoiceApp.Data;

namespace BlazorInvoiceApp.DTOS
{
    public class InvoiceLineItemDTO:IDTO,IOwnedDTO
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = null!;

        public string InvoiceId { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public double UnitPrice { get; set; }
        public double Quantity { get; set; }

    }
}
