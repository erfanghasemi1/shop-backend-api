using System;
using System.Collections.Generic;

namespace ShopProject.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? BuyerId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Buyer { get; set; }
}
