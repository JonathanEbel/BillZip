using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BillZip.Controllers
{
    [Produces("application/json")]
    [Route("api/claims")]
    [Authorize(Policy = Policies.Admin.PolicyName)]
    public class ClaimsController: Controller
    {
        [HttpGet]
        public Dictionary<string, string[]> Get()
        {
            string authorizationPolicyNamespace = "BillZip.Policies";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == authorizationPolicyNamespace
                    select t;
            var list = q.ToList();

            var currentClaims = new Dictionary<string,string[]>();
             
            foreach (var policy in list)
            {
                // get all of the claims that are available from our policy class...
                var props = policy.GetFields();
                var claim = (string)props.Where(prop => prop.Name == "RequireClaim").First().GetRawConstantValue();
                if (!currentClaims.Any(x => x.Key == claim))
                    currentClaims.Add(claim, (string[])props.Where(prop => prop.Name == "RequiredValues").First().GetValue(null));
            }

            return currentClaims;
        }
    }
}
