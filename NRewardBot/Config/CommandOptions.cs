using CommandLine;

namespace NRewardBot.Config
{
    internal class CommandOptions
    {
        [Option('h', "headless", Default = false)]
        public bool Headless { get; set;  }

        [Option('m', "mobile", Default = false)]
        public bool Mobile   { get; set; }

        [Option('d', "desktop", Default = false)]
        public bool Desktop  { get; set; }

        [Option('q', "Quiz", Default = false)]
        public bool Quiz     { get; set; }

        [Option('e', "email", Default = false)]
        public bool Email    { get; set; }

        [Option('a', "all", Default = true)]
        public bool All      { get; set; }
    }
}