using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblMasterDuration
{
    public int DurationId { get; set; }

    public string? DurationName { get; set; }

    public int? DurationDays { get; set; }

    public int? DurationNights { get; set; }
}
