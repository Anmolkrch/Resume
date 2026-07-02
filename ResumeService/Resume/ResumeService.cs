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
            var query = _Context.Resumes.AsQueryable();

            // If userId is not 0, filter by userId
            if (userId != 0)
            {
                query = query.Where(r => r.UserId == userId);
            }

            var resumes = query.ToList();

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
            var query = _Context.PersonalDetails.AsQueryable();

            // Only filter if userId is not 0
            if (userId != 0)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var pdList = query.ToList();

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

        #region Resume CRUD

        public bool UpdateResume(ResumeUploadViewModel resumeViewModel)
        {
            try
            {
                var resume = _Context.Resumes.FirstOrDefault(r => r.Id == resumeViewModel.Id);
                if (resume == null) return false;

                Mapper.Map(resumeViewModel, resume);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // log exception
                return false;
            }
        }

        public bool DeleteResume(long resumeId)
        {
            try
            {
                var resume = _Context.Resumes.FirstOrDefault(r => r.Id == resumeId);
                if (resume == null) return false;

                _Context.Resumes.Remove(resume);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // log exception
                return false;
            }
        }

        #endregion

        #region PersonalDetail CRUD

        public bool UpdatePersonalDetail(PersonalDetailViewModel personalDetailViewModel)
        {
            try
            {
                var pd = _Context.PersonalDetails.FirstOrDefault(x => x.Id == personalDetailViewModel.Id);
                if (pd == null) return false;

                Mapper.Map(personalDetailViewModel, pd);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeletePersonalDetail(long id)
        {
            try
            {
                var pd = _Context.PersonalDetails.FirstOrDefault(x => x.Id == id);
                if (pd == null) return false;

                _Context.PersonalDetails.Remove(pd);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Education CRUD

        public List<EducationViewModel> GetEducationList(long userId)
        {
            var query = _Context.Educations.Where(e => e.UserId == userId).ToList();
            var vmList = new List<EducationViewModel>();
            Mapper.Map(query, vmList);
            return vmList;
        }

        public bool UpdateEducation(EducationViewModel educationViewModel)
        {
            try
            {
                var edu = _Context.Educations.FirstOrDefault(e => e.Id == educationViewModel.Id);
                if (edu == null) return false;

                Mapper.Map(educationViewModel, edu);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteEducation(long id)
        {
            try
            {
                var edu = _Context.Educations.FirstOrDefault(e => e.Id == id);
                if (edu == null) return false;

                _Context.Educations.Remove(edu);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Experience CRUD

        public List<ExperienceViewModel> GetExperienceList(long userId)
        {
            var query = _Context.Experiences.Where(e => e.UserId == userId).ToList();
            var vmList = new List<ExperienceViewModel>();
            Mapper.Map(query, vmList);
            return vmList;
        }

        public bool UpdateExperience(ExperienceViewModel experienceViewModel)
        {
            try
            {
                var exp = _Context.Experiences.FirstOrDefault(e => e.Id == experienceViewModel.Id);
                if (exp == null) return false;

                Mapper.Map(experienceViewModel, exp);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteExperience(long id)
        {
            try
            {
                var exp = _Context.Experiences.FirstOrDefault(e => e.Id == id);
                if (exp == null) return false;

                _Context.Experiences.Remove(exp);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Skill CRUD

        public List<SkillViewModel> GetSkillList(long userId)
        {
            var query = _Context.Skills.Where(s => s.UserId == userId).ToList();
            var vmList = new List<SkillViewModel>();
            Mapper.Map(query, vmList);
            return vmList;
        }

        public bool UpdateSkill(SkillViewModel skillViewModel)
        {
            try
            {
                var skill = _Context.Skills.FirstOrDefault(s => s.Id == skillViewModel.Id);
                if (skill == null) return false;

                Mapper.Map(skillViewModel, skill);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteSkill(long id)
        {
            try
            {
                var skill = _Context.Skills.FirstOrDefault(s => s.Id == id);
                if (skill == null) return false;

                _Context.Skills.Remove(skill);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

    }
}
