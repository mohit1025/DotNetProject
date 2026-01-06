using System;
using Web.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface IProductRepository : IRepository<Products>
{
    void Update(Products obj);
    void SaveChanges();
}
