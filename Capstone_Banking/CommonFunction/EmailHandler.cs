using System.Net;
using System.Net.Mail;

namespace Capstone_Banking.CommonFunction
{
    public class EmailHandler
    {
        public static void SendEmail(string emailAddress, string subject, string body) 
        {
            string senderEmail = "cm.a.53amogh.kharche@gmail.com";
            string emailPassword = "ijjqhxlfiwvpphli";
            using SmtpClient smtpClient = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(senderEmail,emailPassword)
            };
            try
            {
                smtpClient.Send(senderEmail, emailAddress, subject, body);
                Console.WriteLine("Email Sent");
            }
            catch (SmtpException ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
