namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCruises
{
    public class CLS_GetCruises
    {
        public int ItineraryId { get; set; }

        public int? CruiseId { get; set; }

        public string? ItineraryNameSys { get; set; }

        public string? str_OperationDate { get; set; }

        public string? WeekDayName { get; set; }

        public string? CruiseNameSys { get; set; }

        public string? CruiseUrl { get; set; }

        public string? CruisePhoto { get; set; }

        public string? CruiseBanner { get; set; }

        public string? CruiseDescription { get; set; }
        public int AdultNumber { get; set; }
        public int ChildNumber { get; set; }
        public double? PriceAdultBasic { get; set; }


        public IList<CLS_ItineraryDay> ItineraryDays { get; set; }
    }
}
