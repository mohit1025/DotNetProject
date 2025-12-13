using System;
using DotNetProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(

             new Category
             {
                 Id = 1,
                 Name = "Electronics",
                 DisplayOrder = 1
             },
            new Category
            {
                Id = 2,
                Name = "Books",
                DisplayOrder = 2
            },
            new Category
            {
                Id = 3,
                Name = "Clothing",
                DisplayOrder = 3
            },
            new Category
            {
                Id = 4,
                Name = "Home & Kitchen",
                DisplayOrder = 4
            },
            new Category
            {
                Id = 5,
                Name = "Sports & Fitness",
                DisplayOrder = 5

            }
       );
    }
}
