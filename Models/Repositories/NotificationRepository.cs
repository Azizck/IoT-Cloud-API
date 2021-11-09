using packagesentinel.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(APIContext context) : base(context)
        {

        }

        public IEnumerable<Notification> GetNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
