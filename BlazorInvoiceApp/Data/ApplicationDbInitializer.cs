using Microsoft.AspNetCore.Identity;

namespace BlazorInvoiceApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await SeedUserAsync(userManager);

            await SeedInvoiceTermsAsync(db, user);
            await SeedCustomersAsync(db, user);
            await SeedInvoicesAsync(db, user);
        }

        private static async Task<ApplicationUser> SeedUserAsync(
            UserManager<ApplicationUser> userManager)
        {
            var email = Environment.GetEnvironmentVariable("DEMO_EMAIL") ?? "demo@test.com";
            var password = Environment.GetEnvironmentVariable("DEMO_PASSWORD") ?? "Demo123!";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                Console.WriteLine("Creating demo user...");
                
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }

                    throw new Exception("Failed to create demo user.");
                }
            }
            return user;
        }

        private static async Task SeedInvoiceTermsAsync(
            ApplicationDbContext db,
            ApplicationUser user)
        {
            
            Console.WriteLine("Creating invoice terms...");
            if (!db.InvoiceTerms.Any(t => t.Name == "Net 15"))
            {
                db.InvoiceTerms.Add(new InvoiceTerms
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "Net 15"
                });
            }
            if (!db.InvoiceTerms.Any(t => t.Name == "Net 30"))
            {
                db.InvoiceTerms.Add(new InvoiceTerms
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "Net 30"
                });
            }
            if (!db.InvoiceTerms.Any(t => t.Name == "Net 60"))
            {
                db.InvoiceTerms.Add(new InvoiceTerms
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "Net 60"
                });
            }      

            await db.SaveChangesAsync();           
        }

        private static async Task SeedCustomersAsync(
            ApplicationDbContext db,
            ApplicationUser user)
        {
            Console.WriteLine("Creating customers...");
            if (!db.Customers.Any(c => c.Name == "OpenAI SRL"))
            {
                db.Customers.Add(new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "OpenAI SRL"
                });
            }
            if (!db.Customers.Any(c => c.Name == "Contoso SRL"))
            {
                db.Customers.Add(new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "Contoso SRL"
                });
            }
            if (!db.Customers.Any(c => c.Name == "Microsoft SRL"))
            {
                db.Customers.Add(new Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Name = "Microsoft SRL"
                });
            }
            
            await db.SaveChangesAsync();
            
        }

        private static async Task SeedInvoicesAsync(
            ApplicationDbContext db,
            ApplicationUser user)
        {
            if (!db.Invoices.Any(t => t.Description == "Cloud Migration Assessment"))
            {
                var customer = db.Customers.First();
                var terms = db.InvoiceTerms.First();
                Console.WriteLine("Creating invoices...");
                var invoice = new Invoice
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    CustomerId = customer.Id,
                    InvoiceTermsId = terms.Id,
                    Description = "Cloud Migration Assessment",
                    TaxRate = 19,
                    CreateDate = DateTime.UtcNow
                };

                db.Invoices.Add(invoice);

                await db.SaveChangesAsync();

                db.InvoiceLineItems.AddRange(

                new InvoiceLineItem
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    InvoiceId = invoice.Id,
                    Description = "Infrastructure Assessment",
                    UnitPrice = 100,
                    Quantity = 8
                },

                new InvoiceLineItem
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    InvoiceId = invoice.Id,
                    Description = "Documentation",
                    UnitPrice = 50,
                    Quantity = 4
                });
            }
            
            await db.SaveChangesAsync();
        }
    }
}
