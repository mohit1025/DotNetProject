using System;
using Web.Models.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    void Update(OrderDetails obj);
    void SaveChanges();


}
