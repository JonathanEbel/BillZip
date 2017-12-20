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

            //make sure these aren't hard coded....
            var claims = new List<ApplicationUserClaim>(){
                    new ApplicationUserClaim {   
                        //TODO: get these from a list of constants or enums.....
                        claimKey = "EmployeeNumber",
                        claimValue = "tenent_5656545"
                    },
                    new ApplicationUserClaim {   
                        //TODO: get these from a list of constants or enums.....
                        claimKey = "Role",
                        claimValue = "landlord"
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
