using System;
using System.Collections.Generic;

namespace MMS.Models;

public partial class Language
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Movie> Movies { get; } = new List<Movie>();
}
