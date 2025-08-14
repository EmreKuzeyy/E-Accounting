using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.UpdateBank;

public sealed record UpdateBankCommand(
    Guid Id,
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<Result<string>>;



