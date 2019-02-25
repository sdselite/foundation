using System.Linq;

namespace SDSFoundation.Interfaces.Patterns
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Remove(T item);
        void Commit(System.Data.IsolationLevel isolationLevel);
        IQueryable<T> Query();
        IQueryable<T> QueryById(int Id);
    }
}
