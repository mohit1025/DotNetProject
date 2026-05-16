using System;
using Web.Models.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface IOrderHeaderRepository : IRepository<OrderHeader>
{
    void Update(OrderHeader obj);
    void SaveChanges();
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId);

}
