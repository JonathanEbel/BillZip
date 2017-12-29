using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BillZip.StartupHelpers
{
    public static class PolicySetup
    {
        public static void BootstrapAuthorizationPolicies(IServiceCollection services)
        {
            string authorizationPolicyNamespace = "BillZip.Policies";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == authorizationPolicyNamespace
                    select t;
            var list = q.ToList();

            foreach (var policy in list)
            {
                var props = policy.GetFields();
                string policyName = "";
                string requiredClaim = "";
                string[] requiredValues = { };

                policyName = (string)props.Where(prop => prop.Name == "PolicyName").First().GetRawConstantValue();
                requiredClaim = (string)props.Where(prop => prop.Name == "RequireClaim").First().GetRawConstantValue();
                requiredValues = (string[])props.Where(prop => prop.Name == "RequiredValues").First().GetValue(null);
                services.AddAuthorization(options =>
                {
                    if (requiredValues.Length == 0)
                        options.AddPolicy(policyName,
                            p => p.RequireClaim(requiredClaim));
                    else
                        options.AddPolicy(policyName,
                            p => p.RequireClaim(requiredClaim, requiredValues));
                });
            }
        }
    }
}
