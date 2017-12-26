/// <summary>
/// This namespace is very special...
/// Any class in this namespace will be a new policy added for Authorization... 
///  - The property "PolicyName" will be the name of the new policy
///  - The property "RequireClaim" will be the claim that the policy requires for Authorization
///  - The property "RequiredValues" is a list of values in which the claim value for the user must match at least one.
/// </summary>
namespace BillZip.Policies
{

    public static class Tenent
    {
        public const string PolicyName = "Tenent";
        public const string RequireClaim = "Tenent";
        public static string[] RequiredValues = { };
        
    }

    public static class Admin
    {
        public const string PolicyName = "Admin";
        public const string RequireClaim = "Admin";
        public static string[] RequiredValues = { };
        
    }

    public static class Landlord 
    {
        public const string PolicyName = "LandLord";
        public const string RequireClaim = "LandLord";
        public static string[] RequiredValues = { "Buffalo", "NYC" };
        
    }
    
}
