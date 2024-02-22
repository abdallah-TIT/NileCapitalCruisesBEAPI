using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.Models;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/bookingWedget")]
    [ApiController]
    public class CruiseBookingWedgetController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;

        public CruiseBookingWedgetController(NileCapitalCruisesBedbContext dbContext)
        {
            _dbContext = dbContext;
        }




        [HttpGet("getBookingForm/{LangName}/{CruiseUrl}")]
        
        public  ActionResult GetBookingForm(string LangName, string CruiseUrl)
        {
            var vwCruisesItineraries =  _dbContext.VwCruisesItineraries.Where(vw => vw.LangAbbreviation == LangName && vw.CruiseUrl == CruiseUrl).ToList();
            var vwCruisesItinerariesOperationDate = _dbContext.VwCruisesItinerariesOperationDates.Where(ve => ve.CountAllotment > 0).ToList();

            var Obj_bookingForm = vwCruisesItineraries.Select(i => new CLS_BookingForm()
            {
                ItineraryId = i.ItineraryId,
                ItineraryNameSys = i.ItineraryNameSys,
                List_OperationDates = vwCruisesItinerariesOperationDate.Where(vw => vw.ItineraryId ==  i.ItineraryId).Select(y => new CLS_OperationDates()
                {
                    OperationDate = y.OperationDate,
                    OperationDateDay = y.OperationDateDay,
                    OperationDateMonth = y.OperationDateMonth,
                    OperationDateYear = y.OperationDateYear,
                    str_OperationDate = $"{y.OperationDateYear:D4},{y.OperationDateMonth:D2},{y.OperationDateDay:D2}"
                }).ToList()
            }).ToList(); 
            


            if (vwCruisesItineraries.Count < 1 )
                return NotFound();

            return Ok(Obj_bookingForm);
        }



    }
}
