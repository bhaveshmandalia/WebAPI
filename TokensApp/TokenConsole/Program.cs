using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TokenConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string fullFilePath = @"D:\Test Project\MVC\TokensApp\TokenConsole\RequestFiles\TokenRequest.XML";
            string uri = "http://10.1.2.6:8080/api/2.1/auth/signin";
            string responseTask = SendXMLFile(fullFilePath, uri, 500);
            Console.WriteLine("Response: {0}", responseTask);
            Console.ReadLine();
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
