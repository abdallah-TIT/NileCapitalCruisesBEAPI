using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblRatesCabinPerPeriod
{
    public int RateCabinId { get; set; }

    public int? RateId { get; set; }

    public int? CabinId { get; set; }

    public int? PeriodId { get; set; }

    public double? RatePrice { get; set; }

    public int? ItineraryId { get; set; }
}
