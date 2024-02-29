using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwCabinsAmenity
{
    public int CabinAmenitiesId { get; set; }

    public int? AmenitiesId { get; set; }

    public int? CabinId { get; set; }

    public string? CabinNameSys { get; set; }

    public string? RoomAmenitiesSys { get; set; }

    public string? RoomAmenitiesPhoto { get; set; }
}
