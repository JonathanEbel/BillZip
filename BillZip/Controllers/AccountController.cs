using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using BillZip.Provider.JWT;
using System.Collections.Generic;
using Identity.Infrastructure.Repos;

namespace BillZip.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public AccountController(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Dtos.NewUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = new List<ApplicationUserClaim>(){
                    new ApplicationUserClaim {   
                        //TODO: get these from a list of constants or enums.....
                        claimKey = "role",
                        claimValue = "tenent"
                    }
            };

            //create our new User
            var user = new ApplicationUser(dto.UserName, dto.Password, dto.ConfirmPassword, claims);
            _applicationUserRepository.Add(user);
            _applicationUserRepository.Save();
            
            
            var token = new JwtTokenBuilder()
                            .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                            .AddSubject("Jon Ebel")
                            .AddIssuer("BillZip.Security.Bearer")
                            .AddAudience("BillZip.Security.Bearer")
                            .AddClaim("EmployeeNumber", "5656545")
                            .AddClaim("ThisOne", "78676576")
                            .AddExpiry(500)
                            .Build();

            return Ok(token.Value);
            
            
        }
    }
}
