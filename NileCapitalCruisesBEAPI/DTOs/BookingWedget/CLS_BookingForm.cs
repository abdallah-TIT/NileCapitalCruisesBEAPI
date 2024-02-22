namespace NileCapitalCruisesBEAPI.DTOs.BookingWedget
{
    public class CLS_BookingForm
    {

        public int ItineraryId { get; set; }


        public string? ItineraryNameSys { get; set; }
        public List<CLS_OperationDates> List_OperationDates { get; set; } 

    }
}
