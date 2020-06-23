using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Xsolla.Core;

namespace Xsolla.ThirdParty.Playfab.Api.Catalog
{
	[PublicAPI]
	public class PlayfabCatalog
	{
		private const string URL_CATALOG_GET_ITEMS = "https://{0}.playfabapi.com/Client/GetCatalogItems";

		/// <summary>
		/// Retrieves the specified version of the title's catalog of virtual goods.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/title-wide-data-management/getcatalogitems?view=playfab-rest"/>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void GetCatalog([NotNull] Action<CatalogItemsEntity> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_CATALOG_GET_ITEMS);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(url, new CatalogRequestEntity(), headers,
				(CatalogResponseEntity response) => onSuccess?.Invoke(response.data), onError);
		}
	}
}