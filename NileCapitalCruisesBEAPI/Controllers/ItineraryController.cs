using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.Models;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/itinerary")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;

        public ItineraryController(NileCapitalCruisesBedbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("getItineraries/{LangName}/{CruiseUrl}")]

        public ActionResult GetItineraries(string LangName, string CruiseUrl)
        {
            var items = _dbContext.VwCruisesItineraries.Where(vw => vw.LangAbbreviation == LangName && vw.CruiseUrl == CruiseUrl).ToList();

            if (items.Count < 1)
                return NotFound();

            return Ok(items);
        }



    }
}
