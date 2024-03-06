using System.Text.Json.Serialization;

namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCruises
{
    public class CLS_ItineraryDay
    {
        [JsonIgnore]
        public int ItineraryDayId { get; set; }
        public int? ItineraryId { get; set; }

        public string? DayName { get; set; }

        public IList<CLS_ItineraryDayEvent> ItineraryDayEvents { get; set;}
    }
}
