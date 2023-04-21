using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRewardBot.Config;

namespace Tests.Unit.NRewardBot.Config;

[TestClass]
public class AesSecurityTests
{
    [TestMethod]
    public void StringEncryptionShouldDoRoundTrip()
    {
        // arrange
        var plainText = "{\"username\":\"my@Username.com\", \"password\":\"Abcd3fgh!jklMN09qrstu\"}";

        // act
        var encryptedText = plainText.Encrypt(key: "hello");
        var actualPlainText = encryptedText.Decrypt(key: "hello");

        // assert
        using (new AssertionScope())
        {
            encryptedText.Should().NotBe(plainText);
            actualPlainText.Should().Be(plainText);
        }
    }

}