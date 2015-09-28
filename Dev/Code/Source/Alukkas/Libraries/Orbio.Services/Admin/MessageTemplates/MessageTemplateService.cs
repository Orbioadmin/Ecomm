using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.MessageTemplates
{
    public class MessageTemplateService : IMessageTemplateService
    {
       public List<MessageTemplate> GetAllMessageTemplate()
        {
           using(var context=new OrbioAdminContext())
           {
               var query = context.MessageTemplates.ToList();
               return query;
           }
        }

        public int DeleteMessageTemplate(int Id)
        {
           using (var context = new OrbioAdminContext())
           {
               try
               {
                   var query = context.MessageTemplates.Where(m => m.Id == Id).FirstOrDefault();
                   if (query != null)
                   {
                       context.MessageTemplates.Remove(query);
                       context.SaveChanges();
                   }
                   return 1;
               }
               catch(Exception)
               {
                   return 0;
               }
           }
       }

        public MessageTemplate GetMessageTemplateById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.MessageTemplates.Where(m => m.Id == Id).FirstOrDefault();
                return query;
            }
        }

        public  int AddOrUpdateMessageTemplate(MessageTemplate msgTemp)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var query = context.MessageTemplates.Where(m => m.Id == msgTemp.Id).FirstOrDefault();
                    if (query == null)
                    {
                        query = context.MessageTemplates.FirstOrDefault();
                        query.Name = msgTemp.Name;
                        query.IsActive = msgTemp.IsActive;
                        query.BccEmailAddresses = msgTemp.BccEmailAddresses;
                        query.Subject = msgTemp.Subject;
                        query.Body = msgTemp.Body;
                        context.MessageTemplates.Add(query);
                        context.SaveChanges();
                    }
                    else
                    {
                        query.Name = msgTemp.Name;
                        query.IsActive = msgTemp.IsActive;
                        query.BccEmailAddresses = msgTemp.BccEmailAddresses;
                        query.Subject = msgTemp.Subject;
                        query.Body = msgTemp.Body;
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }
    }
}
