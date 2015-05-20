using KarmaRewards.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KarmaRewards.Services
{
    public class RelayHandler : HttpTaskAsyncHandler
    {

        public RelayHandler()
            : this(ObjectFactory.Resolve<IIdentityService>())
        {
        }

        #region IHttpHandler Members
        public RelayHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        private IIdentityService _identityService;

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StreamReader reader = new StreamReader(context.Request.InputStream, System.Text.Encoding.UTF8);
            var payload = reader.ReadToEnd();
            var jObject = JObject.Parse(payload);

            var session = await _identityService.GetUserSessionAsync();

            jObject.Remove("ak");
            jObject.Add("ak", ClientAPIKey);
            if (session != null && string.IsNullOrWhiteSpace(session.Token) == false)
                jObject.Add("ut", session.Token);

            var url = context.Request.Url.AbsoluteUri;

            var response = await Post(jObject.ToString(), url);
            var bytes = ReadFully(response);
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }

        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }

        #endregion

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static async Task<Stream> Post(string payload, string path = null)
        {
            var url = path.Replace(IncomingUrl, OutgoingUrl);
            var result = string.Empty;
            var request = WebRequest.Create(url);
            request.Method = "POST";

            if (string.IsNullOrEmpty(payload) == false)
            {
                request.ContentType = "text/plain";
                var bytes = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = bytes.Length;
                using (var postStream = request.GetRequestStream())
                {
                    postStream.Write(bytes, 0, bytes.Length);
                }
            }
            try
            {
                var response = await request.GetResponseAsync() as HttpWebResponse;
                if (response != null)
                {
                    return response.GetResponseStream();
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private static string ClientAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["appacitive-client-key"];
            }
        }

        private static string IncomingUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["incoming-url"];
            }
        }

        private static string OutgoingUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["appacitive-api-url"];
            }
        }
    }
}
