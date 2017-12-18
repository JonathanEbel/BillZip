using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillZip.AuthModels;
using BillZip.Provider.JWT;
using Microsoft.AspNetCore.Identity;
using Identity.Models;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]

    public class TokenController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> CreateAsync([FromBody]LoginInputModel inputModel)
        {
            //TODO: set this up to pull from the database....
            //if (inputModel.Username != "jon" && inputModel.Password != "password")
            //    return Unauthorized();

            var result = await _signInManager.PasswordSignInAsync(inputModel.Username, inputModel.Password, true, false);
            if (!result.Succeeded)
                return Unauthorized();

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject("Jon Ebel")
                                .AddIssuer("Test.Security.Bearer")
                                .AddAudience("Test.Security.Bearer")
                                .AddClaim("role", "tenent")
                                .AddExpiry(5)
                                .Build();

            //return Ok(token);
            return Ok(token.Value);
        }
    }

}