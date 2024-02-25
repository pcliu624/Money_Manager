using System;
using System.Collections.Generic;

namespace _3206.Entities;

public partial class Purchase
{
    public long Id { get; set; }

    public DateTime? Date { get; set; }

    public string Payby { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? Type { get; set; }

    public string? Store { get; set; }

    public string? Payfor { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }
}
