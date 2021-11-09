using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models {
    public class User {
        public int? DeviceId { get; set; }
       
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string NotificationKey { get; set; }
        public Device Device { get; set; }
    }
}
