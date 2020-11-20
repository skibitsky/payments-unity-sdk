using System.Collections.Generic;
using System.Web;
using Xsolla.Core;
using Xsolla.Payments;

namespace Xsolla.Demo.ServerlessIntegration
{
	public partial class ServerlessDemoImplementation : 
		MonoSingleton<ServerlessDemoImplementation>,
		IStoreDemoImplementation
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
				email = new AccessDataEntity.StringValue {value = "userEmail@gmail.com"}
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
				xsolla_product_tag = PaymentsHelper.GetAdditionalInformation("serverless"),
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