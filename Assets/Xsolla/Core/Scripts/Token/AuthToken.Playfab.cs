using System;
using Newtonsoft.Json;

namespace Xsolla.Core
{
	public abstract partial class AuthToken
	{
		[Serializable]
		public class Playfab : AuthToken
		{
			[JsonProperty] private readonly string _sessionTicket;
			[JsonProperty] private readonly string _expirationTime;
			[JsonProperty] private bool _fromSteam;

			public Playfab(string sessionTicket, string expirationTime)
			{
				_sessionTicket = sessionTicket;
				_expirationTime = expirationTime;
			}

			public override bool IsNullOrEmpty()
			{
				return string.IsNullOrEmpty(_sessionTicket);
			}

			public override bool IsExpired()
			{
				DateTime expired = DateTime.Parse(_expirationTime);
				return expired.CompareTo(DateTime.Now) <= 0;
			}

			protected override string ToStringImpl()
			{
				return IsNullOrEmpty() ? string.Empty : _sessionTicket;
			}

			public string SessionTicket => _sessionTicket;
		}
	}
}