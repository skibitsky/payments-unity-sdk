using System;

namespace Playfab.Auth
{
	[Serializable]
	public class PlayfabUserResetPasswordEntity
	{
		public string Email;
		public string TitleId;

		public PlayfabUserResetPasswordEntity(string email, string titleId)
		{
			Email = email;
			TitleId = titleId;
		}
	}
}