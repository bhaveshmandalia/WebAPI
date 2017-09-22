using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using TokensApp.Models;

namespace TokensApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokensController : ApiController
    {
        static HttpClient client = new HttpClient();
        private const string URL = "https://sub.domain.com/objects.json?api_key=123";

        static async Task<Uri> CreateTokenAsync()
        {
            tsRequest request = new tsRequest();
            request.credentials.name = "nayanp";
            request.credentials.password = "password_123";
            request.site.contentUrl = "";

            client.BaseAddress = new Uri("http://10.1.2.6:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            HttpResponseMessage response = await client.PostAsXmlAsync("api/2.3/auth/signin", request);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        private static void CreateObject()
        {
            tsRequest data = new tsRequest();
            data.credentials.name = "nayanp";
            data.credentials.password = "password_123";
            data.site.contentUrl = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "text/xml";
            //request.ContentLength = DATA.Length;
            using (Stream webStream = request.GetRequestStream())
            using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
            {
                //requestWriter.Write(DATA);
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            Console.Out.WriteLine(response);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }

        }

        public IHttpActionResult GetToken()
        {
            //
            //string fullFilePath = @"D:\Bhavesh\TokensApp\TokensApp\XmlFiles\TokenRequest.XML";
            ////string uri = "http://10.1.2.6:8080/api/2.1/auth/signin";
            //string uri = "http://10.1.2.6:8080/trusted";
            //string responsetoken = SendXMLFile(fullFilePath, uri, 500);
            //string strValue = "Response Empty";
            //using (XmlReader reader = XmlReader.Create(new StringReader(responsetoken)))
            //{
            //    reader.ReadToFollowing("credentials");
            //    reader.MoveToContent();
            //    strValue = reader.GetAttribute("token");
            //    Console.WriteLine(strValue);
            //}
            var responsetoken = GetTokenMethod1();

            if (responsetoken == null)
            {
                return NotFound();
            }
            return Ok(responsetoken);
        }

        private string GetTokenMethod1()
        {
            string username = "username=nayanp";
           
            MyWebRequest myRequest = new MyWebRequest("http://10.1.2.6:8080/trusted", "POST", username);
            string Value = myRequest.GetResponse();

            return Value;
        }

        private string GetTokenMethod2()
        {
            tsRequest request = new tsRequest();
            request.credentials = new credentials { name = "nayanp", password = "password_123" };
            request.site = new site { contentUrl = "" };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://10.1.2.6:8080/");
            // Add an Accept header for JSON format.  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            // List all Names.  
            var response = client.PostAsXmlAsync("api/2.3/auth/signin", request).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.Write("Success");
            }
            else
                Console.Write("Error");
            return string.Empty;
        }

        public static string SendXMLFile(string xmlFilepath, string uri, int timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContentType = "application/xml";
            request.Method = "POST";

            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(xmlFilepath))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
                byte[] postBytes = Encoding.UTF8.GetBytes(sb.ToString());

                if (timeout < 0)
                {
                    request.ReadWriteTimeout = timeout;
                    request.Timeout = timeout;
                }

                request.ContentLength = postBytes.Length;

                try
                {
                    Stream requestStream = request.GetRequestStream();

                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        var encoding = ASCIIEncoding.ASCII;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            return responseText;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    request.Abort();
                    return string.Empty;
                }
            }
        }
    }
}
