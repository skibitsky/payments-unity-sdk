using System;

namespace Xsolla.Payments.Api.SimplifyIntegration
{
    [Serializable]
    public class TransactionStatusResponse
    {
        public string status;
        public uint http_status_code;
        public string message;
    }
}
    