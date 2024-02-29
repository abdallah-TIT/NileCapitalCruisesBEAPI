using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwCruisesItinerariesOperationDate
{
    public int OperationDateId { get; set; }

    public int? CruiseId { get; set; }

    public DateTime? OperationDate { get; set; }

    public int? OperationDateDay { get; set; }

    public int? OperationDateMonth { get; set; }

    public int? OperationDateYear { get; set; }

    public int? OperationDateWeekDayId { get; set; }

    public int ItineraryId { get; set; }

    public string? ItineraryNameSys { get; set; }

    public int? DurationId { get; set; }

    public string? DurationName { get; set; }

    public int? CountAllotment { get; set; }

    public string? CruiseNameSys { get; set; }

    public string? CruiseUrl { get; set; }

    public string? CruisePhoto { get; set; }

    public string? CruiseBanner { get; set; }

    public string? WeekDayName { get; set; }

    public int? WeekDayId { get; set; }
}
