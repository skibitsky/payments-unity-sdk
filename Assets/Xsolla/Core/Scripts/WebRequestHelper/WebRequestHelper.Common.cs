using UnityEngine;
using UnityEngine.Networking;

namespace Xsolla.Core
{
	public partial class WebRequestHelper : MonoSingleton<WebRequestHelper>
	{
		public void AddOptionalHeadersTo(UnityWebRequest request)
		{ }

		public void AddContentTypeHeaderTo(UnityWebRequest request)
		{
			WebRequestHeader contentHeader = WebRequestHeader.ContentTypeHeader();
			request.SetRequestHeader(contentHeader.Name, contentHeader.Value);
		}
	}
}

