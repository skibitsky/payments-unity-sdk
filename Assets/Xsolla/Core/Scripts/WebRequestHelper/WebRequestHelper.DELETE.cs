using System;
using System.Collections;
using UnityEngine.Networking;

namespace Xsolla.Core
{
	public partial class WebRequestHelper : MonoSingleton<WebRequestHelper>
	{
		public void DeleteRequest(string url, WebRequestHeader requestHeader, Action onComplete = null,
			Action<Error> onError = null)
		{
			StartCoroutine(DeleteRequestCor(url, requestHeader, onComplete, onError));
		}

		IEnumerator DeleteRequestCor(string url, WebRequestHeader requestHeader, Action onComplete = null,
			Action<Error> onError = null)
		{
			var webRequest = UnityWebRequest.Delete(url);
			webRequest.downloadHandler = new DownloadHandlerBuffer();

			AttachHeadersToDeleteRequest(webRequest, requestHeader);

			yield return StartCoroutine(PerformWebRequest(webRequest, onComplete, onError));
		}

		private void AttachHeadersToDeleteRequest(UnityWebRequest webRequest, WebRequestHeader requestHeader)
		{
			AddOptionalHeadersTo(webRequest);

			if (requestHeader != null)
			{
				webRequest.SetRequestHeader(requestHeader.Name, requestHeader.Value);
			}
		}
	}
}