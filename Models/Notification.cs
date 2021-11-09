using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
	public class Notification
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public string Title { get; set; }
		public bool IsDismissed { get; set; }
		public string Data { get; set; }
		public int PackageId { get; set; }
		public DateTime createdAt { get; set; }
		public virtual Package Package {get; set;}

	}	
}
