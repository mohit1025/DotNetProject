using System;
using Web.Models.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
     void Update(Company obj);
    void SaveChanges();

}
