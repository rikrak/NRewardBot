using System;
using CommandLine;

namespace NRewardBot.Config
{
    public class ConfigurationFactory
    {
        public ConfigurationFactory()
        {

        }

        public IAllConfiguration GetConfiguration(string[] arguments)
        {
            Configuration configuration = new Configuration();

            var baseConfig = BaseConfiguration.Bootstrap();
            baseConfig.ApplyTo(configuration);


            CommandLine.Parser.Default
                .ParseArguments<CommandOptions>(arguments)
                .WithParsed(o =>
                {
                    configuration.Username = o.Username;
                    configuration.Password = o.Password;

                    if (o.All.HasValue && o.All.Value)
                    {
                        configuration.All(true);
                    }
                    else
                    {
                        if (o.Email.HasValue)
                        {
                            configuration.Email = o.Email.Value;
                        }

                        if (o.Desktop.HasValue)
                        {
                            configuration.Desktop = o.Desktop.Value;
                        }

                        if (o.Headless.HasValue)
                        {
                            configuration.Headless = o.Headless.Value;
                        }

                        if (o.Mobile.HasValue)
                        {
                            configuration.Mobile = o.Mobile.Value;
                        }

                        if (o.Quiz.HasValue)
                        {
                            configuration.Quiz = o.Quiz.Value;
                        }
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