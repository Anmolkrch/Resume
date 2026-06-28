using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TastehubModel.ViewModel
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public long UserTypeId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }
        [Remote("IsUserEmailExists", "User", ErrorMessage = "Email is already taken by someone", AdditionalFields = "Email,Id")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email *")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number *")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone format is not valid.")]
        public string PhoneNumber { get; set; }
        public string WorkPhone { get; set; }
        public string UserName { get; set; }
        public string SaltKey { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<bool> DefaultPassword { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<bool> IsOnLine { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public Nullable<long> ReportingTo { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> AssignedRegionId { get; set; }

        public virtual UserDetailViewModel UserDetail { get; set; }
        public virtual UserTypeViewModel UserType { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string UserTypeCode { get; set; }
        public string UserTypeName { get; set; }

        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

      
    }
}
