using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblMasterRoomAmenitiesContent
{
    public int RoomAmenitiesContentId { get; set; }

    public int? RoomAmenitiesId { get; set; }

    public int? LangId { get; set; }

    public string? RoomAmenitiesName { get; set; }

    public virtual TblMasterRoomAmenity? RoomAmenities { get; set; }
}
