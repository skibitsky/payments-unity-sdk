using System;

namespace Xsolla.ThirdParty.Playfab.Api
{
	[Serializable]
	public class PlayfabResponseEntity<T>
	{
		public uint code;
		public string status;
		public T data;
	}
}