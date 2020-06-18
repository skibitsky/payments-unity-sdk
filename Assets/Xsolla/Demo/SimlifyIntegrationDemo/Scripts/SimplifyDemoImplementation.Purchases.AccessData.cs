using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public partial class SimplifyDemoImplementation : MonoSingleton<SimplifyDemoImplementation>, IStoreDemoImplementation
	{
		private string CreateAccessData(uint projectId, string transactionId, CatalogItemModel item)
		{
			AccessDataEntity accessData = new AccessDataEntity();
			FillUserInfo(accessData);
			FillSettings(accessData, projectId, transactionId);
			FillPurchaseInfo(accessData, item);
			
			string json = accessData.SerializeToJson();
			return HttpUtility.UrlEncode(json);
		}

		private void FillUserInfo(AccessDataEntity accessData)
		{
			accessData.user = new AccessDataEntity.UserId
			{
				id = new AccessDataEntity.StringValue {value = "Some id"},
				name = new AccessDataEntity.StringValue {value = "Username"},
				email = new AccessDataEntity.StringValue {value = "m.levin@xsolla.com"},
				country = new AccessDataEntity.UserId.Country {value = "US", allow_modify = true}
			};
		}

		private void FillSettings(AccessDataEntity accessData, uint projectId, string transactionId)
		{
			accessData.settings = new AccessDataEntity.Settings
			{
				project_id = projectId,
				currency = "USD",
				mode = XsollaSettings.IsSandbox ? "sandbox" : null,
				external_id = transactionId,
				ui = new AccessDataEntity.Settings.UI
				{
					size = "medium",
					theme = PaystationThemeHelper.ConvertToSettings(XsollaSettings.PaystationTheme) 
				}
			};
		}

		private void FillPurchaseInfo(AccessDataEntity accessData, CatalogItemModel item)
		{
			accessData.purchase = new AccessDataEntity.PurchaseItem
			{
				virtual_items = new AccessDataEntity.PurchaseItem.VirtualItems
				{
					items = new List<AccessDataEntity.PurchaseItem.VirtualItems.VirtualItem>
					{
						new AccessDataEntity.PurchaseItem.VirtualItems.VirtualItem {sku = item.Sku, amount = 1}
					}
				}
			};
		}
	}
}