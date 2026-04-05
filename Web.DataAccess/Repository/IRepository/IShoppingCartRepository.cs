using System;
using Web.Models;
using Web.Models.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    void Update(ShoppingCart obj);
    void SaveChanges();

}
