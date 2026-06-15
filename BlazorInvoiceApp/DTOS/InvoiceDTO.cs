using BlazorInvoiceApp.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoiceApp.DTOS
{
    public class InvoiceDTO:IDTO,IOwnedDTO
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = null!;
       
        public int InvoiceNumber { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string Description { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
      
        public string InvoiceTermsId { get; set; } = string.Empty;
        public string InvoiceTermsName { get; set; } = string.Empty;


        public double Paid { get; set; } = 0;
        public double Credit { get; set; } = 0;
        public double TaxRate { get; set; } = 0;
        public double InvoiceTotal { get; set; } = 0;
    }
    
}
