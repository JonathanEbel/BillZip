using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Models;
using BillZip.Provider.JWT;

namespace BillZip.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Dtos.NewUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.UserName,
                claims = {
                    new ApplicationUserClaim {   
                        //TODO get these from a list of constants.....
                        claimKey = "role",
                        claimValue = "tenent"
                    }
                }
            };
            


            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
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
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return StatusCode(500, ModelState);  //return the errors..
            }

            return StatusCode(201);
        }
    }
}
