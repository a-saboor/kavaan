using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PartnerService(IUnitOfWork unitOfWork, IPartnerRepository partnerRepository)
        {

            this._unitOfWork = unitOfWork;
            this._partnerRepository = partnerRepository;
        }

        #region IPartnerService Members

        public IEnumerable<Partner> GetPartners()
        {
            var partner = this._partnerRepository.GetAll().Where(i => i.IsDeleted == false);
            return partner;
        }

        public IEnumerable<object> GetPartnerForDropDown(string lang = "en")
        {
            var partners = GetPartners().Where(x => x.IsActive == true);
            var dropdownList = from partner in partners
                               select new { value = partner.ID, text = lang == "en" ? partner.Name : partner.NameAr };
            return dropdownList;
        }
        public Partner GetPartner(long id)
        {
            var Partner = this._partnerRepository.GetById(id);
            return Partner;
        }

        public IEnumerable<SP_GetFilteredPartners_Result> GetFilteredPartners(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var partners = _partnerRepository.GetFilteredPartners(search, pageSize, pageNumber, sortBy, lang, imageServer);
            return partners;
        }

        public bool CreatePartner(Partner partner, ref string message)
        {
            try
            {
                if (this._partnerRepository.GetPartnersByName(partner.Name) == null)
                {


                    partner.IsActive = true;
                    partner.IsDeleted = false;
                    partner.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    this._partnerRepository.Add(partner);
                    if (SaveData())
                    {
                        message = "Partner added successfully ...";
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
                    message = "Partner already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdatePartner(ref Partner partner, ref string message)
        {
            try
            {
                if (this._partnerRepository.GetPartnersByName(partner.Name, partner.ID) == null)
                {
                    Partner CurrentPartner = this._partnerRepository.GetById(partner.ID);

                    CurrentPartner.Name = partner.Name;
                    CurrentPartner.NameAr = partner.NameAr;
                    CurrentPartner.Description = partner.Description;
                    CurrentPartner.DescriptionAr = partner.DescriptionAr;
                    CurrentPartner.Image = partner.Image;
                    CurrentPartner.Position = partner.Position;
                    partner = null;

                    this._partnerRepository.Update(CurrentPartner);
                    if (SaveData())
                    {
                        partner = CurrentPartner;
                        message = "Partner updated successfully ...";
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
                    message = "Partner already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeletePartner(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Partner partner = this._partnerRepository.GetById(id);

                if (softDelete)
                {
                    partner.IsDeleted = true;
                    this._partnerRepository.Update(partner);
                }
                else
                {
                    this._partnerRepository.Delete(partner);
                }
                if (SaveData())
                {
                    message = "Partner deleted successfully ...";
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
    public interface IPartnerService
    {
       
        IEnumerable<Partner> GetPartners();
        IEnumerable<object> GetPartnerForDropDown(string lang = "en");
        Partner GetPartner(long id);
        bool CreatePartner(Partner partner, ref string message);
        bool UpdatePartner(ref Partner partner, ref string message);
        bool DeletePartner(long id, ref string message, bool softDelete = true);
        bool SaveData();

        IEnumerable<SP_GetFilteredPartners_Result> GetFilteredPartners(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

    }
}
