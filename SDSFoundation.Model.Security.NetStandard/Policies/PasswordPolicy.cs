using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SDSFoundation.Model.Security.Policies
{
    public class PasswordPolicy
    {
        /// <summary>
        /// Checks if the password created is valid
        /// </summary>
        /// <param name="includeLowercase">Bool to say if lowercase are required</param>
        /// <param name="includeUppercase">Bool to say if uppercase are required</param>
        /// <param name="includeNumeric">Bool to say if numerics are required</param>
        /// <param name="includeSpecial">Bool to say if special characters are required</param>
        /// <param name="minimumLength">Minimum length of a password</param>
        /// <param name="password">Generated password</param>
        /// <returns>True or False to say if the password is valid or not</returns>
        public static bool IsValid(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, int minimumLength, string password)
        {
            const string REGEX_LOWERCASE = @"[a-z]";
            const string REGEX_UPPERCASE = @"[A-Z]";
            const string REGEX_NUMERIC = @"[\d]";
            const string REGEX_SPECIAL = @"([!#$%&*@\\])+";

            bool lowerCaseIsValid = !includeLowercase || (includeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
            bool upperCaseIsValid = !includeUppercase || (includeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
            bool numericIsValid = !includeNumeric || (includeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
            bool symbolsAreValid = !includeSpecial || (includeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));

            //Validate length
            if(string.IsNullOrWhiteSpace(password) || password.Length < minimumLength)
            {
                return false;
            }

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid;
        }


 
    }
}
