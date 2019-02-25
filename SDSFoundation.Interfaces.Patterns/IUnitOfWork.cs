using SDSFoundation.Interfaces.EntityFramework;
using System;

namespace SDSFoundation.Interfaces.Patterns
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit<T>(System.Data.IsolationLevel isolationLevel) where T : class, IEntityObject;
    }
}
