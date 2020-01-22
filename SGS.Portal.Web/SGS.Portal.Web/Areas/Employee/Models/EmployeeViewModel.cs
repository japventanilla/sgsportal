using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SGS.Portal.Web.Areas.Employee.Models
{
    public class EmployeeViewModel
    {
        public int UserId { get; set; }
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        [StringLength(30)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(30)]
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName + " " + MiddleName;
            }
        }

        [Required(ErrorMessage = "Nick Name is required")]
        [StringLength(15)]
        public string NickName { get; set; }

        public string Gender { get; set; }
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthString
        {
            get
            {
                if (this.DateOfBirth != null)
                {
                    return this.DateOfBirth.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [Required(ErrorMessage = "Birth Place is required")]
        [StringLength(250)]
        public string PlaceOfBirth { get; set; }

        [Required(ErrorMessage = "Present Address is required")]
        [StringLength(250)]
        public string PresentAddress { get; set; }

        public string ProvincialAddress { get; set; }

        public string LandLine { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(20)]
        public string MobileNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required(ErrorMessage = "Reference Name is required")]
        [StringLength(100)]
        public string ReferenceName { get; set; }

        [Required(ErrorMessage = "Reference Address is required")]
        [StringLength(250)]
        public string ReferenceAddress { get; set; }

        [Required(ErrorMessage = "Reference Relationship is required")]
        [StringLength(20)]
        public string ReferenceRelationship { get; set; }

        [Required(ErrorMessage = "Reference Contact No is required")]
        [StringLength(20)]
        public string ReferenceContactNo { get; set; }

        public string TaxIdNo { get; set; }
        public string SSSNo { get; set; }
        public string HDMFNo { get; set; }
        public string PhilHealthNo { get; set; }
        public int RoleId { get; set; }
        public string Status { get; set; }
        public string EmploymentType { get; set; }
        public string Client { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(150)]
        public string Position { get; set; }
        
        public DateTime? DateHired { get; set; }
        public string DateHiredString
        {
            get
            {
                if (this.DateHired != null)
                {
                    return this.DateHired.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string Picture { get; set; }
        public string PictureUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this.Picture))
                    return System.Configuration.ConfigurationManager.AppSettings["NoProfileImage"].ToString();
                else
                    return System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString() + this.Picture;
            }
        }
    }
}