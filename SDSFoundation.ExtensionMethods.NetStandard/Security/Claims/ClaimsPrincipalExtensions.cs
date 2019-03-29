using System.Linq;
using System.Security.Claims;

namespace SDSFoundation.ExtensionMethods.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {


        public static bool HasClaim(this ClaimsPrincipal principal, string name, string value)
        {
            return ValidateClaim(principal, name, value);
        }

        /// <summary>
        /// Validate a claim value based on an enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="principal"></param>
        /// <param name="enumVal"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasClaim<TEnum>(this ClaimsPrincipal principal, TEnum enumVal, string value) where TEnum : System.Enum
        {
            return ValidateClaim(principal, enumVal.ToString(), value);
        }


        private static bool ValidateClaim(ClaimsPrincipal principal, string name, string value)
        {
            try
            {
                //Find a simple claim key value pair
                if (principal.HasClaim(name, value))
                {
                    return true;
                }

                //If we didn't find the claim, attempt to parse the claim values
                var matchingClaim = principal.Claims.ToList().Where(x => x.Type == name).FirstOrDefault();
                //split the value by delimiter
                if (matchingClaim != null)
                {
                    if (!string.IsNullOrWhiteSpace(matchingClaim.Value))
                    {
                        //split the value by delimiter.  If it isn't delimited that isn't a problem - just take the first
                        var splitValues = matchingClaim.Value.Split(',').ToList();
                        foreach (var claimVal in splitValues)
                        {
                            if (claimVal.Trim() == value.Trim())
                            {
                                return true;
                            }
                        }

                    }


                }
            }
            catch (System.Exception)
            {
                return false;
            }


            return false;
        }


    }
}
