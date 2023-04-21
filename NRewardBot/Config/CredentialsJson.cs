#nullable enable
namespace NRewardBot.Config
{
    internal class CredentialsJson : ICredentials
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}