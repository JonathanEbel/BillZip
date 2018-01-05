using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using BillZip.Provider.JWT;
using Identity.Infrastructure.Repos;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
            
            if (dto.Claims != null && dto.Claims.Count > 0)
            {
                foreach (var claim in dto.Claims)
                {
                    user.AddClaim(claim.Key, claim.Value);
                }
            }

            _applicationUserRepository.Add(user);
            _applicationUserRepository.Save();


            return Ok(UserJwtToken.GetToken(dto.UserName, user.Claims, _appSettings.tokenExpirationInMinutes));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin.PolicyName)]
        public IActionResult Delete(Guid id)
        {
            var user = _applicationUserRepository.GetById(id);
            if (user == null)
                return BadRequest();

            _applicationUserRepository.Delete(user);
            _applicationUserRepository.Save();

            return Ok();
        }

        [HttpPatch]
        [Authorize(Policy = Policies.Admin.PolicyName)]
        public IActionResult Patch([FromBody] Dtos.PatchUserDto dto)
        {
            var user = _applicationUserRepository.Get(dto.Id);
            if (user == null)
                return BadRequest();

            if (dto.UserName.Trim() != user.UserName.Trim())
                user.OverwriteUserName(dto.UserName);

            if (dto.Claims != null && dto.Claims.Count > 0)
            {
                user.ClearClaims();
                foreach (var claim in dto.Claims)
                {
                    user.AddClaim(claim.Key, claim.Value);
                }
            }
            _applicationUserRepository.Save();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public Dtos.UserResponseDto GetUser(string userName)
        {
            var user = _applicationUserRepository.Get(userName);
            if (user == null)
                return null;

            var userResponseDto = new Dtos.UserResponseDto {
                Id = user.Id,
                UserName = user.UserName,
                LastLogin = user.LastLogin,
                DateCreated = user.DateCreated
            };
            userResponseDto.Claims = new Dictionary<string, string>();
            foreach (var userClaim in user.Claims)
                userResponseDto.Claims.Add(userClaim.claimKey, userClaim.claimValue);
            return userResponseDto;
        }

    }
}
