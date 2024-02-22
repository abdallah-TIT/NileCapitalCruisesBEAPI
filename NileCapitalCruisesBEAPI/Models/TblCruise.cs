using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCruise
{
    public int CruiseId { get; set; }

    public string? CruiseNameSys { get; set; }

    public string? CruiseUrl { get; set; }

    public bool? CruiseStatus { get; set; }

    public virtual ICollection<TblCruisesContent> TblCruisesContents { get; set; } = new List<TblCruisesContent>();
}
