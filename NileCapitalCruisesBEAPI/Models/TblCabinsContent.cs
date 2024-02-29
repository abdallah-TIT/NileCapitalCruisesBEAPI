using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCabinsContent
{
    public int CabinContentId { get; set; }

    public int? CabinId { get; set; }

    public int? LangId { get; set; }

    public string? CabinName { get; set; }

    public string? CabinDescription { get; set; }

    public string? CabinSize { get; set; }

    public string? CabinBed { get; set; }

    public virtual TblCabin? Cabin { get; set; }
}
