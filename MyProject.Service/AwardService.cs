using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class AwardService : IAwardService
    {
        private readonly IAwardRepository _awardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AwardService(IUnitOfWork unitOfWork, IAwardRepository awardRepository)
        {

            this._unitOfWork = unitOfWork;
            this._awardRepository = awardRepository;
        }


        #region IAwardService Members

        public IEnumerable<Award> GetAwards()
        {
            var award = this._awardRepository.GetAll().Where(i => i.IsDeleted == false);
            return award;
        }

        public IEnumerable<object> GetAwardForDropDown(string lang = "en")
        {
            var awards = GetAwards().Where(x => x.IsActive == true);
            var dropdownList = from award in awards
                               select new { value = award.ID, text = lang == "en" ? award.Title : award.TitleAr };
            return dropdownList;
        }
        public Award GetAward(long id)
        {
            var award = this._awardRepository.GetById(id);
            return award;
        }



        public bool CreateAward(Award award, ref string message)
        {
            try
            {
                if (this._awardRepository.GetAwardsByName(award.Title) == null)
                {
                    award.IsActive = true;
                    award.IsDeleted = false;
                    award.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    this._awardRepository.Add(award);
                    if (SaveData())
                    {
                        message = "Award added successfully ...";
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
                    message = "Award already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateAward(ref Award award, ref string message)
        {
            try
            {
                if (this._awardRepository.GetAwardsByName(award.Title, award.ID) == null)
                {
                    Award CurrentAward = this._awardRepository.GetById(award.ID);

                    CurrentAward.Title = award.Title;
                    CurrentAward.TitleAr = award.TitleAr;
                    CurrentAward.Description = award.Description;
                    CurrentAward.DescriptionAr = award.DescriptionAr;
                    CurrentAward.Image = award.Image;
                    CurrentAward.Position = award.Position;
                    award = null;

                    this._awardRepository.Update(CurrentAward);
                    if (SaveData())
                    {
                        award = CurrentAward;
                        message = "Award updated successfully ...";
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
                    message = "Award already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteAward(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Award award = this._awardRepository.GetById(id);

                if (softDelete)
                {
                    award.IsDeleted = true;
                    this._awardRepository.Update(award);
                }
                else
                {
                    this._awardRepository.Delete(award);
                }
                if (SaveData())
                {
                    message = "Award deleted successfully ...";
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

        public bool SaveData()
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

    public interface IAwardService
    {
        IEnumerable<Award> GetAwards();
        IEnumerable<object> GetAwardForDropDown(string lang = "en");
        Award GetAward(long id);
        bool CreateAward(Award award, ref string message);
        bool UpdateAward(ref Award award, ref string message);
        bool DeleteAward(long id, ref string message, bool softDelete = true);
        bool SaveData();
    }
}
