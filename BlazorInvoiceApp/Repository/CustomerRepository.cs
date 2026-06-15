using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public class CustomerRepository : GenericOwnedRepository<Customer, CustomerDTO>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context, IMapper mapper) 
            : base(context, mapper)
        {
        }

        //in case the logged in user doesnt have any customers it created a dummy customer
        public async Task Seed(ClaimsPrincipal User)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                int count = await context.Customers.Where(c => c.UserId == userid).CountAsync();
                if (count == 0)
                {
                    CustomerDTO defaultCustomer = new CustomerDTO
                    {
                        Name = "My first customer"
                    };
                    await AddMine(User, defaultCustomer);
                }
            }
            return;
        }
    }
}
