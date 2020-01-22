using SGS.Common.Controllers;
using SGS.Data.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS.Business.Controllers
{
    public class UserManager
    {
        private static UserManager instance;
        public static UserManager Instance
        {
            get
            {
                if (instance == null) instance = new UserManager();
                return instance;
            }
        }

        private UserManager()
        {

        }

        public User Get(int id)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Users
                        where x.UserId == id
                        select x).FirstOrDefault();
            }
        }

        public User Get(string empId, string password)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                //string pwdEncript = EncryptionManager.encrypt(password);
                User result = null;
                User item = (from x in db.Users
                                where x.EmployeeId == empId && x.Password == password
                                    && x.Status == "Active" && x.RecState == "A"
                                select x).FirstOrDefault();

                if (item != null)
                {
                    result = new User();
                    result.UserId = item.UserId;
                    result.EmployeeId = item.EmployeeId;
                    result.LastName = item.LastName;
                    result.FirstName = item.FirstName;
                    result.MiddleName = item.MiddleName;
                    result.NickName = item.NickName;
                    result.EmploymentType = item.EmploymentType;
                    result.RoleId = item.RoleId;
                    result.RecState = item.RecState;
                    result.Picture = item.Picture;
                }

                return result;
            }
        }

        public IEnumerable<User> GetAllByClient(string client)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Users
                        where x.RecState == "A" && x.Client == client
                        select x).ToList();
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Users
                        where x.RecState == "A"
                        select x).ToList();
            }
        }

        public int GetDefaultRole()
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return db.Roles.Where(x => x.RoleName == "User").First().RoleId;
            }
        }

        public int SaveUser(User user)
        {
            int memberId = user.UserId;

            using (SGSDBEntities db = new SGSDBEntities())
            {
                int nextId = db.Users.Where(x => x.Client == user.Client).Count() + 1;
                User item = db.Users.SingleOrDefault(x => x.UserId == user.UserId);

                if (item == null)
                    item = new User();

                item.FirstName = user.FirstName;
                item.MiddleName = user.MiddleName;
                item.LastName = user.LastName;
                item.NickName = user.NickName;
                item.Gender = user.Gender;
                item.MaritalStatus = user.MaritalStatus;
                item.DateOfBirth = user.DateOfBirth;
                item.PlaceOfBirth = user.PlaceOfBirth;
                item.PresentAddress = user.PresentAddress;
                item.ProvincialAddress = user.ProvincialAddress;
                item.LandlineNo = user.LandlineNo;
                item.MobileNo = user.MobileNo;
                item.Email = user.Email;
                item.Nationality = user.Nationality;
                item.Ref_Name = user.Ref_Name;
                item.Ref_Relationship = user.Ref_Relationship;
                item.Ref_Address = user.Ref_Address;
                item.Ref_ContactNo = user.Ref_ContactNo;
                item.TaxIdNo = user.TaxIdNo;
                item.SSSNo = user.SSSNo;
                item.HDMFNo = user.HDMFNo;
                item.PhilHealthNo = user.PhilHealthNo;                
                item.Status = user.Status;
                item.EmploymentType = user.EmploymentType;
                item.Position = user.Position;
                item.Client = user.Client;
                item.DateHired = user.DateHired;

                if (user.Picture != null)
                    item.Picture = user.Picture;

                if (item.UserId == 0)
                {
                    item.RoleId = GetDefaultRole();

                    if(item.Client == "HO")
                        item.EmployeeId = String.Format("E-{0:D4}", nextId);
                    else
                        item.EmployeeId = String.Format("C-" + item.Client + "-{0:D3}", nextId);

                    item.Password = user.Password;
                    item.CreatedDate = DateHelper.DateTimeNow;
                    item.CreatedBy = user.CreatedBy;
                    item.RecState = "A";
                    db.Users.Add(item);
                }
                else
                {
                    item.ModifiedBy = user.ModifiedBy;
                    item.ModifiedDate = DateHelper.DateTimeNow;
                }

                db.SaveChanges();
                memberId = item.UserId;
            }            

            return memberId;

        }

        public IEnumerable<Role> GetAllRoles()
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                return (from x in db.Roles
                        select x).ToList();
            }
        }

        public string[] GetRoles(string userName)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                int memberId = int.Parse(userName);
                User user = db.Users.FirstOrDefault(u => u.UserId == memberId && u.RecState == "A");
                if (user != null)
                    return new string[] { user.Role.RoleName };
                else
                    return new string[] { };
            }
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            using (SGSDBEntities db = new SGSDBEntities())
            {
                User user = db.Users.FirstOrDefault(u => u.EmployeeId.Equals(userName, StringComparison.CurrentCultureIgnoreCase) && u.RecState == "A" && u.Role.RoleName == roleName);
                if (user != null)
                    return true;
                else
                    return false;
            }
        }

    }
}
