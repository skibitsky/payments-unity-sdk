using System;
using Newtonsoft.Json;

namespace Xsolla.Core
{
	[Serializable]
	public abstract partial class AuthToken
	{
		[JsonProperty] public bool FromSteam { get; set; }
		[JsonProperty] public string SteamUserId { get; set; }

		public abstract bool IsNullOrEmpty();
		public abstract bool IsExpired();
		protected abstract string ToStringImpl();

		public static implicit operator string(AuthToken token) => (token != null) ? token.ToString() : string.Empty;

		public override string ToString()
		{
			return IsNullOrEmpty() ? string.Empty : ToStringImpl();
		}
	}
}