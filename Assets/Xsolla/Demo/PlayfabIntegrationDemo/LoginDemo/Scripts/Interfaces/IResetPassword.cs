using System;

public interface IResetPassword
{
	void ResetPassword();
	Action OnSuccessfulResetPassword { get; set; }
	Action<Xsolla.Core.Error> OnUnsuccessfulResetPassword { get; set; }
}