using Nop.Data;
using Orbio.Core.Domain.Email;
using Orbio.Services.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IDbContext context;

        public EmailService(IDbContext context)
        {
            this.context = context;
        }

        public int SentEmail(Mail_Sending email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email.ToAddress);
                mail.From = new MailAddress(email.FromAddress, email.FromName);
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

        public int SendNotification(string fromaddress, string fromname, string message, string subject)
        {
            context.ExecuteFunction<Mail_Sending>("usp_EmailSending",
                new SqlParameter() { ParameterName = "@profilename", Value = "Emailsending", DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@toaddress", Value = ConfigurationManager.AppSettings["EmailFromAddress"], DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@todisplayname", Value = ConfigurationManager.AppSettings["EmailFromName"], DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@fromaddress", Value = fromaddress, DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@fromname", Value = ConfigurationManager.AppSettings["EmailFromName"], DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@subject", Value = fromname, DbType = System.Data.DbType.String },
                new SqlParameter() { ParameterName = "@body", Value = message, DbType = System.Data.DbType.String });

            return 0;
        }
    }
}