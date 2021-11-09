using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models.Interfaces
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotifications();
    }
}
