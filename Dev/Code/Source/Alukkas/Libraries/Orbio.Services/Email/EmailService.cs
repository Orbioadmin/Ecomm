using Nop.Data;
using Orbio.Core.Domain.Email;
using Orbio.Services.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Email
{
    public class EmailService : IEmailService
    {
        public int SentEmail(Mail_Sending email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email.ToAddress);
                mail.From = new MailAddress(email.FromAddress,email.FromName);
                mail.Subject = email.Subject;
                mail.Body = "Hi " + email.ToName + "," + "<br />" + email.Body;
                mail.IsBodyHtml = true;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(email.FromAddress, email.Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);               
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }
}
