using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.UpdateUser
{
    public sealed record UpdateUserCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string? Password,
        bool IsAdmin,
        List<Guid> CompanyIds) : IRequest<Result<string>>;
}
