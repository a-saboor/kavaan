using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class CustomerDocumentDetailService : ICustomerDocumentDetailService
	{
		private readonly INumberRangeService numberRangeService;
		private readonly ICustomerDocumentDetailRepository _customerDocumentDetailRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CustomerDocumentDetailService(IUnitOfWork unitOfWork, ICustomerDocumentDetailRepository customerDocumentDetailRepository, INumberRangeService numberRangeService)
		{
			this._unitOfWork = unitOfWork;
			this._customerDocumentDetailRepository = customerDocumentDetailRepository;
			this.numberRangeService = numberRangeService;
		}
		#region ICustomerDocumentService Members

		public IEnumerable<CustomerDocumentDetail> GetCustomerDocumentDetails()
		{
			var customerDocumentDetail = this._customerDocumentDetailRepository.GetAll().Where(i => i.IsDeleted == false);
			return customerDocumentDetail;
		}

		public CustomerDocumentDetail GetCustomerDocumentDetail(long id)
		{
			var customerDocumentDetail = this._customerDocumentDetailRepository.GetById(id);
			return customerDocumentDetail;
		}

		public bool GetExistCustomerDocument(long customerID, long customerDocumentID)
		{
			bool customerDocumentDetail = this._customerDocumentDetailRepository.GetExistCustomerDocument(customerID, customerDocumentID);
			return customerDocumentDetail;
		}

		public IEnumerable<CustomerDocumentDetail> GetCustomerDocumentDetailsByCustomerID(long id)
		{
			var customerDocumentDetails = this._customerDocumentDetailRepository.GetAll().Where(i => i.IsDeleted == false && i.CustomerID == id);
			return customerDocumentDetails;
		}

		public bool CreateCustomerDocumentDetail(CustomerDocumentDetail customerDocumentDetail, ref string message)
		{
			try
			{
				customerDocumentDetail.IsActive = true;
				customerDocumentDetail.IsDeleted = false;
				customerDocumentDetail.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				customerDocumentDetail.Status = "Pending";
				customerDocumentDetail.ServiceNo = this.numberRangeService.GetNextValueFromNumberRangeByName("REQUEST");

				this._customerDocumentDetailRepository.Add(customerDocumentDetail);
				if (SaveData())
				{
					message = "Customer document added successfully ...";
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

		public bool UpdateCustomerDocumentDetail(ref CustomerDocumentDetail customerDocumentDetail, ref string message)
		{
			try
			{
				CustomerDocumentDetail CurrentCustomerDocumentDetail = this._customerDocumentDetailRepository.GetById(customerDocumentDetail.ID);

				CurrentCustomerDocumentDetail.ServiceNo = customerDocumentDetail.ServiceNo;
				CurrentCustomerDocumentDetail.CustomerID = customerDocumentDetail.CustomerID;
				CurrentCustomerDocumentDetail.CustomerDocumentID = customerDocumentDetail.CustomerDocumentID;
				CurrentCustomerDocumentDetail.Path = customerDocumentDetail.Path;

				customerDocumentDetail = null;

				this._customerDocumentDetailRepository.Update(CurrentCustomerDocumentDetail);
				if (SaveData())
				{
					customerDocumentDetail = CurrentCustomerDocumentDetail;
					message = "CustomerDocumentDetail updated successfully ...";
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

		public bool DeleteCustomerDocument(long id, ref string message, bool softDelete = true)
		{
			try
			{
				CustomerDocumentDetail customerDocumentDetail = this._customerDocumentDetailRepository.GetById(id);
				var filepath = customerDocumentDetail.Path;

				if (softDelete)
				{
					customerDocumentDetail.IsDeleted = true;
					this._customerDocumentDetailRepository.Update(customerDocumentDetail);
				}
				else
				{
					this._customerDocumentDetailRepository.Delete(customerDocumentDetail);
				}
				if (SaveData())
				{
					message = "Customer document deleted successfully ...";
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
	public interface ICustomerDocumentDetailService
	{
		IEnumerable<CustomerDocumentDetail> GetCustomerDocumentDetails();
		CustomerDocumentDetail GetCustomerDocumentDetail(long id);
		IEnumerable<CustomerDocumentDetail> GetCustomerDocumentDetailsByCustomerID(long id);
		bool GetExistCustomerDocument(long customerID, long customerDocumentID);
		bool CreateCustomerDocumentDetail(CustomerDocumentDetail customerDocumentDetail, ref string message);
		bool UpdateCustomerDocumentDetail(ref CustomerDocumentDetail customerDocumentDetail, ref string message);
		bool DeleteCustomerDocument(long id, ref string message, bool softDelete = true);
		bool SaveData();
	}
}
