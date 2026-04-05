using System;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category {get; }
    IProductRepository Product{get;}
    ICompanyRepository Company { get; }
    IShoppingCartRepository shoppingCart {get;}
    IApplicationUserRepository ApplicationUser {get;}
    void SaveChanges();
}
