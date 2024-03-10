using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwExtrasExtraPriceType
{
    public int CruiseId { get; set; }

    public string? CruiseUrl { get; set; }

    public string? ExtraNameSys { get; set; }

    public string? ExtraDescription { get; set; }

    public string? ExtraPhoto { get; set; }

    public int? ExtraPosition { get; set; }

    public DateTime? ExtraStartDate { get; set; }

    public DateTime? ExtraEndDate { get; set; }

    public bool? ExtraIsDeleted { get; set; }

    public string? ExtraPriceTypeNameSys { get; set; }

    public double? ExtraPrice { get; set; }

    public int ExtraId { get; set; }
}
