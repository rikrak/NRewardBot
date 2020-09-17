using CommandLine;

namespace NRewardBot.Config
{
    internal class CommandOptions
    {
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
    }
}