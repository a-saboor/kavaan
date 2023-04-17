using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class EmailSettingService : IEmailSettingService
    {
        private readonly IEmailSettingRepository _emailSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailSettingService(IEmailSettingRepository emailRepository, IUnitOfWork unitOfWork)
        {
            this._emailSettingRepository = emailRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IEmailSettingService Members

        public IEnumerable<EmailSetting> GetEmailSettings()
        {
            var emails = _emailSettingRepository.GetAll();
            return emails;
        }

        public EmailSetting GetEmailSetting(long id)
        {
            var email = _emailSettingRepository.GetById(id);
            return email;
        }

        public EmailSetting GetDefaultEmailSetting()
        {
            var email = _emailSettingRepository.GetAll().FirstOrDefault();
            return email;
        }

        public bool CreateEmailSetting(EmailSetting email, ref string message)
        {
            try
            {
                email.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _emailSettingRepository.Add(email);
                if (SaveEmailSetting())
                {
                    message = "EmailSetting added successfully ...";
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

        public bool UpdateEmailSetting(ref EmailSetting email, ref string message)
        {
            try
            {
                EmailSetting CurrentEmailSetting = _emailSettingRepository.GetById(email.ID);

                CurrentEmailSetting.EmailAddress = email.EmailAddress;
                CurrentEmailSetting.ContactEmail = email.ContactEmail;
                CurrentEmailSetting.EnableSSL = email.EnableSSL;
                CurrentEmailSetting.Host = email.Host;
                CurrentEmailSetting.Password = email.Password;
                CurrentEmailSetting.Port = email.Port;
                email = null;

                _emailSettingRepository.Update(CurrentEmailSetting);
                if (SaveEmailSetting())
                {
                    email = CurrentEmailSetting;
                    message = "EmailSetting updated successfully ...";
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

        public bool DeleteEmailSetting(long id, ref string message)
        {
            try
            {
                EmailSetting email = _emailSettingRepository.GetById(id);

                _emailSettingRepository.Delete(email);
                if (SaveEmailSetting())
                {
                    message = "EmailSetting deleted successfully ...";
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

        public bool SaveEmailSetting()
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

    public interface IEmailSettingService
    {
        IEnumerable<EmailSetting> GetEmailSettings();
        EmailSetting GetEmailSetting(long id);
        EmailSetting GetDefaultEmailSetting();
        bool CreateEmailSetting(EmailSetting email, ref string message);
        bool UpdateEmailSetting(ref EmailSetting email, ref string message);
        bool DeleteEmailSetting(long id, ref string message);
        bool SaveEmailSetting();
    }
}

