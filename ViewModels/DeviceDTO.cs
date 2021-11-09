using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.ViewModels
{
    public class DashboardDTO
    {
        // Either connection or disconnected
        public string ConnectionStatus { get; set; }
        // toggle button
        public string Status { get; set; }
        public bool IsAlarmOn { get; set; }
        // second last box e.g Your Package Has Been Picked Up
        public string PackageMessage { get; set; }
      
        // show image of empty box when count is zero
        public int PackagesPlacedCount { get; set; }

    }
    
}
