using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillZip.AuthModels;
using BillZip.Provider.JWT;
using System.Collections.Generic;
using Identity.Infrastructure.Repos;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]

    public class TokenController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public TokenController(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody]LoginInputModel inputModel)
        {
            
            if (!_applicationUserRepository.UserAuthenticates(inputModel.Username, inputModel.Password))
                return Unauthorized();

            //claims will need to be a list and dynamic...
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject("Jon Ebel")
                                .AddIssuer("BillZip.Security.Bearer")
                                .AddAudience("BillZip.Security.Bearer")
                                .AddClaim("EmployeeNumber", "5656545")
                                .AddClaim("ThisOne","78676576")
                                .AddExpiry(500)
                                .Build();


            return Ok(token.Value);
                
        }
    }

}