using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCruises;
using NileCapitalCruisesBEAPI.Models;
using System.ComponentModel;
using System.Linq;

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
            //var itinerariesIds = vwCruisesItinerariesOperationDates.Select(x =>x.ItineraryId).ToList();
            var itineraryDays = _dbContext.ItinerariesDays.ToList();
            var itineraryDayEvents = _dbContext.ItinerariesDaysEvents.ToList();
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
                CruiseDescription = i.CruiseDescription,
                str_OperationDate = date.ToString("dddd dd, MMMM , yyyy"),
                AdultNumber = AdultNumber,
                ChildNumber = ChildNumber,
                ItineraryDays = itineraryDays.Where(id => id.ItineraryId == i.ItineraryId).Select(id => new CLS_ItineraryDay()
                {
                    ItineraryDayId = id.ItineraryDayId,
                    ItineraryId = id.ItineraryId,
                    DayName = id.DayName,
                    ItineraryDayEvents = itineraryDayEvents.Where(ide => ide.ItineraryDayId == id.ItineraryDayId).Select(ide => new CLS_ItineraryDayEvent()
                    {
                        EventDescription = ide.EventDescription
                    }).ToList()
                }).ToList(),
            }).ToList();



            if (vwCruisesItinerariesOperationDates.Count < 1)
                return NotFound();

            return Ok(Obj_GetCruises);
        }



    }
}
