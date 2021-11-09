using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.ViewModels
{
    public class DeviceDTO
    {
        // Either connection or disconnected
      public bool IsSleep { get; set; }
      public bool IsAlarmOn { get; set; }

    }
    
}
