using System;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository;

public interface IUnitOfWork
{
    ICategoryRepository Category {get; }
    IProductRepository Product{get;}
    void SaveChanges();
}
