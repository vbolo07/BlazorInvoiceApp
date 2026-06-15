using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoiceApp.Data
{
    public class Invoice : IEntity, IOwnedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; } = null;//navigation property to let the compiler know that this prop is a foreign key

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceNumber { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string Description { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;
        public Customer? Customer { get; set; } = null;// one to one relation

        public string InvoiceTermsId { get; set; } = string.Empty;
        public InvoiceTerms? InvoiceTerms { get; set; } = null;

        public double Paid { get; set; } = 0;
        public double Credit { get; set; } = 0;
        public double TaxRate { get; set; } = 0;

        public ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>(); //one to many relation, one invoice can contain more line items

    }
}
