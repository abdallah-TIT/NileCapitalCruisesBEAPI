using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.Models;

namespace NileCapitalCruisesBEAPI.Helpers
{
    public static class RatePriceStructure
    {
        
        public static CLS_RatePriceStructure GetRatePriceStructures(NileCapitalCruisesBedbContext _dbContext)
        {
            var ratesPricesStructures = _dbContext.TblRatesPricesStructures.ToList();
            var infant = ratesPricesStructures.FirstOrDefault(r => r.Description == "Infant");
            var child6 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child6");
            var child12 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child12");
            var singleAdult = ratesPricesStructures.FirstOrDefault(r => r.Description == "SingleAdult");

            return new CLS_RatePriceStructure()
            {
                Infant = infant,
                Child6 = child6,
                Child12 = child12,
                SingleAdult = singleAdult,

            };
        }

        public static double? CalculatePrice(TblRatesPricesStructure ratesPrice, double? price)
        {
            return (ratesPrice.IsPercentage ?? false) ?
                        ((ratesPrice.IsPositiveSign ?? false) ? price + ((ratesPrice.Value / 100) * price) : price - ((ratesPrice.Value / 100) * price)) :
                        ((ratesPrice.IsPositiveSign ?? false) ? price + ratesPrice.Value : price - ratesPrice.Value);
        }
    }
}
