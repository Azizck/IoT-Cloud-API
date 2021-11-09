using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        T Update(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        public IQueryable<T> Map(Expression<Func<T, bool>> expression);
        
        void RemoveRange(IEnumerable<T> entities);
    }
}
