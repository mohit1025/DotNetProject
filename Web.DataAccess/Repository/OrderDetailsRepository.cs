using System;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models.Models;

namespace Web.DataAccess.Repository;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository

{
     private readonly ApplicationDbContext _db;
    public OrderDetailsRepository (ApplicationDbContext db) : base(db)
    {
        _db = db;
        
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    public void Update(OrderDetails obj)
    {
        _db.OrderDetails.Update(obj);
    }
}
