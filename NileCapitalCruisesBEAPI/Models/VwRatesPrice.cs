using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwRatesPrice
{
    public int RateCabinId { get; set; }

    public int? RateId { get; set; }

    public int? CabinId { get; set; }

    public int? PeriodId { get; set; }

    public double? RatePrice { get; set; }

    public int? ItineraryId { get; set; }

    public string? RateNameSys { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? ItineraryNameSys { get; set; }

    public int? WeekDayId { get; set; }

    public string? WeekDayName { get; set; }

    public int? DestinationIdfrom { get; set; }

    public int? DestinationIdto { get; set; }

    public string? DestinationFrom { get; set; }

    public string? DestinationTo { get; set; }

    public string? DurationName { get; set; }

    public int? DurationDays { get; set; }

    public int? DurationNights { get; set; }

    public int? DurationId { get; set; }

    public bool? ItineraryStatus { get; set; }

    public string? CruiseNameSys { get; set; }

    public string? CruiseUrl { get; set; }

    public string? CabinNameSys { get; set; }

    public string? CabinPhoto { get; set; }

    public bool? CabinStatus { get; set; }

    public int? CabinMaxAdults { get; set; }

    public int? CabinMaxChild { get; set; }

    public int? CabinMaxOccupancy { get; set; }

    public int? CabinNumbers { get; set; }
}
