using System;
using Web.DataAccess.Data;
using Web.DataAccess.Repository.IRepository;
using Web.Models.Models;

namespace Web.DataAccess.Repository;

public class CompanyRepository :Repository<Company>, ICompanyRepository
{
     private readonly ApplicationDbContext _db;
    public CompanyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
        
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }
     public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }

}
