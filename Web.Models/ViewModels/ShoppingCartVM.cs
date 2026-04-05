using System;

namespace Web.Models.ViewModels;

public class ShoppingCartVM
{   
    public IEnumerable<Models.ShoppingCart> ShoppingCartList { get; set; }
    public double OrderTotal { get; set; }

}
