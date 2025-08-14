using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Enums
{
    public sealed class CurrencyTypeEnum : SmartEnum<CurrencyTypeEnum>
    {
        public static readonly CurrencyTypeEnum TL = new("TL", 1);
        public static readonly CurrencyTypeEnum USD = new("USD", 2);
        public static readonly CurrencyTypeEnum EUR = new("Euro", 3);
        public CurrencyTypeEnum(string name, int value) : base(name, value)
        {
        }
    }
}
