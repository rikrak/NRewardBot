using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRewardBot.Config;

namespace Tests.Unit.NRewardBot.Config;

[TestClass]
public class FileCredentialsProviderTests
{
    [TestMethod]
    public void HappyPath()
    {
        // arrange
        var target = new FileCredentialsProvider();

        // act
        var credentials = target.SetCredentials("user@domain.com", "u4S#788&rem84ki@@763", "key-phrase");
        var actualCredentials = target.GetCredentials("key-phrase");

        // assert
        credentials.Password.Should().NotBeNull();
        credentials.Username.Should().NotBeNull();
        credentials.Should().NotBeSameAs(actualCredentials);
        credentials.Should().BeEquivalentTo(actualCredentials);
    }

}