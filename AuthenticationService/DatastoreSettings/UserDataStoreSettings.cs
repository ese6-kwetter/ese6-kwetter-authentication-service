namespace AuthenticationService.DataStoreSettings
{
    public class UserDataStoreSettings : IUserDataStoreSettings
    {
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUserDataStoreSettings
    {
        string UserCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
