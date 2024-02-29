using System.Text.Json.Serialization;

namespace NileCapitalCruisesBEAPI.DTOs.Step_Order
{
    public class CLS_Order 
    {


        public string? OrderConfirmationNumber { get; set; }

        public double? OrderTotalPrice { get; set; }

        public int? AdultsNo { get; set; }

        public int? ChildNo { get; set; }
        [JsonIgnore]
        public int? OperationDateId { get; set; }
        public string? str_OperationDate { get; set; }
        [JsonIgnore]
        public int? CabinId { get; set; }
        public string? CabinNameSys { get; set; }
        [JsonIgnore]
        public int? ItineraryId { get; set; }
        public string? ItineraryNameSys { get; set; }
        public string? CustomerFirstName { get; set; }

        public string? CustomerLastName { get; set; }

        public string? CustomerEmail { get; set; }

        public string? CustomerPhone { get; set; }

        




    }
}
