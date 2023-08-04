using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BackendProjem.Infrastructure;

public abstract class SqlRepository<T, TContext> : IRepository<T> where T : class where TContext : DbContext
{
    protected TContext Context;

    public SqlRepository(TContext context)
    {
        Context = context;
    }
    public async Task<List<T>> FindList(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).SingleOrDefaultAsync();
    }

    public async Task<object> Insert(T entity)
    {
        await Context.Set<T>().AddAsync(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task Remove(T entity)
    {
        Context.Set<T>().Remove(entity);

        await Context.SaveChangesAsync();
    }

    public async Task BatchRemove(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);

        await Context.SaveChangesAsync();
    }

    public async Task BatchRemove(Expression<Func<T, bool>> predicate)
    {
        var entities = Context.Set<T>();
        entities.RemoveRange(entities.Where(predicate));

        await Context.SaveChangesAsync();
    }

    public async Task Remove(Expression<Func<T, bool>> predicate)
    {
        var entity = await Context.Set<T>().Where(predicate).FirstOrDefaultAsync();

        Context.Set<T>().Remove(entity);

        await Context.SaveChangesAsync();
    }

    public async Task<T> Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }



    public async Task<List<T>> FindList()

    {

        return await Context.Set<T>().ToListAsync();

    }

}
