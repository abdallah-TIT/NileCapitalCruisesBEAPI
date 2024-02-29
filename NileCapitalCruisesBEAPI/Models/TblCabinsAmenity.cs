using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCabinsAmenity
{
    public int? AmenitiesId { get; set; }

    public int? CabinId { get; set; }

    public int CabinAmenitiesId { get; set; }
}
