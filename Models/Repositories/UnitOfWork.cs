using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
   public class UnitOfWork : IUnitOfWork
{
    private readonly APIContext _context;
    public UnitOfWork(APIContext context)
    {
        _context = context;
        Devices = new DeviceRepository(_context);

    }
    public IDeviceRepository Devices { get; private set; }
   
    public int Complete()
    {
        return _context.SaveChanges();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
}
