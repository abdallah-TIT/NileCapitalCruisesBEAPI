using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NileCapitalCruisesBEAPI.DTOs.BookingWedget;
using NileCapitalCruisesBEAPI.DTOs.Step_GetCabins;
using NileCapitalCruisesBEAPI.DTOs.Step_GetNationalities;
using NileCapitalCruisesBEAPI.Models;

namespace BookingNile.API.Controllers.CMS
{
    [Route("api/nationality")]
    [ApiController]
    public class NationalityController : ControllerBase
    {
        private readonly NileCapitalCruisesBeopdbContext _dbContextOp;

        public NationalityController(NileCapitalCruisesBeopdbContext dbContextOp)
        {
            _dbContextOp = dbContextOp;
        }




        [HttpGet("getnatioNalities")]

        public ActionResult GetNalities()
        {


            var vwRatesPrices = _dbContextOp.TblCountriesNationalities
                                .ToList();

            if (vwRatesPrices.Count < 1)
                return NotFound();

            var Obj_GetNationalities = vwRatesPrices.Select(i => new CLS_GetNationalities()
            {
                NationalityId = i.NationalityId,
                NationalityName = i.NationalityName,
                IsVisa = i.IsVisa
            }).ToList();





            return Ok(Obj_GetNationalities);
        }



    }
}
