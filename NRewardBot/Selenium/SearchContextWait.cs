using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NRewardBot.Selenium
{
    /// <summary>
    /// Allows wait conditions to be specified on a <see cref="ISearchContext"/>
    /// </summary>
    /// <remarks>
    /// Based on <see cref="WebDriverWait"/>
    /// </remarks>
    public class SearchContextWait : DefaultWait<ISearchContext>
    {
        private static readonly TimeSpan DefaultSleepTimeout = TimeSpan.FromMilliseconds(500.0);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.UI.WebDriverWait"/> class.
        /// 
        /// </summary>
        /// <param name="context">The WebDriver instance used to wait.</param><param name="timeout">The timeout value indicating how long to wait for the condition.</param>
        public SearchContextWait(ISearchContext context, TimeSpan timeout)
            : this(new SystemClock(), context, timeout, SearchContextWait.DefaultSleepTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OpenQA.Selenium.Support.UI.WebDriverWait"/> class.
        /// 
        /// </summary>
        /// <param name="clock">An object implementing the <see cref="T:OpenQA.Selenium.Support.UI.IClock"/> interface used to determine when time has passed.</param><param name="context">The WebDriver instance used to wait.</param><param name="timeout">The timeout value indicating how long to wait for the condition.</param><param name="sleepInterval">A <see cref="T:System.TimeSpan"/> value indicating how often to check for the condition to be true.</param>
        public SearchContextWait(IClock clock, ISearchContext context, TimeSpan timeout, TimeSpan sleepInterval)
            : base(context, clock)
        {
            this.Timeout = timeout;
            this.PollingInterval = sleepInterval;
            this.IgnoreExceptionTypes(new[] { typeof(NotFoundException) });
        }

    }
}
