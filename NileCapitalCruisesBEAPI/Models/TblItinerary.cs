using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblItinerary
{
    public int ItineraryId { get; set; }

    public int? CruiseId { get; set; }

    public string? ItineraryNameSys { get; set; }

    public int? DurationId { get; set; }

    public bool? ItineraryStatus { get; set; }

    public int? WeekDayId { get; set; }

    public int? DestinationIdfrom { get; set; }

    public int? DestinationIdto { get; set; }
}
