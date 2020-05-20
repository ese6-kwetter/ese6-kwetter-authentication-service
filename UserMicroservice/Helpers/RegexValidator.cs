using System.Text.RegularExpressions;

namespace UserMicroservice.Helpers
{
    public class RegexValidator : IRegexValidator
    {
        private const string EmailPattern =
        @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        + "@"
        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
        /*
         * ^: first line
         * (?=.*[a-z]) : Should have at least one lower case
         * (?=.*[A-Z]) : Should have at least one upper case
         * (?=.*\d) : Should have at least one number
         * (?=.*[#$^+=!*()@%&] ) : Should have at least one special character
         * .{8,} : Minimum of 8 characters
         * $ : end line
         */
        private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$";
        
        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, EmailPattern);
        }

        public bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, PasswordPattern);
        }
    }
}
