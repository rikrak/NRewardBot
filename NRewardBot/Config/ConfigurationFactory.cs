using System;
using System.Text;
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
            var attachDebugger = false;

            CommandLine.Parser.Default
                .ParseArguments<CommandOptions>(arguments)
                .WithParsed(o =>
                {
                    attachDebugger = o.Debug;

                    if (string.IsNullOrWhiteSpace(o.Username))
                    {
                        Console.WriteLine();
                        Console.Write("Please enter you username:  ");
                        configuration.Username = Console.ReadLine();
                        Console.WriteLine();
                    }
                    else
                    {
                        configuration.Username = o.Username;
                    }

                    if (string.IsNullOrWhiteSpace(o.Password))
                    {
                        Console.WriteLine();
                        configuration.Password = ReadPassword();
                        Console.WriteLine();
                    }
                    else
                    {
                        configuration.Password = o.Password;
                    }

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

            if (attachDebugger)
            {
                System.Diagnostics.Debugger.Launch();
            }
            return configuration;
        }

        private static string ReadPassword()
        {
            Console.Write("Please enter your password: ");
            ConsoleKeyInfo keyInfo;
            var password = new StringBuilder();
            do
            {
                keyInfo = Console.ReadKey(true);
                // Skip if Backspace or Enter is Pressed
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        // Remove last character if Backspace is Pressed
                        password.Remove(password.Length - 1, 1);
                        Console.Write("X");
                    }
                }
            }
            // Stops Getting Password Once Enter is Pressed
            while (keyInfo.Key != ConsoleKey.Enter);

            return password.ToString();
        }
    }
}