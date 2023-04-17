using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UnitEnquiriesService : IUnitEnquiriesService
    {
        private readonly IUnitEnquiriesRepository unitEnquiriesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitEnquiriesService(IUnitEnquiriesRepository unitEnquiriesRepository, IUnitOfWork unitOfWork)
        {
            this.unitEnquiriesRepository = unitEnquiriesRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IUnitEnquiryService Members

        public IEnumerable<UnitEnquiry> GetUnitEnquiry(DateTime FromDate, DateTime ToDate)
        {
            var paymentPlans = this.unitEnquiriesRepository.GetAll().Where( i => i.CreatedOn >= FromDate && i.CreatedOn <= ToDate);
            return paymentPlans;
        }

        public UnitEnquiry GetUnitEnquiry(long id)
        {
            var unitenquiry = this.unitEnquiriesRepository.GetById(id);
            return unitenquiry;
        }
        //public UnitEnquiry GetUnitEnquiryByName(string name)
        //{
        //    var paymentplan = this.unitEnquiriesRepository.GetUnitEnquiryeByName(name);
        //    return paymentplan;
        //}

        public bool CreateUnitEnquiry( ref UnitEnquiry unitEnquiry, ref string message)
        {
            try
            {
                //if (this.unitEnquiriesRepository.GetUnitEnquiryeByName(paymentPlan.Milestones) == null)
                //{
                    unitEnquiry.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    unitEnquiry.Status = null;
                    
                    this.unitEnquiriesRepository.CreateUnitEnquiry(ref unitEnquiry,ref message);
                 
                    if (SaveUnitEnquiry())
                    {
                        message = "Unit enquiry submitted successfully ...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
               // }
                //else
                //{
                //    message = "Unit Payment Plan MileStone already exist  ...";
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateUnitEnquiry(ref UnitEnquiry unitEnquiry, ref string message)
        {
            try
            {
                //if (this.unitEnquiriesRepository.GetUnitEnquiryeByName(unitEnquiry.Milestones, unitEnquiry.ID) == null)
                //{
                    UnitEnquiry CurrentUnitEnquiry = this.unitEnquiriesRepository.GetById(unitEnquiry.ID);

                    CurrentUnitEnquiry.Status = unitEnquiry.Status;
                  

                    this.unitEnquiriesRepository.Update(CurrentUnitEnquiry);
                    if (SaveUnitEnquiry())
                    {
                        unitEnquiry = null;
                        unitEnquiry = CurrentUnitEnquiry;
                        message = "Unit Payment Plan updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
               // }
                //else
                //{
                //    message = "Unit Payment Plan already exist  ...";
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        //public bool DeleteUnitEnquiry(long id, ref string message, bool softDelete = true)
        //{
        //    try
        //    {
        //        UnitEnquiry unitenquiry = this.unitEnquiriesRepository.GetById(id);

        //        if (softDelete)
        //        {
        //            unitenquiry.IsDeleted = true;
        //            this.unitEnquiriesRepository.Update(unitenquiry);
        //        }
        //        else
        //        {
        //            this.unitEnquiriesRepository.Delete(unitenquiry);
        //        }
        //        if (SaveUnitEnquiry())
        //        {
        //            message = "UnitEnquiry deleted successfully ...";
        //            return true;
        //        }
        //        else
        //        {
        //            message = "Oops! Something went wrong. Please try later...";
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Oops! Something went wrong. Please try later...";
        //        return false;
        //    }
        //}

        public bool SaveUnitEnquiry()
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

    public interface IUnitEnquiriesService
    {

        IEnumerable<UnitEnquiry> GetUnitEnquiry(DateTime FromDate, DateTime ToDate);
        UnitEnquiry GetUnitEnquiry(long id);
        bool CreateUnitEnquiry(ref UnitEnquiry unitenquiry, ref string message);
        bool UpdateUnitEnquiry(ref UnitEnquiry unitenquiry, ref string message);
    //    bool DeleteUnitEnquiry(long id, ref string message, bool softDelete = true);
        bool SaveUnitEnquiry();
      //  UnitEnquiry GetUnitEnquiryByName(string name);
    }
}
