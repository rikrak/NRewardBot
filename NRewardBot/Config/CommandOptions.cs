using CommandLine;

namespace NRewardBot.Config
{
    internal class CommandOptions
    {
        [Option('u', "username", Required = false)]
        public string Username { get; set; }

        [Option('p', "password", Required = false)]
        public string Password { get; set; }

        [Option('n', "pin", Required = false)]
        public string Pin { get; set; }

        [Option('h', "headless")]
        public bool? Headless { get; set;  }

        [Option('m', "mobile")]
        public bool? Mobile   { get; set; }

        [Option('d', "desktop")]
        public bool? Desktop  { get; set; }

        [Option('q', "Quiz")]
        public bool? Quiz     { get; set; }

        [Option('e', "email")]
        public bool? Email    { get; set; }

        [Option('a', "all")]
        public bool? All      { get; set; }

        [Option("debug", Default = false, Required = false)]
        public bool Debug { get; set; }
    }
}