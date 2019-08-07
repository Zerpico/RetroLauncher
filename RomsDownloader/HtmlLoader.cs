using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace RomsDownloader
{
    /// <summary>
    /// Загрузчик Html документа с настройками парсера
    /// </summary>
    class HtmlLoader
    {
        readonly HttpClient client;
        readonly string url;
        public string ErrorMessage { get; private set; }

        public HtmlLoader(IParserSettings parserSettings)
        {
            client = new HttpClient();
            
            url = parserSettings.BaseUrl;
        }

        /// <summary>
        /// Cкачивает Html документ по указаной Url + prefixUrl
        /// </summary>
        /// <param name="prefixUrl"></param>
        /// <returns></returns>
        public async Task<string> GetSource(string prefixUrl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; ;
            var response = await client.GetAsync(url + prefixUrl);
            string source = null;

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ErrorMessage = null;
                source = await response.Content.ReadAsStringAsync();
            }
            else ErrorMessage = response.ReasonPhrase;
            return source;
        }
    }

}
