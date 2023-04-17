using MyProject.Data.Infrastructure;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MyProject.Data.Repositories
{
	class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
	{
		public TransactionRepository(IDbFactory dbFactory)
			: base(dbFactory) { }
		public List<Transaction> GetFilteredTransactions(DateTime FromDate, DateTime ToDate)
		{
			var Transactions = this.DbContext.Transactions.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
			return Transactions;
		}

	}
	public interface ITransactionRepository : IRepository<Transaction>
	{
		List<Transaction> GetFilteredTransactions(DateTime FromDate, DateTime ToDate);
	}
}
