using SGS.Business.Controllers;
using SGS.Business.Objects;
using SGS.Common.Controllers;
using SGS.Portal.Web.Areas.Employee.Models;
using SGS.Portal.Web.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SGS.Portal.Web.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Admin/Employee/

        [CustAuthFilter]
        [HttpGet]
        public string GetEmployeesByClient(string c)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            var empList = EmployeeHelper.GetAllByClient(c);            
            foreach (EmployeeViewModel x in empList.OrderBy(x=>x.FullName))
            {
                result.Add(new SelectListItem { Text = x.FullName, Value = x.EmployeeId });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result);
        }

        [CustAuthFilter]
        public ActionResult Index()
        {
            var model = EmployeeHelper.GetAll();
            return View(model);
        }

        [CustAuthFilter]
        public ActionResult Record(int? id)
        {
            var model = new EmployeeViewModel();
            SetDropdownFields();

            if (id != null)
            {
                model = EmployeeHelper.Get(id.Value);
            }
            else
            {
                model.Gender = "Male";
                model.MaritalStatus = "Single";
                model.DateOfBirth = DateHelper.DateTimeNow.AddYears(-18);
                model.RoleId = UserManager.Instance.GetDefaultRole();
                model.EmployeeId = "New Record";
            }

            return View(model);
        }

        [CustAuthFilter]
        [HttpPost]
        public ActionResult Record(EmployeeViewModel model, HttpPostedFileBase file)
        {
            EmployeeViewModel updModel = model;
            ViewBag.IsSuccess = false;
            SetDropdownFields();

            if (ModelState.IsValid)
            {
                SGS.Data.EntityFrameworks.User employee = new SGS.Data.EntityFrameworks.User();
                employee.UserId = model.UserId;
                employee.FirstName = model.FirstName;
                employee.MiddleName = model.MiddleName;
                employee.LastName = model.LastName;
                employee.NickName = model.NickName;
                employee.Gender = model.Gender;
                employee.MaritalStatus = model.MaritalStatus;
                employee.DateOfBirth = model.DateOfBirth.Value;
                employee.PlaceOfBirth = model.PlaceOfBirth;
                employee.PresentAddress = model.PresentAddress;
                employee.ProvincialAddress = model.ProvincialAddress;
                employee.LandlineNo = model.LandLine;
                employee.MobileNo = model.MobileNo;
                employee.Email = model.Email;
                employee.Nationality = model.Nationality;
                employee.Ref_Name = model.ReferenceName;
                employee.Ref_Relationship = model.ReferenceRelationship;
                employee.Ref_Address = model.ReferenceAddress;
                employee.Ref_ContactNo = model.ReferenceContactNo;
                employee.TaxIdNo = model.TaxIdNo;
                employee.SSSNo = model.SSSNo;
                employee.HDMFNo = model.HDMFNo;
                employee.PhilHealthNo = model.PhilHealthNo;
                employee.RoleId = model.RoleId;
                employee.Status = model.Status;
                employee.EmploymentType = model.EmploymentType;
                employee.Position = model.Position;
                employee.Client = model.Client;
                employee.DateHired = model.DateHired;

                if (employee.UserId == 0)
                {
                    //employee.Password = EncryptionManager.encrypt(ConfigurationManager.AppSettings["DefaultPassword"].ToString());
                    employee.Password = ConfigurationManager.AppSettings["DefaultPassword"].ToString();
                    employee.CreatedBy = UserHelper.CurrentUser.EmployeeId;
                }
                else
                {
                    employee.ModifiedBy = UserHelper.CurrentUser.EmployeeId;
                }


                if (file != null)
                {
                    string filename = "p_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string pic = System.IO.Path.GetFileName(filename);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath(ConfigurationManager.AppSettings["UploadPath"].ToString()), pic);

                    file.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }

                    employee.Picture = filename;
                }

                try
                {
                    updModel = EmployeeHelper.Get(UserManager.Instance.SaveUser(employee));
                    ViewBag.Success = true;
                }
                catch (Exception ex)
                {
                    ViewBag.SysError = ex.Message;
                }
            }
            else
            {
                ViewBag.Error = true;
            }

            return View(updModel);
        }

        private void SetDropdownFields()
        {
            //String[] roles = Roles.GetRolesForUser(UserHelper.CurrentUser.UserId.ToString());
            //ViewBag.MemberRole = roles[0];

            //if (roles[0] == "SuperAdmin" || roles[0] == "DevAdmin")
            //{
            //    List<SelectListItem> roleList = (from x in UserManager.Instance.GetAllRoles()
            //                                     select new SelectListItem()
            //                                     {
            //                                         Value = x.RoleId.ToString(),
            //                                         Text = x.Display
            //                                     }).ToList();

            //    ViewBag.Roles = roleList;
            //}

            List<SelectListItem> empTypes = new List<SelectListItem>();
            foreach (string x in ConfigurationManager.AppSettings["EmpTypes"].ToString().Split(','))
            {
                empTypes.Add(new SelectListItem { Text=x, Value=x });
            }
            ViewBag.EmpTypes = empTypes;

            //List<SelectListItem> clients = new List<SelectListItem>();
            //foreach (string x in ConfigurationManager.AppSettings["Clients"].ToString().Split(','))
            //{
            //    clients.Add(new SelectListItem { Text = x, Value = x });
            //}
            //ViewBag.Clients = clients;

            List<SelectListItem> clients = new List<SelectListItem>();
            foreach (ClientObject x in ClientManager.Instance.GetClients().OrderBy(x => x.Name))
            {
                clients.Add(new SelectListItem { Text = x.Name, Value = x.Code });
            }
            ViewBag.Clients = clients;

            List<SelectListItem> empStatus = new List<SelectListItem>();
            foreach (string x in ConfigurationManager.AppSettings["EmpStatus"].ToString().Split(','))
            {
                empStatus.Add(new SelectListItem { Text = x, Value = x });
            }
            ViewBag.EmpStatus = empStatus;

        }
    }
}
