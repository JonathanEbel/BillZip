using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillZip.AuthModels;
using BillZip.Provider.JWT;
using Identity.Infrastructure.Repos;
using Microsoft.Extensions.Options;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/token")]
    [AllowAnonymous]

    public class TokenController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly AppSettingsSingleton _appSettings;

        public TokenController(IApplicationUserRepository applicationUserRepository, IOptions<AppSettingsSingleton> appSettings)
        {
            _applicationUserRepository = applicationUserRepository;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public IActionResult Create([FromBody]LoginDto loginDto)
        {
            if (!_applicationUserRepository.UserAuthenticates(loginDto.Username, loginDto.Password))
                return Unauthorized();

            var user = _applicationUserRepository.Get(loginDto.Username);
            user.UpdateLastLogin();   //we will count this as a login, so we need to update te last login date...
            _applicationUserRepository.Save();
            
            return Ok(UserJwtToken.GetToken(loginDto.Username, user.Claims, _appSettings.tokenExpirationInMinutes));
                
        }
    }

}