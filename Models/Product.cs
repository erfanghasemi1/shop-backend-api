using System;
using System.Collections.Generic;

namespace ShopProject.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? SellerId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User? Seller { get; set; }
}
