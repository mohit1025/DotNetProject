using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models.Models;

public class ShoppingCart
{
    public int ProductId {get; set;}
    [ValidateNever]
    [ForeignKey("ProductId")]
    public Products Product {get; set;}  
    [Range(1,1000,ErrorMessage ="Please enter a value between 1 and 1000")]
    public int Count {get; set;}

    public int Id {get; set;}

    public string ApplicationUserId {get; set;}
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser {get; set;}

    [NotMapped]
    public double Price {get; set;}

}
