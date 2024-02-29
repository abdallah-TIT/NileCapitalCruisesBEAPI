using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCruises;
using NileCapitalCruisesBEAPI.Models;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/cruise")]
    [ApiController]
    public class CruiseController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;
        private readonly IConfiguration _configuration ;

        public CruiseController(NileCapitalCruisesBedbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }




        [HttpGet("getCruises")]

        public ActionResult GetCruises([FromQuery] string LanguageCode, int DurationId, string Str_Date, int AdultNumber, int ChildNumber)
        {
            string format = "yyyy,MM,dd"; // Specify the format
            DateTime date = DateTime.ParseExact(Str_Date, format, null); // Parse the string to DateTime

            var vwCruisesItinerariesOperationDates = _dbContext.VwCruisesItinerariesOperationDates
                                .Where(vw => vw.DurationId == DurationId
                                      && vw.OperationDate == date
                                      && vw.CountAllotment > 0).ToList();


            var Obj_GetCruises = vwCruisesItinerariesOperationDates.Select(i => new CLS_GetCruises()
            {
                ItineraryId = i.ItineraryId,
                CruiseId = i.CruiseId,
                ItineraryNameSys = i.ItineraryNameSys,
                WeekDayName = i.WeekDayName,
                CruiseNameSys = i.CruiseNameSys,
                CruiseUrl = i.CruiseUrl,
                CruisePhoto = _configuration["ImageURL"] + i.CruisePhoto,
                CruiseBanner = _configuration["ImageURL"] + i.CruiseBanner,
                str_OperationDate = $"{i.OperationDateYear:D4},{i.OperationDateMonth:D2},{i.OperationDateDay:D2}",
                AdultNumber = AdultNumber,
                ChildNumber = ChildNumber,
            }).ToList();



            if (vwCruisesItinerariesOperationDates.Count < 1)
                return NotFound();

            return Ok(Obj_GetCruises);
        }



    }
}
