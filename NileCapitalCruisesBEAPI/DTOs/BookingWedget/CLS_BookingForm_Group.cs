namespace NileCapitalCruisesBEAPI.DTOs.BookingWedget
{
    public class CLS_BookingForm_Group
    {

        public int? DurationId { get; set; }


        public string? ItineraryNameSys { get; set; }
        public List<CLS_OperationDates> List_OperationDates { get; set; } 

    }
}
