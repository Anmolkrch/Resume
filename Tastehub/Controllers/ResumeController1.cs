using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Tastehub.Model.ViewModel;
using Tastehub.Service.ResumeService;
using Tastehub.Web.Helper;

namespace Tastehub.Controllers
{
    public class ResumeController1 : Controller
    {
        private readonly ResumeService _resumeService;

        // Parameterless constructor for MVC
        public ResumeController1()
        {
            _resumeService = new ResumeService();
        }

        // Constructor injection (if DI is configured)
        public ResumeController1(ResumeService resumeService)
        {
            _resumeService = resumeService;
        }
        public ActionResult UploadResume()
        {
            ResumeUploadViewModel model = new ResumeUploadViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResume(ResumeUploadViewModel model, HttpPostedFileBase ResumeFile)
        {
            if (ResumeFile != null && ResumeFile.ContentLength > 0)
            {
                //string path = Path.Combine(Server.MapPath("~/Resumes"), Path.GetFileName(ResumeFile.FileName));
                //ResumeFile.SaveAs(path);
                bool result; string path="";
                string extension = Path.GetExtension(ResumeFile.FileName);
                if (ResumeFile != null && ResumeFile.ContentLength > 0)
                {
                    string pic = $@"{Guid.NewGuid()}"+ extension;
                    string ResumePath = ConfigurationManager.AppSettings["ResumePath"].ToString();
                    path = System.IO.Path.Combine(Server.MapPath(ResumePath), pic);
                    ResumeFile.SaveAs(path);
                    model.FilePath = (ResumePath.Replace("~", "..")) + pic;
                }
                var resumeVm = new ResumeUploadViewModel
                {
                    UserId = int.Parse(UserAuthenticate.LogId),
                    FilePath = model.FilePath
                };
                _resumeService.SaveResume(resumeVm);

                string resumeText = ExtractText(path,extension);

                var emailMatch = Regex.Match(resumeText, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}");
                string email = emailMatch.Success ? emailMatch.Value : null;

                var phoneMatch = Regex.Match(resumeText, @"\+?\d{10,}");
                string phone = phoneMatch.Success ? phoneMatch.Value : null;

                var pdVm = new PersonalDetailViewModel
                {
                    UserId = resumeVm.UserId,
                    Email = email,
                    Phone = phone
                };
                _resumeService.SavePersonalDetail(pdVm);
                GetOtherDetails(resumeText, resumeVm.UserId);
                TempData["Message"] = "Resume uploaded and parsed successfully.";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Please upload a valid resume file.");
            return View(model);
        }


        public ActionResult ManageResumes()
        {
            var userId = int.Parse(UserAuthenticate.LogId);
            var resumes = _resumeService.GetResumeList(userId);
            var personalDetails = _resumeService.GetPersonalDetails(userId);

            var model = new ManageResumeViewModel
            {
                Resumes = resumes,
                PersonalDetails = personalDetails
            };

            return View(model);
        }

        public FileResult DownloadResume(long resumeId)
        {
            var resume = _resumeService.GetResumeById(resumeId);
            if (resume != null && System.IO.File.Exists(resume.FilePath))
            {
                string fileName = Path.GetFileName(resume.FilePath);
                return File(resume.FilePath, "application/octet-stream", fileName);
            }
            return null;
        }

        private string ExtractText(string path, string fileExtension)
        {
            fileExtension = fileExtension.ToLower();

            try
            {
                switch (fileExtension)
                {
                    case ".pdf":
                        return ExtractTextFromPdf(path);

                    case ".docx":
                        return ExtractTextFromDocx(path);

                    case ".doc":
                        // Optional: Handle .doc using Microsoft.Office.Interop.Word or NPOI
                        // For now return empty
                        return "";

                    default:
                        return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        private string ExtractTextFromPdf(string path)
        {
            StringBuilder text = new StringBuilder();

            PdfReader reader = null;
            try
            {
                reader = new PdfReader(path);
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return text.ToString();
        }
        private string ExtractTextFromDocx(string path)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
            {
                return wordDoc.MainDocumentPart.Document.Body.InnerText;
            }
        }
        private void GetOtherDetails(string resumeText,long userId)
        {
            // Education parsing
            //var educationMatches = Regex.Matches(resumeText,
            //    @"(?<Degree>B\.Tech|MCA|MBA|B\.Sc|M\.Sc|Bachelor|Master)[^\n]*\n?(?<Institution>[A-Za-z\s]+)\n?(?<Year>\d{4})");
            var educationMatches = Regex.Matches(resumeText,
            @"(?<Degree>[A-Za-z\.\s]+)\s*,?\s*(?<Institution>[A-Za-z\s&]+)\s*,?\s*(?<Year>\d{4})",
            RegexOptions.IgnoreCase);

            foreach (Match match in educationMatches)
            {
                var eduVm = new EducationViewModel
                {
                    UserId = userId,
                    Degree = match.Groups["Degree"].Value,
                    Institution = match.Groups["Institution"].Value,
                    YearOfPassing = int.TryParse(match.Groups["Year"].Value, out var year) ? year : 0
                };
                _resumeService.SaveEducation(eduVm);
            }

            // Experience parsing
            //var expMatches = Regex.Matches(resumeText,
            //    @"(?<Company>[A-Za-z\s]+)\s+(?<Designation>[A-Za-z\s]+)\s+[^\n]*\n?(?<Start>\w+\s\d{4})\s*-\s*(?<End>\w+\s\d{4}|Present)",
            //    RegexOptions.IgnoreCase);
            var expMatches = Regex.Matches(resumeText,
            @"(?<Company>[A-Za-z&.,\-\s]{2,50})\s+(?<Designation>[A-Za-z&.,\-\s]{2,50})\s+(?<Start>[A-Za-z]{3,9}\s\d{4})\s*-\s*(?<End>[A-Za-z]{3,9}\s\d{4}|Present)",
            RegexOptions.IgnoreCase);

            foreach (Match match in expMatches)
            {
                var expVm = new ExperienceViewModel
                {
                    UserId = userId,
                    CompanyName = match.Groups["Company"].Value,
                    Designation = match.Groups["Designation"].Value,
                    StartDate = DateTime.TryParse(match.Groups["Start"].Value, out var start) ? start : DateTime.MinValue,
                    EndDate = DateTime.TryParse(match.Groups["End"].Value, out var end) ? end : DateTime.MinValue,
                    Responsibilities = "" // optional: extract following lines
                };
                _resumeService.SaveExperience(expVm);
            }

            // Skills parsing
            var skillSection = Regex.Match(resumeText, @"Skills\s*[:\-]?(?<Skills>.+)", RegexOptions.IgnoreCase);
            if (skillSection.Success)
            {
                var skills = skillSection.Groups["Skills"].Value.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var skill in skills)
                {
                    var skillVm = new SkillViewModel
                    {
                        UserId = userId,
                        SkillName = skill.Trim(),
                        ProficiencyLevel = "Intermediate" // default, or infer if mentioned
                    };
                    _resumeService.SaveSkill(skillVm);
                }
            }

        }
    }
}
