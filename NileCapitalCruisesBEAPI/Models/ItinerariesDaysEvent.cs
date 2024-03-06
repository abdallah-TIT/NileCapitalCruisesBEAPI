using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class ItinerariesDaysEvent
{
    public int ItinerariesDaysEventsId { get; set; }

    public int? ItineraryDayId { get; set; }

    public string? EventDescription { get; set; }
}
