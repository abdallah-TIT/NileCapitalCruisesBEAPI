using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblOperationDatesAllotment
{
    public int? OperationDateId { get; set; }

    public int? CabinId { get; set; }

    public int? CabinAllotment { get; set; }

    public int OperationDateIdcabinId { get; set; }

    public virtual TblOperationDate? OperationDate { get; set; }
}
