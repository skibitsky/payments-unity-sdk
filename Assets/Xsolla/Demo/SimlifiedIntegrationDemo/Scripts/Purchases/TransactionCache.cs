using System;

namespace Xsolla.Demo.SimplifiedIntegration
{
	[Serializable]
	public class TransactionCache
	{
		public string projectId;
		public string transactionId;
		public CatalogItemModel item;
		public DateTime dateTime;
	}
}