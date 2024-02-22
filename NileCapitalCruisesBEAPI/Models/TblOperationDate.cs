using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblOperationDate
{
    public int OperationDateId { get; set; }

    public int? CruiseId { get; set; }

    public DateTime? OperationDate { get; set; }

    public int? OperationDateDay { get; set; }

    public int? OperationDateMonth { get; set; }

    public int? OperationDateYear { get; set; }

    public int? OperationDateWeekDayId { get; set; }

    public virtual ICollection<TblOperationDatesAllotment> TblOperationDatesAllotments { get; set; } = new List<TblOperationDatesAllotment>();
}
