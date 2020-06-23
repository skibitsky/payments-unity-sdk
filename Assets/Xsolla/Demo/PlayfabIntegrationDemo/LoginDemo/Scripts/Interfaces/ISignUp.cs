using System;
using Xsolla.Core;

public interface ISignUp
{
	void SignUp();
	string SignUpEmail { get; }
	Action<AuthToken> OnSuccessfulSignUp { get; set; }
	Action<Xsolla.Core.Error> OnUnsuccessfulSignUp { get; set; }
}