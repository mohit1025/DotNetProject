using System;

namespace Web.Models.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class OrderHeader
{
    public int Id { get; set; }

    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; }

    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
    public DateTime OrderDate { get; set; }

    public DateTime ShippingDate { get; set; }
    public double OrderTotal { get; set; }
    public string OrderStatus { get; set; }
    public string? PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public string? SessionId { get; set; }

    public DateOnly PaymentDate { get; set; }
    public DateOnly PaymentDueDate { get; set; }

    public string? paymentIntentId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? StreetAddress { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? State { get; set; }
    [Required]
    public string? PostalCode { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }


}
