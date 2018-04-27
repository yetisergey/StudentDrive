namespace StudentDrive.Controllers.Utils
{
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Net;
    using System.Security.Policy;
    public class GetJSONWebRequest
    {
        public static JObject Get(Url url)
        {
            WebRequest request = WebRequest.Create(url.Value);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return JObject.Parse(reader.ReadToEnd());
                }
            }
        }
    }
}