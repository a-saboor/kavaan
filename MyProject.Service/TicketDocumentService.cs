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
	class TicketDocumentService : ITicketDocumentService
	{
		public readonly ITicketDocumentRepository _TicketDocumentRepository;
		public readonly IUnitOfWork _unitOfWork;

		public TicketDocumentService(ITicketDocumentRepository TicketDocumentRepository, IUnitOfWork unitOfWork)
		{
			this._TicketDocumentRepository = TicketDocumentRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<TicketDocument> GetTicketDocuments()
		{
			var TicketDocument = _TicketDocumentRepository.GetAll();
			return TicketDocument;
		}


		//public IEnumerable<object> GetTicketDocumentsByTicketID(long ticketID)
		//{
		//	var TicketDocument = _TicketDocumentRepository.GetAll().Where(mod=> mod.TicketID == ticketID);
		//	var vendors = _vendorRepository.GetAll();
		//	var users = _userRepository.GetAll();
		//	var conversation = from tickets in TicketDocument
		//					   select new TicketConversationViewModel()
		//					   {

		//						   senderName = (tickets.SenderType.Equals("Vendor") ?
		//									 from vendor in vendors where vendor.ID.Equals(tickets.SenderID) select vendor.Name :
		//									 from user in users where user.ID.Equals(tickets.SenderID) select user.Name).FirstOrDefault() ,
		//						   message = tickets.Message,
		//						   senderType = tickets.SenderType,
		//						   datetime = tickets.CreatedOn.ToString("dd MMM yyyy, h: mm tt"),
		//						   id = tickets.ID,

		//					   };

		//	return conversation;
		//}

		public TicketDocument GetTicketDocument(long id)
		{
			var TicketDocument = _TicketDocumentRepository.GetById(id);
			return TicketDocument;
		}

		public bool CreateTicketDocument(TicketDocument TicketDocument, ref string message)
		{
			try
			{

				TicketDocument.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_TicketDocumentRepository.Add(TicketDocument);
				if (SaveTicketDocument())
				{
					message = "Ticket document added successfully ...";
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

		public bool UpdateTicketDocument(ref TicketDocument TicketDocument, ref string message)
		{
			try
			{
				TicketDocument CurrentDocument = _TicketDocumentRepository.GetById(TicketDocument.ID);

				CurrentDocument.Url = TicketDocument.Url;

				TicketDocument = null;

				_TicketDocumentRepository.Update(CurrentDocument);
				if (SaveTicketDocument())
				{
					TicketDocument = CurrentDocument;
					message = "Ticket document updated successfully ...";
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

		public bool DeleteTicketDocument(long id, ref string message, bool softDelete = true)
		{
			try
			{
				TicketDocument ticket = _TicketDocumentRepository.GetById(id);

				if (softDelete)
				{
					_TicketDocumentRepository.Update(ticket);
				}
				else
				{
					_TicketDocumentRepository.Delete(ticket);
				}
				if (SaveTicketDocument())
				{
					message = "Ticket document deleted successfully ...";
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

		public bool SaveTicketDocument()
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

	public interface ITicketDocumentService
	{
		//IEnumerable<object> GetTicketDocumentsByTicketID(long ticketID);
		IEnumerable<TicketDocument> GetTicketDocuments();
		TicketDocument GetTicketDocument(long id);
		bool CreateTicketDocument(TicketDocument TicketDocument, ref string message);
		bool UpdateTicketDocument(ref TicketDocument TicketDocument, ref string message);
		bool DeleteTicketDocument(long id, ref string message, bool softDelete = true);
		bool SaveTicketDocument();

	}
}
