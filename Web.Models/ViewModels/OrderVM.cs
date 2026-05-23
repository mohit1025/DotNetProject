using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.Models;

namespace Web.Models.ViewModels;

public class OrderVM
{
    [Required]
    public OrderHeader OrderHeader { get; set; } 

    [ValidateNever]
    public IEnumerable<OrderDetails> OrderDetail { get; set; }
}
