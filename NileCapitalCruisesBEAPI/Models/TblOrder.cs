using System;
using System.Collections.Generic;

namespace NileCapitalCruisesBEAPI.Models;

public partial class TblOrder
{
    public int OrderId { get; set; }

    public string? OrderConfirmationNumber { get; set; }

    public double? OrderTotalPrice { get; set; }

    public int? AdultsNo { get; set; }

    public int? ChildNo { get; set; }

    public DateTime? OrderDate { get; set; } = DateTime.Now;

    public int? OperationDateId { get; set; }

    public int? CabinId { get; set; }

    public int? ItineraryId { get; set; }

    public string? CustomerFirstName { get; set; }

    public string? CustomerLastName { get; set; }

    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }

    public int? CustomerNationalityId { get; set; }

    public string? OrderSpecialRequest { get; set; }

    public double? PriceAdultRate { get; set; }

    public int? CurrencyId { get; set; }

    public double? PriceChildRate { get; set; }

    public int? OrderStatusId { get; set; }

    public string? PmpaymentStatus { get; set; }

    public string? PmcardDataToken { get; set; }

    public string? PmmaskedCard { get; set; }

    public string? PmmerchantOrderId { get; set; }

    public string? PmorderId { get; set; }

    public string? PmcardBrand { get; set; }

    public string? PmorderReference { get; set; }

    public string? PmtransactionId { get; set; }

    public string? Pmcurrency { get; set; }

    public string? Pmmode { get; set; }

    public string? Pmsignature { get; set; }
}
