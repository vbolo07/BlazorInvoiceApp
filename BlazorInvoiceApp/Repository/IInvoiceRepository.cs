using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface IInvoiceRepository:IGenericOwnedRepository<Invoice,InvoiceDTO>
    {
        //join the data to include it in one DTO object
        public Task<List<InvoiceDTO>> GetAllMineFlat(ClaimsPrincipal User);

        //delete also the line items coresponding to that invoiceID
        public Task DeleteWithLineItems(ClaimsPrincipal User, string invoiceid);
    }
}
