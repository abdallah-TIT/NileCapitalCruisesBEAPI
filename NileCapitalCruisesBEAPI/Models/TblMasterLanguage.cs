using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblMasterLanguage
{
    public int LangId { get; set; }

    public string? LangName { get; set; }

    public string? LangAbbreviation { get; set; }
}
