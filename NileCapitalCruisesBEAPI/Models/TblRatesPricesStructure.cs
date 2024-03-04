using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblRatesPricesStructure
{
    public int RatePriceStructureId { get; set; }

    public string? Description { get; set; }

    public double? AgeFrom { get; set; }

    public double? AgeTo { get; set; }

    public bool? IsPercentage { get; set; }

    public bool? IsPositiveSign { get; set; }

    public double? Value { get; set; }
}
