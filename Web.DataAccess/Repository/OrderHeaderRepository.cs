using System;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models.Models;

namespace Web.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository

{
     private readonly ApplicationDbContext _db;
    public OrderHeaderRepository (ApplicationDbContext db) : base(db)
    {
        _db = db;
        
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    public void Update(OrderHeader obj)
    {
        _db.OrderHeaders.Update(obj);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderFromDb != null)
        {
            orderFromDb.OrderStatus = orderStatus;
            if (!string.IsNullOrEmpty(paymentStatus) )
            {
                orderFromDb.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if(!string.IsNullOrEmpty(sessionId))
        {
            orderFromDb.SessionId = sessionId;
        }
        if(!string.IsNullOrEmpty(paymentIntentId))
        {
            orderFromDb.paymentIntentId = paymentIntentId;
            orderFromDb.PaymentDate = DateOnly.FromDateTime(DateTime.Now);
        }
    }

}
