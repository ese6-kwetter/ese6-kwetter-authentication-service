using NUnit.Framework;
using UserMicroservice.Helpers;

namespace UserMicroserviceTests.Helpers
{
    [TestFixture]
    public class RegexValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            _regexValidator = new RegexValidator();
        }

        private IRegexValidator _regexValidator;

        [TestCaseSource(nameof(_validEmails))]
        public void IsValidEmail_ValidEmail_ReturnsTrue(string email)
        {
            Assert.IsTrue(_regexValidator.IsValidEmail(email));
        }

        [TestCaseSource(nameof(_invalidEmails))]
        public void IsValidEmail_InvalidEmail_ReturnsFalse(string email)
        {
            Assert.IsFalse(_regexValidator.IsValidEmail(email));
        }

        private static string[] _validEmails =
        {
            "joe@home.org",
            "joe@joebob.name",
            "joe&bob@bob.com",
            "~joe@bob.com",
            "joe$@bob.com",
            "joe+bob@bob.com",
            "o'reilly@there.com",
            "joe@home.com",
            "joe.bob@home.com",
            "joe@his.home.com",
            "a@abc.org",
            "a@abc-xyz.org",
            "a@192.168.0.1",
            "a@10.1.100.1"
        };

        private static string[] _invalidEmails =
        {
            "joe", // should fail
            "joe@home", // should fail
            "a@b.c", // should fail because .c is only one character but must be 2-4 characters
            "joe-bob[at]home.com", // should fail because [at] is not valid
            "joe@his.home.place", // should fail because place is 5 characters but must be 2-4 characters
            "joe.@bob.com", // should fail because there is a dot at the end of the local-part
            ".joe@bob.com", // should fail because there is a dot at the beginning of the local-part
            "john..doe@bob.com", // should fail because there are two dots in the local-part
            "john.doe@bob..com", // should fail because there are two dots in the domain
            "joe<>bob@bob.com", // should fail because <> are not valid
            "joe@his.home.com.", // should fail because it can't end with a period
            //"john.doe@bob-.com", // should fail because there is a dash at the start of a domain part
            //"john.doe@-bob.com", // should fail because there is a dash at the end of a domain part
            "a@10.1.100.1a", // Should fail because of the extra character
            "joe<>bob@bob.com\n", // should fail because it end with \n
            "joe<>bob@bob.com\r" // should fail because it ends with \r
        };
    }
}
