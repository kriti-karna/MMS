using System;
using System.Collections.Generic;

namespace MMS.Models;

public partial class Movie
{
    public int Id { get; set; }

    public int? Language { get; set; }

    public string? Name { get; set; }

    public int? Rating { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Genre> Genres { get; } = new List<Genre>();

    public virtual Language? LanguageNavigation { get; set; }
}
