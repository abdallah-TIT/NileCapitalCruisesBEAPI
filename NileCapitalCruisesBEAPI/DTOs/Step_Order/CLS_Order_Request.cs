namespace NileCapitalCruisesBEAPI.DTOs.Step_Order
{
    public class CLS_Order_Request
    {
       


       
        public int? AdultsNo { get; set; }

        public int? ChildNo { get; set; }

       

        public string? str_OperationDate { get; set; }

        public int? CabinId { get; set; }

        public int? ItineraryId { get; set; }

        public string? CustomerFirstName { get; set; }

        public string? CustomerLastName { get; set; }

        public string? CustomerEmail { get; set; }

        public string? CustomerPhone { get; set; }

        public int? CustomerNationalityId { get; set; }

        public string? OrderSpecialRequest { get; set; }

        public double? PriceAdultRate { get; set; }
    }
}
