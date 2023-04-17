using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class IntroductionService : IIntroductionService
	{
		private readonly IIntroductionRepository _introductionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public IntroductionService(IUnitOfWork unitOfWork, IIntroductionRepository introductionRepository)
		{

			this._unitOfWork = unitOfWork;
			this._introductionRepository = introductionRepository;
		}
		#region IOzoneService Members

		public IEnumerable<Introduction> GetIntroductions()
		{
			var introduction = this._introductionRepository.GetAll().Where(i => i.IsDeleted == false);
			return introduction;
		}
		public Introduction GetIntroductionfirstordefault()
		{
			var introduction = this._introductionRepository.GetAll().Where(i => i.IsDeleted == false).FirstOrDefault();
			return introduction;
		}
		public Introduction GetIntroduction(long id)
		{
			var introduction = this._introductionRepository.GetById(id);
			return introduction;
		}
		public bool CreateIntroduction(Introduction introduction, ref string message)
		{
			try
			{
				introduction.IsActive = true;
				introduction.IsDeleted = false;
				introduction.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				this._introductionRepository.Add(introduction);
				if (SaveData())
				{
					message = "Introduction added successfully ...";
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

		public bool UpdateIntroduction(ref Introduction introduction, ref string message)
		{
			try
			{
				Introduction CurrentIntroduction = this._introductionRepository.GetById(introduction.ID);

				CurrentIntroduction.Description = introduction.Description;
				CurrentIntroduction.DescriptionAr = introduction.DescriptionAr;
				if (introduction.Thumbnail != null)
				{

				CurrentIntroduction.Thumbnail = introduction.Thumbnail;
				}

				
				if (introduction.Video != null)
				{
					CurrentIntroduction.Video = introduction.Video;

				}
				


				introduction = null;

				this._introductionRepository.Update(CurrentIntroduction);
				if (SaveData())
				{
					introduction = CurrentIntroduction;
					message = "Introduction updated successfully ...";
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

		public bool DeleteIntroduction(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Introduction introduction = this._introductionRepository.GetById(id);

				if (softDelete)
				{
					introduction.IsDeleted = true;
					this._introductionRepository.Update(introduction);
				}
				else
				{
					this._introductionRepository.Delete(introduction);
				}
				if (SaveData())
				{
					message = "Introduction deleted successfully ...";
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
	public interface IIntroductionService
	{
		IEnumerable<Introduction> GetIntroductions();
		Introduction GetIntroduction(long id);
		bool CreateIntroduction(Introduction introduction, ref string message);
		bool UpdateIntroduction(ref Introduction introduction, ref string message);
		bool DeleteIntroduction(long id, ref string message, bool softDelete = true);
		bool SaveData();
		Introduction GetIntroductionfirstordefault();
	}
}
