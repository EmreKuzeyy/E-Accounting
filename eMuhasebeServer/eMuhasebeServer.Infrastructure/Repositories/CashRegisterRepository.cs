using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Infrastructure.Context;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Infrastructure.Repositories
{
    internal sealed class CashRegisterRepository : Repository<CashRegister, CompanyDbContext>, ICashRegisterRepository
    {
        public CashRegisterRepository(CompanyDbContext context) : base(context)
        {
        }
    }
}
