using SDSFoundation.Interfaces.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Interfaces.Device
{
    public interface IDeviceRepository<T> : IRepository<T>
    {
        IQueryable<T> GetDeviceEntityByType(bool? deepLoad);
        IQueryable<T> QueryById(int Id, bool? deepLoad);
    }
}
