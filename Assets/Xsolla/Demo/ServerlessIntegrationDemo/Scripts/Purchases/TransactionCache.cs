using System;

namespace Xsolla.Demo.ServerlessIntegration
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