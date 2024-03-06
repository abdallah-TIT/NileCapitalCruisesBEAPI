using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCruisesContent
{
    public int CruiseContentId { get; set; }

    public int? CruiseId { get; set; }

    public int? LangId { get; set; }

    public string? CruiseName { get; set; }

    public string? CruiseDescription { get; set; }

    public virtual TblCruise? Cruise { get; set; }
}
