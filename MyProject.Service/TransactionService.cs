using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using MyProject.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class TransactionService : ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly IUnitOfWork _unitOfWork;

		public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
		{
			this._transactionRepository = transactionRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ITransactionService Members

		public IEnumerable<Transaction> GetTransactions(DateTime FromDate, DateTime ToDate)
		{

			var data = _transactionRepository.GetFilteredTransactions(FromDate, ToDate);
			return data;
		}

		public Transaction GetTransaction(long id)
		{
			var data = _transactionRepository.GetById(id);
			return data;
		}

		public Transaction GetTransactionByCode(string code)
		{
			var data = _transactionRepository.GetAll().FirstOrDefault(x=>x.InvoiceCode == code);
			return data;
		}

		public bool CreateTransaction(Transaction transaction, ref string message)
		{
			try
			{
				transaction.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
				_transactionRepository.Add(transaction);
				if (SaveTransaction())
				{
					_transactionRepository.Update(transaction);
					if (SaveTransaction())
					{
						message = "Transaction added successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later...";
						return false;
					}
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

		public bool SaveTransaction()
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

	public interface ITransactionService
	{
		IEnumerable<Transaction> GetTransactions(DateTime FromDate, DateTime ToDate);
        Transaction GetTransaction(long id);
        Transaction GetTransactionByCode(string code);
		bool CreateTransaction(Transaction transaction, ref string message);
		bool SaveTransaction();
	}
}
