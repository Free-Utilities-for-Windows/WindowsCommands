using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace WindowsCommands;

public static class WebCertificateInformation
{
    public static void GetWebCertificateInfo(string url)
    {
        try
        {
            var uri = new Uri(url);

            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                var tcpClient = new TcpClient(uri.Host, uri.Port);

                using (var sslStream = new SslStream(tcpClient.GetStream(), false,
                           (sender, certificate, chain, errors) => true))
                {
                    sslStream.AuthenticateAsClient(uri.Host);
                    var serverCertificate = sslStream.RemoteCertificate;

                    if (serverCertificate == null)
                    {
                        Console.WriteLine("No certificate found for the specified URL.");
                        return;
                    }

                    string certName = serverCertificate.Subject.Replace("CN=", "");
                    string certOwner = serverCertificate.Issuer.Split(", ")[1].Replace("O=", "");
                    DateTime dateEnd = DateTime.Parse(serverCertificate.GetExpirationDateString());

                    var webCertificateInfo = new WebCertificateInfo
                    {
                        Host = url,
                        Server = uri.Host,
                        StatusCode = HttpStatusCode.OK,
                        Certificate = certName,
                        Issued = certOwner,
                        End = dateEnd
                    };

                    Console.WriteLine("Host: " + webCertificateInfo.Host);
                    Console.WriteLine("Server: " + webCertificateInfo.Server);
                    Console.WriteLine("Status Code: " + webCertificateInfo.StatusCode);
                    Console.WriteLine("Certificate: " + webCertificateInfo.Certificate);
                    Console.WriteLine("Issued By: " + webCertificateInfo.Issued);
                    Console.WriteLine("Expiration Date: " + webCertificateInfo.End);
                }
            }
            else if (uri.Scheme == Uri.UriSchemeHttp)
            {
                Console.WriteLine("The specified URL uses HTTP, not HTTPS. No certificate information available.");
            }
            else
            {
                Console.WriteLine("The specified URL is not a valid HTTP or HTTPS URL.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    public class WebCertificateInfo
    {
        public string Host { get; set; }
        public string Server { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Certificate { get; set; }
        public string Issued { get; set; }
        public DateTime End { get; set; }
    }
}