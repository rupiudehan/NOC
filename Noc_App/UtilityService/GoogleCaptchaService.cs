using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Noc_App.Helpers;

namespace Noc_App.UtilityService
{
    public class GoogleCaptchaService
    {
        private GoogleCaptchaConfig _settings;
        public GoogleCaptchaService(IOptions<GoogleCaptchaConfig> settings)
        {
            _settings = settings.Value;
        }

        public virtual async Task<GoogleCaptchaResponse> VerifyreCaptcha(string _Token)
        {
            GoogleCaptchaData _myData = new GoogleCaptchaData
            {
                response = _Token,
                secret = _settings.SecretKey
            };

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_myData.secret}&response={_myData.response}");
            var capres = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(response);
            return capres;
        }
    }
    public class GoogleCaptchaData
    {
        public string secret { get; set; }
        public string response { get; set; }
    }

    public class GoogleCaptchaResponse
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
