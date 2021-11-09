using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public interface IUnitOfWork : IDisposable
    {
        IDeviceRepository Devices { get; }
      
        int Complete();
    }
}
