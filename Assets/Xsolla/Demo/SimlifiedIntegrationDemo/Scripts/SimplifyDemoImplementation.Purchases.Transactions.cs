using System;
using System.Collections.Generic;
using System.Linq;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifiedIntegration
{
	public partial class SimplifiedDemoImplementation : 
		MonoSingleton<SimplifiedDemoImplementation>,
		IStoreDemoImplementation
	{
		private List<TransactionCache> _transactions;

		private void LoadTransactions()
		{
			_transactions = LoadUserData<List<TransactionCache>>(SimplifiedDemoConstants.USER_TRANSACTIONS)
			                ?? new List<TransactionCache>();
			_transactions = _transactions.Where(
				t => t.projectId.Equals(XsollaSettings.SimplifiedProjectId.ToString())).ToList();
			SaveUserData(SimplifiedDemoConstants.USER_TRANSACTIONS, _transactions);
		}

		private List<TransactionCache> GetAllTransactions() => _transactions;

		private TransactionCache TransactionCaching(string projectId, string transactionId, CatalogItemModel item)
		{
			var cache = new TransactionCache
			{
				projectId = projectId,
				transactionId = transactionId,
				item = item,
				dateTime = DateTime.Now
			};
			_transactions.Add(cache);
			SaveUserData(SimplifiedDemoConstants.USER_TRANSACTIONS, _transactions);
			return cache;
		}

		private void ClearTransactionCache(TransactionCache cache)
		{
			_transactions.Remove(cache);
			SaveUserData(SimplifiedDemoConstants.USER_TRANSACTIONS, _transactions);
		}
	}
}