using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Web.Models.Models;

public class OrderDetails
{
    [Required]
    public int Id { get; set; }
    [ForeignKey("OrderHeaderId")]
    public int OrderHeaderId { get; set; }
    public OrderHeader OrderHeader { get; set; }
    [ForeignKey("ProductId")]
    [Required]
    public int ProductId { get; set; }
    [ValidateNever]
    public Products Product { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }

}
