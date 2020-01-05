using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileProtect.Model.Parser
{
    class HtmlLoader
    {
        private HttpClient client;
        private readonly string url;

        public HtmlLoader(IParserSettings settings)
        {
            url = settings.BaseUrl;
            client = new HttpClient();
        }

        public async Task<string> GetSource()
        {
            var response = await client.GetAsync(url);
            string source = default;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                source = await response.Content.ReadAsStringAsync();
            }

            return source;
        }
    }
}
