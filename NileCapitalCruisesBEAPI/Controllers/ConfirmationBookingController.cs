using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_ConfirmationBooking;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_GetExtras;
using NileCapitalCruisesBEAPI.Helpers;
using NileCapitalCruisesBEAPI.Models;
using System.Linq;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/confirmationBooking")]
    [ApiController]
    public class ConfirmationBookingController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;
        private readonly IConfiguration _configuration ;

        public ConfirmationBookingController(NileCapitalCruisesBedbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
       

        [HttpGet("confirmationBooking")]

        public ActionResult ConfirmationBooking([FromQuery] string LanguageCode, int ItineraryId, int cabinId, string Str_Date, int AdultNumber, int ChildNumber, [FromQuery] List<int>? extras, float ChildAge1 = 0, float ChildAge2 = 0)
        {
            
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
                                    && vw.OperationDateId == operationDateID
                                    && vw.CabinId == cabinId).FirstOrDefault();
            //var cruiseUrl = vwRatesPrices?.CruiseUrl;
            var vwExtrasExtraPriceTypes = _dbContext.VwExtrasExtraPriceTypes.Where(vw => extras.Contains(vw.ExtraId)).OrderBy(x => x.ExtraPosition).ToList();

            var getRatePriceStructures = RatePriceStructure.GetRatePriceStructures(_dbContext);

            // Calculate Price_Adult_Basic
            double? priceAdultBasic = AdultNumber == 1 ? RatePriceStructure.CalculatePrice(getRatePriceStructures.SingleAdult, vwRatesPrices?.RatePrice) : vwRatesPrices?.RatePrice;


            double? priceInfant = (ChildAge1 >= 0 && ChildAge1 <= 1.99) || (ChildAge2 >= 0 && ChildAge2 <= 1.99) ? RatePriceStructure.CalculatePrice(getRatePriceStructures.Infant, priceAdultBasic) : 0;
            double? priceChild6 = (ChildAge1 >= 2 && ChildAge1 <= 5.99) || (ChildAge2 >= 2 && ChildAge2 <= 5.99) ? RatePriceStructure.CalculatePrice(getRatePriceStructures.Child6, priceAdultBasic) : 0;
            double? priceChild12 = (ChildAge1 >= 6 && ChildAge1 <= 11.99) || (ChildAge2 >= 6 && ChildAge2 <= 11.99) ? RatePriceStructure.CalculatePrice(getRatePriceStructures.Child12, priceAdultBasic) : 0;

            double? priceChildren = (ChildNumber > 0 ? (ChildAge1 == ChildAge2 ?
                                          ChildNumber * priceInfant + ChildNumber * priceChild6 + ChildNumber * priceChild12 :
                                          priceInfant + priceChild6 + priceChild12) : 0);

            double? priceAdults = (AdultNumber == 1 ? priceAdultBasic : AdultNumber * priceAdultBasic);

            double? netPriceNightTotal = priceAdults + priceChildren;


            double? netPriceTotal = netPriceNightTotal * vwRatesPrices?.DurationNights;


            // Calculate total extras
            double? totalExtras = 0;

            var Obj_ConfirmationBooking = new CLS_ConfirmationBooking()
            {
                
                AdultNumber = AdultNumber,
                ChildNumber = ChildNumber,
                ChildAge1 = ChildAge1,
                ChildAge2 = ChildAge2,
                ItineraryId = vwRatesPrices?.ItineraryId,
                OperationDateId = vwRatesPrices?.OperationDateId,
                ItineraryNameSys = vwRatesPrices?.ItineraryNameSys,
                WeekDayName = vwRatesPrices?.WeekDayName,
                DestinationFrom = vwRatesPrices?.DestinationFrom,
                DestinationTo = vwRatesPrices?.DestinationTo,
                DurationName = vwRatesPrices?.DurationName,
                DurationDays = vwRatesPrices?.DurationDays,
                DurationNights = vwRatesPrices?.DurationNights,
                ItineraryStatus = vwRatesPrices?.ItineraryStatus,
                CruiseNameSys = vwRatesPrices?.CruiseNameSys,
                CruiseUrl = vwRatesPrices?.CruiseUrl,
                CruisePhoto = _configuration["ImageURL"] + vwRatesPrices?.CruisePhoto,
                CruiseBanner = _configuration["ImageURL"] + vwRatesPrices?.CruiseBanner,
                CabinId = cabinId,
                CabinNameSys = vwRatesPrices?.CabinNameSys,
                CabinPhoto = _configuration["ImageURL"] + vwRatesPrices?.CabinPhoto,
                CabinDescription = vwRatesPrices?.CabinDescription,
                CabinSize = vwRatesPrices?.CabinSize,
                CabinBed = vwRatesPrices?.CabinBed,

                List_Extras = vwExtrasExtraPriceTypes.Select(i => {

                    double? totalExtraPrice = 0;
                    if (i.ExtraPriceTypeNameSys == "Per Booking")
                    {
                        totalExtraPrice = i.ExtraPrice;
                    }
                    else if (i.ExtraPriceTypeNameSys == "Per Cabin")
                    {
                        var nights = vwRatesPrices?.DurationNights;
                        totalExtraPrice = i.ExtraPrice * nights;
                    }
                    else if (i.ExtraPriceTypeNameSys == "Per Person")
                    {
                        totalExtraPrice = (i.ExtraPrice * AdultNumber) + ((i.ExtraPrice / 2) * ChildNumber);
                    }

                    totalExtras += totalExtraPrice;

                    return new CLS_GetExtras
                    {
                        ExtraId = i.ExtraId,
                        ExtraNameSys = i.ExtraNameSys,
                        ExtraDescription = i.ExtraDescription,
                        ExtraPhoto = i.ExtraPhoto,
                        BasicExtraPrice = i.ExtraPrice,
                        TotalExtraPrice = totalExtraPrice,
                    };

                }).ToList(),

                PriceAdultBasic = priceAdultBasic,
                NetPriceNightTotal = netPriceNightTotal,
                TotalExtras = totalExtras,

                NetPriceTotal = netPriceTotal + totalExtras,

            };

            // Calculate Taxes

            CLS_PriceTaxes.CalculatePriceTaxes(netPriceTotal + totalExtras);
            double? amountService = CLS_PriceTaxes.AmountService;
            double? amountVat = CLS_PriceTaxes.AmountVat;
            double? amountCityTax = CLS_PriceTaxes.AmountCityTax;
            double? totalPriceAfterTax = CLS_PriceTaxes.TotalPriceAfterTax;

            Obj_ConfirmationBooking.AmountService = amountService;
            Obj_ConfirmationBooking.AmountVat = amountVat;
            Obj_ConfirmationBooking.AmountCityTax = amountCityTax;
            Obj_ConfirmationBooking.TotalPriceAfterTax = totalPriceAfterTax;

            if (vwRatesPrices == null)
                return NotFound();

            return Ok(Obj_ConfirmationBooking);
        }





    }
}
 