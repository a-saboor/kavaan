using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Service
{
	class ProgressService : IProgressService
	{
		private readonly IProgressRepository _progressRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProgressService(IUnitOfWork unitOfWork, IProgressRepository progressRepository)
		{

			this._unitOfWork = unitOfWork;
			this._progressRepository = progressRepository;
		}
		#region IOzoneService Members

		public IEnumerable<Progress> GetProgresses()
		{
			var progress = this._progressRepository.GetAll().Where(x=>x.IsDeleted==false);
			return progress;
		}

		public Progress GetProgress(long id)
		{
			var progress = this._progressRepository.GetById(id);
			return progress;
		}
		public bool CreateProgress(Progress progress, ref string message)
		{
			try
			{
				progress.IsActive = true;
				progress.IsDeleted = false;
				progress.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				this._progressRepository.Add(progress);
				if (SaveData())
				{
					message = "Progress added successfully ...";
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

		public bool UpdateProgress(ref Progress progress, ref string message)
		{
			try
			{

				this._progressRepository.Update(progress);
				if (SaveData())
				{
					message = "Progress updated successfully ...";
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

		public bool DeleteProgress(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Progress progress = this._progressRepository.GetById(id);

				if (softDelete)
				{
					progress.IsDeleted = true;
					this._progressRepository.Update(progress);
				}
				else
				{
					this._progressRepository.Delete(progress);
				}
				if (SaveData())
				{
					message = "Progress deleted successfully ...";
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
	public interface IProgressService
	{
		IEnumerable<Progress> GetProgresses();
		Progress GetProgress(long id);
		bool CreateProgress(Progress progress, ref string message);
		bool UpdateProgress(ref Progress progress, ref string message);
		bool DeleteProgress(long id, ref string message, bool softDelete = true);
		bool SaveData();
	}
}
