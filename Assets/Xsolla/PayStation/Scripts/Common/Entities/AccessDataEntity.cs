using System;
using System.Collections.Generic;

namespace Xsolla.PayStation
{
	[Serializable]
	public class AccessDataEntity
	{
		[Serializable]
		public class StringValue
		{
			public string value;
		}

		[Serializable]
		public class UserId
		{
			[Serializable]
			public class Country : StringValue
			{
				public bool allow_modify;
			}

			public StringValue id;
			public StringValue name;
			public StringValue email;
			public Country country;
		}

		[Serializable]
		public class Settings
		{
			[Serializable]
			public class UI
			{
				public string size;
				public string theme;
			}

			public uint project_id;
			public string currency;
			public string mode;
			public string external_id;
			public UI ui;
		}

		[Serializable]
		public class PurchaseItem
		{
			[Serializable]
			public class VirtualItems
			{
				[Serializable]
				public class VirtualItem
				{
					public string sku;
					public uint amount;
				}

				public List<VirtualItem> items;
			}

			public VirtualItems virtual_items;
		}

		public UserId user;
		public Settings settings;
		public PurchaseItem purchase;
	}
}