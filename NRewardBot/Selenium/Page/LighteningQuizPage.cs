using System;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;

namespace NRewardBot.Selenium.Page
{
    public class LighteningQuizPage : OfferPageBase, IOfferPage
    {
        #region Logger
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        public const string AnswerOption1Id = "rqAnswerOption0";
        public const string StartQuizId = "rqStartQuiz";

        public LighteningQuizPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement GetAnswerOption(int idx)
        {
            return this.Driver.WaitUntilElementIsDisplayed(By.Id($"rqAnswerOption{idx}"), throwOnTimeout:false, TimeSpan.FromSeconds(2));
        }

        private void StartQuiz()
        {
            var startButton = this.Driver.WaitUntilElementIsDisplayed(By.Id(StartQuizId), throwOnTimeout:false, TimeSpan.FromSeconds(2));
            startButton?.Click();
        }

        public override IOfferPage CompleteOffer()
        {
            Log.Info("Attempting a lightening quiz");
            this.StartQuiz();

            for (int i = 0; i < 10; i++)
            {
                var firstAnswer = GetAnswerOption(0);
                if (firstAnswer != null)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var answerElement = GetAnswerOption(j);
                        if (answerElement != null)
                        {
                            this.Driver.DoWait(2);
                            this.Driver.ScriptExecute(
                                $"document.querySelectorAll('#rqAnswerOption{j}').forEach(el=>el.click());");
                        }
                    }
                }

                if (this.Driver.ElementExists(By.Id("quizCompleteContainer")))
                {
                    break;
                }
            }

            var completeButtonElement = this.Driver.WaitUntilElementIsDisplayed(By.CssSelector(".cico.btCloseBack"), throwOnTimeout: false);
            completeButtonElement?.Click();

            return this;
        }

        public static bool IsLightningQuizPage(IWebDriver driver)
        {
            return driver.ElementExists(By.Id("rqAnswerOption0"));
        }
    }
}