namespace UserService.Helpers
{
    public interface IHashGenerator
    {
        byte[] Salt();
        byte[] Hash(string plainText, byte[] salt);
        bool Verify(string plainText, byte[] salt, byte[] hash);
    }
}
