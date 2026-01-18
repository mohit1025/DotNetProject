using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Web.Models.Models;

namespace Web.Models;

public class ApplicationUser : IdentityUser
{   
    
    public string? Name { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    [DisplayName("Company Name")]
    public int? CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    [ValidateNever]
    public Company ? Company { get; set; }
}