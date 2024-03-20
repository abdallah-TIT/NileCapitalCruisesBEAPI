using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NileCapitalCruisesBEAPI.Models
{
    public class NileCapitalCruisesUsersdbContext : IdentityDbContext<ApplicationUser>
    {
        public NileCapitalCruisesUsersdbContext(DbContextOptions<NileCapitalCruisesUsersdbContext> options) : base(options)
        {
        }
       
    }
}
