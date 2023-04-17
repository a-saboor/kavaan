using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class IntroductionSettingService : IIntroductionSettingService
	{
		private readonly IIntroductionSettingRepository _introductionSettingRepository;
		private readonly IUnitOfWork _unitOfWork;

		public IntroductionSettingService(IUnitOfWork unitOfWork, IIntroductionSettingRepository introductionSettingRepository)
		{

			this._unitOfWork = unitOfWork;
			this._introductionSettingRepository = introductionSettingRepository;
		}
		#region IOzoneService Members

		public IEnumerable<IntroductionSetting> GetIntroductionSetting()
		{
			var introductionSetting = this._introductionSettingRepository.GetAll();
			return introductionSetting;
		}
		public IntroductionSetting GetIntroductionSetting(long id)
		{
			var introductionSetting = this._introductionSettingRepository.GetById(id);
			return introductionSetting;
		}
		public IntroductionSetting GetIntroductionSettingByType(string Type)
		{
			var introductionSetting = this._introductionSettingRepository.GetAll().Where(i => i.Type == Type).FirstOrDefault();
			return introductionSetting;
		}
		public bool CreateIntroductionSetting(ref IntroductionSetting introductionSetting, ref string message)
		{
			try
			{
				this._introductionSettingRepository.Add(introductionSetting);
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

		public bool UpdateIntroductionSetting(ref IntroductionSetting introductionSetting, ref string message)
		{
			try
			{
				IntroductionSetting CurrentIntroductionSetting = this._introductionSettingRepository.GetById(introductionSetting.ID);

				CurrentIntroductionSetting.City = introductionSetting.City;
				CurrentIntroductionSetting.Heading = introductionSetting.Heading;
				CurrentIntroductionSetting.HeadingAr = introductionSetting.HeadingAr;
				CurrentIntroductionSetting.Paragraph = introductionSetting.Paragraph;
				CurrentIntroductionSetting.ParagraphAr = introductionSetting.ParagraphAr;

				introductionSetting = null;

				this._introductionSettingRepository.Update(CurrentIntroductionSetting);
				if (SaveData())
				{
					introductionSetting = CurrentIntroductionSetting;
					message = "Introduction Setting updated successfully ...";
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

		public bool DeleteIntroductionSetting(long id, ref string message, bool softDelete = true)
		{
			try
			{
				IntroductionSetting introductionSetting = this._introductionSettingRepository.GetById(id);

				if (softDelete)
				{
					this._introductionSettingRepository.Update(introductionSetting);
				}
				else
				{
					this._introductionSettingRepository.Delete(introductionSetting);
				}
				if (SaveData())
				{
					message = "Introduction Setting deleted successfully ...";
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
	public interface IIntroductionSettingService
	{
		IEnumerable<IntroductionSetting> GetIntroductionSetting();
		IntroductionSetting GetIntroductionSettingByType(string Type = "");
		IntroductionSetting GetIntroductionSetting(long id);
		bool CreateIntroductionSetting(ref IntroductionSetting introductionSetting, ref string message);
		bool UpdateIntroductionSetting(ref IntroductionSetting introductionSetting, ref string message);
		bool DeleteIntroductionSetting(long id, ref string message, bool softDelete = true);
		bool SaveData();
	}
}
