using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Model_Layer.Models
{
    public class Send
    {
        public string SendEmail(string ToEmail,string Token)
        {
            string FromEmail = "pagadalachirudeep@gmail.com";
            MailMessage mailMessage = new MailMessage(FromEmail,ToEmail);
            string MailBody = "Token for Reset Password : " + Token;
            mailMessage.Body = MailBody.ToString();
            mailMessage.Subject = "Token generate for reset password";
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            NetworkCredential networkCredential = new NetworkCredential("pagadalachirudeep@gmail.com", "lluu rtap efhe hxet");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(mailMessage);
            return ToEmail;

        }
    }
}
