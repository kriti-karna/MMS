using System;
using System.Collections.Generic;

namespace MMS.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? MovieId { get; set; }

    public virtual Movie? Movie { get; set; }
}
