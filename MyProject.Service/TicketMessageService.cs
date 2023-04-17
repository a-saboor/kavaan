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
	class TicketMessageService : ITicketMessageService
	{
		public readonly ITicketMessageRepository _ticketMessageRepository;
		public readonly IVendorRepository _vendorRepository;
		public readonly IUserRepository _userRepository;
		public readonly IUnitOfWork _unitOfWork;

		public TicketMessageService(ITicketMessageRepository ticketMessageRepository, IVendorRepository vendorRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
		{
			this._ticketMessageRepository = ticketMessageRepository;
			this._vendorRepository = vendorRepository;
			this._userRepository = userRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<TicketMessage> GetTicketMessages()
		{
			var ticketMessage = _ticketMessageRepository.GetAll();
			return ticketMessage;
		}

		public IEnumerable<SP_GetTicketConversation_Result> GetTicketConversation(int ticketID)
		{
			var conversation = _ticketMessageRepository.GetTicketConversation(ticketID);
			return conversation;
		}

		//public IEnumerable<object> GetTicketMessagesByTicketID(long ticketID)
		//{
		//	var ticketMessage = _ticketMessageRepository.GetAll().Where(mod=> mod.TicketID == ticketID);
		//	var vendors = _vendorRepository.GetAll();
		//	var users = _userRepository.GetAll();
		//	var conversation = from tickets in ticketMessage
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

		public TicketMessage GetTicketMessage(long id)
		{
			var ticketMessage = _ticketMessageRepository.GetById(id);
			return ticketMessage;
		}

		public bool CreateTicketMessage(TicketMessage ticketMessage, ref string message)
		{
			try
			{

				ticketMessage.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_ticketMessageRepository.Add(ticketMessage);
				if (SaveTicketMessage())
				{
					message = "Ticket Message added successfully ...";
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

		public bool UpdateTicketMessage(ref TicketMessage ticketMessage, ref string message)
		{
			try
			{
				TicketMessage CurrentTicket = _ticketMessageRepository.GetById(ticketMessage.ID);

				CurrentTicket.Message = ticketMessage.Message;

				ticketMessage = null;

				_ticketMessageRepository.Update(CurrentTicket);
				if (SaveTicketMessage())
				{
					ticketMessage = CurrentTicket;
					message = "Ticket Message Message updated successfully ...";
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

		public bool DeleteTicketMessage(long id, ref string message, bool softDelete = true)
		{
			try
			{
				TicketMessage ticket = _ticketMessageRepository.GetById(id);

				if (softDelete)
				{
					_ticketMessageRepository.Update(ticket);
				}
				else
				{
					_ticketMessageRepository.Delete(ticket);
				}
				if (SaveTicketMessage())
				{
					message = "Ticket Message deleted successfully ...";
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

		public bool SaveTicketMessage()
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

	public interface ITicketMessageService
	{
		//IEnumerable<object> GetTicketMessagesByTicketID(long ticketID);
		IEnumerable<SP_GetTicketConversation_Result> GetTicketConversation(int ticketID);
		IEnumerable<TicketMessage> GetTicketMessages();
		TicketMessage GetTicketMessage(long id);
		bool CreateTicketMessage(TicketMessage ticketMessage, ref string message);
		bool UpdateTicketMessage(ref TicketMessage ticketMessage, ref string message);
		bool DeleteTicketMessage(long id, ref string message, bool softDelete = true);
		bool SaveTicketMessage();

	}
}
