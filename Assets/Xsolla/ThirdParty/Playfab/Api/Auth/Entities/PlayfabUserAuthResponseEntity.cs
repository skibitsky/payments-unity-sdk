using System;

namespace Xsolla.ThirdParty.Playfab.Api.Auth
{
	[Serializable]
	public class PlayfabUserAuthResponseEntity : PlayfabResponseEntity<PlayfabUserAuthResponseEntity.Payload>
	{
		[Serializable]
		public class Payload
		{
			[Serializable]
			public class EntityTokenField
			{
				public string TokenExpiration;
			}

			public string SessionTicket;
			public EntityTokenField EntityToken;
		}

		public string SessionTicket => data.SessionTicket;
		public string TokenExpiration => data.EntityToken.TokenExpiration;
	}
}