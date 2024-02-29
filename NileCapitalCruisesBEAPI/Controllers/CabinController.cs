using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.Models;

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

        public ActionResult GetCabins([FromQuery] string LanguageCode, int ItineraryId, string Str_Date, int AdultNumber, int ChildNumber)
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
                                    && vw.OperationDateId == operationDateID).ToList();

            var vwCabinsPhotos = _dbContext.VwCabinsPhotos.ToList();
            var VwCabinsAmenities = _dbContext.VwCabinsAmenities.ToList();

            var Obj_GetCabins = new CLS_GetCabins()
            {
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
                List_Cabins = vwRatesPrices.Select(i => new CLS_Cabin()
                {
                    CabinId = i.CabinId,
                    CabinPhoto = _configuration["ImageURL"] + i.CabinPhoto,
                    RatePrice = i.RatePrice,
                    TotalPrice = (i.RatePrice * AdultNumber) + ((i.RatePrice * ChildNumber) / 2),
                    RateNameSys = i.RateNameSys,
                    CabinNameSys = i.CabinNameSys,
                    CabinDescription = i.CabinDescription,
                    CabinSize = i.CabinSize,
                    CabinBed = i.CabinBed,
                    CabinAllotment = i.CabinAllotment,
                    List_CabinPhotos = vwCabinsPhotos.Where(vw => vw.CabinId == i.CabinId).Select(y => new CLS_Cabin_Photos()
                    {
                        PhotoFileName = _configuration["ImageURL"] + y.PhotoFileName,

                    }).ToList(),
                    List_Cabin_Amenities = VwCabinsAmenities.Where(vw => vw.CabinId == i.CabinId).Select(y => new CLS_Cabin_Amenities()
                    {
                        AmenityNameSys =  y.RoomAmenitiesSys,
                        AmenityPhoto = _configuration["ImageURL"] + "icons/" +  y.RoomAmenitiesPhoto

                    }).ToList()
                }
                ).ToList(),
                
            };



            if (vwRatesPrices.Count < 1)
                return NotFound();

            return Ok(Obj_GetCabins);
        }



    }
}
