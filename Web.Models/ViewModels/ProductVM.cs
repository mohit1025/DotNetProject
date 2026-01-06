using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models.ViewModels;

public class ProductVM
{
    [Required]
    public Products Products { get; set; } 

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
}
