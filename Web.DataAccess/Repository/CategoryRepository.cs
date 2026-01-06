using System;
using System.Linq.Expressions;
using DotNetProject.Data;
using DotNetProject.Models;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{   
    private readonly ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
        
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }
}
