using System;
using System.Collections.Generic;
using System.Linq;
using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;

namespace MyProject.Service
{
    public class ShippingClassService : IShippingClassService
    {
        private readonly IShippingClassRepository _shippingClassRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShippingClassService(IShippingClassRepository shippingClassRepository, IUnitOfWork unitOfWork)
        {
            this._shippingClassRepository = shippingClassRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IShippingClassService Members

        public IEnumerable<ShippingClass> GetShippingClasses()
        {
            var shippingClasses = _shippingClassRepository.GetAll();
            return shippingClasses;
        }

        public IEnumerable<object> GetShippingClassesForDropDown(string lang = "en")
        {
            var Classes = GetShippingClasses().Where(x => x.IsDeleted == false);
            var dropdownList = from classes in Classes
                               select new { value = classes.ID, text = classes.Name };
            return dropdownList;
        }

        public ShippingClass GetShippingClass(long id)
        {
            var shippingClass = _shippingClassRepository.GetById(id);
            return shippingClass;
        }

        public bool CreateShippingClass(ShippingClass shippingClass, ref string message)
        {
            try
            {
                if (_shippingClassRepository.GetShippingClassByName(shippingClass.Name) == null)
                {
                    shippingClass.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    shippingClass.IsActive = true;
                    shippingClass.IsDeleted = false;

                    _shippingClassRepository.Add(shippingClass);
                    if (SaveShippingClass())
                    {
                        message = "Shipping class added successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later.";
                        return false;
                    }
                }
                else
                {
                    message = "Shipping class already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool UpdateShippingClass(ref ShippingClass shippingClass, ref string message)
        {
            try
            {
                if (_shippingClassRepository.GetShippingClassByName(shippingClass.Name, shippingClass.ID) == null)
                {
                    ShippingClass CurrentShippingClass = _shippingClassRepository.GetById(shippingClass.ID);

                    CurrentShippingClass.Name = shippingClass.Name;
                    CurrentShippingClass.Slug = shippingClass.Slug;
                    CurrentShippingClass.Description = shippingClass.Description;

                    shippingClass = null;

                    _shippingClassRepository.Update(CurrentShippingClass);
                    if (SaveShippingClass())
                    {
                        shippingClass = CurrentShippingClass;
                        message = "Shipping class updated successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later.";
                        return false;
                    }
                }
                else
                {
                    message = "Shipping class already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool DeleteShippingClass(long id, ref string message, bool softDelete = true)
        {
            try
            {
                ShippingClass shippingClass = _shippingClassRepository.GetById(id);

                _shippingClassRepository.Delete(shippingClass);

                if (SaveShippingClass())
                {
                    message = "Shipping class deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool SaveShippingClass()
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

    public interface IShippingClassService
    {
        IEnumerable<ShippingClass> GetShippingClasses();
        IEnumerable<object> GetShippingClassesForDropDown(string lang = "en");
        ShippingClass GetShippingClass(long id);
        bool CreateShippingClass(ShippingClass shippingClass, ref string message);
        bool UpdateShippingClass(ref ShippingClass shippingClass, ref string message);
        bool DeleteShippingClass(long id, ref string message, bool softDelete = true);
        bool SaveShippingClass();
    }
}
