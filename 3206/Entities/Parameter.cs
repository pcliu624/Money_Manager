using System;
using System.Collections.Generic;

namespace _3206.Entities;

public partial class Parameter
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;
}
