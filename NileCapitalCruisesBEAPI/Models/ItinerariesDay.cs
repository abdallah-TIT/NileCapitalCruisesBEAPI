using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class ItinerariesDay
{
    public int ItineraryDayId { get; set; }

    public int? ItineraryId { get; set; }

    public string? DayName { get; set; }
}
