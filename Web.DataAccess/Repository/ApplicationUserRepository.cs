using System;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models;

namespace Web.DataAccess.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{

    private readonly ApplicationDbContext _db;
    public ApplicationUserRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
        
    }
    public void SaveChanges()
    {
        _db.SaveChanges();
    }

    public void Update(ApplicationUser obj)
    {
        _db.Update(obj);
    }
}
