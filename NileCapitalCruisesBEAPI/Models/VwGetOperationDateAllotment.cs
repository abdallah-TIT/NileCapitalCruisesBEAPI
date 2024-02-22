using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class VwGetOperationDateAllotment
{
    public int? OperationDateId { get; set; }

    public DateTime? OperationDate { get; set; }

    public int? CountAllotment { get; set; }
}
