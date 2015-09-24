using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.MessageTemplates
{
    public interface IMessageTemplateService
    {
        List<MessageTemplate> GetAllMessageTemplate();

        int DeleteMessageTemplate(int Id);

        MessageTemplate GetMessageTemplateById(int Id);

        int AddOrUpdateMessageTemplate(MessageTemplate msgTemp);
    }
}
