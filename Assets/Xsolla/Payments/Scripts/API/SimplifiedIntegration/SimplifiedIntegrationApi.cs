using System;
using System.Text;
using Xsolla.Core;

namespace Xsolla.Payments.Api.SimplifiedIntegration
{
	public static class SimplifiedIntegrationApi
	{
		private const string URL_GET_TRANSACTION_STATUS =
			"https://api.xsolla.com/merchant/projects/{0}/transactions/external/{1}/status";

		/// <summary>
		/// Requests transaction status.
		/// </summary>
		/// <param name="projectId">Project ID from your Publisher Account.</param>
		/// <param name="transactionId">Transaction UUID, generated by client.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="SignIn"/>
		/// <seealso cref="ResetPassword"/>
		public static void GetTransactionStatus(string projectId, string transactionId, Action<TransactionStatusResponse> onSuccess, Action<Error> onError)
		{
			var urlBuilder = new StringBuilder(
				string.Format(URL_GET_TRANSACTION_STATUS, projectId, transactionId));
			string url = urlBuilder.ToString();
			WebRequestHelper.Instance.GetRequest(url, null, onSuccess, onError);
		}
	}
}