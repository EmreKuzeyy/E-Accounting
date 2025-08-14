using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Auth.SendConfirmEmail
{
    public sealed record SendConfirmEmailCommand(string Email) : IRequest<Result<string>>;



    internal sealed class SendConfirmEmailCommandHandler
        (UserManager<AppUser> userManager,
        IMediator mediator) : IRequestHandler<SendConfirmEmailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(SendConfirmEmailCommand request, CancellationToken cancellationToken)
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

            await mediator.Publish(new AppUserEvent(appUser.Id));

            return "Onay Maili Başarıyla Gönderildi";
        }
    }
}
