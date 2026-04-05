using System;

namespace Web.DataAccess.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


public class ApplicationDbContextFactory :  IDesignTimeDbContextFactory<ApplicationDbContext>
{


    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlite("Data Source=app.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }


}



