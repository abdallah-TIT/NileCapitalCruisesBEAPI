using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwCabinsPhoto
{
    public int PhotoId { get; set; }

    public int? CabinId { get; set; }

    public string? PhotoFileName { get; set; }

    public bool? PhotoStatus { get; set; }

    public int? PhotoPosition { get; set; }

    public int? CruiseId { get; set; }

    public string? CabinNameSys { get; set; }

    public string? CabinPhoto { get; set; }

    public string? CruiseNameSys { get; set; }

    public string? CruiseUrl { get; set; }
}
