using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCountry
{
    public string? CountryName { get; set; }

    public string? CountryCode { get; set; }

    public int CountryId { get; set; }

    public string? CountryNameAr { get; set; }

    public bool? CountryStatus { get; set; }

    public bool? IsVisa { get; set; }
}
