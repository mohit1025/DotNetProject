using System;
using Web.Models.Models;

namespace Web.Models.ViewModels;

public class ShoppingCartVM
{   
    public IEnumerable<Models.ShoppingCart> ShoppingCartList { get; set; }
    public double OrderTotal { get; set; }
    public OrderHeader OrdersHeader { get; set; }  

}
