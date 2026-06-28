using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Packaging;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Tastehub.Model.ViewModel;
using Tastehub.Service.ResumeService;
using Tastehub.Web.Helper;

namespace Tastehub.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeService _resumeService;

        private readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".doc",
            ".docx",
            ".txt"
        };

        private const int MaxFileSize = 5 * 1024 * 1024;

        public ResumeController()
        {
            _resumeService = new ResumeService();
        }

        public ResumeController(ResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        #region Upload Resume

        public ActionResult UploadResume()
        {
            return View(new ResumeUploadViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResume(
            ResumeUploadViewModel model,
            HttpPostedFileBase ResumeFile)
        {
            try
            {
                string message;

                if (!ValidateResume(ResumeFile, out message))
                {
                    ModelState.AddModelError("", message);
                    return View(model);
                }

                string relativePath;

                string physicalPath = SaveResumeFile(
                    ResumeFile,
                    out relativePath);

                model.UserId = Convert.ToInt32(UserAuthenticate.LogId);
                model.FilePath = relativePath;

                _resumeService.SaveResume(model);

                string resumeText = ExtractText(
                    physicalPath,
                    Path.GetExtension(physicalPath));

                if (!String.IsNullOrWhiteSpace(resumeText))
                {
                    SavePersonalDetails(
                        resumeText,
                        model.UserId);

                    GetOtherDetails(
                        resumeText,
                        model.UserId);
                }

                TempData["Message"] =
                    "Resume uploaded successfully.";

                return RedirectToAction(
                    "ManageResumes",
                    "Resume");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);

                return View(model);
            }
        }

        #endregion

        #region Manage Resume

        public ActionResult ManageResumes()
        {
            int userId =
                Convert.ToInt32(UserAuthenticate.LogId);

            ManageResumeViewModel model =
                new ManageResumeViewModel
                {
                    Resumes =
                        _resumeService.GetResumeList(userId),

                    PersonalDetails =
                        _resumeService.GetPersonalDetails(userId)
                };

            return View(model);
        }

        #endregion

        #region Download Resume

        public ActionResult DownloadResume(long resumeId)
        {
            var resume =
                _resumeService.GetResumeById(resumeId);

            if (resume == null)
                return HttpNotFound();

            string filePath =
                Server.MapPath(
                    resume.FilePath.Replace("..", "~"));

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound();

            return File(
                filePath,
                MimeMapping.GetMimeMapping(filePath),
                Path.GetFileName(filePath));
        }

        #endregion

        #region Save Personal Details

        private void SavePersonalDetails(
            string resumeText,
            long userId)
        {
            string email = "";

            Match emailMatch = Regex.Match(
                resumeText,
                @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}");

            if (emailMatch.Success)
                email = emailMatch.Value;

            string phone = "";

            Match phoneMatch = Regex.Match(
                resumeText,
                @"(\+91[- ]?)?[6-9]\d{9}");

            if (phoneMatch.Success)
                phone = phoneMatch.Value;

            PersonalDetailViewModel vm =
                new PersonalDetailViewModel
                {
                    UserId = userId,
                    Email = email,
                    Phone = phone
                };

            _resumeService.SavePersonalDetail(vm);
        }

        #endregion
        #region Resume Validation

        private bool ValidateResume(HttpPostedFileBase file, out string message)
        {
            message = "";

            if (file == null || file.ContentLength == 0)
            {
                message = "Please select a resume.";
                return false;
            }

            string extension = Path.GetExtension(file.FileName).ToLower();

            if (!AllowedExtensions.Contains(extension))
            {
                message = "Only PDF, DOC, DOCX and TXT files are allowed.";
                return false;
            }

            if (file.ContentLength > MaxFileSize)
            {
                message = "Resume size should not exceed 5 MB.";
                return false;
            }

            return true;
        }

        #endregion

        #region Save Resume

        private string SaveResumeFile(HttpPostedFileBase file,
            out string relativePath)
        {
            string extension = Path.GetExtension(file.FileName);

            string fileName = file.FileName; //Guid.NewGuid() + extension;

            string uploadFolder =
                ConfigurationManager.AppSettings["ResumePath"];

            if (!uploadFolder.EndsWith("/"))
                uploadFolder += "/";

            string folder =
                Server.MapPath(uploadFolder);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string physicalPath =
                Path.Combine(folder, fileName);

            file.SaveAs(physicalPath);

            relativePath =
                uploadFolder.Replace("~", "..") + fileName;

            return physicalPath;
        }

        #endregion

        #region Extract Resume Text

        private string ExtractText(string path, string extension)
        {
            extension = extension.ToLower();

            switch (extension)
            {
                case ".pdf":
                    return ExtractTextFromPdf(path);

                case ".docx":
                    return ExtractTextFromDocx(path);

                case ".txt":
                    return ExtractTextFromTxt(path);

                case ".doc":
                    return ExtractTextFromDoc(path);

                default:
                    return "";
            }
        }

        #endregion

        #region PDF Reader

        private string ExtractTextFromPdf(string path)
        {
            StringBuilder sb = new StringBuilder();

            PdfReader reader = null;
            try
            {
                reader = new PdfReader(path);
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    sb.AppendLine(
                        PdfTextExtractor.GetTextFromPage(
                            reader,
                            i));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return sb.ToString();
        }

        #endregion

        #region DOCX Reader

        private string ExtractTextFromDocx(string path)
        {
            try
            {
                using (WordprocessingDocument word =
                    WordprocessingDocument.Open(path, false))
                {
                    if (word.MainDocumentPart == null)
                        return "";

                    return word.MainDocumentPart
                        .Document
                        .Body
                        .InnerText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "";
            }
        }

        #endregion

        #region TXT Reader

        private string ExtractTextFromTxt(string path)
        {
            try
            {
                return System.IO.File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "";
            }
        }

        #endregion

        #region DOC Reader

        private string ExtractTextFromDoc(string path)
        {
            try
            {
                // Legacy DOC support.
                // Recommended:
                // Aspose.Words
                // NPOI
                // Spire.Doc

                return "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "";
            }
        }

        #endregion

        #region Resume Parser


        #endregion

        #region Skills

        private void ParseSkills(string text, long userId)
        {
            try
            {
                Match skillSection = Regex.Match(
                    text,
                    @"Skills?\s*[:\-]?(?<Skills>[\s\S]*?)(Education|Experience|Projects|Certification|Languages|$)",
                    RegexOptions.IgnoreCase);

                if (!skillSection.Success)
                    return;

                string allSkills = string.Join(", ",
                    skillSection.Groups["Skills"].Value
                        .Split(new[] { ',', ';', '\n', '\r', '|', '•', '-' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                );

                if (!string.IsNullOrWhiteSpace(allSkills))
                {
                    _resumeService.SaveSkill(new SkillViewModel
                    {
                        UserId = userId,
                        SkillName = allSkills,
                        ProficiencyLevel = "Intermediate"
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Education

        private void ParseEducation(string resumeText, long userId)
        {
            try
            {
                Match section = Regex.Match(
                    resumeText,
                    @"Education\s*[:\-]?(?<Data>[\s\S]*?)(Experience|Skills|Projects|Certification|Languages|Personal Details|$)",
                    RegexOptions.IgnoreCase);

                if (!section.Success)
                    return;

                string educationText = section.Groups["Data"].Value;

                string[] lines = educationText
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

                List<string> degreeKeywords = new List<string>
        {
            "Bachelor of Computer Science",
            "Bachelor of Computer Applications",
            "Bachelor of Engineering",
            "Bachelor of Technology",
            "Bachelor of Science",
            "Bachelor of Commerce",
            "Bachelor of Arts",

            "Master of Computer Science",
            "Master of Computer Applications",
            "Master of Engineering",
            "Master of Technology",
            "Master of Science",
            "Master of Commerce",
            "Master of Arts",
            "Master of Business Administration",

            "B.Tech",
            "M.Tech",
            "B.E.",
            "M.E.",
            "BE",
            "ME",
            "BCA",
            "MCA",
            "MBA",
            "BBA",
            "B.Com",
            "M.Com",
            "B.Sc",
            "M.Sc",
            "BA",
            "MA",
            "LLB",
            "LLM",
            "Diploma",
            "Polytechnic",
            "PhD",
            "MBBS",
            "BDS"
        };

                EducationViewModel current = null;

                foreach (string line in lines)
                {
                    // New education starts
                    string degree = degreeKeywords
                        .FirstOrDefault(x =>
                            line.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (!string.IsNullOrEmpty(degree))
                    {
                        if (current != null)
                        {
                            _resumeService.SaveEducation(current);
                        }

                        current = new EducationViewModel
                        {
                            UserId = userId,
                            Degree = degree
                        };

                        Match year = Regex.Match(line, @"(19|20)\d{2}");

                        if (year.Success)
                            current.YearOfPassing = Convert.ToInt32(year.Value);

                        continue;
                    }

                    if (current == null)
                        continue;

                    // Year
                    if (current.YearOfPassing == 0)
                    {
                        Match year = Regex.Match(line, @"(19|20)\d{2}");

                        if (year.Success)
                        {
                            current.YearOfPassing = Convert.ToInt32(year.Value);
                            continue;
                        }
                    }

                    // Percentage
                    Match percent = Regex.Match(line, @"\d{1,2}(\.\d+)?\s*%");

                    if (percent.Success)
                    {
                        continue;
                    }

                    // CGPA
                    Match cgpa = Regex.Match(line, @"CGPA\s*[:\-]?\s*\d+(\.\d+)?",
                        RegexOptions.IgnoreCase);

                    if (cgpa.Success)
                    {
                        continue;
                    }

                    // Institution
                    if (string.IsNullOrWhiteSpace(current.Institution))
                    {
                        if (!Regex.IsMatch(line, @"(19|20)\d{2}") &&
                            line.Length > 5)
                        {
                            current.Institution = line;
                        }
                    }
                }

                if (current != null)
                    _resumeService.SaveEducation(current);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        #endregion

        #region Experience

        private void ParseExperience(string text, long userId)
        {
            try
            {
                var matches = Regex.Matches(
                    text,
                    @"(?<Company>[A-Za-z0-9&.,\-\s]{3,80})\s+(?<Designation>Software Engineer|Developer|Consultant|Architect|Lead|Manager|Analyst|Tester|Programmer|Engineer|Administrator)[\s\S]{0,30}?(?<Start>(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)?\s?\d{4})\s*[-–]\s*(?<End>(Present|(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)?\s?\d{4}))",
                    RegexOptions.IgnoreCase);

                foreach (Match item in matches)
                {
                    DateTime.TryParse(item.Groups["Start"].Value, out DateTime start);

                    DateTime.TryParse(item.Groups["End"].Value.Replace("Present", DateTime.Now.ToString("MMM yyyy")), out DateTime end);

                    _resumeService.SaveExperience(
                        new ExperienceViewModel
                        {
                            UserId = userId,
                            CompanyName = item.Groups["Company"].Value.Trim(),
                            Designation = item.Groups["Designation"].Value.Trim(),
                            StartDate = start,
                            EndDate = end,
                            Responsibilities = ""
                        });
                }
            }
            catch
            {
            }
        }

        #endregion

        #region Certifications

        private void ParseCertifications(string text, long userId)
        {
            try
            {
                Match cert = Regex.Match(
                    text,
                    @"Certification[s]?\s*[:\-]?(?<Data>[\s\S]*?)(Projects|Education|Experience|Languages|$)",
                    RegexOptions.IgnoreCase);

                if (!cert.Success)
                    return;

                // Future table insertion goes here.
                // Currently only extracted.
                string certifications = cert.Groups["Data"].Value.Trim();
            }
            catch
            {
            }
        }

        #endregion

        #region Projects

        private void ParseProjects(string text, long userId)
        {
            try
            {
                Match project = Regex.Match(
                    text,
                    @"Projects?\s*[:\-]?(?<Data>[\s\S]*?)(Experience|Education|Certification|Languages|$)",
                    RegexOptions.IgnoreCase);

                if (!project.Success)
                    return;

                // Future table insertion
                string projects = project.Groups["Data"].Value.Trim();
            }
            catch
            {
            }
        }

        #endregion

        #region Resume Parser

        private void GetOtherDetails(string resumeText, long userId)
        {
            if (String.IsNullOrWhiteSpace(resumeText))
                return;

            ParseSkills(resumeText, userId);

            ParseEducation(resumeText, userId);

            ParseExperience(resumeText, userId);

            ParseCertifications(resumeText, userId);

            ParseProjects(resumeText, userId);
        }

        #endregion

    }
}