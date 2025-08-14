using eMuhasebeServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class BankDetail : Entity
    {
        public Guid BankId { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DepositAmount { get; set; } //giriş
        public decimal WithDrawalAmount { get; set; } // çıkış
        public Guid? BankDetailId { get; set; }
        public Guid? CashRegisterDetailId { get; set; }
        public Guid? CustomerDetailId { get; set; }


        //public BankDetail? BankDetailOpposite { get; set; }
    }
}
