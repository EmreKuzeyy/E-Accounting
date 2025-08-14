using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.DeleteUserById
{
    internal sealed class DeleteUserByIdCommandHandler(UserManager<AppUser> userManager,
        ICacheService cacheService) : IRequestHandler<DeleteUserByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(request.Id.ToString());

            if (appUser == null)
            {
                return Result<string>.Failure("Kullanıcı Bulunamadı");
            }

            appUser.IsDeleted = true;

            IdentityResult ıdentityResult = await userManager.UpdateAsync(appUser);

            if (!ıdentityResult.Succeeded)
            {
                return Result<string>.Failure(ıdentityResult.Errors.Select(s => s.Description).ToList());
            }

            cacheService.Remove("users");

            return "Kullanıcı Başarıyla Silindi";
        }
    }
}
