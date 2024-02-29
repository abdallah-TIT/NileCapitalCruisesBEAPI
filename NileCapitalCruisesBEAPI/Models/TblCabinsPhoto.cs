using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblCabinsPhoto
{
    public int? CabinId { get; set; }

    public string? PhotoFileName { get; set; }

    public bool? PhotoStatus { get; set; }

    public int? PhotoPosition { get; set; }

    public int PhotoId { get; set; }
}
