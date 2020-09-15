using System;
using CommandLine;

namespace NRewardBot.Config
{
    public class ConfigurationFactory
    {
        public ConfigurationFactory()
        {
            
        }

        public IConfiguration GetConfiguration(string[] arguments)
        {
            Configuration configuration = new Configuration();

            CommandLine.Parser.Default
                .ParseArguments<CommandOptions>(arguments)
                .WithParsed(o =>
                {
                    if (o.All)
                    {
                        configuration.All(true);
                    }
                    else
                    {
                        configuration.Email = o.Email;
                        configuration.Desktop = o.Desktop;
                        configuration.Headless = o.Headless;
                        configuration.Mobile = o.Mobile;
                        configuration.Quiz = o.Quiz;
                    }
                })
                .WithNotParsed(err =>
                {
                    Environment.Exit(2);
                });

            return configuration;
        }
    }
}