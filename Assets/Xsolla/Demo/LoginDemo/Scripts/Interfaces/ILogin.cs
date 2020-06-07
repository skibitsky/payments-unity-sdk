using System;
using Xsolla.Core;

public interface ILogin
{
    Action<AuthToken> OnSuccessfulLogin { get; set; }
    Action<Xsolla.Core.Error> OnUnsuccessfulLogin { get; set; }
}
