using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class AreaService : IAreaService
	{
		private readonly IAreaRepository _areaRepository;
		private readonly ICountryRepository _countryRepository;
		private readonly ICityRepository _cityRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AreaService(IAreaRepository areaRepository, ICountryRepository countryRepository, ICityRepository cityRepository, IUnitOfWork unitOfWork)
		{
			this._areaRepository = areaRepository;
			this._countryRepository = countryRepository;
			this._cityRepository = cityRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IAreaService Members

		public IEnumerable<Area> GetAreas()
		{
			var areas = _areaRepository.GetAll().Where(i => i.IsDeleted == false);
			return areas;
		}

		public IEnumerable<Area> GetAreas(long cityId)
		{
			var areas = _areaRepository.GetAllByCity(cityId);
			return areas;
		}

		public IEnumerable<object> GetAreasForDropDown(string lang = "en")
		{
			var Areas = _areaRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from areas in Areas
							   select new { value = areas.ID, text = lang == "en" ? areas.Name : areas.NameAR };
			return dropdownList;
		}

		public IEnumerable<object> GetAreasForDropDown(long cityId, string lang = "en")
		{
			var areas = _areaRepository.GetAllByCity(cityId).Where(x => x.IsActive == true && x.IsDeleted == false);
			var dropdownList = from area in areas
							   select new { value = area.ID, text = lang == "en" ? area.Name : area.NameAR };
			return dropdownList;
		}

		public Area GetArea(long id)
		{
			var area = _areaRepository.GetById(id);
			return area;
		}

		public bool CreateArea(Area area, ref string message)
		{
			try
			{
				if (_areaRepository.GetAreaByName((long)area.CityID, area.Name) == null)
				{


					area.IsActive = true;
					area.IsDeleted = false;
					area.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_areaRepository.Add(area);
					if (SaveArea())
					{
						message = "Area added successfully ...";
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
					message = "Area already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateArea(ref Area area, ref string message)
		{
			try
			{
				if (_areaRepository.GetAreaByNameOnly(area.Name, area.ID) == null)
				{
					Area CurrentArea = _areaRepository.GetById(area.ID);

					CurrentArea.Name = area.Name;
					CurrentArea.NameAR = area.NameAR;
					CurrentArea.CountryID = area.CountryID;
					CurrentArea.CityID = area.CityID;
					area = null;

					_areaRepository.Update(CurrentArea);
					if (SaveArea())
					{
						area = CurrentArea;
						message = "Area updated successfully ...";
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
					message = "Area already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteArea(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Area area = _areaRepository.GetById(id);

				if (softDelete)
				{
					area.IsDeleted = true;
					_areaRepository.Update(area);
				}
				else
				{
					_areaRepository.Delete(area);
				}
				if (SaveArea())
				{
					message = "Area deleted successfully ...";
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

		public bool SaveArea()
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

		public bool PostExcelData(string Name, string NameAr,string CountryName, string CityName)
		{
			try
			{
				DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				bool IsActive = true;
				bool IsDeleted = false;
				var countryData = _countryRepository.GetCountryByName(CountryName);
				var cityData = _cityRepository.GetCityByName(CityName);
				
				if(_areaRepository.InsertArea(Name, NameAr, CountryName, CityName, IsActive, CreatedOn, IsDeleted))
				{
					return true;
				}
				else
				{
					return false;
				}
				
				
				
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

	public interface IAreaService
	{
		bool PostExcelData(string Name, string NameAr, string CountryName, string CityName);
		IEnumerable<Area> GetAreas();
		IEnumerable<Area> GetAreas(long cityId);
		IEnumerable<object> GetAreasForDropDown(string lang = "en");
		IEnumerable<object> GetAreasForDropDown(long cityId, string lang = "en");
		Area GetArea(long id);
		bool CreateArea(Area area, ref string message);
		bool UpdateArea(ref Area area, ref string message);
		bool DeleteArea(long id, ref string message, bool softDelete = true);
		bool SaveArea();
	}
}
