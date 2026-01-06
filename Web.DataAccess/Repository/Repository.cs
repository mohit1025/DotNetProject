using System;
using System.Linq.Expressions;
using DotNetProject.Data;
using Microsoft.EntityFrameworkCore;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
        _db.Products.Include(u => u.Category);
    }
    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public void DeleteRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }
        }

        query = query.Where(filter);

        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }
        }
        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }
}
