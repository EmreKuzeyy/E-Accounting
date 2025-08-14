using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string Name,
    int TypeValue,
    string City,
    string Town,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber) : IRequest<Result<string>>;



