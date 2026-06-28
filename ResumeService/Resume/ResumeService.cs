using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Tastehub.Core.EntityModel;
using Tastehub.Model.ViewModel;

namespace Tastehub.Service.ResumeService
{
    public class ResumeService
    {
        private OnBoadTaskEntities _Context = new OnBoadTaskEntities();

        #region Resume_Methods

        public bool SaveResume(ResumeUploadViewModel resumeViewModel)
        {
            bool status = false;
            Resume resume = new Resume();
            try
            {
                Mapper.Map(resumeViewModel, resume);
                resume.UploadedOn = DateTime.Now;
                _Context.Resumes.Add(resume);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public List<ResumeUploadViewModel> GetResumeList(long userId)
        {
            var resumes = _Context.Resumes.Where(r => r.UserId == userId).ToList();
            var resumeViewModels = new List<ResumeUploadViewModel>();
            Mapper.Map(resumes, resumeViewModels);
            return resumeViewModels;
        }

        public bool SavePersonalDetail(PersonalDetailViewModel personalDetailViewModel)
        {
            bool status = false;
            PersonalDetail pd = new PersonalDetail();
            try
            {
                Mapper.Map(personalDetailViewModel, pd);
                _Context.PersonalDetails.Add(pd);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool SaveEducation(EducationViewModel educationViewModel)
        {
            bool status = false;
            Education edu = new Education();
            try
            {
                Mapper.Map(educationViewModel, edu);
                _Context.Educations.Add(edu);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool SaveExperience(ExperienceViewModel experienceViewModel)
        {
            bool status = false;
            Experience exp = new Experience();
            try
            {
                Mapper.Map(experienceViewModel, exp);
                _Context.Experiences.Add(exp);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public bool SaveSkill(SkillViewModel skillViewModel)
        {
            bool status = false;
            Skill skill = new Skill();
            try
            {
                Mapper.Map(skillViewModel, skill);
                _Context.Skills.Add(skill);
                _Context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return status;
        }

        public List<PersonalDetailViewModel> GetPersonalDetails(long userId)
        {
            var pdList = _Context.PersonalDetails.Where(x => x.UserId == userId).ToList();
            var pdVmList = new List<PersonalDetailViewModel>();
            Mapper.Map(pdList, pdVmList);
            return pdVmList;
        }

        public ResumeUploadViewModel GetResumeById(long resumeId)
        {
            var resume = _Context.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume == null) return null;

            var resumeVm = new ResumeUploadViewModel();
            Mapper.Map(resume, resumeVm);
            return resumeVm;
        }

        #endregion
    }
}
