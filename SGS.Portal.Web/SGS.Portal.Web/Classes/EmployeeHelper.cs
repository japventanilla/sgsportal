using SGS.Business.Controllers;
using SGS.Data.EntityFrameworks;
using SGS.Portal.Web.Areas.Employee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Classes
{
    public static class EmployeeHelper
    {
        public static IEnumerable<EmployeeViewModel> GetAll()
        {
            IEnumerable<EmployeeViewModel> employees = null;

            employees = (from x in UserManager.Instance.GetAll()
                         select new EmployeeViewModel
                         {
                             UserId = x.UserId,
                             EmployeeId = x.EmployeeId,
                             FirstName = x.FirstName,
                             MiddleName = x.MiddleName,
                             LastName = x.LastName,
                             EmploymentType = x.EmploymentType,
                             Position = x.Position,
                             Client = x.Client,
                             Status = x.Status
                         });

            return employees;
        }

        public static IEnumerable<EmployeeViewModel> GetAllByClient(string client)
        {
            IEnumerable<EmployeeViewModel> employees = null;

            employees = (from x in UserManager.Instance.GetAllByClient(client)
                         select new EmployeeViewModel
                         {
                             UserId = x.UserId,
                             EmployeeId = x.EmployeeId,
                             FirstName = x.FirstName,
                             MiddleName = x.MiddleName,
                             LastName = x.LastName,
                             EmploymentType = x.EmploymentType,
                             Position = x.Position,
                             Client = x.Client,
                             Status = x.Status
                         });

            return employees;
        }

        public static EmployeeViewModel Get(int id)
        {
            EmployeeViewModel employee = new EmployeeViewModel();
            var item = UserManager.Instance.Get(id);

            if (item != null)
            {
                employee = BuildEmployee(item);
            }            

            return employee;
        }

        private static EmployeeViewModel BuildEmployee(User employee)
        {
            EmployeeViewModel result = new EmployeeViewModel();

            result.UserId = employee.UserId;
            result.EmployeeId = employee.EmployeeId;

            //Personal Details
            result.FirstName = employee.FirstName;
            result.MiddleName = employee.MiddleName;
            result.LastName = employee.LastName;
            result.NickName = employee.NickName;
            result.DateOfBirth = employee.DateOfBirth;
            result.PlaceOfBirth = employee.PlaceOfBirth;
            result.Gender = employee.Gender;
            result.MaritalStatus = employee.MaritalStatus;
            result.Nationality = employee.Nationality;
            result.PresentAddress = employee.PresentAddress;
            result.ProvincialAddress = employee.ProvincialAddress;
            result.Picture = employee.Picture;

            //Contact Details
            result.MobileNo = employee.MobileNo;
            result.LandLine = employee.LandlineNo;
            result.Email = employee.Email;

            //Emergency Contact
            result.ReferenceName = employee.Ref_Name;
            result.ReferenceRelationship = employee.Ref_Relationship;
            result.ReferenceContactNo = employee.Ref_ContactNo;
            result.ReferenceAddress = employee.Ref_Address;

            //Work Details
            result.EmploymentType = employee.EmploymentType;
            result.Client = employee.Client;
            result.Status = employee.Status;
            result.Position = employee.Position;
            result.DateHired = employee.DateHired;
            result.TaxIdNo = employee.TaxIdNo;
            result.SSSNo = employee.SSSNo;
            result.HDMFNo = employee.HDMFNo;
            result.PhilHealthNo = employee.PhilHealthNo;


            return result;
        }
    }
}