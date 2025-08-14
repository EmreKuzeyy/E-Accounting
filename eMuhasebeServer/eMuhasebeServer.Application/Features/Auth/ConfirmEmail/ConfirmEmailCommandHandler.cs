using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Auth.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(request.Email);
            if (appUser == null)
            {
                return "Mail Adresi Sistemde Kayıtlı Değil";
            }

            if (appUser.EmailConfirmed)
            {
                return "Mail Adresi Zaten Onaylanmış";
            }

            appUser.EmailConfirmed = true;
            await userManager.UpdateAsync(appUser);

            return "Email Adresiniz Başarıyla Onaylandı";
        }
    }
}
