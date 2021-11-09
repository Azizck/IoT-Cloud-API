using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class ApplicationUser : IdentityUser
    {
       
        public int? DeviceId { get; set; }
        public string FullName { get; set; }
        public string NotificationKey { get; set; }
        public Device Device { get; set; }
    }
}


