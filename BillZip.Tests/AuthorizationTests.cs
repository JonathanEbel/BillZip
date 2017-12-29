using System.Linq;
using System.Reflection;
using Xunit;

namespace BillZip.Tests
{
    public class AuthorizationTests
    {
        [Fact]
        public void IsPolicyStructureProper()
        {
            Assembly asm = Assembly.LoadFrom(@"BillZip.dll");
            string authorizationPolicyNamespace = "BillZip.Policies";

            var q = from t in asm.GetTypes()
                    where t.IsClass && t.Namespace == authorizationPolicyNamespace
                    select t;
            var list = q.ToList();

            foreach (var policy in list)
            {
                var props = policy.GetFields();
                
                Assert.NotNull(props.Where(prop => prop.Name == "PolicyName").FirstOrDefault());
                Assert.NotNull(props.Where(prop => prop.Name == "RequireClaim").FirstOrDefault());
                Assert.NotNull(props.Where(prop => prop.Name == "RequiredValues").FirstOrDefault());
            }
        }
            
    }
}
