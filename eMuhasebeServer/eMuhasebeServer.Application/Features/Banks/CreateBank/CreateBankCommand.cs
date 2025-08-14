using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.CreateBank;

public sealed record CreateBankCommand(
    string Name,
    string IBAN,
    int CurrencyTypeValue) : IRequest<Result<string>>;

