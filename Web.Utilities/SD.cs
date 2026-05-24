using System;

namespace Web.Utilities;

public class SD
{
    public const string Role_Admin = "Admin";
    public const string Role_Employee = "Employee";
    public const string Role_User_Indi = "Customer";
    public const string Role_User_Comp = "Company";

    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusInProcess = "Processing";
    public const string StatusShipped = "Shipped";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Rejected";

    public const string StatusDelayed = "Delayed";

    public const string PaymentStatusPending = "Pending";
    public const string PaymentStatusApproved = "Approved";
    public const string PaymentStatusDelayedPayment = "Delayed Payment";
    public const string PaymentStatusRefunded = "Refunded";

    public const string PaymentStatusCancelled = "Cancelled";

    public const string SessionCart = "SessionShoppingCart";
}
