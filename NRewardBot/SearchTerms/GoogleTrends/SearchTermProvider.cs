using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NRewardBot.SearchTerms.GoogleTrends
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// derived from ms_rewards.py => get_search_terms()
    /// </remarks>
    public class SearchTermProvider
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private static readonly Random Randomiser = new Random(DateTime.Now.Millisecond);

        public async Task<IEnumerable<string>> GetTerms()
        {
            var terms = new List<string>();

            Action<string> addTerm = (string term) =>
            {
                term = term.ToLower();
                if (!terms.Contains(term, StringComparer.InvariantCultureIgnoreCase))
                {
                    terms.Add(term);
                }
            };

            using (var client = new HttpClient())
            {
                foreach (var date in GetDates())
                {
                    Log.Info("Getting search terms from Google Trends for {date}", date.ToShortDateString());
                    var url = $"https://trends.google.com/trends/api/dailytrends?hl=en-US&ed={date:yyyyMMdd}&geo=GB&ns=15";
                    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                    var trendsRaw = await client.GetStringAsync(url);
                    // for some reason the results are always prefixed with ")]}'," which makes the json invalid!
                    trendsRaw = trendsRaw.Substring(")]}',".Length);

                    var trends = JsonConvert.DeserializeObject<GoogleTrends>(trendsRaw);
                    var trendingSearchesDay = trends?.Default?.TrendingSearchesDays.FirstOrDefault();
                    if (trendingSearchesDay != null)
                    {
                        var searches = trendingSearchesDay.TrendingSearches;
                        foreach (var search in searches)
                        {
                            var term = search.Title.Query;
                            addTerm(term);

                            foreach (var relatedQuery in search.RelatedQueries)
                            {
                                addTerm(relatedQuery.Query);
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(Randomiser.Next(3, 5)));

                }
            }

            return terms;
        }

        /// <summary>
        /// Returns a list of dates from today to a 20 days ago
        /// </summary>
        /// <remarks>Derived from ms_rewards.py => get_dates(days_to_get=4)</remarks>
        /// <param name="numberOfDates"># of days to get from api</param>
        /// <returns>list of dates</returns>
        public IEnumerable<DateTime> GetDates(int numberOfDates = 4)
        {
            var offsets = new List<int>();
            while (offsets.Count < numberOfDates)
            {
                var candidate = Randomiser.Next(0, 20);
                if (!offsets.Contains(candidate))
                {
                    offsets.Add(-candidate);
                }
            }
            for (int i = 0; i < numberOfDates; i++)
            {
                yield return DateTime.Today.AddDays(offsets[i]);
            }
        }
    }
}
