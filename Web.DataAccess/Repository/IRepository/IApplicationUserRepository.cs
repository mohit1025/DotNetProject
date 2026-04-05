using System;
using Web.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
     void Update(ApplicationUser obj);
    void SaveChanges();
}
