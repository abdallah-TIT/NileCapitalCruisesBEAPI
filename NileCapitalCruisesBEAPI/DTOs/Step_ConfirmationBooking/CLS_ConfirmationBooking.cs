using NileCapitalCruisesBEAPI.DTOs.Step_GetExtras;

namespace NileCapitalCruisesBEAPI.DTOs.Step_ConfirmationBooking
{
    public class CLS_ConfirmationBooking
    {
        public int? AdultNumber { get; set; }
        public int? ChildNumber { get; set; }
        public float ChildAge1 { get; set; }
        public float ChildAge2 { get; set; }
        public int? OperationDateId { get; set; }
        public int? ItineraryId { get; set; }

        public string? ItineraryNameSys { get; set; }


        public string? WeekDayName { get; set; }


        public string? DestinationFrom { get; set; }

        public string? DestinationTo { get; set; }

        public string? DurationName { get; set; }

        public int? DurationDays { get; set; }

        public int? DurationNights { get; set; }



        public bool? ItineraryStatus { get; set; }

        public string? CruiseNameSys { get; set; }
        public string? CruiseBanner { get; set; }
        public string? CruiseUrl { get; set; }
        public string? CruisePhoto { get; set; }

        public int? CabinId { get; set; }

        public double? PriceAdultBasic { get; set; }
       
        public double? NetPriceNightTotal { get; set; }

        public double? TotalExtras { get; set; }
        public double? NetPriceTotal { get; set; }
        public double? AmountService { get; set; }
        public double? AmountVat { get; set; }
        public double? AmountCityTax { get; set; }
        public double? TotalPriceAfterTax { get; set; }





        public string? CabinNameSys { get; set; }
        public string? CabinPhoto { get; set; }
        public string? CabinDescription { get; set; }
        public string? CabinSize { get; set; }

        public string? CabinBed { get; set; }


        public List<CLS_GetExtras> List_Extras { get; set; }
    }
}
