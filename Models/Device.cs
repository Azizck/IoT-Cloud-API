using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class Device
    {

        public Device()
        {
            Number = new Guid().ToString();
        }
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ApplicationUser> Owners { get; set; } = new List<ApplicationUser>();
        public ICollection<Package> Packages { get; set; } = new List<Package>();
        public Setting Setting { get; set; }
        public Camera Camera { get; set; }
        public Alarm Alarm { get; set; }
        public string Version { get; set; }
        public DateTime SyncedAt { get; set; }
        public string Note { get; set; }
        public bool IsConnected { get => SyncedAt != DateTime.MinValue && (SyncedAt - DateTime.Now).TotalSeconds >= -15; }
    }
}
