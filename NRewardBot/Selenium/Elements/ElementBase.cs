using System;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Elements
{
    internal class ElementBase
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ElementBase"/>
        /// </summary>
        public ElementBase(IWebDriver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            Driver = driver;
        }

        #endregion

        protected IWebDriver Driver { get; }

        protected T Page<T>()
        {
            var constructor = typeof(T).GetConstructor(new Type[]
            {
                typeof (IWebDriver)
            });
            if (constructor == null)
            {
                throw new ArgumentException("No constructor for the specified class containing a single argument of type IWebDriver can be found");
            }
            T obj = (T)constructor.Invoke(new object[]
            {
                this.Driver
            });

            return obj;
        }

        public virtual void CheckIsLoaded()
        { }

        private void Log(string format, params object[] args)
        {
            this.Log(string.Format(format, args));
        }

        private void Log(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        protected void LogElement(string name, IWebElement element)
        {
            Log("{0} displayed: {1}", name, element.Displayed);
            Log("{0} enabled: {1}", name, element.Enabled);
            Log("{0} selected: {1}", name, element.Selected);
        }
    }
}
