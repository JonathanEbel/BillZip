using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/info")]
    [Authorize]

    public class InfoController : Controller
    {
        /// <summary>
        /// This will return the logged in user based upon their bearer token...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var dict = new Dictionary<string, string>();
            HttpContext.User.Claims.ToList().ForEach(item => dict.Add(item.Type, item.Value));

            return Ok(dict);
        }

        

    }
}