using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
	public class Package
	{

		public int Id { get; set; }
		public int DeviceId { get; set; }
		public Device Device { get; set; }
		public DateTime PickedUpAt { get; set; }
		public DateTime PlacedAt { get; set; }
		public double Confidence {get ; set;}
		public string PickedUpBy { get; set; }
		public bool PickupAcknowledged { get; set; }
		public ICollection<Video> Videos { get; set; }
		public ICollection<Notification> Notifications { get; set; }


	}
}