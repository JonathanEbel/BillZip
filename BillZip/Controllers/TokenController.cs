using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillZip.AuthModels;
using BillZip.Provider.JWT;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]

    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody]LoginInputModel inputModel)
        {
            //TODO: set this up to pull from the database....
            if (inputModel.Username != "jon" && inputModel.Password != "password")
                return Unauthorized();

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject("Jon Ebel")
                                .AddIssuer("Test.Security.Bearer")
                                .AddAudience("Test.Security.Bearer")
                                .AddClaim("EmployeeNumber", "6109")
                                .AddExpiry(5)
                                .Build();

            //return Ok(token);
            return Ok(token.Value);
        }
    }

}