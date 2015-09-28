using Orbio.Core.Data;
using Orbio.Services.Admin.MessageTemplates;
using Orbio.Web.UI.Areas.Admin.Models.MessageTemplates;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class MessageTemplateController : Controller
    {
        private readonly IMessageTemplateService _messageTemplateService;

        public MessageTemplateController(IMessageTemplateService _messageTemplateService)
        {
            this._messageTemplateService = _messageTemplateService;
        }

        // GET: Admin/MessageTemplate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult MessageTemplateList(int? page)
        {
            var model = new List<MessageTemplateModel>();
            var result = _messageTemplateService.GetAllMessageTemplate();
            model = (from mt in result
                     select new MessageTemplateModel(mt)
                     ).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(model.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult DeleteMessageTemplate(int? Id)
        {
            var result = _messageTemplateService.DeleteMessageTemplate(Id.GetValueOrDefault());
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            var model = new MessageTemplateModel();
            return View("AddOrEditMessageTemplate",model);
        }

        public ActionResult Edit(int? Id)
        {
            var model = new MessageTemplateModel();
            var result = _messageTemplateService.GetMessageTemplateById(Id.GetValueOrDefault());
            model = new MessageTemplateModel(result);
            return View("AddOrEditMessageTemplate", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(MessageTemplateModel model)
        {
            var msgTemp = new MessageTemplate()
                            {
                                Id=model.Id,
                                Name=model.Name,
                                IsActive=model.IsActive,
                                BccEmailAddresses=model.BCC,
                                Subject=model.Subject,
                                Body=model.Body,
                            };
            var result = _messageTemplateService.AddOrUpdateMessageTemplate(msgTemp);
            return RedirectToAction("List");
        }
    }
}