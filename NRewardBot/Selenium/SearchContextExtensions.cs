using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium
{
    public static class SearchContextExtensions
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        private static readonly TimeSpan StandardTimeout = TimeSpan.FromSeconds(1);

        public static bool ElementExists(this ISearchContext context, By by)
        {
            try
            {
                var exists = context.FindElements(by).Count > 0;
                return exists;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        public static IWebElement WaitUntilElementIsDisplayed(this ISearchContext context, By elementLocator, bool throwOnTimeout = true, TimeSpan? timeout = null)
        {
            timeout = timeout ?? StandardTimeout;
            Log.Debug($"Waiting for {elementLocator}");
            var isDisplayed = context.WaitUntil(d =>
            {
                try
                {
                    var elementToCheck = context.FindElement(elementLocator);
                    var displayed = elementToCheck.Displayed;
                    Log.Debug($"Element {elementLocator} is {(displayed ? "" : "not ")}displayed");
                    if (!displayed)
                    {
                        Log.Debug($"  Enabled:     {elementToCheck.Enabled}");
                        Log.Debug($"  Location:    {elementToCheck.Location}");
                        Log.Debug($"  Size:        {elementToCheck.Size}");
                        Log.Debug($"  Text:        {elementToCheck.Text}");
                        Log.Debug($"  Style:       {elementToCheck.GetAttribute("style")}");
                        Log.Debug($"  Class:       {elementToCheck.GetAttribute("class")}");
                        Log.Debug($"  css-display: {elementToCheck.GetCssValue("display")}");
                    }
                    return displayed;
                }
                catch (StaleElementReferenceException)
                {
                    Log.Debug($"Element {elementLocator} is Stale");
                    return false;
                }
                catch (NoSuchElementException)
                {
                    Log.Debug($"Element {elementLocator} cannot be found");
                    return false;
                }
            }, throwOnTimeout, timeout);

            return isDisplayed
                ? context.FindElement(elementLocator)
                : null;
        }

        /// <summary>
        /// Waits until a given element exists in the DOM, or until the timeout has been exhausted.
        /// </summary>
        /// <remarks>
        /// This method does not require the element to be visible, just that it exists in the DOM
        /// </remarks>
        /// <param name="context"></param>
        /// <param name="elementLocator"></param>
        /// <param name="throwOnTimeout"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementIsAvailable(this ISearchContext context, By elementLocator, bool throwOnTimeout = true, TimeSpan? timeout = null)
        {
            timeout = timeout ?? StandardTimeout;
            Log.Debug($"Waiting for {elementLocator}");
            var isAvailable = context.WaitUntil(d =>
            {
                try
                {
                    var elementToCheck = context.FindElement(elementLocator);
                    Log.Debug($"Element {elementLocator} is available");
                    return elementToCheck != null;
                }
                catch (StaleElementReferenceException)
                {
                    Log.Debug($"Element {elementLocator} is Stale");
                    return false;
                }
                catch (NoSuchElementException)
                {
                    Log.Debug($"Element {elementLocator} cannot be found");
                    return false;
                }
            }, throwOnTimeout, timeout);

            return isAvailable
                ? context.FindElement(elementLocator)
                : null;
        }

        public static TResult WaitUntil<TResult>(this ISearchContext context, Func<ISearchContext, TResult> condition, bool throwOnTimeout = true, TimeSpan? timeout = null)
        {
            timeout = timeout ?? StandardTimeout;
            var wait = new SearchContextWait(context, timeout.Value);
            TResult result = default(TResult);
            try
            {
                result = wait.Until(condition);
            }
            catch (WebDriverTimeoutException)
            {
                if (throwOnTimeout)
                {
                    throw;
                }
            }
            return result;
        }
    }
}