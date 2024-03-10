using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_GetExtras;
using NileCapitalCruisesBEAPI.Models;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/extra")]
    [ApiController]
    public class ExtraController : ControllerBase
    {
        private readonly NileCapitalCruisesBedbContext _dbContext;

        public ExtraController(NileCapitalCruisesBedbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("GetExtras")]

        public ActionResult GetExtras([FromQuery]string LangName, string CruiseUrl, int AdultNumber, int ChildNumber,int itineraryId)
        {
            var vwExtrasExtraPriceTypes = _dbContext.VwExtrasExtraPriceTypes.Where(vw => vw.CruiseUrl == CruiseUrl && vw.ExtraIsDeleted == false && DateTime.Now >= vw.ExtraStartDate && DateTime.Now <= vw.ExtraEndDate).OrderBy(x =>x.ExtraPosition).ToList();

            var Obj_GetExtras = vwExtrasExtraPriceTypes.Select(vw => {

                double? totalExtraPrice = 0;
                if(vw.ExtraPriceTypeNameSys == "Per Booking")
                {
                    totalExtraPrice = vw.ExtraPrice;
                }
                else if (vw.ExtraPriceTypeNameSys == "Per Cabin")
                {
                    var nights = _dbContext.VwRatesPrices.Where(x => x.ItineraryId == itineraryId).FirstOrDefault()?.DurationNights;
                    totalExtraPrice = vw.ExtraPrice * nights;
                }
                else if (vw.ExtraPriceTypeNameSys == "Per Person")
                {
                    totalExtraPrice = (vw.ExtraPrice * AdultNumber) + ((vw.ExtraPrice / 2 ) * ChildNumber);
                }
                return new CLS_GetExtras ()
                {
                    ExtraId = vw.ExtraId,
                    ExtraNameSys = vw.ExtraNameSys,
                    ExtraDescription = vw.ExtraDescription,
                    ExtraPhoto = vw.ExtraPhoto,
                    BasicExtraPrice = vw.ExtraPrice,
                    TotalExtraPrice = totalExtraPrice,
                };

            }).ToList();


            
            if (Obj_GetExtras.Count < 1)
                return NotFound();

            return Ok(Obj_GetExtras);
        }



    }
}
