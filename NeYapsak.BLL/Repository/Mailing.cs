using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.BLL.Repository
{
    public class Mailing
    {
        //bu servisi kullanmak için karşı taraftan bir IdentityMessage türünden mesajın gelmiş olması yeterli.
        public void SendMail(IdentityMessage message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress("neyapsakservis@gmail.com");
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(mail.From.ToString(), "123Qw.123");
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(message.Destination));
            mail.Subject = message.Subject;
            //Formatted mail body
            mail.IsBodyHtml = true;
            //string st = "Test";
            //mail.Body = st;
            mail.Body = message.Body;
            smtp.Send(mail);
        }
    }
}
