namespace Noc_App.Helpers
{
    public class PaymentConfig
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public PaymentConfig(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
