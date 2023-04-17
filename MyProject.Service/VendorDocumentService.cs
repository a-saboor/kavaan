using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{
    class VendorDocumentService : IVendorDocumentService
    {

		private readonly IVendorDocumentRepository _vendorDocumentRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorDocumentService(IVendorDocumentRepository vendorDocumentRepository, IUnitOfWork unitOfWork)
		{
			this._vendorDocumentRepository = vendorDocumentRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<VendorDocument> GetDocuments()
		{
			var documents = _vendorDocumentRepository.GetAll();
			return documents;
		}

		public IEnumerable<VendorDocument> GetDocumentByVendorID(long carID)
		{
			var documents = _vendorDocumentRepository.GetAllByVendorID(carID);
			return documents;
		}

		public VendorDocument GetDocument(long id)
		{
			var documents = _vendorDocumentRepository.GetById(id);
			return documents;
		}

		public bool CreateDocument(ref VendorDocument documents, ref string message)
		{
			try
			{
				if (_vendorDocumentRepository.GetDocumentByName(documents.Name, documents.ID) == null)
				{
					documents.IsActive = true;
					documents.IsDeleted = false;
					documents.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_vendorDocumentRepository.Add(documents);
					if (SaveDocument())
					{

						message = "Documents added successfully ...";
						return true;

					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Document already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateDocument(ref VendorDocument documents, ref string message)
		{
			try
			{
				if (_vendorDocumentRepository.GetDocumentByName(documents.Name, documents.ID) == null)
				{
					VendorDocument CurrentCity = _vendorDocumentRepository.GetById(documents.ID);

					CurrentCity.Name = documents.Name;
					CurrentCity.Path = documents.Path;
					CurrentCity.VendorID = documents.VendorID;



					documents = null;

					_vendorDocumentRepository.Update(CurrentCity);
					if (SaveDocument())
					{
						documents = CurrentCity;
						message = "Document updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Document already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteDocument(long id, ref string message)
		{
			try
			{
				VendorDocument documents = _vendorDocumentRepository.GetById(id);

				_vendorDocumentRepository.Delete(documents);

				if (SaveDocument())
				{
					message = "Document deleted successfully ...";
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

		public bool SaveDocument()
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
	}

	public interface IVendorDocumentService
	{

		IEnumerable<VendorDocument> GetDocuments();
		IEnumerable<VendorDocument> GetDocumentByVendorID(long vendorId);

		VendorDocument GetDocument(long id);
		bool CreateDocument(ref VendorDocument city, ref string message);
		bool UpdateDocument(ref VendorDocument city, ref string message);
		bool DeleteDocument(long id, ref string message);
		bool SaveDocument();
	}
}
