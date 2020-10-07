using System;
using NRewardBot.Selenium.Elements;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class OfferLinkElement : ElementBase
    {
        private readonly IWebElement _element;

        public OfferLinkElement(IWebDriver driver, IWebElement element) : base(driver)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
        }

        public void Click()
        {
            this._element.Click();
        }
    }
}