using System.Text.Json.Serialization;

namespace NileCapitalCruisesBEAPI.DTOs.Step_Order
{
    public class CLS_Order_Response
    {

        public int OrderId { get; set; }
        public string? OrderStatus { get; set; }
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }

        public string? CustomerPhone { get; set; }
        public string? NationalityName { get; set; }
        public string? CruiseNameSys { get; set; }
        public string? CabinNameSys { get; set; }
        public string? ItineraryNameSys { get; set; }
        public string? str_OperationDate { get; set; }
        public int? AdultsNo { get; set; }

        public int? ChildNo { get; set; }
        public double? OrderTotalPrice { get; set; }
        public string? OrderSpecialRequest { get; set; }

        public string? OrderConfirmationNumber { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Currency { get; set; }
        public string? Card { get; set; }
        public string? OrderReference { get; set; }
        public string? CardBrand { get; set; }
        public string? Str_OrderDate { get; set; }


        



    }
}
