namespace NRewardBot.Config
{
    public interface IAllConfiguration : IConfiguration, ISeleniumConfiguration
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
}