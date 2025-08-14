using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Bank : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public decimal DepositAmount { get; set; } //giriş
        public decimal WithDrawalAmount { get; set; } // çıkış
        public CurrencyTypeEnum CurrencyType { get; set; } = CurrencyTypeEnum.TL;
        public List<BankDetail>? Details { get; set; }
    }
}
