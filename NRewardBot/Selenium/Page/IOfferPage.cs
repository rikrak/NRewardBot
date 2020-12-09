namespace NRewardBot.Selenium.Page
{
    public interface IOfferPage
    {
        IOfferPage EnsureLoggedIn();
        IOfferPage AcceptCookies();
        IOfferPage CompleteOffer();
        void Close();
    }
}