using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCountriesNationality
{
    public string? NationalityName { get; set; }

    public int NationalityId { get; set; }

    public bool? IsVisa { get; set; }
}
