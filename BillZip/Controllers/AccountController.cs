using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using BillZip.Provider.JWT;
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
            
            //create our new User
            var user = new ApplicationUser(dto.UserName, dto.Password, dto.ConfirmPassword);
            //TODO: make sure these claims aren't hard coded.  They need to be passed in from the client
            user.AddClaim(Policies.Landlord.RequireClaim, Policies.Landlord.RequiredValues[0]);
            user.AddClaim(Policies.Admin.RequireClaim, "");
            _applicationUserRepository.Add(user);
            _applicationUserRepository.Save();


            return Ok(UserJwtToken.GetToken(dto.UserName, user.Claims, _appSettings.tokenExpirationInMinutes));
        }
    }
}
