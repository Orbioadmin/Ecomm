using Orbio.Services.Admin.ACL;
using Orbio.Services.Admin.Customers;
using Orbio.Web.UI.Areas.Admin.Models.ACL;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ACLController : Controller
    {
        private readonly IPermissionService permissionService;
        private readonly ICustomerRoleService customerRoleService;

        public ACLController(IPermissionService permissionService, ICustomerRoleService customerRoleService)
        {
            this.permissionService = permissionService;
            this.customerRoleService = customerRoleService;
        }

        // GET: Admin/ACL
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var model = new AccessControlDetailModel();
            var permission = permissionService.GetAllPermission();
            model.ACL = (from p in permission
                         select new AccessControlListModel(p)).ToList();

            model.Roles = (from CR in customerRoleService.GetAllCustomerRole()
                           select new CustomerRoleModel(CR)).OrderBy(m => m.Name).ToList();
            return View(model);
        }

        //public ActionResult AccessControlList()
        //{
        //    var model = new AccessControlDetailModel();
        //    var permission = permissionService.GetAllPermission();
        //    model.ACL = (from p in permission
        //                 select new AccessControlListModel(p)).ToList();

        //    model.Roles = (from CR in customerRoleService.GetAllCustomerRole()
        //                   select new CustomerRoleModel(CR)).OrderBy(m=>m.Name).ToList();
        //    return PartialView(model);
        //}

        [HttpPost]
        public ActionResult UpdatePermission(FormCollection form)
        {
            var permissionRecords = permissionService.GetAllPermission();
            var customerRoles = customerRoleService.GetAllCustomerRole();


            foreach (var cr in customerRoles)
            {
                string formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = form[formKey] != null ? form[formKey].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (var pr in permissionRecords)
                {

                    bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    if (allow)
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) == null)
                        {
                            pr.CustomerRoles.Add(cr);
                            permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                    else
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) != null)
                        {
                            pr.CustomerRoles = pr.CustomerRoles.Where(x => x.Id != cr.Id).ToList();
                            permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                }
            }
            return RedirectToAction("List");
        }
    }
}