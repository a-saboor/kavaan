using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class VendorSuggestionService : IVendorSuggestionService
	{
		private readonly IVendorSuggestionRepository _vendorSuggestionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorSuggestionService(IVendorSuggestionRepository vendorSuggestionRepository, IUnitOfWork unitOfWork)
		{
			this._vendorSuggestionRepository = vendorSuggestionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorSuggestionService Members

		public IEnumerable<VendorSuggestion> GetVendorSuggestions()
		{
			var VendorSuggestions = _vendorSuggestionRepository.GetAll();
			return VendorSuggestions;
		}

		public VendorSuggestion GetVendorSuggestion(long id)
		{
			var VendorSuggestion = _vendorSuggestionRepository.GetById(id);
			return VendorSuggestion;
		}

		public bool CreateVendorSuggestion(VendorSuggestion VendorSuggestion, ref string message)
		{
			try
			{
				VendorSuggestion.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_vendorSuggestionRepository.Add(VendorSuggestion);
				if (SaveVendorSuggestion())
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

		public bool UpdateVendorSuggestion(ref VendorSuggestion VendorSuggestion, ref string message)
		{
			try
			{
				_vendorSuggestionRepository.Update(VendorSuggestion);
				if (SaveVendorSuggestion())
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

		public bool DeleteVendorSuggestion(long id, ref string message, bool softDelete = true)
		{
			try
			{
				VendorSuggestion VendorSuggestion = _vendorSuggestionRepository.GetById(id);

				_vendorSuggestionRepository.Delete(VendorSuggestion);

				if (SaveVendorSuggestion())
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

		public bool SaveVendorSuggestion()
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

        public List<VendorSuggestion> GetVendorSuggestionDateWise(DateTime FromDate, DateTime ToDate)
        {
			var Sugggestions = _vendorSuggestionRepository.GetFilteredCustomerSuggestions(FromDate, ToDate);
			return Sugggestions;
        }

        #endregion
    }

	public interface IVendorSuggestionService
	{
		IEnumerable<VendorSuggestion> GetVendorSuggestions();
		VendorSuggestion GetVendorSuggestion(long id);
		bool CreateVendorSuggestion(VendorSuggestion VendorSuggestion, ref string message);
		bool UpdateVendorSuggestion(ref VendorSuggestion VendorSuggestion, ref string message);
		bool DeleteVendorSuggestion(long id, ref string message, bool softDelete = true);
		bool SaveVendorSuggestion();
        List<VendorSuggestion> GetVendorSuggestionDateWise(DateTime FromDate, DateTime ToDate);
	}
}
