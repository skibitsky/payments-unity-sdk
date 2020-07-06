using System;

namespace Xsolla.Payments.Api.SimplifiedIntegration
{
    [Serializable]
    public class TransactionStatusResponse
    {
        public string status;
        public uint http_status_code;
        public string message;
    }
}
    