namespace NRewardBot.Config
{
    internal class Configuration : IAllConfiguration
    {
        public bool Headless { get; set; }
        public bool Mobile { get; set; }
        public bool Desktop { get; set; }
        public bool Quiz { get; set; }
        public bool Email { get; set; }

        public string SeleniumUrl { get; set; }
        public string DriverLocation { get; set; }

        public void All(bool value)
        {
            this.Desktop = value;
            this.Email = value;
            this.Headless = value;
            this.Mobile = value;
            this.Quiz = value;
        }

    }
}
