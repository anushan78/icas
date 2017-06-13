using System.Net.Http;
using Newtonsoft.Json;

namespace IcasDrive.Core
{
    public class HttpDataProvider
    {
        public HttpDataProvider(string baseUrl)
        {
            BaseApiUrl = baseUrl;
        }

        private string BaseApiUrl { get; set; }

        public TResult GetData<TResult>(string relativeUrl)
        {
            TResult result;

            using (var client = new HttpClient())
            {
                var baseUrl = BaseApiUrl;
                if (!baseUrl.EndsWith("\\") && !baseUrl.EndsWith("/")) baseUrl = string.Format("{0}/", baseUrl);

                var url = string.Format("{0}{1}", baseUrl, relativeUrl);
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var data = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<TResult>(data);
            }

            return result;
        }

        public TResult PostAndReturn<TModel, TResult>(string relativeUrl, TModel model)
        {
            TResult result;

            using (var client = new HttpClient())
            {
                var baseUrl = BaseApiUrl;
                if (!baseUrl.EndsWith("\\") && !baseUrl.EndsWith("/")) baseUrl = string.Format("{0}/", baseUrl);

                var url = string.Format("{0}{1}", baseUrl, relativeUrl);
                var response = client.PostAsJsonAsync(url, model).Result;
                response.EnsureSuccessStatusCode();
                var data = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<TResult>(data);
            }

            return result;
        }

    }
}