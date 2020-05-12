namespace UserMicroservice.Helpers
{
    public interface IRegexValidator
    {
        bool IsValidEmail(string email);
        bool IsValidPassword(string password);
    }
}
