using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblMasterRoomAmenity
{
    public int RoomAmenitiesId { get; set; }

    public int? RoomAmenitiesCategoryId { get; set; }

    public string? RoomAmenitiesSys { get; set; }

    public string? RoomAmenitiesIconFont { get; set; }

    public string? RoomAmenitiesPhoto { get; set; }

    public bool? RoomAmenitiesStatus { get; set; }

    public int? RoomAmenitiesPosition { get; set; }

    public int? RoomPhotoHieght { get; set; }

    public int? RoomPhotoWidth { get; set; }

    public virtual ICollection<TblMasterRoomAmenitiesContent> TblMasterRoomAmenitiesContents { get; set; } = new List<TblMasterRoomAmenitiesContent>();
}
