using System;

namespace Playfab.Purchases
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
		}
		public string FunctionName;
		public CloudScriptArgs FunctionParameter;
	}
}
