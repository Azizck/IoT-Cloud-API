
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class Setting
    {

        public int Id { get; set; }
        public bool IsNotificationOn { get; set; }
        public bool IsSleep { get; set; }
        public bool IsAlarmOn { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
