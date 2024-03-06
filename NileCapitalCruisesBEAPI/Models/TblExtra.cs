using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblExtra
{
    public int ExtraId { get; set; }

    public string? ExtraNameSys { get; set; }

    public string? ExtraDescription { get; set; }

    public string? ExtraPhoto { get; set; }

    public int? ExtraPosition { get; set; }

    public bool? ExtraStatus { get; set; }

    public DateTime? ExtraStartDate { get; set; }

    public DateTime? ExtraEndDate { get; set; }

    public bool? ExtraIsDeleted { get; set; }

    public int? ExtraPriceTypeId { get; set; }

    public double? ExtraPrice { get; set; }
}
