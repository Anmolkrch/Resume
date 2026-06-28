using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tastehub.Model.ViewModel
{
    public class ResumeUploadViewModel
    {
        //public HttpPostedFileBase ResumeFile { get; set; }
        public string FilePath {get;set;}
        public int UserId { get; set; }
        public int Id { get; set; }
    }

    public class PersonalDetailViewModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
    }

    public class EducationViewModel
    {
        public long UserId { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int YearOfPassing { get; set; }
        public string Grade { get; set; }
    }

    public class ExperienceViewModel
    {
        public long UserId { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Responsibilities { get; set; }
    }

    public class SkillViewModel
    {
        public long UserId { get; set; }
        public string SkillName { get; set; }
        public string ProficiencyLevel { get; set; }
    }
    public class ManageResumeViewModel
    {
        public List<ResumeUploadViewModel> Resumes { get; set; }
        public List<PersonalDetailViewModel> PersonalDetails { get; set; }
    }

}
