using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.GetAllCashRegisters
{
    public sealed record GetAllCashRegistersQuery : IRequest<Result<List<CashRegister>>>;
}
