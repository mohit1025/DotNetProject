using System;
using DotNetProject.Data;
using Web.Models;
using Web.DataAccess.Repository.IRepository;

namespace Web.DataAccess.Repository;

public class ProductRepository : Repository<Products>, IProductRepository
{
	private readonly ApplicationDbContext _db;
	public ProductRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}

	public void SaveChanges()
	{
		_db.SaveChanges();
	}

	public void Update(Products obj)
	{

		var ProductObject = _db.Products.FirstOrDefault(e => e.Id == obj.Id);
		if (ProductObject != null)
		{
			ProductObject.Title = obj.Title;
			ProductObject.Description = obj.Description;
			ProductObject.Price = obj.Price;
			ProductObject.CategoryId = obj.CategoryId;
			if (!string.IsNullOrEmpty(obj.ImageUrl))
			{
				ProductObject.ImageUrl = obj.ImageUrl;
			}
			ProductObject.ISBN = obj.ISBN;
			ProductObject.Price50 = obj.Price50;
			ProductObject.Price100 = obj.Price100;
			ProductObject.Author = obj.Author;
			ProductObject.ListPrice = obj.ListPrice;
			ProductObject.CategoryId = obj.CategoryId;
		}
	}
}
