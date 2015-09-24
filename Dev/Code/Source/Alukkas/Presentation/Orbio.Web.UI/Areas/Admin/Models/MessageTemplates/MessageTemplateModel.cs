using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.MessageTemplates
{
    public class MessageTemplateModel
    {
        public MessageTemplateModel()
        {

        }
        public MessageTemplateModel(MessageTemplate mt)
        {
            Id = mt.Id;
            Name = mt.Name;
            IsActive = mt.IsActive;
            BCC = mt.BccEmailAddresses;
            Subject = mt.Subject;
            Body = mt.Body;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        [AllowHtml]
        public string Body { get; set; }
    }
}