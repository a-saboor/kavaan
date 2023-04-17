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
	class TicketService : ITicketService
	{
		public readonly ITicketRepository _ticketRepository;
		public readonly IUnitOfWork _unitOfWork;

		public TicketService(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
		{
			this._ticketRepository = ticketRepository;
			this._unitOfWork = unitOfWork;
		}
		public IEnumerable<Ticket> GetTickets()
		{
			var tickets = _ticketRepository.GetAll().Where(i => i.IsDeleted == false);
			return tickets;
		}

		public IEnumerable<Ticket> GetTicketsByOpenStatus()
		{
			var tickets = _ticketRepository.GetAll().Where(i => i.IsDeleted == false && i.Status == "OPEN");
			return tickets;
		}
		public IEnumerable<Ticket> GetTicketsByUser(long userID)
		{
			var tickets = _ticketRepository.GetAll().Where(i => i.IsDeleted == false && i.UserID == userID);
			return tickets;
		}

		public IEnumerable<Ticket> GetTicketsByVendor(long vendorID)
		{
			var tickets = _ticketRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == vendorID);
			return tickets;
		}

		public Ticket GetTicket(long id)
		{
			var tickets = _ticketRepository.GetById(id);
			return tickets;
		}

		public bool CreateTicket(Ticket Ticket, ref string message)
		{
			try
			{

				Ticket.IsDeleted = false;
				Ticket.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_ticketRepository.Add(Ticket);
				if (SaveTicket())
				{
					message = "Ticket added successfully ...";
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

		public bool UpdateTicket(ref Ticket ticket, ref string message)
		{
			try
			{
				Ticket CurrentTicket = _ticketRepository.GetById(ticket.ID);

				CurrentTicket.UserID = ticket.UserID;
				CurrentTicket.Status = ticket.Status;

				ticket = null;

				_ticketRepository.Update(CurrentTicket);
				if (SaveTicket())
				{
					ticket = CurrentTicket;
					message = "Ticket updated successfully ...";
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

		public bool DeleteTicket(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Ticket ticket = _ticketRepository.GetById(id);

				if (softDelete)
				{
					ticket.IsDeleted = true;
					_ticketRepository.Update(ticket);
				}
				else
				{
					_ticketRepository.Delete(ticket);
				}
				if (SaveTicket())
				{
					message = "Ticket deleted successfully ...";
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

		public bool SaveTicket()
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

	public interface ITicketService
	{
		IEnumerable<Ticket> GetTicketsByOpenStatus();

		IEnumerable<Ticket> GetTickets();
		Ticket GetTicket(long id);
		IEnumerable<Ticket> GetTicketsByVendor(long vendorID);
		IEnumerable<Ticket> GetTicketsByUser(long userID);
		bool CreateTicket(Ticket ticket, ref string message);
		bool UpdateTicket(ref Ticket ticket, ref string message);
		bool DeleteTicket(long id, ref string message, bool softDelete = true);
		bool SaveTicket();

	}
}
