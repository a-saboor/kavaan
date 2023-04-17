using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UnitPaymentPlanService : IUnitPaymentPlanService
    {
        private readonly IUnitPaymentPlanRepository unitPaymentPlanRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitPaymentPlanService(IUnitPaymentPlanRepository unitPaymentPlanRepository, IUnitOfWork unitOfWork, IUnitService unitService)
        {
            this.unitPaymentPlanRepository = unitPaymentPlanRepository;
            this._unitOfWork = unitOfWork;

        }

        #region IUnitPaymentPlanService Members

        public IEnumerable<UnitPaymentPlan> GetUnitPaymentPlan()
        {
            var paymentPlans = this.unitPaymentPlanRepository.GetAll().Where(x => x.IsDeleted == false);
            return paymentPlans;
        }

        public UnitPaymentPlan GetUnitPaymentPlan(long id)
        {
            var paymentPlan = this.unitPaymentPlanRepository.GetById(id);
            return paymentPlan;
        }

        public IEnumerable<UnitPaymentPlan> GetPaymentPlanByUnitID(long id)
        {
            var paymentPlan = this.unitPaymentPlanRepository.GetUnitPaymentPlaneByUnitId(id);
            return paymentPlan;
        }

        public bool CreateUnitPaymentPlan(ref UnitPaymentPlan paymentPlan, ref string message)
        {
            try
            {
                if (this.unitPaymentPlanRepository.GetUnitPaymentPlaneByName(paymentPlan.Milestones, (long)paymentPlan.UnitID) == null)
                {



                    paymentPlan.CreatedOn = Helpers.TimeZone.GetLocalDateTime();

                    this.unitPaymentPlanRepository.CreateUnitPaymentPlan(ref paymentPlan, ref message);

                    if (SaveUnitPaymentPlan())
                    {
                        message = "Unit payment plan added successfully ...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! something went wrong ...";
                        return false;
                    }
                }
                else
                {
                    message = "Unit payment plan milestone already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool UpdateUnitPaymentPlan(ref UnitPaymentPlan unitPaymentPlan, ref string message)
        {
            try
            {
                if (this.unitPaymentPlanRepository.GetUnitPaymentPlaneByName(unitPaymentPlan.Milestones, (long)unitPaymentPlan.UnitID, unitPaymentPlan.ID) == null)
                {
                    UnitPaymentPlan CurrentUnitPaymentPlan = this.unitPaymentPlanRepository.GetById(unitPaymentPlan.ID);
                    CurrentUnitPaymentPlan.UnitID = unitPaymentPlan.UnitID;
                    CurrentUnitPaymentPlan.Milestones = unitPaymentPlan.Milestones;
                    CurrentUnitPaymentPlan.Percentage = unitPaymentPlan.Percentage;
                    CurrentUnitPaymentPlan.Amount = unitPaymentPlan.Amount;


                    this.unitPaymentPlanRepository.Update(CurrentUnitPaymentPlan);
                    if (SaveUnitPaymentPlan())
                    {
                        unitPaymentPlan = null;
                        unitPaymentPlan = CurrentUnitPaymentPlan;
                        message = "Unit payment plan updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! something went wrong ...";
                        return false;
                    }
                }
                else
                {
                    message = "Unit payment plan already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool DeleteUnitPaymentPlan(long id, ref string message, bool softDelete = true)
        {
            try
            {
                UnitPaymentPlan unitPaymentPlan = this.unitPaymentPlanRepository.GetById(id);

                if (softDelete)
                {
                    unitPaymentPlan.IsDeleted = true;
                    this.unitPaymentPlanRepository.Update(unitPaymentPlan);
                }
                else
                {
                    this.unitPaymentPlanRepository.Delete(unitPaymentPlan);
                }
                if (SaveUnitPaymentPlan())
                {
                    message = "Unit payment plan deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! something went wrong ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool SaveUnitPaymentPlan()
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

        public IEnumerable<UnitPaymentPlan> GetUnitPaymentPlans(long unitid)

        {

            var paymentPlans = this.unitPaymentPlanRepository.GetAll().Where(x => x.IsDeleted == false && x.UnitID == unitid);
            return paymentPlans;
        }










        #endregion
    }

    public interface IUnitPaymentPlanService
    {

        IEnumerable<UnitPaymentPlan> GetUnitPaymentPlan();
        IEnumerable<UnitPaymentPlan> GetUnitPaymentPlans(long unitid);
        IEnumerable<UnitPaymentPlan> GetPaymentPlanByUnitID(long id);
        UnitPaymentPlan GetUnitPaymentPlan(long id);
        bool CreateUnitPaymentPlan(ref UnitPaymentPlan paymentPlan, ref string message);
        bool UpdateUnitPaymentPlan(ref UnitPaymentPlan paymentPlan, ref string message);
        bool DeleteUnitPaymentPlan(long id, ref string message, bool softDelete = true);
        bool SaveUnitPaymentPlan();
        //UnitPaymentPlan GetUnitPaymentPlanByName(string name);
    }
}
