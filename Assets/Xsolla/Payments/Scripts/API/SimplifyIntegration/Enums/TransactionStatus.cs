using System;
using System.Linq;

namespace Xsolla.Payments.Api.SimplifyIntegration
{
	public class TransactionStatus
	{
		private enum Success
		{
			done,
			lost
		}

		private enum Failed
		{
			canceled,
			error,
			xsollaRefund,
			xsollaRefundFailed,
			test,
			fraud,
			checkLenya,
			held,
			denied,
			stop,
			partiallyRefunded
		}

		private enum InProgress
		{
			created,
			processing,
			authorized
		}

		public static bool IsSuccess(string transactionStatus)
		{
			return Enum.GetNames(typeof(Success)).ToList().Contains(transactionStatus);
		}

		public static bool IsFailed(string transactionStatus)
		{
			return Enum.GetNames(typeof(Failed)).ToList().Contains(transactionStatus);
		}

		public static bool IsInProgress(string transactionStatus)
		{
			return string.IsNullOrEmpty(transactionStatus) ||
			       Enum.GetNames(typeof(InProgress)).ToList().Contains(transactionStatus);
		}
	}
}