using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class JobCandidateService : IJobCandidateService
    {
        private readonly IJobCandidatesRepository _jobJobCandidateRepository;
        private readonly IMail _email;
        private readonly IUnitOfWork _unitOfWork;

        public JobCandidateService(IJobCandidatesRepository jobJobCandidatesRepository, IMail email, IUnitOfWork unitOfWork)
        {
            this._jobJobCandidateRepository = jobJobCandidatesRepository;
            this._email = email;
            this._unitOfWork = unitOfWork;
        }

        public int GetJobCandidatesForDashboard()
        {
            var JobCandidates = _jobJobCandidateRepository.GetAll().Where(i => i.IsDeleted == false).ToList();
            return JobCandidates.Count();
        }
        public int GetApprovedJobCandidatesForDashboard()
        {
            var JobCandidates = _jobJobCandidateRepository.GetAll().Where(i => i.IsDeleted == false && i.Status == "Approved").ToList();
            return JobCandidates.Count();
        }
        public int GetRejectedJobCandidatesForDashboard()
        {
            var JobCandidates = _jobJobCandidateRepository.GetAll().Where(i => i.IsDeleted == false && i.Status == "Rejected").ToList();
            return JobCandidates.Count();
        }

        public IEnumerable<JobCandidate> GetJobCandidates()
        {
            var JobCandidates = _jobJobCandidateRepository.GetAll();
            return JobCandidates;
        }

        public JobCandidate GetJobCandidate(long id)
        {
            var JobCandidate = _jobJobCandidateRepository.GetById(id);
            return JobCandidate;
        }

        public bool CreateJobCandidate(JobCandidate candidate, ref string message)
        {
            try
            {
                candidate.IsDeleted = false;
                candidate.MarkAsRead = false;
                candidate.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _jobJobCandidateRepository.Add(candidate);
                if (SaveJobCandidate())
                {
                    message = "Thank you! Your cv has been successfully received.";
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

        public bool UpdateJobCandidate(JobCandidate candidate, ref string message, string path, bool sendMail = false)
        {
            try
            {
                JobCandidate CurrentCandidate = _jobJobCandidateRepository.GetById(candidate.ID);

                CurrentCandidate.MarkAsRead = candidate.MarkAsRead;
                CurrentCandidate.IsFlaged = candidate.IsFlaged;

                CurrentCandidate.MiddleName = candidate.MiddleName;
                CurrentCandidate.LastName = candidate.LastName;
                CurrentCandidate.Nationality = candidate.Nationality;
                CurrentCandidate.VISAStatus = candidate.VISAStatus;
                CurrentCandidate.Gender = candidate.Gender;
                CurrentCandidate.TotalExperience = candidate.TotalExperience;
                CurrentCandidate.UAEExperience = candidate.UAEExperience;
                CurrentCandidate.INTExperience = candidate.INTExperience;
                CurrentCandidate.Mobile1 = candidate.Mobile1;
                CurrentCandidate.Mobile2 = candidate.Mobile2;
                CurrentCandidate.NoticePeriod = candidate.NoticePeriod;
                CurrentCandidate.DrivingLicense = candidate.DrivingLicense;
                CurrentCandidate.Email = candidate.Email;
                CurrentCandidate.Address1 = candidate.Address1;
                CurrentCandidate.Address2 = candidate.Address2;
                CurrentCandidate.Cv = candidate.Cv;
                CurrentCandidate.MaritalStatus = candidate.MaritalStatus;

                CurrentCandidate.Status = candidate.Status;
                CurrentCandidate.Schedule = candidate.Schedule;

                candidate = null;

                _jobJobCandidateRepository.Update(CurrentCandidate);
                if (SaveJobCandidate())
                {
                    if (sendMail && CurrentCandidate.Status == "Approved")
                    {
                        if (_email.SendInterviewSchedule(CurrentCandidate.Email,
                            CurrentCandidate.FirstName + " " + CurrentCandidate.MiddleName + " " + CurrentCandidate.LastName,
                            CurrentCandidate.Schedule.Value.ToString("dd MMM yyyy, h: mm tt")
                            , path))
                        {
                            CurrentCandidate.IsEmailSent = true;
                            _jobJobCandidateRepository.Update(CurrentCandidate);
                            SaveJobCandidate();
                        }
                    }
                    else if (sendMail && CurrentCandidate.Status == "Rejected")
                    {
                        if (_email.SendInterviewSchedule(CurrentCandidate.Email,
                            CurrentCandidate.FirstName + " " + CurrentCandidate.MiddleName + " " + CurrentCandidate.LastName,
                            "nil"
                            , path))
                        {
                            CurrentCandidate.IsEmailSent = true;
                            _jobJobCandidateRepository.Update(CurrentCandidate);
                            SaveJobCandidate();
                        }
                    }
                    message = "Candidate updated successfully ...";
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

        public List<JobCandidate> GetJobCandidatesDateWise(DateTime FromDate, DateTime ToDate)
        {
            var JobCandidates = _jobJobCandidateRepository.GetFilteredJobCandidates(FromDate, ToDate);
            return JobCandidates;
        }

        public IEnumerable<SP_GetFilteredCandidates_Result> GetSPCandidates(int displayLength, int displayStart, int sortCol, string sortDir, string search, string imageServer, DateTime? FromDate = null, DateTime? ToDate = null)
        {
            var candidates = _jobJobCandidateRepository.GetSPCandidates(displayLength, displayStart, sortCol, sortDir, search, imageServer, FromDate, ToDate);
            return candidates;
        }

        public bool SaveJobCandidate()
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
    }
    public interface IJobCandidateService
    {
        bool CreateJobCandidate(JobCandidate candidate, ref string message);
        bool UpdateJobCandidate(JobCandidate candidate, ref string message, string path, bool sendMail);

        int GetJobCandidatesForDashboard();
        int GetApprovedJobCandidatesForDashboard();
        int GetRejectedJobCandidatesForDashboard();

        IEnumerable<JobCandidate> GetJobCandidates();
        List<JobCandidate> GetJobCandidatesDateWise(DateTime FromDate, DateTime ToDate);

        IEnumerable<SP_GetFilteredCandidates_Result> GetSPCandidates(int displayLength, int displayStart, int sortCol, string sortDir, string search, string imageServer, DateTime? FromDate = null, DateTime? ToDate = null);

        JobCandidate GetJobCandidate(long id);

        bool SaveJobCandidate();
    }
}
