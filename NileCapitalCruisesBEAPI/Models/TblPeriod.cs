using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblPeriod
{
    public int PeriodId { get; set; }

    public int? CruiseId { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }
}
