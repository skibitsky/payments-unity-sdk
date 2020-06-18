using System;

namespace Xsolla.ThirdParty.Playfab.Api.Auth
{
	[Serializable]
	public class PlayfabUserAuthEntity
	{
		public string Username;
		public string Password;
		public string TitleId;

		public PlayfabUserAuthEntity(string userName, string password, string titleId)
		{
			Username = userName;
			Password = password;
			TitleId = titleId;
		}
	}
}