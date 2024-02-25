using System;
using System.Collections.Generic;

namespace _3206.Entities;

public partial class Budget
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public decimal Budget1 { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }
}
