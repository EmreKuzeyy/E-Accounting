using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMuhasebeServer.Domain.Events
{
    public sealed class AppUserEvent : INotification
    {
        public Guid UserId { get; private set; }

        public AppUserEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
