using eMuhasebeServer.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Deposit { get; set; }
        public decimal WithDrawal { get; set; }
        public List<ProductDetail>? Details { get; set; }
    }
}
