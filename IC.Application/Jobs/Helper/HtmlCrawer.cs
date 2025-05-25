using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IC.Application.Settings;

namespace IC.Application.Jobs.Helper
{
    public class HtmlCrawer
    {
        public static string WHOSCORE_COOKIES = "_ga=GA1.2.1522980637.1638238274; _gid=GA1.2.1608048762.1638238274; _fbp=fb.1.1638238275487.601814135; _pbjs_userid_consent_data=6683316680106290; _pubcid=771f9e5f-9707-4b00-b58e-bdcc08a56348; _xpid=3332333905; _xpkey=AVNP2VWOE7vCxdhFmBuShSFOF1FkKxOM; __qca=P0-1000656407-1638238275987; _lr_env_src_ats=false; _unifiedid=%7B%22TDID%22%3A%22801a42c3-661b-4a1d-8fa5-534d927ada4d%22%2C%22TDID_LOOKUP%22%3A%22TRUE%22%2C%22TDID_CREATED_AT%22%3A%222021-10-30T02%3A11%3A18%22%7D; _cc_id=3872495c44b939128bae8da5ea8c5003; visid_incap_774904=EiT0yfmpTeGrVPtBfix0f12SpWEAAAAAQUIPAAAAAADjf37CXxUIpj2ZCwGLYdyM; __gads=ID=54286bf39144672c:T=1638241904:S=ALNI_MbvUgosk2E_OXJrTZOxxnQroaYudg; visid_incap_774906=TxEWYgBsTlKhrIvlkX0SLz+IpWEAAAAAQ0IPAAAAAACAxqegAWcD/iYq/JPTUAhO3L4wqaKmRMxJ; incap_ses_1047_774906=tbzUWuGG4zRFnsELX7GHDkArqGEAAAAAbD5ZM3+O6HpNeRlMiWbSgA==; _gat=1; _gat_subdomainTracker=1; _lr_retry_request=true; panoramaId_expiry=1639015908027; panoramaId=34b53f0597abb51df35567c611cb4945a7022e0cab29cc6874edf072d9968707; cto_bidid=iZynil9xUTh3cU5HQzZpNWttRHlkYWp3TXgzUFBYVm9pME90cm5sbmVQJTJCb20lMkYlMkI5MXkyYzdUWktlUWtUZ3FmeFVWR29idSUyQnN0eHJlWHc2SVZQbXBDem81OTE5UjNvOVRMbSUyQnZSV1lJZk5zY3VEUmMlM0Q; cto_bundle=AtxXdV8lMkJ1MklZc1hMcFglMkZIdDIyRjd4cXBhR25SZnBSeGNDZW5KQWtibHRBVXZJZ2M4eUFCRCUyQmlucGloR0YwYXBsVFVjSUN6ZHFseFBTWEJ6bHVqTVBlNU5MTHdiRSUyQkhSa1BraFlycFZLQkFTMUJTNzM5WW5uUGhvNktIUHhIcjlidTZ1N3J6eENYUDh4RThtM0UlMkJZNE1yTzhBJTNEJTNE";
        public static string LIVESCORE_COOKIES = "";
        public static string Craw(string url)
        {
            string retVal = string.Empty;

            try
            {
                /*url = "https://prod-public-api.livescore.com/v1/api/app/scoreboard/soccer/899970";
                if (url.Contains("?")) {
                    url = url + "&k=" + DateTime.Now.ToString("HHmmss.fff").Replace(".", "");
                }
                else
                {
                    url = url + "?k=" + DateTime.Now.ToString("HHmmss.fff").Replace(".", "");
                }*/
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                        | SecurityProtocolType.Tls11
                                                        | SecurityProtocolType.Tls12
                                                        | SecurityProtocolType.Tls13;
                // Skip validation of SSL/TLS certificate
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json;charset=\"utf-8\"";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";


                if (!string.IsNullOrEmpty(LIVESCORE_COOKIES))
                {
                    request.Headers.Add("cookie", LIVESCORE_COOKIES);
                }

                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;

                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    retVal = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        public async Task<string> CrawAsync(string url)
        {
            string retVal = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36");

                    if (!string.IsNullOrEmpty(LIVESCORE_COOKIES))
                    {
                        client.DefaultRequestHeaders.Add("cookie", LIVESCORE_COOKIES);
                    }

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    retVal = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        private static async Task<List<Cookie>> GetCookiesAsync(string url)
        {
            var cookieContainer = new CookieContainer();
            var uri = new Uri(url);
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            })
            {
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    await httpClient.GetAsync(uri);
                    return cookieContainer.GetCookies(uri).Cast<Cookie>().ToList();
                }
            }
        }

        public static string Craw(string url, string refer)
        {
            string retVal = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                        | SecurityProtocolType.Tls11
                                                        | SecurityProtocolType.Tls12
                                                        | SecurityProtocolType.Ssl3;
                // Skip validation of SSL/TLS certificate
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";

                if (url.Contains("whoscored"))
                {
                    FieldInfo headersFieldInfo = request.GetType().GetField("_HttpRequestHeaders", System.Reflection.BindingFlags.NonPublic
                                                    | System.Reflection.BindingFlags.Instance
                                                    | System.Reflection.BindingFlags.GetField);

                    CusteredHeaderCollection WssHeaders = new CusteredHeaderCollection("1xbet.whoscored.com");
                    WssHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
                    WssHeaders.Add("content-type", "text/xml;charset=\"utf-8\"");
                    WssHeaders.Add("cookie", WHOSCORE_COOKIES);

                    //WssHeaders.Add("accept-encoding", "gzip, deflate, br");
                    //WssHeaders.Add("accept-language", "en-US,en;q=0.9");
                    //WssHeaders.Add("cache-control", "max-age=0");
                    //WssHeaders.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"96\", \"Google Chrome\";v=\"96\"");
                    //WssHeaders.Add("sec-ch-ua-mobile", "?0");
                    //WssHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    //WssHeaders.Add("sec-fetch-dest", "document");
                    //WssHeaders.Add("sec-fetch-mode", "navigate");
                    //WssHeaders.Add("sec-fetch-site", "none");
                    //WssHeaders.Add("sec-fetch-user", "?1");
                    //WssHeaders.Add("upgrade-insecure-requests", "1");
                    //WssHeaders.Add("cookie", Constants.WHOSCORE_COOKIES);
                    //WssHeaders.Add("upgrade-insecure-requests", "1");
                    //WssHeaders.Add("referer", refer);
                    //WssHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*; q=0.8,application/signed-exchange;v=b3;q=0.9");

                    headersFieldInfo.SetValue(request, WssHeaders);

                    //request.Referer = refer;
                    //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

                    /*request.Headers.Add("accept-encoding", "gzip, deflate, br");
                    request.Headers.Add("accept-language", "en-US,en;q=0.9");
                    request.Headers.Add("cache-control", "max-age=0");
                    request.Headers.Add("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"96\", \"Google Chrome\";v=\"96\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "document");
                    request.Headers.Add("sec-fetch-mode", "navigate");
                    request.Headers.Add("sec-fetch-site", "none");
                    request.Headers.Add("sec-fetch-user", "?1");
                    request.Headers.Add("upgrade-insecure-requests", "1");
                    request.Headers.Add("cookie", Constants.WHOSCORE_COOKIES);*/

                    //request.Headers.Add("referer", refer);

                    //request.Credentials = CredentialCache.DefaultCredentials;

                    //sms.utils.Log.writeLog(JsonConvert.SerializeObject(request), "Craw");
                }

                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                request.CachePolicy = noCachePolicy;

                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(data))
                {
                    retVal = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
    }
    public class CusteredHeaderCollection : WebHeaderCollection
    {
        public bool HostHeaderValueReplaced { get; private set; }

        public string ClusterUrl { get; private set; }

        public CusteredHeaderCollection(string commonClusterUrl) : base()
        {
            if (string.IsNullOrEmpty(commonClusterUrl))
                throw new ArgumentNullException(commonClusterUrl);

            this.ClusterUrl = commonClusterUrl;
        }

        public override string ToString()
        {
            this["Host"] = this.ClusterUrl;
            string tmp = base.ToString();
            this.HostHeaderValueReplaced = true;

            return tmp;
        }

    }
}
