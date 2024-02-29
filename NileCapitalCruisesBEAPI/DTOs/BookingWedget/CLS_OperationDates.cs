using System.Text.Json.Serialization;

namespace NileCapitalCruisesBEAPI.DTOs.BookingWedget
{
    public class CLS_OperationDates
    {
        public int OperationDateId { get; set; }

        [JsonIgnore]
        public DateTime? OperationDate { get; set; }

        [JsonIgnore]
        public int? OperationDateDay { get; set; }

        [JsonIgnore]
        public int? OperationDateMonth { get; set; }

        [JsonIgnore]
        public int? OperationDateYear { get; set; }

        public string str_OperationDate { get; set; }
        
    }
}
