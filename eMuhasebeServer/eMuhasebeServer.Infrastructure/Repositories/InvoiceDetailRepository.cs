using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;

internal sealed class InvoiceDetailRepository : Repository<InvoiceDetail, CompanyDbContext>, IInvoiceDetailRepository
{
    public InvoiceDetailRepository(CompanyDbContext context) : base(context)
    {
    }
}