using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class OzoneService :IOzoneService
	{
		private readonly IOzoneRepository _ozoneRepository;
		private readonly IUnitOfWork _unitOfWork;

		public OzoneService(IUnitOfWork unitOfWork, IOzoneRepository ozoneRepository)
		{

			this._unitOfWork = unitOfWork;
			this._ozoneRepository = ozoneRepository;
		}
		#region IOzoneService Members

		public IEnumerable<Ozone> GetOzones()
		{
			var ozone = this._ozoneRepository.GetAll().Where(i => i.IsDeleted == false);
			return ozone;
		}
		public Ozone GetOzonefirstordefault()
		{
			var ozone = this._ozoneRepository.GetAll().Where(i => i.IsDeleted == false).FirstOrDefault();
			return ozone;
		}
		public Ozone GetOzone(long id)
		{
			var ozone = this._ozoneRepository.GetById(id);
			return ozone;
		}
		public bool CreateOzone(Ozone ozone, ref string message)
		{
			try
			{
				ozone.IsActive = true;
				ozone.IsDeleted = false;
				ozone.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				this._ozoneRepository.Add(ozone);
				if (SaveData())
				{
					message = "Ozone added successfully ...";
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

		public bool UpdateOzone(ref Ozone ozone, ref string message)
		{
			try
			{
				Ozone CurrentOzone = GetOzonefirstordefault();

				CurrentOzone.Description = ozone.Description;
				CurrentOzone.DescriptionAr = ozone.DescriptionAr;
				if (ozone.Thumbnail != null)
				{

					CurrentOzone.Thumbnail = ozone.Thumbnail;
				}


				if (ozone.Video != null)
				{
					CurrentOzone.Video = ozone.Video;

				}

				ozone = null;

				this._ozoneRepository.Update(CurrentOzone);
				if (SaveData())
				{
					ozone = CurrentOzone;
					message = "Ozone updated successfully ...";
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

		public bool DeleteOzone(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Ozone ozone = this._ozoneRepository.GetById(id);

				if (softDelete)
				{
					ozone.IsDeleted = true;
					this._ozoneRepository.Update(ozone);
				}
				else
				{
					this._ozoneRepository.Delete(ozone);
				}
				if (SaveData())
				{
					message = "Ozone deleted successfully ...";
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
		#endregion
	}
	public interface IOzoneService
	{
		IEnumerable<Ozone> GetOzones();
		Ozone GetOzone(long id);
		bool CreateOzone(Ozone ozone, ref string message);
		bool UpdateOzone(ref Ozone ozone, ref string message);
		bool DeleteOzone(long id, ref string message, bool softDelete = true);
		bool SaveData();
		Ozone GetOzonefirstordefault();
	}
}
