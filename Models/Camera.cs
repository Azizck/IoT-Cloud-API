using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class Camera
    {

        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
    	public int DeviceId { get; set; }
		public Device Device { get; set; }
    }
}
