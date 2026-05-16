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
    public IShoppingCartRepository shoppingCart {get; private set;}
    public IApplicationUserRepository ApplicationUser {get; private set;}
    public IOrderDetailsRepository OrderDetails {get; private set;}
    public IOrderHeaderRepository OrderHeader {get; private set;}

     private readonly ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db) 
    {
        _db = db;
        ApplicationUser = new ApplicationUserRepository(_db);
        Category = new CategoryRepository(_db);
        Product = new ProductRepository(_db);
        Company = new CompanyRepository(_db);
        shoppingCart = new ShoppingCartRepository(_db);
        OrderDetails = new OrderDetailsRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);

    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
}
