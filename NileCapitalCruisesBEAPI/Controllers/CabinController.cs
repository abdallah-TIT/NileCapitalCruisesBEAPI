using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.Models;
using System.Linq;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/cabin")]
    [ApiController]
    public class CabinController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;
        private readonly IConfiguration _configuration ;

        public CabinController(NileCapitalCruisesBedbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
       

        [HttpGet("getCabins")]

        public ActionResult GetCabins([FromQuery] string LanguageCode, int ItineraryId, string Str_Date, int AdultNumber, int ChildNumber, float ChildAge1 = 0, float ChildAge2 = 0)
        {
            if(ChildNumber == 1 && ChildAge1 > 0 && ChildAge2 > 0)
            {
                return BadRequest("The ChildNumber = 1 but ChildAge1 and ChildAge2 > 0, The right is ChildAge1 or ChildAge2 > 0");
            }

            if (ChildNumber > 2)
            {
                return BadRequest("The ChildNumber must be 1 or 2");
            }

            //if (ChildNumber == 2 && (ChildAge1 == 0 || ChildAge2 == 0))
            //{
            //    return BadRequest("The ChildNumber = 2 and ChildAge1 or ChildAge2 = 0");
            //}
            string format = "yyyy,MM,dd"; // Specify the format
            DateTime date = DateTime.ParseExact(Str_Date, format, null); // Parse the string to DateTime

            var operationDateID = _dbContext.TblOperationDates.Where(op => op.OperationDate == date).FirstOrDefault()?.OperationDateId;

            var vwRatesPrices = _dbContext.VwRatesPrices
                                .Where(vw => vw.ItineraryId == ItineraryId
                                    && vw.CabinMaxAdults >= AdultNumber
                                    && vw.CabinMaxChild >= ChildNumber
                                    && vw.CabinMaxOccupancy >= AdultNumber + ChildNumber
                                    && vw.DateStart <= date
                                    && vw.DateEnd >= date
                                    && vw.OperationDateId == operationDateID).ToList();

            var vwCabinsPhotos = _dbContext.VwCabinsPhotos.ToList();
            var VwCabinsAmenities = _dbContext.VwCabinsAmenities.ToList();
            var getRatePriceStructures = GetRatePriceStructures();
            var Obj_GetCabins = new CLS_GetCabins()
            {
                AdultNumber = AdultNumber,
                ChildNumber = ChildNumber,
                ChildAge1 = ChildAge1,
                ChildAge2 = ChildAge2,
                ItineraryId = vwRatesPrices.FirstOrDefault()?.ItineraryId,
                OperationDateId = vwRatesPrices.FirstOrDefault()?.OperationDateId,
                ItineraryNameSys = vwRatesPrices.FirstOrDefault()?.ItineraryNameSys,
                WeekDayName = vwRatesPrices.FirstOrDefault()?.WeekDayName,
                DestinationFrom = vwRatesPrices.FirstOrDefault()?.DestinationFrom,
                DestinationTo = vwRatesPrices.FirstOrDefault()?.DestinationTo,
                DurationName = vwRatesPrices.FirstOrDefault()?.DurationName,
                DurationDays = vwRatesPrices.FirstOrDefault()?.DurationDays,
                DurationNights = vwRatesPrices.FirstOrDefault()?.DurationNights,
                ItineraryStatus = vwRatesPrices.FirstOrDefault()?.ItineraryStatus,
                CruiseNameSys = vwRatesPrices.FirstOrDefault()?.CruiseNameSys,
                CruiseUrl = vwRatesPrices.FirstOrDefault()?.CruiseUrl,
                CruisePhoto = _configuration["ImageURL"] + vwRatesPrices.FirstOrDefault()?.CruisePhoto,
                CruiseBanner = _configuration["ImageURL"] + vwRatesPrices.FirstOrDefault()?.CruiseBanner,

                List_Cabins = vwRatesPrices/*.Where(vw =>vw.OperationDateId == operationDateID && vw.CabinAllotment > 0)*/.Select(i => {

                    // Calculate Price_Adult_Basic
                    double? priceAdultBasic = AdultNumber == 1 ? CalculatePrice(getRatePriceStructures.SingleAdult, i.RatePrice): i.RatePrice;


                    double? priceInfant = (ChildAge1 >= 0 && ChildAge1 <= 1.99) || (ChildAge2 >= 0 && ChildAge2 <= 1.99) ? CalculatePrice(getRatePriceStructures.Infant, priceAdultBasic) : 0;
                    double? priceChild6 = (ChildAge1 >= 2 && ChildAge1 <= 5.99) || (ChildAge2 >= 2 && ChildAge2 <= 5.99) ? CalculatePrice(getRatePriceStructures.Child6, priceAdultBasic) : 0;
                    double? priceChild12 = (ChildAge1 >= 6 && ChildAge1 <= 11.99) || (ChildAge2 >= 6 && ChildAge2 <= 11.99) ? CalculatePrice(getRatePriceStructures.Child12, priceAdultBasic) : 0;

                    double? priceChildren = (ChildNumber > 0 ? (ChildAge1 == ChildAge2 ?
                                                  ChildNumber * priceInfant + ChildNumber * priceChild6 + ChildNumber * priceChild12 :
                                                  priceInfant + priceChild6 + priceChild12) : 0);

                    double? priceAdults = (AdultNumber == 1 ? priceAdultBasic : AdultNumber * priceAdultBasic);

                    double? netPriceNightTotal = priceAdults + priceChildren;


                    double? netPriceTotal = netPriceNightTotal * vwRatesPrices.FirstOrDefault()?.DurationNights;

                    //float? netTotalPrice = (AdultNumber == 1 ? priceAdultBasic : AdultNumber * priceAdultBasic) + infantPrice + child6Price + child12Price;

                    // Calculate Taxes

                    CLS_PriceTaxes.CalculatePriceTaxes(netPriceTotal);
                    double? amountService = CLS_PriceTaxes.AmountService;
                    double? amountVat = CLS_PriceTaxes.AmountVat;
                    double? amountCityTax = CLS_PriceTaxes.AmountCityTax;
                    double? totalPriceAfterTax = CLS_PriceTaxes.TotalPriceAfterTax;

                    // Create a new CLS_Cabin object and set its properties
                    return new CLS_Cabin
                    {
                        CabinId = i.CabinId,
                        CabinPhoto = _configuration["ImageURL"] + i.CabinPhoto,

                        PriceAdultBasic = priceAdultBasic,
                        PriceAdults = priceAdults,
                        PriceChildren = priceChildren,
                        NetPriceNightTotal = netPriceNightTotal,
                        NetPriceTotal = netPriceTotal,

                        AmountService = amountService,
                        AmountVat = amountVat,
                        AmountCityTax = amountCityTax,
                        TotalPriceAfterTax = totalPriceAfterTax,

                        RateNameSys = i.RateNameSys,
                        CabinNameSys = i.CabinNameSys,
                        CabinDescription = i.CabinDescription,
                        CabinSize = i.CabinSize,
                        CabinBed = i.CabinBed,
                        CabinAllotment = i.CabinAllotment,
                        List_CabinPhotos = vwCabinsPhotos.Where(vw => vw.CabinId == i.CabinId)
                                                        .Select(y => new CLS_Cabin_Photos
                                                        {
                                                            PhotoFileName = _configuration["ImageURL"] + y.PhotoFileName
                                                        }).ToList(),
                        List_Cabin_Amenities = VwCabinsAmenities.Where(vw => vw.CabinId == i.CabinId)
                                                                .Select(y => new CLS_Cabin_Amenities
                                                                {
                                                                    AmenityNameSys = y.RoomAmenitiesSys,
                                                                    AmenityPhoto = _configuration["ImageURL"] + "icons/" + y.RoomAmenitiesPhoto
                                                                }).ToList()
                    };
                }).ToList()

            //List_Cabins = vwRatesPrices.Select(i => new CLS_Cabin()
            //{

            //    CabinId = i.CabinId,
            //    CabinPhoto = _configuration["ImageURL"] + i.CabinPhoto,

            //    CabinPrice = AdultNumber == 1 ? GetRatePriceStructures(i.RatePrice).SingleAdult : i.RatePrice,

            //    DoublePrice = AdultNumber > 1 ? i.RatePrice : 0,
            //    InfantPrice = ChildAge >= 0 && ChildAge <= 1.99 ?GetRatePriceStructures(i.CabinPrice).Infant : 0,
            //    Child6Price = ChildAge >= 2 && ChildAge <= 5.99 ? GetRatePriceStructures(i.RatePrice).Child6 : 0,
            //    Child12Price = ChildAge >= 6 && ChildAge <= 11.99 ? GetRatePriceStructures(i.RatePrice).Child12 : 0,
            //    SingleAdultPrice = AdultNumber == 1 ? GetRatePriceStructures(i.RatePrice).SingleAdult : 0,
            //    TotalPrice = 0,
            //    //TotalPrice = DoublePrice * AdultNumber + InfantPrice * ChildNumber + Child6Price * ChildNumber + Child12Price * ChildNumber + SingleAdultPrice * ChildNumber, 

            //    //TotalPrice = (AdultNumber == 0 ? GetRatePriceStructures(i.RatePrice).SingleAdult : i.RatePrice * AdultNumber) + 
            //    //              (ChildNumber > 0 ? () : 0 )  

            //    //TotalPrice = (i.RatePrice * AdultNumber) + ((i.RatePrice * ChildNumber) / 2),
            //    RateNameSys = i.RateNameSys,
            //    CabinNameSys = i.CabinNameSys,
            //    CabinDescription = i.CabinDescription,
            //    CabinSize = i.CabinSize,
            //    CabinBed = i.CabinBed,
            //    CabinAllotment = i.CabinAllotment,
            //    List_CabinPhotos = vwCabinsPhotos.Where(vw => vw.CabinId == i.CabinId).Select(y => new CLS_Cabin_Photos()
            //    {
            //        PhotoFileName = _configuration["ImageURL"] + y.PhotoFileName,

            //    }).ToList(),
            //    List_Cabin_Amenities = VwCabinsAmenities.Where(vw => vw.CabinId == i.CabinId).Select(y => new CLS_Cabin_Amenities()
            //    {
            //        AmenityNameSys =  y.RoomAmenitiesSys,
            //        AmenityPhoto = _configuration["ImageURL"] + "icons/" +  y.RoomAmenitiesPhoto

            //    }).ToList()
            //}
            //).ToList(),

            };

            //Obj_GetCabins.List_Cabins.ForEach(i => i.TotalPrice  = i.DoublePrice * AdultNumber + i.InfantPrice * ChildNumber + i.Child6Price * ChildNumber + i.Child12Price * ChildNumber + i.SingleAdultPrice);

            if (vwRatesPrices.Count < 1)
                return NotFound();

            return Ok(Obj_GetCabins);
        }



        private CLS_RatePriceStructure GetRatePriceStructures()
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

        private double? CalculatePrice(TblRatesPricesStructure ratesPrice, double? price)
        {
            return (ratesPrice.IsPercentage ?? false) ?
                        ((ratesPrice.IsPositiveSign ?? false) ? price + ((ratesPrice.Value / 100) * price) : price - ((ratesPrice.Value / 100) * price)) :
                        ((ratesPrice.IsPositiveSign ?? false) ? price + ratesPrice.Value : price - ratesPrice.Value);
        }


        
        //private CLS_PriceTaxes CalculatePriceTaxe(float netTotalPrice)
        //{
        //    float totalPriceAfterTax = 0;
        //    float amount_Service = 0;
        //    float amount_Vat = 0;
        //    float amount_CityTax = 0;

        //    var prec_var_services = 12;
        //    var prec_var_vat = 14;
        //    var prec_var_city = 1;

        //    amount_Service = netTotalPrice * (prec_var_services / 100);

        //    amount_Vat = (netTotalPrice + amount_Service) * (prec_var_vat / 100);

        //    amount_CityTax = netTotalPrice * (prec_var_city / 100);

        //    totalPriceAfterTax = netTotalPrice + amount_Service + amount_Vat + amount_CityTax;

        //    return new CLS_PriceTaxes()
        //    {

        //    };
        //}


        //private CLS_RatePriceStructure GetRatePriceStructures(double? price)
        //{
        //    var ratesPricesStructures = _dbContext.TblRatesPricesStructures.ToList();
        //    var infant = ratesPricesStructures.FirstOrDefault(r => r.Description == "Infant");
        //    var child6 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child6");
        //    var child12 = ratesPricesStructures.FirstOrDefault(r => r.Description == "Child12");
        //    var singleAdult = ratesPricesStructures.FirstOrDefault(r => r.Description == "SingleAdult");

        //    return new CLS_RatePriceStructure()
        //    {
        //        Infant = (infant.IsPercentage ?? false) ?
        //                ((infant.IsPositiveSign ?? false) ? price + ((infant.Value / 100) * price) : price - ((infant.Value / 100) * price)) :
        //                ((infant.IsPositiveSign ?? false) ? price + infant.Value : price - infant.Value),

        //        Child6 = (child6.IsPercentage ?? false) ?
        //                ((child6.IsPositiveSign ?? false) ? price + ((child6.Value / 100) * price) : price - ((child6.Value / 100) * price)) :
        //                ((child6.IsPositiveSign ?? false) ? price + child6.Value : price - child6.Value),

        //        Child12 = (child12.IsPercentage ?? false) ?
        //                ((child12.IsPositiveSign ?? false) ? price + ((child12.Value / 100) * price) : price - ((child12.Value / 100) * price)) :
        //                ((child12.IsPositiveSign ?? false) ? price + child12.Value : price - child12.Value),

        //        SingleAdult = (singleAdult.IsPercentage ?? false) ?
        //                ((singleAdult.IsPositiveSign ?? false) ? price + ((singleAdult.Value / 100) * price) : price - ((singleAdult.Value / 100) * price)) :
        //                ((singleAdult.IsPositiveSign ?? false) ? price + singleAdult.Value : price - singleAdult.Value)

        //    };
        //}




    }
}
//float? priceInfant = (ChildAge1 >= 0 && ChildAge1 <= 1.99) || (ChildAge2 >= 0 && ChildAge2 <= 1.99) ? CalculatePrice(getRatePriceStructures.Infant, priceAdultBasic) : 0;
//float? child6Price = (ChildAge1 >= 2 && ChildAge1 <= 5.99) || (ChildAge2 >= 2 && ChildAge2 <= 5.99) ? CalculatePrice(getRatePriceStructures.Child6, priceAdultBasic) : 0;
//float? child12Price = (ChildAge1 >= 6 && ChildAge1 <= 11.99) || (ChildAge2 >= 6 && ChildAge2 <= 11.99) ? CalculatePrice(getRatePriceStructures.Child12, priceAdultBasic) : 0;
//float? netTotalPrice = (AdultNumber == 1 ? priceAdultBasic : AdultNumber * priceAdultBasic) + infantPrice + child6Price + child12Price;





// try

//double? priceAdults = 0;
//double? priceChildren = 0;
//double? priceInfant = CalculatePrice(getRatePriceStructures.Infant, priceAdultBasic);
//double? priceChild6 = CalculatePrice(getRatePriceStructures.Child6, priceAdultBasic);
//double? priceChild12 = CalculatePrice(getRatePriceStructures.Child12, priceAdultBasic);

//if (AdultNumber == 1)
//{
//    priceAdults = priceAdultBasic;
//}
//else
//{
//    priceAdults = priceAdultBasic * AdultNumber;
//}

//if (ChildNumber > 0)
//{
//    if (ChildAge1 == ChildAge2)
//    {

//    }
//}

////