using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {

        IEnumerable<Device> GetRecentDevices(int count);
    }
}
