using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UnitTypeService : IUnitTypeService
    {
        private readonly IUnitTypeRepository _unitTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitTypeService(ICountryRepository countryRepository, IUnitOfWork unitOfWork, IUnitTypeRepository unitTypeRepository)
        {

            this._unitOfWork = unitOfWork;
            _unitTypeRepository = unitTypeRepository;
        }

        #region IUnitTypeService Members

        public IEnumerable<UnitType> GetUnits()
        {
            var unitTypes = _unitTypeRepository.GetAll().Where(i => i.IsDeleted == false);
            return unitTypes;
        }

        public IEnumerable<object> GetUnitTypeForDropDown(string lang = "en")
        {
            var Units = GetUnits().Where(x => x.IsActive == true);
            var dropdownList = from unit in Units
                               select new { value = unit.ID, text = lang == "en" ? unit.Name : unit.NameAr };
            return dropdownList;
        }
        public UnitType GetUnitType(long id)
        {
            var UnitType = _unitTypeRepository.GetById(id);
            return UnitType;
        }
        public UnitType GetUnitTypeByName(string name)
        {
            var UnitType = _unitTypeRepository.GetUnitTypeByName(name);
            return UnitType;
        }

        public bool CreateUnitType(ref UnitType UnitType, ref string message)
        {
            try
            {
                if (_unitTypeRepository.GetUnitTypeByName(UnitType.Name) == null)
                {


                    UnitType.IsActive = true;
                    UnitType.IsDeleted = false;
                    UnitType.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _unitTypeRepository.Add(UnitType);
                    if (SaveUnitType())
                    {
                        message = "UnitType added successfully ...";
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
                    message = "UnitType already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool UpdateUnitType(ref UnitType UnitType, ref string message)
        {
            try
            {
                if (_unitTypeRepository.GetUnitTypeByName(UnitType.Name, UnitType.ID) == null)
                {
                    UnitType CurrentUnitType = _unitTypeRepository.GetById(UnitType.ID);

                    CurrentUnitType.Name = UnitType.Name;
                    CurrentUnitType.NameAr = UnitType.NameAr;
                    UnitType = null;

                    _unitTypeRepository.Update(CurrentUnitType);
                    if (SaveUnitType())
                    {
                        UnitType = CurrentUnitType;
                        message = "UnitType updated successfully ...";
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
                    message = "UnitType already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! something went wrong ...";
                return false;
            }
        }

        public bool DeleteUnitType(long id, ref string message, bool softDelete = true)
        {
            try
            {
                UnitType UnitType = _unitTypeRepository.GetById(id);

                if (softDelete)
                {
                    UnitType.IsDeleted = true;
                    _unitTypeRepository.Update(UnitType);
                }
                else
                {
                    _unitTypeRepository.Delete(UnitType);
                }
                if (SaveUnitType())
                {
                    message = "UnitType deleted successfully ...";
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

        public bool SaveUnitType()
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

    public interface IUnitTypeService
    {

        IEnumerable<UnitType> GetUnits();
        IEnumerable<object> GetUnitTypeForDropDown(string lang = "en");
        UnitType GetUnitType(long id);
        bool CreateUnitType(ref UnitType UnitType, ref string message);
        bool UpdateUnitType(ref UnitType UnitType, ref string message);
        bool DeleteUnitType(long id, ref string message, bool softDelete = true);
        bool SaveUnitType();
        UnitType GetUnitTypeByName(string name);
    }
}
