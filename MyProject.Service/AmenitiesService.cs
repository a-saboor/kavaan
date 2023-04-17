using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenitiesRepository amenitiesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IUnitOfWork unitOfWork, IAmenitiesRepository amenitiesRepository)
        {

            this._unitOfWork = unitOfWork;
            this.amenitiesRepository = amenitiesRepository;
        }

        #region ICountryService Members

        public IEnumerable<Amenity> GetAmenites()
        {
            var amenity = this.amenitiesRepository.GetAll().Where(i => i.IsDeleted == false);
            return amenity;
        }

        public IEnumerable<object> GetAmenitiesForDropDown(string lang = "en")
        {
            var amenities = GetAmenites().Where(x => x.IsActive == true);
            var dropdownList = from amenity in amenities
                               select new { value = amenity.ID, text = lang == "en" ? amenity.Name : amenity.NameAr };
            return dropdownList;
        }
        public Amenity GetAmenity(long id)
        {
            var Amenity = this.amenitiesRepository.GetById(id);
            return Amenity;
        }
        public Amenity GetAmenityByName(string name)
        {
            var Amenity = this.amenitiesRepository.GetAmenitiesByName(name);
            return Amenity;
        }

        public bool CreateAmenity(Amenity Amenity, ref string message)
        {
            try
            {
                if (this.amenitiesRepository.GetAmenitiesByName(Amenity.Name) == null)
                {


                    Amenity.IsActive = true;
                    Amenity.IsDeleted = false;
                    Amenity.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    this.amenitiesRepository.Add(Amenity);
                    if (SaveData())
                    {
                        message = "Amenity added successfully ...";
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
                    message = "Amenity already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateAmenity(ref Amenity Amenity, ref string message)
        {
            try
            {
                if (this.amenitiesRepository.GetAmenitiesByName(Amenity.Name, Amenity.ID) == null)
                {
                    Amenity CurrentCountry = this.amenitiesRepository.GetById(Amenity.ID);

                    CurrentCountry.Name = Amenity.Name;
                    CurrentCountry.NameAr = Amenity.NameAr;
                    Amenity = null;

                    this.amenitiesRepository.Update(CurrentCountry);
                    if (SaveData())
                    {
                        Amenity = CurrentCountry;
                        message = "Amenity updated successfully ...";
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
                    message = "Amenity already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteAmenity(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Amenity Amenity = this.amenitiesRepository.GetById(id);

                if (softDelete)
                {
                    Amenity.IsDeleted = true;
                    this.amenitiesRepository.Update(Amenity);
                }
                else
                {
                    this.amenitiesRepository.Delete(Amenity);
                }
                if (SaveData())
                {
                    message = "Amenity deleted successfully ...";
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

        //public bool PostExcelData(string Name, string NameAr)
        //{
        //    try
        //    {
        //        DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
        //        bool IsActive = true;
        //        bool IsDeleted = false;

        //         this.amenitiesRepository .InsertCountry(Name, NameAr, IsActive, CreatedOn, IsDeleted);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.Error("Error", ex);
        //        //log.Error("Error", ex);
        //        return false;
        //    }
        //}








        #endregion
    }

    public interface IAmenityService
    {
        //  bool PostExcelData(string Name, string NameAr);
        IEnumerable<Amenity> GetAmenites();
        IEnumerable<object> GetAmenitiesForDropDown(string lang = "en");
        Amenity GetAmenity(long id);
        bool CreateAmenity(Amenity Amenity, ref string message);
        bool UpdateAmenity(ref Amenity Amenity, ref string message);
        bool DeleteAmenity(long id, ref string message, bool softDelete = true);
        bool SaveData();
        Amenity GetAmenityByName(string name);
    }
}
