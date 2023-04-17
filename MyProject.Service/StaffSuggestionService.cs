using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class StaffSuggestionService : IStaffSuggestionService
	{
		private readonly IStaffSuggestionRepository _staffSuggestionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public StaffSuggestionService(IStaffSuggestionRepository staffSuggestionRepository, IUnitOfWork unitOfWork)
		{
			this._staffSuggestionRepository = staffSuggestionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IStaffSuggestionService Members

		public IEnumerable<StaffSuggestion> GetStaffSuggestions()
		{
			var StaffSuggestions = _staffSuggestionRepository.GetAll();
			return StaffSuggestions;
		}

		public StaffSuggestion GetStaffSuggestion(long id)
		{
			var StaffSuggestion = _staffSuggestionRepository.GetById(id);
			return StaffSuggestion;
		}

		public bool CreateStaffSuggestion(StaffSuggestion StaffSuggestion, ref string message)
		{
			try
			{
				StaffSuggestion.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_staffSuggestionRepository.Add(StaffSuggestion);
				if (SaveStaffSuggestion())
				{
					message = "Suggestion added successfully ...";
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

		public bool UpdateStaffSuggestion(ref StaffSuggestion StaffSuggestion, ref string message)
		{
			try
			{
				_staffSuggestionRepository.Update(StaffSuggestion);
				if (SaveStaffSuggestion())
				{
					message = "Suggestion updated successfully ...";
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

		public bool DeleteStaffSuggestion(long id, ref string message, bool softDelete = true)
		{
			try
			{
				StaffSuggestion staffSuggestion = _staffSuggestionRepository.GetById(id);

				_staffSuggestionRepository.Delete(staffSuggestion);

				if (SaveStaffSuggestion())
				{
					message = "Suggestion deleted successfully ...";
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

		public bool SaveStaffSuggestion()
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

        public List<StaffSuggestion> GetStaffSuggestionDateWise(DateTime FromDate, DateTime ToDate)
        {
			var Sugggestions = _staffSuggestionRepository.GetFilteredCustomerSuggestions(FromDate, ToDate);
			return Sugggestions;
        }

        #endregion
    }

	public interface IStaffSuggestionService
	{
		IEnumerable<StaffSuggestion> GetStaffSuggestions();
		StaffSuggestion GetStaffSuggestion(long id);
		bool CreateStaffSuggestion(StaffSuggestion staffSuggestion, ref string message);
		bool UpdateStaffSuggestion(ref StaffSuggestion staffSuggestion, ref string message);
		bool DeleteStaffSuggestion(long id, ref string message, bool softDelete = true);
		bool SaveStaffSuggestion();
        List<StaffSuggestion> GetStaffSuggestionDateWise(DateTime FromDate, DateTime ToDate);
	}
}
