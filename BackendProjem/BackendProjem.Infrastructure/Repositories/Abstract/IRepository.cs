using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BackendProjem.Infrastructure;

public interface IRepository<T> where T : class
{
    Task<object> Insert(T entity);
    Task<T> Update(T entity);
    Task Remove(T entity);
    Task BatchRemove(IEnumerable<T> entities);
    Task BatchRemove(Expression<Func<T, bool>> predicate);
    Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    Task<List<T>> FindList(Expression<Func<T, bool>> predicate);
    Task Remove(Expression<Func<T, bool>> predicate);

    Task<List<T>> FindList();

}
