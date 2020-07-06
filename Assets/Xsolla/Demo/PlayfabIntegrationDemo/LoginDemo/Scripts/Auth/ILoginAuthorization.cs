using System;
using Xsolla.Core;

public interface ILoginAuthorization
{
	Action<AuthToken> OnSuccess { get; set; }
	Action OnFailed { get; set; }
}