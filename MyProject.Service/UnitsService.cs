using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository unitRepository;
        
        public UnitService(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
        {

            this._unitOfWork = unitOfWork;
            this.unitRepository = unitRepository;
           
        }

        #region IUnitService Members

        public IEnumerable<Unit> GetUnits()
        {
            var units = unitRepository.GetAll().Where(i => i.IsDeleted == false && i.Property.IsDeleted ==false);
            return units;
        }
        public int GetUnitsCount()
        {
            var unitsCount = GetUnits().Count();
            return unitsCount;
        }

        public IEnumerable<Unit> GetUnitsByProjectId(long projectId)
        {
            var units = unitRepository.GetAll().Where(i => i.IsDeleted == false && i.PropertyID == projectId);
            return units;
        }
        //to be done
        public IEnumerable<Unit> GetUnits(long unitid)
        {
            var units = unitRepository.GetAll();
            return units;
        }

        public IEnumerable<object> GetUnitsForDropDown()
        {
            var Units = GetUnits().Where(x => x.IsDeleted == false);
            var dropdownList = from units in Units
                               select new { value = units.ID, text = units.Title };
            return dropdownList;
        }

        //public IEnumerable<object> GetUnitsForDropDown(long countryId, string lang = "en")
        //{
        //	var Cities = _unitRepository.GetAllByCountry(countryId).Where(x => x.IsActive == true && x.IsDeleted == false);
        //	var dropdownList = from units in Cities
        //					   select new { value = units.ID, text = lang == "en" ? units.Name : units.NameAR };
        //	return dropdownList;
        //}

        public Unit GetUnit(long id)
        {
            var unit = unitRepository.GetById(id);
            return unit;
        }

        //public bool ActivateUnits(long propertyID, ref string message, bool isActive = true)
        //{
        //    try
        //    {
        //        var units = GetUnitsByProjectId(propertyID);

        //        foreach (var unit in units)
        //        {
        //            unit.IsActive = isActive;
        //            unitRepository.Update(unit);
        //        }

        //        if (SaveUnit())
        //        {
        //            message = "Unit updated successfully ...";
        //            return true;
        //        }
        //        message = "Oops! something went wrong ...";
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Oops Something Went Wrong";
        //        return false;
        //    }
        //}

        public bool CreateUnit(ref Unit unit, ref string message)
        {
            try
            {
                if (unitRepository.GetUnitByTitle(unit.Title) == null)
                {

                    unit.IsPublished = true;
                    unit.IsDeleted = false;
                    unit.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    unitRepository.Add(unit);
                    if (SaveUnit())
                    {

                        message = "Unit added successfully ...";
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
                    message = "Unit already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool UpdateUnit(ref Unit unit, ref string message)
        {
            try
            {
                if (unitRepository.GetUnitByTitle(unit.Title, unit.ID) == null)
                {
                    unitRepository.Update(unit);
                    if (SaveUnit())
                    {

                        message = "Unit updated successfully ...";
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
                    message = "Unit already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool DeleteUnit(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Unit unit = unitRepository.GetById(id);

                if (softDelete)
                {
                    unit.IsDeleted = true;
                    if (unit.UnitPaymentPlans!=null)
                    {
                        foreach (UnitPaymentPlan paymentPlan in unit.UnitPaymentPlans)
                        {
                            paymentPlan.IsDeleted = true;
                        }
                    }
                    unitRepository.Update(unit);
                }
                else
                {
                   
                    unitRepository.Delete(unit);

                }
                if (SaveUnit())
                {
                    message = "Unit deleted successfully ...";
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


        public bool PostExcelData(string Name, string NameAr, string Country)
        {
            try
            {
                DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                bool IsDeleted = false;
                if (unitRepository.GetUnitByTitle(Country) == null)
                {
                    Country = null;
                }
                //unitRepository.InsertUnit(Name, NameAr, Country, CreatedOn, IsDeleted);
                return true;

            }
            catch (Exception ex)
            {
                //log.Error("Error", ex);
                //log.Error("Error", ex);
                return false;
            }
        }

        public Unit GetUnitByName(string Name)
        {
            Unit unit = unitRepository.GetAll().Where(x => x.Title.Trim().ToLower() == Name.Trim().ToLower()).FirstOrDefault();
            return unit;
        }
        public bool SaveUnit()
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

        public IEnumerable<SP_GetFilteredUnits_Result> GetFilteredUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID)
        {
            var units = unitRepository.GetFilteredUnits(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID);
            return units;
        }

        public IEnumerable<SP_GetFilteredProjectUnits_Result> GetFilteredUnitsByProject(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type)
        {
            var units = unitRepository.GetFilteredProjectUnits(search, pageSize, pageNumber, sortBy, lang, imageServer, propertyID,  features,  amenities,  bedRooms,  bathRooms,minPrice,  maxPrice,  type);
            return units;
        }


        #endregion
    }

    public interface IUnitService
    {
        Unit GetUnitByName(string Name);
        bool PostExcelData(string Name, string NameAr, string Country);
        IEnumerable<Unit> GetUnits();
        int GetUnitsCount();
        IEnumerable<Unit> GetUnitsByProjectId(long projectId);
        IEnumerable<Unit> GetUnits(long id);
        IEnumerable<object> GetUnitsForDropDown();
        // IEnumerable<object> GetUnitsForDropDown(long countryId, string lang = "en");
        Unit GetUnit(long id);
        //bool ActivateUnits(long propertyID, ref string message, bool isActive = true);
        bool CreateUnit(ref Unit unit, ref string message);
        bool UpdateUnit(ref Unit unit, ref string message);
        bool DeleteUnit(long id, ref string message, bool softDelete = true);
        bool SaveUnit();

        IEnumerable<SP_GetFilteredUnits_Result> GetFilteredUnits(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID);

        IEnumerable<SP_GetFilteredProjectUnits_Result> GetFilteredUnitsByProject(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> propertyID, string features, string amenities, Nullable<int> bedRooms, Nullable<int> bathRooms, Nullable<decimal> minPrice, Nullable<decimal> maxPrice, Nullable<long> type);

    }
}
