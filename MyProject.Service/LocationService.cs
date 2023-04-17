using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LocationService(IUnitOfWork unitOfWork, ILocationRepository locationRepository)
        {
            this._unitOfWork = unitOfWork;
            this._locationRepository = locationRepository;
        }
        public IEnumerable<Location> GetLocations(long VendorID)
        {
            var locations = _locationRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return locations;
        }
        public Location GetLocation(long id)
        {
            var locations = this._locationRepository.GetById(id);
            return locations;
        }
        public IEnumerable<object> GetLocationsForDropDown(string lang = "en", long VendorID = 0)
        {

            var Locations = GetLocations(VendorID).Where(x => x.IsActive == true);
            var dropdownList = from location in Locations
                               select new { value = location.ID, text = lang == "en" ? location.OfficeName : location.OfficeName };
            return dropdownList;
        }
        public bool CreateLocation(Location location, ref string message)
        {
            try
            {

                location.IsActive = true;
                location.IsDeleted = false;
                location.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _locationRepository.Add(location);
                if (SaveLocation())
                {
                    message = "Location added successfully...";
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
        public bool UpdateLocation(ref Location location, ref string message)
        {
            try
            {
                Location currentContractor = _locationRepository.GetById(location.ID);
                currentContractor.IsActive = location.IsActive;

                _locationRepository.Update(currentContractor);
                if (SaveLocation())
                {
                    location = currentContractor;
                    message = "Location updated successfully ...";
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
        public bool DeleteLocation(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    Location location = _locationRepository.GetById(id);
                    //When category delete, its Contractor will be deleted
                    //*
                    location.IsDeleted = true;
                    _locationRepository.Update(location);

                    if (SaveLocation())
                    {
                        message = "Location deleted successfully ...";
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
            else
            {

                //hard delete
                try
                {
                    Location location = _locationRepository.GetById(id);

                    _locationRepository.Delete(location);

                    if (SaveLocation())
                    {
                        message = "Location deleted successfully ...";
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
        }
        public bool SaveLocation()
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
    public interface ILocationService
    {
        Location GetLocation(long id);
        IEnumerable<Location> GetLocations(long VendorID = 0);
        IEnumerable<object> GetLocationsForDropDown(string lang = "en", long VendorID = 0);
        bool CreateLocation(Location location, ref string message);
        bool UpdateLocation(ref Location location, ref string message);
        bool DeleteLocation(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveLocation();
    }
}
