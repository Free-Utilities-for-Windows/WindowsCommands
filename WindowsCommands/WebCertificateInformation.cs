using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using WindowsCommands.Logger;

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
                    var serverCertificate = sslStream.RemoteCertificate as X509Certificate2;

                    if (serverCertificate == null)
                    {
                        string noCertMessage = "No certificate found for the specified URL.";
                        Console.WriteLine(noCertMessage);
                        StaticFileLogger.LogError(noCertMessage);
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

                    string certInfo = $"Host: {webCertificateInfo.Host}\n" +
                                      $"Server: {webCertificateInfo.Server}\n" +
                                      $"Status Code: {webCertificateInfo.StatusCode}\n" +
                                      $"Certificate: {webCertificateInfo.Certificate}\n" +
                                      $"Issued By: {webCertificateInfo.Issued}\n" +
                                      $"Expiration Date: {webCertificateInfo.End}";
                    Console.WriteLine(certInfo);
                    StaticFileLogger.LogInformation(certInfo);
                }
            }
            else if (uri.Scheme == Uri.UriSchemeHttp)
            {
                string httpMessage = "The specified URL uses HTTP, not HTTPS. No certificate information available.";
                Console.WriteLine(httpMessage);
                StaticFileLogger.LogError(httpMessage);
            }
            else
            {
                string invalidUrlMessage = "The specified URL is not a valid HTTP or HTTPS URL.";
                Console.WriteLine(invalidUrlMessage);
                StaticFileLogger.LogError(invalidUrlMessage);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
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