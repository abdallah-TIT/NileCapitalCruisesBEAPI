using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblItinerariesContent
{
    public int ItineraryContentId { get; set; }

    public int? ItineraryId { get; set; }

    public int? LangId { get; set; }

    public string? ItineraryName { get; set; }
}
