using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(APIContext context) : base(context)
        {
        }

       
        public IEnumerable<Device> GetRecentDevices(int count)
        {
            return _context.Device.ToList();
        }
    }
}
