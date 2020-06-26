using System;

namespace Xsolla.Payments.Api.Playfab
{
	[Serializable]
	public class PlayfabResponseEntity<T>
	{
		public uint code;
		public string status;
		public T data;
	}
}