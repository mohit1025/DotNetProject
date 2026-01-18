using System;
using Web.DataAccess.Repository.IRepository;
using Web.DataAccess.Data;
using Web.Models.Models;
namespace Web.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    public ICategoryRepository Category {get; private set;}
    public IProductRepository Product { get; private set; }
    public ICompanyRepository Company { get; private set; }

     private readonly ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db) 
    {
        _db = db;
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        Company = new CompanyRepository(_db);
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
}
