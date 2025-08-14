using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.GetAllInvoices;

internal sealed class GetAllInvoicesQueryHandler(
    IInvoiceRepository invoiceRepository,
    ICacheService cacheService) : IRequestHandler<GetAllInvoicesQuery, Result<List<Invoice>>>
{
    public async Task<Result<List<Invoice>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        List<Invoice>? invoices;
        string key = "invoices";

       
        invoices = cacheService.Get<List<Invoice>>(key);

        if (invoices is null)
        {
            invoices = await invoiceRepository
                .GetAll()
                .Include(p=> p.Customer)
                .Include(p=> p.Details!)
                .ThenInclude(p=> p.Product)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            cacheService.Set(key, invoices);
        }

        return invoices;


        #region Uzun Yöntem

        //List<Invoice>? invoices;

        //if (request.Type == 1)
        //{
        //    invoices = cacheService.Get<List<Invoice>>("purchaseInvoices");

        //    if (invoices is null)
        //    {
        //        invoices = await invoiceRepository
        //            .Where(p => p.Type.Value == request.Type)
        //            .OrderBy(p => p.Date)
        //            .ToListAsync(cancellationToken);

        //        cacheService.Set("purchaseInvoices", invoices);
        //    }
        //}
        //else
        //{
        //    invoices = cacheService.Get<List<Invoice>>("sellingInvoices");

        //    if (invoices is null)
        //    {
        //        invoices = await invoiceRepository
        //            .Where(p => p.Type.Value == request.Type)
        //            .OrderBy(p => p.Date)
        //            .ToListAsync(cancellationToken);

        //        cacheService.Set("sellingInvoices", invoices);
        //    }
        //}
        #endregion

    }
}
