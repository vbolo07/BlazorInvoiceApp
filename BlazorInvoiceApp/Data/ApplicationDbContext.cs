using BlazorInvoiceApp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorInvoiceApp.Data
{

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    { 
        public DbSet<Invoice> Invoices {  get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceTerms> InvoiceTerms { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }



        //Disabling cascade deleting, for ex when we delete an invoice item doesnt delete all line items unless you are doing that on purpose
        protected void RemoveFixups(ModelBuilder modelBuilder, Type type)
        {
            foreach (var relationship in modelBuilder.Model.FindEntityType(type)!.GetForeignKeys())
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientNoAction;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //customizations
            RemoveFixups(modelBuilder, typeof(Invoice));
            RemoveFixups(modelBuilder, typeof(Customer));
            RemoveFixups(modelBuilder, typeof(InvoiceTerms));
            RemoveFixups(modelBuilder, typeof(InvoiceLineItem));

            //don't send the value to the database while changing it
            modelBuilder.Entity<Invoice>().Property(u => u.InvoiceNumber).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            //don't create total price column in the database because it is a computed column
            modelBuilder.Entity<InvoiceLineItem>()
                .Property(u => u.TotalPrice)
                .HasComputedColumnSql("[UnitPrice]*[Quantity]");

            base.OnModelCreating(modelBuilder);
        }

    }
}
