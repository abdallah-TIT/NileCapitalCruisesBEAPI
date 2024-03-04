using NileCapitalCruisesBEAPI.Models;

namespace NileCapitalCruisesBEAPI.DTOs.Step_GetCabins
{
    public class CLS_RatePriceStructure
    {
        //public double? Infant { get; set; }
        //public double? Child6 { get; set; }
        //public double? Child12 { get; set; }
        //public double? SingleAdult { get; set; }

        public TblRatesPricesStructure? Infant { get; set; }
        public TblRatesPricesStructure? Child6 { get; set; }
        public TblRatesPricesStructure? Child12 { get; set; }
        public TblRatesPricesStructure? SingleAdult { get; set; }
    }
}
