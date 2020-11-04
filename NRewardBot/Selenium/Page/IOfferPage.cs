namespace NRewardBot.Selenium.Page
{
    public interface IOfferPage
    {
        IOfferPage AcceptCookies();
        IOfferPage CompleteOffer();
        void Close();
    }
}