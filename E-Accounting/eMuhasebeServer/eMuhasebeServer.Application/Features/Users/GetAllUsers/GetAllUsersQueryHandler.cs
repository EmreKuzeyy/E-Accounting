using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler(
        UserManager<AppUser> userManager,
        ICacheService cacheService
        ) : IRequestHandler<GetAllUsersQuery, Result<List<AppUser>>>
    {
        public async Task<Result<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<AppUser>? users;

           users = cacheService.Get<List<AppUser>>("users");

            if(users is null) 
            {
                users = await userManager.Users
                .Include(u => u.CompanyUsers!)
                .ThenInclude(u => u.Company)
                .OrderBy(u => u.FirstName)
                .ToListAsync(cancellationToken);

                cacheService.Set<List<AppUser>>("users", users);
            }

            return users;
        }
    }
}
