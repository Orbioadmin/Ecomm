using Orbio.Core.Domain.Email;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Email
{ 
    public partial interface IEmailService
    {
        int SentEmail(EmailDetail email);
        //int SendNotification(string fromaddress, string fromname, string message,string subject);
    }
}
