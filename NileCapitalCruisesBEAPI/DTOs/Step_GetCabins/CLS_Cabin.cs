using System.Text.Json.Serialization;

namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCabins
{
    public class CLS_Cabin
    {

        public int? CabinId { get; set; }

        
        
        public double? PriceAdultBasic { get; set; }
        public double? PriceChildren { get; set; }
        public double? PriceAdults { get; set; }
        public double? NetPriceNightTotal { get; set; }
        public double? NetPriceTotal { get; set; }
        public double? AmountService { get; set; }
        public double? AmountVat { get; set; }
        public double? AmountCityTax { get; set; }
        public double? TotalPriceAfterTax { get; set; }




        public string? RateNameSys { get; set; }
        public string? CabinNameSys { get; set; }
        public string? CabinPhoto { get; set; }
        public string? CabinDescription { get; set; }
        public string? CabinSize { get; set; }

        public string? CabinBed { get; set; }

        public int? CabinAllotment { get; set; }
        public List<CLS_Cabin_Photos> List_CabinPhotos { get; set; }
        public List<CLS_Cabin_Amenities> List_Cabin_Amenities { get; set; }
    }
}
