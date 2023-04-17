using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

    public class JobOpeningService : IJobOpeningService
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobOpeningService(IJobOpeningRepository jobOpeningRepository, IUnitOfWork unitOfWork)
        {
            this._jobOpeningRepository = jobOpeningRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IJobOpeningService Members

        public IEnumerable<JobOpening> GetJobOpenings()
        {
            var jobOpenings = _jobOpeningRepository.GetAll().Where(i => i.IsDeleted == false);
            return jobOpenings;
        }
        public int GetJobOpeningForDashboard()
        {
            var jobOpenings = _jobOpeningRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
            return jobOpenings.Sum(i => i.Position).Value;
        }
        
        public IEnumerable<JobOpening> GetJobOpeningByCategory(long categoryId)
        {
            var jobOpenings = _jobOpeningRepository.GetJobOpeningByCategory(categoryId);
            return jobOpenings;
        }

        public IEnumerable<object> GetJobOpeningsForDropDown(string lang = "en")
        {
            var JobOpenings = GetJobOpenings().Where(x => x.IsActive == true);
            var dropdownList = from jobOpenings in JobOpenings
                               select new { value = jobOpenings.ID, text = lang == "en" ? jobOpenings.Title : jobOpenings.TitleAr };
            return dropdownList;
        }

        public JobOpening GetJobOpening(long id)
        {
            var jobOpening = _jobOpeningRepository.GetById(id);
            return jobOpening;
        }

        public JobOpening GetJobOpeningByName(string name)
        {
            var jobOpening = _jobOpeningRepository.GetJobOpeningByTitle(name);
            return jobOpening;
        }

        public bool CreateJobOpening(JobOpening jobOpening, ref string message)
        {
            try
            {
                if (_jobOpeningRepository.GetJobOpeningByCategoryAndTitle((long)jobOpening.CategoryID, jobOpening.Title) == null)
                {
                    jobOpening.IsActive = true;
                    jobOpening.IsDeleted = false;
                    jobOpening.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _jobOpeningRepository.Add(jobOpening);
                    if (SaveJobOpening())
                    {
                        message = "Job opening added successfully ...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "Job opening already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateJobOpening(ref JobOpening jobOpening, ref string message)
        {
            try
            {
                if (_jobOpeningRepository.GetJobOpeningByCategoryAndTitle((long)jobOpening.CategoryID, jobOpening.Title, jobOpening.ID) == null)
                {

                    JobOpening CurrentJobOpening = _jobOpeningRepository.GetById(jobOpening.ID);

                    CurrentJobOpening.Title = jobOpening.Title;
                    CurrentJobOpening.TitleAr = jobOpening.TitleAr;
                    CurrentJobOpening.Requirements = jobOpening.Requirements;
                    CurrentJobOpening.RequirementsAr = jobOpening.RequirementsAr;
                    CurrentJobOpening.LastDate = jobOpening.LastDate;
                    CurrentJobOpening.Location = jobOpening.Location;
                    CurrentJobOpening.Position = jobOpening.Position;
                    CurrentJobOpening.CategoryID = jobOpening.CategoryID;

                    jobOpening = null;

                    _jobOpeningRepository.Update(CurrentJobOpening);
                    if (SaveJobOpening())
                    {
                        jobOpening = CurrentJobOpening;
                        message = "Job opening updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "Job opening already exist  ...";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteJobOpening(long id, ref string message, bool softDelete = true)
        {
            try
            {
                JobOpening jobOpening = _jobOpeningRepository.GetById(id);

                if (softDelete)
                {
                    jobOpening.IsDeleted = true;
                    _jobOpeningRepository.Update(jobOpening);
                }
                else
                {
                    _jobOpeningRepository.Delete(jobOpening);
                }
                if (SaveJobOpening())
                {
                    message = "Job opening deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool SaveJobOpening()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public interface IJobOpeningService
    {
        IEnumerable<JobOpening> GetJobOpenings();

        int GetJobOpeningForDashboard();
        IEnumerable<JobOpening> GetJobOpeningByCategory(long categoryId);
        IEnumerable<object> GetJobOpeningsForDropDown(string lang = "en");
        JobOpening GetJobOpening(long id);
        bool CreateJobOpening(JobOpening jobOpening, ref string message);
        bool UpdateJobOpening(ref JobOpening jobOpening, ref string message);
        bool DeleteJobOpening(long id, ref string message, bool softDelete = true);
        bool SaveJobOpening();
        JobOpening GetJobOpeningByName(string name);
    }
}
