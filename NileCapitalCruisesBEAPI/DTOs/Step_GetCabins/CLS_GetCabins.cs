using NileCapitalCruisesBEAPI.DTOs.BookingWedget;

namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCabins
{
    public class CLS_GetCabins
    {


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

        public string? CruiseUrl { get; set; }

        

        public string? CruisePhoto { get; set; }
        public string? CruiseBanner { get; set; }
        public List<CLS_Cabin> List_Cabins { get; set; }

    }
}
