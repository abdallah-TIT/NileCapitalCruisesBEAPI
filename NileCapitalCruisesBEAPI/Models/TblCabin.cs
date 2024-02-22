using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCabin
{
    public int CabinId { get; set; }

    public int? CruiseId { get; set; }

    public string? CabinNameSys { get; set; }

    public string? CabinPhoto { get; set; }

    public bool? CabinStatus { get; set; }

    public int? CabinMaxAdults { get; set; }

    public int? CabinMaxChild { get; set; }

    public int? CabinMaxOccupancy { get; set; }

    public int? CabinNumbers { get; set; }

    public virtual ICollection<TblCabinsContent> TblCabinsContents { get; set; } = new List<TblCabinsContent>();
}
