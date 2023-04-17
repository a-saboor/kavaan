using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class CityService : ICityService
	{
		private readonly ICityRepository _cityRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CityService(ICityRepository cityRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
		{
			this._cityRepository = cityRepository;
			this._countryRepository = countryRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICityService Members

		public IEnumerable<City> GetCities()
		{
			var cities = _cityRepository.GetAll().Where(i => i.IsDeleted == false);
			return cities;
		}

		public IEnumerable<City> GetCities(long countryId)
		{
			var cities = _cityRepository.GetAllByCountry(countryId);
			return cities;
		}

		public IEnumerable<object> GetCitiesForDropDown(string lang = "en")
		{
			var Cities = GetCities().Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from cities in Cities
							   select new { value = cities.ID, text = lang == "en" ? cities.Name : cities.NameAR };
			return dropdownList;
		}

		public IEnumerable<object> GetCitiesForDropDown(long countryId, string lang = "en")
		{
			var Cities = _cityRepository.GetAllByCountry(countryId).Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from cities in Cities
							   select new { value = cities.ID, text = lang == "en" ? cities.Name : cities.NameAR };
			return dropdownList;
		}

		public City GetCity(long id)
		{
			var city = _cityRepository.GetById(id);
			return city;
		}

		public bool CreateCity(ref City city, ref string message)
		{
			try
			{
				if (_cityRepository.GetCityByName(city.Name) == null)
				{
					city.IsActive = true;
					city.IsDeleted = false;
					city.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_cityRepository.Add(city);
					if (SaveCity())
					{

						message = "City added successfully ...";
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
					message = "City already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateCity(ref City city, ref string message)
		{
			try
			{
				if (_cityRepository.GetCityByName(city.Name, city.ID) == null)
				{
					City CurrentCity = _cityRepository.GetById(city.ID);

					CurrentCity.Name = city.Name;
					CurrentCity.NameAR = city.NameAR;
					CurrentCity.CountryID = city.CountryID;

					city = null;

					_cityRepository.Update(CurrentCity);
					if (SaveCity())
					{
						city = CurrentCity;
						message = "City updated successfully ...";
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
					message = "City already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteCity(long id, ref string message, bool softDelete = true)
		{
			try
			{
				City city = _cityRepository.GetById(id);
				if (softDelete)
				{
					city.IsDeleted = true;
					_cityRepository.Update(city);
				}
				else
				{
					_cityRepository.Delete(city);
				}
				if (SaveCity())
				{
					message = "City deleted successfully ...";
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

		public bool SaveCity()
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

		public bool PostExcelData(string Name, string NameAr, string Country)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				bool IsDeleted = false;
				if (_countryRepository.GetCountryByName(Country) == null)
				{
					Country = null;
				}
					_cityRepository.InsertCity(Name, NameAr, Country, CreatedOn, IsDeleted);
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

	public interface ICityService
	{
		bool PostExcelData(string Name, string NameAr, string Country);
		IEnumerable<City> GetCities();
		IEnumerable<City> GetCities(long countryId);
		IEnumerable<object> GetCitiesForDropDown(string lang = "en");
		IEnumerable<object> GetCitiesForDropDown(long countryId, string lang = "en");
		City GetCity(long id);
		bool CreateCity(ref City city, ref string message);
		bool UpdateCity(ref City city, ref string message);
		bool DeleteCity(long id, ref string message, bool softDelete = true);
		bool SaveCity();
	}
}
