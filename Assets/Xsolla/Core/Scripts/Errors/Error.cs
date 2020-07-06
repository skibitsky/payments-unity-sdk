using System;
using System.Collections.Generic;

namespace Xsolla.Core
{
	[Serializable]
	public class Error
	{
		public uint code;
		public string status;
		public string error;
		public uint errorCode;
		public string errorMessage;
		public bool IsNetworkError { get; set; }

		public static Error NetworkError => new Error {status = "Network error", IsNetworkError = true};

		public static Error UnknownError => new Error {status = "Unknown error"};

		public bool IsValid()
		{
			return (errorCode != 0) || !string.IsNullOrEmpty(error) || !string.IsNullOrEmpty(errorMessage);
		}

		public override string ToString()
		{
			var result = string.IsNullOrEmpty(status) ? string.Empty : $"Request status: {status}. ";
			result += string.IsNullOrEmpty(error) ? string.Empty : $"Error type: {error}. ";
			result += errorCode == 0 ? string.Empty : $"Error code: {errorCode}. ";
			result += string.IsNullOrEmpty(errorMessage) ? string.Empty : $"Error message: {errorMessage}";
			return result;
		}
	}
}