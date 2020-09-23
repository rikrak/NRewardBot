using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium
{
    public static class WebElementExtensions
    {
        public static void ClearAndSendKeys(this IWebElement element, string text)
        {
            //element.Click();
            element.Clear();
            element.SendKeys(text ?? string.Empty);
        }

        /// <summary>
        /// Attempts to get the parent element in the DOM
        /// </summary>
        /// <param name="e">The current element</param>
        /// <returns>The parent element in the page DOM</returns>
        public static IWebElement GetParent(this IWebElement e)
        {
            return e.FindElement(By.XPath(".."));
        }

        /// <summary>
        /// Gets the content of the value attribute.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            return element.GetAttribute("value");
        }
    }
}