using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Events;
using FluentEmail.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;

public sealed class SendConfirmEmailEvent(
    UserManager<AppUser> userManager,
    IFluentEmail fluentEmail) : INotificationHandler<AppUserEvent>
{
    public async Task Handle(AppUserEvent notification, CancellationToken cancellationToken)
    {
        AppUser? appUser =  await userManager.FindByIdAsync(notification.UserId.ToString());

        if (appUser is not null )
        {
            await fluentEmail
                .To(appUser.Email)
                .Subject("Onay Maili")
                .Body(CreateBody(appUser), true)
                .SendAsync(cancellationToken);

        }
    }

    private string CreateBody(AppUser appUser) 
    {
        string body = $@"Mail Adresinizi Onaylamak İçin Aşağıdaki Adrese Tıklayın.
 <br/>
<a href ='http://localhost:4200/confirm-email/{appUser.Email}' target='_blank'>Mailinizi Onaylamak İçin Tıklayın
</a> 
";

        return body;
    }
}
