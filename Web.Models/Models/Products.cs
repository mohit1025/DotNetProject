using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using DotNetProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models;

public class Products
{

    [Key]
    public int Id { get; set; }
    [MaxLength(30)]
    [Required]
    public string Title { get; set; } = "";
    [Required]
    public string Description { get; set; } = "";
    [Required]
    public string ISBN { get; set; } = "";
    [Required]
    public string Author { get; set; } = "";
    [Required]
    [Range(1, 1000)]
     [DisplayName("List Price")]
    public double ListPrice { get; set; }
    
    [Required]
    [Range(1, 1000)]
     [DisplayName("Price for 1-50")]
    public double Price { get; set; }
    [Required]
    [Range(1, 1000)]
     [DisplayName("Price for 50+")]
    public double Price50 { get; set; }
    [Required]
    [Range(1, 1000)]
     [DisplayName("Price for 100+")]
    public double Price100 { get; set; }
    [Required]
     
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category? Category { get; set; }

    public string? ImageUrl { get; set; }
}
