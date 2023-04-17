using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class CustomerSuggestionService : ICustomerSuggestionService
	{
		private readonly ICustomerSuggestionRepository _customerSuggestionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CustomerSuggestionService(ICustomerSuggestionRepository customerSuggestionRepository, IUnitOfWork unitOfWork)
		{
			this._customerSuggestionRepository = customerSuggestionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ICustomerSuggestionService Members

		public IEnumerable<CustomerSuggestion> GetCustomerSuggestions()
		{
			var customerSuggestions = _customerSuggestionRepository.GetAll();
			return customerSuggestions;
		}

		public CustomerSuggestion GetCustomerSuggestion(long id)
		{
			var customerSuggestion = _customerSuggestionRepository.GetById(id);
			return customerSuggestion;
		}

		public bool CreateCustomerSuggestion(CustomerSuggestion customerSuggestion, ref string message)
		{
			try
			{
				customerSuggestion.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_customerSuggestionRepository.Add(customerSuggestion);
				if (SaveCustomerSuggestion())
				{
					message = "Suggestion sent successfully ...";
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

		public bool UpdateCustomerSuggestion(ref CustomerSuggestion customerSuggestion, ref string message)
		{
			try
			{
				_customerSuggestionRepository.Update(customerSuggestion);
				if (SaveCustomerSuggestion())
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

		public bool DeleteCustomerSuggestion(long id, ref string message, bool softDelete = true)
		{
			try
			{
				CustomerSuggestion customerSuggestion = _customerSuggestionRepository.GetById(id);

				_customerSuggestionRepository.Delete(customerSuggestion);

				if (SaveCustomerSuggestion())
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

		public bool SaveCustomerSuggestion()
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

        public List<CustomerSuggestion> GetCustomerSuggestionDateWise(DateTime FromDate, DateTime ToDate)
        {
			var Sugggestions = _customerSuggestionRepository.GetFilteredCustomerSuggestions(FromDate, ToDate);
			return Sugggestions;
        }

        #endregion
    }

	public interface ICustomerSuggestionService
	{
		IEnumerable<CustomerSuggestion> GetCustomerSuggestions();
		CustomerSuggestion GetCustomerSuggestion(long id);
		bool CreateCustomerSuggestion(CustomerSuggestion customerSuggestion, ref string message);
		bool UpdateCustomerSuggestion(ref CustomerSuggestion customerSuggestion, ref string message);
		bool DeleteCustomerSuggestion(long id, ref string message, bool softDelete = true);
		bool SaveCustomerSuggestion();
        List<CustomerSuggestion> GetCustomerSuggestionDateWise(DateTime FromDate, DateTime ToDate);
	}
}
