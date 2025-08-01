﻿using System;
using System.Collections.Generic;

namespace ShopProject.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? EncryptedPassword { get; set; }

    public string? Role { get; set; }

    public decimal? Wallet { get; set; }

    public DateTime? CreatedAt { get; set; }
}
