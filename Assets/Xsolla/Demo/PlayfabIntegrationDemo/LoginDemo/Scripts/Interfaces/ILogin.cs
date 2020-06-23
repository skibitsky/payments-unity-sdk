using System;
using Xsolla.Core;

public interface ILogin
{
	Action<AuthToken> OnSuccessfulLogin { get; set; }
	Action<Error> OnUnsuccessfulLogin { get; set; }
}