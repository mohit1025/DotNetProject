using System;
using DotNetProject.Models;
using Web.Models;

namespace Web.DataAccess.Repository.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category obj);
    void SaveChanges();

}
