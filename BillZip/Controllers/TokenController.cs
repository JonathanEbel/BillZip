using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BillZip.AuthModels;
using BillZip.Provider.JWT;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using System.Collections.Generic;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]

    public class TokenController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenController(/*SignInManager<ApplicationUser> signInManager*/)
        {
           // _signInManager = signInManager;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> CreateAsync([FromBody]LoginInputModel inputModel)
        {
            
            //var result = await _signInManager.PasswordSignInAsync(inputModel.Username, inputModel.Password, true, false);
            //if (!result.Succeeded)
            //    return Unauthorized();
            
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