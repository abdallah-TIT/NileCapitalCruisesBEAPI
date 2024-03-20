using System.ComponentModel.DataAnnotations;

namespace NileCapitalCruisesBEAPI.DTOs.Step_Authentication
{
    public class TokenRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
