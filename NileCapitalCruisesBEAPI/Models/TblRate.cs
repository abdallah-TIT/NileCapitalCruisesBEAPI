using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblRate
{
    public int RateId { get; set; }

    public int? CruiseId { get; set; }

    public string? RateNameSys { get; set; }
}
