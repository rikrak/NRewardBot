namespace NRewardBot.Config
{
    public interface ISeleniumConfiguration
    {
        string SeleniumUrl { get; }
        string DriverLocation { get; }
    }
}