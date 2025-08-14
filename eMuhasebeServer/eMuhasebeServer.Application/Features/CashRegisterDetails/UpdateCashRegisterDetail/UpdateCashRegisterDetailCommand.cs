using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.UpdateCashRegisterDetail;

public sealed record UpdateCashRegisterDetailCommand(
    Guid Id,
    Guid CashRegisterId,
    int Type,
    DateOnly Date,
    decimal Amount,
    string Description) : IRequest<Result<string>>;


