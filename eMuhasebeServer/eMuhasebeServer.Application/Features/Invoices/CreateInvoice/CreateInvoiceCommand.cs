using eMuhasebeServer.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.CreateInvoice;

public sealed record CreateInvoiceCommand(
    int TypeValue,
    DateOnly Date,
    string InvoiceNumber,
    Guid CustomerId,
    List<InvoiceDetailDto> Details) : IRequest<Result<string>>;
