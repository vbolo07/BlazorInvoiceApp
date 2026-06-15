using BlazorInvoiceApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoiceApp.DTOS
{
    public class InvoiceTermsDTO:IDTO,IOwnedDTO
    {
      
        public string Id { get; set; } = String.Empty;

        public string UserId { get; set; } = null!;

        public string Name { get; set; } = String.Empty;
    }
}
