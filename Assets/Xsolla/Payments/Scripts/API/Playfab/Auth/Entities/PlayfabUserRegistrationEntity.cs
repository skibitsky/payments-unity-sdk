using System;

namespace Xsolla.Payments.Api.Playfab.Auth
{
	[Serializable]
	public class PlayfabUserRegistrationEntity
	{
		public string Username;
		public string Password;
		public string Email;
		public string TitleId;

		public PlayfabUserRegistrationEntity(string userName, string password, string email, string titleId)
		{
			Username = userName;
			Password = password;
			Email = email;
			TitleId = titleId;
		}
	}
}