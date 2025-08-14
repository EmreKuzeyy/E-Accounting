using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Invoice : Entity
    {
        public InvoiceTypeEnum Type { get; set; } = InvoiceTypeEnum.Purchase;
        public DateOnly Date { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public List<InvoiceDetail>? Details { get; set; }
    }
}
