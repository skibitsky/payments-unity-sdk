using System;
using JetBrains.Annotations;
using Xsolla.Core;

namespace Xsolla.Payments.Api.Playfab.Auth
{
	[PublicAPI]
	public class PlayfabAuth
	{
		private const string URL_USER_REGISTRATION = "https://{0}.playfabapi.com/Client/RegisterPlayFabUser";
		private const string URL_USER_SIGNIN = "https://{0}.playfabapi.com/Client/LoginWithPlayFab";
		private const string URL_PASSWORD_RESET = "https://{0}.playfabapi.com/Client/SendAccountRecoveryEmail";

		/// <summary>
		/// Registers a new Playfab user account.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/authentication/registerplayfabuser?view=playfab-rest"/>
		/// <param name="username">PlayFab username for the account (3-20 characters).</param>
		/// <param name="password">Password for the PlayFab account (6-100 characters).</param>
		/// <param name="email">User email address attached to their account.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="SignIn"/>
		/// <seealso cref="ResetPassword"/>
		public void Registration(string username, string password, string email,
			[NotNull] Action<PlayfabUserRegistrationResponseEntity> onSuccess, [CanBeNull] Action<Error> onError = null
		)
		{
			var registrationData =
				new PlayfabUserRegistrationEntity(username, password, email, XsollaSettings.PlayfabTitleId);
			var url = PlayfabApi.GetFormattedUrl(URL_USER_REGISTRATION);
			WebRequestHelper.Instance.PostRequest(url, registrationData, onSuccess, onError);
		}

		/// <summary>
		/// Signs the user into the PlayFab account.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/authentication/loginwithplayfab?view=playfab-rest"/>
		/// <param name="username">PlayFab username for the account (3-20 characters).</param>
		/// <param name="password">Password for the PlayFab account (6-100 characters).</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="Registration"/>
		/// <seealso cref="ResetPassword"/>
		public void SignIn(string username, string password,
			[NotNull] Action<AuthToken.Playfab> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var authData = new PlayfabUserAuthEntity(username, password, XsollaSettings.PlayfabTitleId);
			var url = PlayfabApi.GetFormattedUrl(URL_USER_SIGNIN);

			WebRequestHelper.Instance.PostRequest<PlayfabUserAuthResponseEntity, PlayfabUserAuthEntity>(
				url, authData,
				response =>
				{
					onSuccess?.Invoke(new AuthToken.Playfab(response.SessionTicket, response.TokenExpiration));
				}, onError);
		}

		/// <summary>
		/// Forces an email to be sent to the registered email address for the user's account,
		/// with a link allowing the user to change the password
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/account-management/sendaccountrecoveryemail?view=playfab-rest"/>
		/// <param name="email">User email address attached to their account.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="Registration"/>
		/// <seealso cref="SignIn"/>
		public void ResetPassword(string email, [NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_PASSWORD_RESET);
			var resetData = new PlayfabUserResetPasswordEntity(email, XsollaSettings.PlayfabTitleId);
			WebRequestHelper.Instance.PostRequest(url, resetData, onSuccess, onError);
		}
	}
}