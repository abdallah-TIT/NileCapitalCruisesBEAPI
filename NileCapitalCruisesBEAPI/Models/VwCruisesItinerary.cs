using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwCruisesItinerary
{
    public int ItineraryId { get; set; }

    public int? CruiseId { get; set; }

    public string? ItineraryNameSys { get; set; }

    public int? DurationId { get; set; }

    public bool? ItineraryStatus { get; set; }

    public int? WeekDayId { get; set; }

    public int? DestinationIdfrom { get; set; }

    public int? DestinationIdto { get; set; }

    public int ItineraryContentId { get; set; }

    public int? LangId { get; set; }

    public string? ItineraryName { get; set; }

    public string? LangName { get; set; }

    public string? LangAbbreviation { get; set; }

    public string? WeekDayName { get; set; }

    public string? CruiseNameSys { get; set; }

    public string? CruiseUrl { get; set; }

    public string? CruisePhoto { get; set; }

    public string? CruiseBanner { get; set; }

    public string? CruiseDescription { get; set; }
}
