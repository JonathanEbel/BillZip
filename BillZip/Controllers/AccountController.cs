using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using BillZip.Provider.JWT;
using System.Collections.Generic;
using Identity.Infrastructure.Repos;
using Microsoft.Extensions.Options;

namespace BillZip.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly AppSettingsSingleton _appSettings;

        public AccountController(IApplicationUserRepository applicationUserRepository, IOptions<AppSettingsSingleton> appSettings)
        {
            _applicationUserRepository = applicationUserRepository;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Dtos.NewUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //make sure these aren't hard coded.  
            //They need to be passed in from the client but I need to think about this first....
            var claims = new List<ApplicationUserClaim>(){
                    new ApplicationUserClaim {   
                        claimKey = Policies.Landlord.RequireClaim,
                        claimValue = Policies.Landlord.RequiredValues[0]
                    },
                    new ApplicationUserClaim {   
                        claimKey = Policies.Admin.RequireClaim,
                        claimValue = ""
                    }
            };

            //create our new User
            var user = new ApplicationUser(dto.UserName, dto.Password, dto.ConfirmPassword, claims);
            _applicationUserRepository.Add(user);
            _applicationUserRepository.Save();


            return Ok(UserJwtToken.GetToken(dto.UserName, user.Claims, _appSettings.tokenExpirationInMinutes));


        }
    }
}
