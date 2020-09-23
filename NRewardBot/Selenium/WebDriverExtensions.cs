using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NRewardBot.Selenium
{
    public static class WebDriverExtensions
    {
        private static readonly TimeSpan StandardTimeout = TimeSpan.FromSeconds(1);

        public static bool ElementExists(this IWebDriver driver, By by)
        {
            var overallTimeout = TimeSpan.FromSeconds(10);
            var sleepCycle = TimeSpan.FromMilliseconds(50);
            var wait = new WebDriverWait(new SystemClock(), driver, overallTimeout, sleepCycle);

            try
            {
                var exists = wait.Until((d) => d.FindElements(by).Count > 0);
                return exists;
            }
            catch (WebDriverException)
            {
                return false;
            }
        }

        public static void WaitForAlert(this IWebDriver driver)
        {
            int i = 0;
            while (i++ < 5)
            {
                try
                {
                    IAlert alert = driver.SwitchTo().Alert();
                    break;
                }
                catch (NoAlertPresentException)
                {
                    Thread.Sleep(1000);
                    continue;
                }
            }
        }

#if obsolete
        public static IWebElement WaitUntilElementIsDisplayed(this IWebDriver driver, By elementLocator, bool throwOnTimeout = true, TimeSpan? timeout = null)
        {
            timeout = timeout ?? StandardTimeout;
            Debug.WriteLine($"Waiting for {elementLocator}");
            var isDisplayed = driver.WaitUntil(d =>
            {
                try
                {
                    var elementToCheck = driver.FindElement(elementLocator);
                    var displayed = elementToCheck.Displayed;
                    Debug.WriteLine($"Element {elementLocator} is {(displayed ? "" : "not ")}displayed");
                    if (!displayed)
                    {
                        Debug.WriteLine($"  Enabled:     {elementToCheck.Enabled}");
                        Debug.WriteLine($"  Location:    {elementToCheck.Location}");
                        Debug.WriteLine($"  Size:        {elementToCheck.Size}");
                        Debug.WriteLine($"  Text:        {elementToCheck.Text}");
                        Debug.WriteLine($"  Style:       {elementToCheck.GetAttribute("style")}");
                        Debug.WriteLine($"  Class:       {elementToCheck.GetAttribute("class")}");
                        Debug.WriteLine($"  css-display: {elementToCheck.GetCssValue("display")}");
                    }
                    return displayed;
                }
                catch (StaleElementReferenceException)
                {
                    Debug.WriteLine($"Element {elementLocator} is Stale");
                    return false;
                }
                catch (NoSuchElementException)
                {
                    Debug.WriteLine($"Element {elementLocator} cannot be found");
                    return false;
                }
            }, throwOnTimeout, timeout);

            return isDisplayed
                ? driver.FindElement(elementLocator)
                : null;
        }
#endif
        public static TResult WaitUntil<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition, bool throwOnTimeout = true, TimeSpan? timeout = null)
        {
            Debug.WriteLine($"WaitUntil - {condition.Method.Name} ");
            timeout = timeout ?? StandardTimeout;

            var wait = new WebDriverWait(driver, timeout.Value);
            TResult result = default(TResult);
            try
            {
                Debug.Write("waiting... ");
                result = wait.Until(condition);
                Debug.WriteLine("waited!");
            }
            catch (WebDriverTimeoutException)
            {
                Debug.WriteLine("WaitUntil - timeout!");
                if (throwOnTimeout)
                {
                    throw;
                }
            }
            return result;
        }

        public static void WaitUntil(this IWebDriver driver, string conditionExpression, int seconds = 15, params object[] args)
        {
            int cnt = 0;
            bool result;
            do
            {
                if (cnt >= seconds)
                {
                    throw new TimeoutException("Wait until true exceeded wait limit.");
                }

                if (cnt++ > 0)
                {
                    Thread.Sleep(1000);
                }

                string script = $@"return {conditionExpression};";
                result = driver.ScriptQuery<bool>(script, args);
                Debug.WriteLine("({0}) returns {1}", conditionExpression, result);
            } while (result == false);
        }

        public static void DoWait(this IWebDriver driver)
        {
            driver.DoWait(1);
        }

        /// <summary>
        /// Pauses execution.  This should be a last resort.  Prefer one of the WaitUntil... methods
        /// </summary>
        public static void DoWait(this IWebDriver driver, int waitFactor)
        {
            const int factorMultiplier = 1000;
            var milliseconds = waitFactor * factorMultiplier;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(milliseconds));

            var waitComplete = wait.Until<bool>(
                arg =>
                {
                    Thread.Sleep(milliseconds);
                    return true;
                });
        }

        public static void ScriptExecute(this IWebDriver driver, string script, params object[] args)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(script, args);
        }

        public static T ScriptQuery<T>(this IWebDriver driver, string script, params object[] args)
        {
            return (T)((IJavaScriptExecutor)driver).ExecuteScript(script, args);
        }

        public static bool IsRemote(this IWebDriver driver)
        {
            return driver is OpenQA.Selenium.Remote.RemoteWebDriver;
        }

        public static void MoveFocusTo(this IWebDriver driver, IWebElement element)
        {
            // adapted from http://stackoverflow.com/questions/11337353/correct-way-to-focus-an-element-in-selenium-webdriver-using-java
            if (element.TagName == "input")
            {
                element.SendKeys(string.Empty);
            }
            else
            {
                new Actions(driver).MoveToElement(element).Perform();
            }
        }
    }
}