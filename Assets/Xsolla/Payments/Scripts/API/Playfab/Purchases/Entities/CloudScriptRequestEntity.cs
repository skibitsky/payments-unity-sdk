using System;

namespace Xsolla.Payments.Api.Playfab.Purchases
{
	[Serializable]
	public class CloudScriptRequestEntity
	{
		[Serializable]
		public class CloudScriptArgs
		{
			public string sku;
			public uint amount;
			public string orderId;
			public string sdkTag;
			public string theme;
		}

		public string FunctionName;
		public CloudScriptArgs FunctionParameter;
	}
}