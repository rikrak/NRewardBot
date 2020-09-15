using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRewardBot.SearchTerms.GoogleTrends;

namespace Tests.Unit.NRewardBot.SearchTerms.GoogleTrends
{
    [TestClass]
    public class SearchTermProviderTests
    {
        [TestMethod]
        public async Task ShouldGetTerms()
        {
            // arrange
            var target = new SearchTermProvider();

            // act
            var actual = await target.GetTerms();

            // assert
            actual.Should().NotBeEmpty();
        }

    }
}
