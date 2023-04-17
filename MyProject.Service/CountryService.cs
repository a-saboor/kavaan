using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            this._countryRepository = countryRepository;
            this._unitOfWork = unitOfWork;
        }

        #region ICountryService Members

        public IEnumerable<Country> GetCountries()
        {
            var countries = _countryRepository.GetAll().Where(i => i.IsDeleted == false);
            return countries;
        }

        public IEnumerable<object> GetCountriesForDropDown(string lang = "en")
        {
            var Countries = GetCountries().Where(x => x.IsActive == true);
            var dropdownList = from countries in Countries
                               select new { value = countries.ID, text = lang == "en" ? countries.Name : countries.NameAr };
            return dropdownList;
        }
        public Country GetCountry(long id)
        {
            var country = _countryRepository.GetById(id);
            return country;
        }
        public Country GetCountryByName(string name)
        {
            var country = _countryRepository.GetCountryByName(name);
            return country;
        }

        public bool CreateCountry(Country country, ref string message)
        {
            try
            {
                if (_countryRepository.GetCountryByName(country.Name) == null)
                {


                    country.IsActive = true;
                    country.IsDeleted = false;
                    country.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _countryRepository.Add(country);
                    if (SaveCountry())
                    {
                        message = "Country added successfully ...";
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
                    message = "Country already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateCountry(ref Country country, ref string message)
        {
            try
            {
                if (_countryRepository.GetCountryByName(country.Name, country.ID) == null)
                {
                    Country CurrentCountry = _countryRepository.GetById(country.ID);

                    CurrentCountry.Name = country.Name;
                    CurrentCountry.NameAr = country.NameAr;
                    country = null;

                    _countryRepository.Update(CurrentCountry);
                    if (SaveCountry())
                    {
                        country = CurrentCountry;
                        message = "Country updated successfully ...";
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
                    message = "Country already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteCountry(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Country country = _countryRepository.GetById(id);

                if (softDelete)
                {
                    country.IsDeleted = true;
                    _countryRepository.Update(country);
                }
                else
                {
                    _countryRepository.Delete(country);
                }
                if (SaveCountry())
                {
                    message = "Country deleted successfully ...";
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

        public bool SaveCountry()
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

        public bool PostExcelData(string Name, string NameAr)
        {
            try
            {
                DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                bool IsActive = true;
                bool IsDeleted = false;

                _countryRepository.InsertCountry(Name, NameAr, IsActive, CreatedOn, IsDeleted);
                return true;
            }
            catch (Exception ex)
            {
                //log.Error("Error", ex);
                //log.Error("Error", ex);
                return false;
            }
        }








        #endregion
    }

    public interface ICountryService
    {
        bool PostExcelData(string Name, string NameAr);
        IEnumerable<Country> GetCountries();
        IEnumerable<object> GetCountriesForDropDown(string lang = "en");
        Country GetCountry(long id);
        bool CreateCountry(Country country, ref string message);
        bool UpdateCountry(ref Country country, ref string message);
        bool DeleteCountry(long id, ref string message, bool softDelete = true);
        bool SaveCountry();
        Country GetCountryByName(string name);
    }
}
