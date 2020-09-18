namespace NRewardBot.Config
{
    public interface IAllConfiguration : IConfiguration, ISeleniumConfiguration, ICredentials
    {

    }
    public interface IConfiguration
    {
        bool Headless { get; }
        bool Mobile { get; }
        bool Desktop { get; }
        bool Quiz { get; }
        bool Email { get; }
    }

    public interface ICredentials
    {
        string Username { get; }
        string Password { get; }
    }
}