using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Xsolla.Core
{
	public partial class WebRequestHelper : MonoSingleton<WebRequestHelper>
	{
		public void PostRequest<T, D>(string url, D jsonObject, List<WebRequestHeader> requestHeaders, Action<T> onComplete = null, Action<Error> onError = null)
			where T : class
			where D : class
		{
			StartCoroutine(PostRequestCor<T>(url, jsonObject, requestHeaders, onComplete, onError));
		}

		public void PostRequest<T, D>(string url, D jsonObject, [NotNull]WebRequestHeader requestHeader, Action<T> onComplete = null, Action<Error> onError = null)
			where T : class
			where D : class
		{
			var headers = (requestHeader != null) ? new List<WebRequestHeader> { requestHeader } : null;
			PostRequest<T, D>(url, jsonObject, headers, onComplete, onError);
		}

		public void PostRequest<T, D>(string url, D jsonObject, Action<T> onComplete = null, Action<Error> onError = null)
			where T : class
			where D : class
		{
			PostRequest<T, D>(url, jsonObject, new List<WebRequestHeader>(), onComplete, onError);
		}

		public void PostRequest<T>(string url, List<WebRequestHeader> requestHeaders, Action<T> onComplete = null, Action<Error> onError = null)
			where T : class
		{
			StartCoroutine(PostRequestCor(url, null, requestHeaders, onComplete, onError));
		}

		public void PostRequest<T>(string url, Action<T> onComplete = null, Action<Error> onError = null)
			where T : class
		{
			PostRequest(url, null, onComplete, onError);
		}

		public void PostRequest<D>(string url, D jsonObject, List<WebRequestHeader> requestHeaders, Action onComplete = null, Action<Error> onError = null)
			where D : class
		{
			StartCoroutine(PostRequestCor(url, jsonObject, requestHeaders, onComplete, onError));
		}

		public void PostRequest<D>(string url, D jsonObject = null, Action onComplete = null, Action<Error> onError = null)
			where D : class
		{
			PostRequest(url, jsonObject, new List<WebRequestHeader>(), onComplete, onError);
		}

		public void PostRequest(string url, List<WebRequestHeader> requestHeaders, Action<string> onComplete = null, Action<Error> onError = null)
		{
			StartCoroutine(PostRequestCor(url, null, requestHeaders, onComplete, onError));
		}

		public void PostRequest(string url, Action<string> onComplete = null, Action<Error> onError = null)
		{
			PostRequest(url, null, onComplete, onError);
		}

		public void PostRequest(string url, List<WebRequestHeader> requestHeaders, Action onComplete = null, Action<Error> onError = null)
		{
			StartCoroutine(PostRequestCor(url, null, requestHeaders, onComplete, onError));
		}

		public void PostRequest(string url, Action onComplete = null, Action<Error> onError = null)
		{
			PostRequest(url, null, onComplete, onError);
		}

		IEnumerator PostRequestCor(string url, object jsonObject, List<WebRequestHeader> requestHeaders, Action onComplete = null, Action<Error> onError = null)
		{
			UnityWebRequest webRequest = PreparePostWebRequest(url, jsonObject, requestHeaders);

			yield return StartCoroutine(PerformWebRequest(webRequest, onComplete, onError));
		}

		IEnumerator PostRequestCor(string url, object jsonObject, List<WebRequestHeader> requestHeaders, Action<string> onComplete = null, Action<Error> onError = null)
		{
			UnityWebRequest webRequest = PreparePostWebRequest(url, jsonObject, requestHeaders);

			yield return StartCoroutine(PerformWebRequest(webRequest, onComplete, onError));
		}

		IEnumerator PostRequestCor<T>(string url, object jsonObject, List<WebRequestHeader> requestHeaders, Action<T> onComplete = null, Action<Error> onError = null) where T : class
		{
			UnityWebRequest webRequest = PreparePostWebRequest(url, jsonObject, requestHeaders);

			yield return StartCoroutine(PerformWebRequest(webRequest, onComplete, onError));
		}

		private UnityWebRequest PreparePostWebRequest(string url, object jsonObject, List<WebRequestHeader> requestHeaders)
		{
			UnityWebRequest webRequest = UnityWebRequest.Post(url, "POST");
			webRequest.timeout = 10;

			AttachBodyToPostRequest(webRequest, jsonObject);
			AttachHeadersToPostRequest(webRequest, requestHeaders);

			return webRequest;
		}

		private void AttachBodyToPostRequest(UnityWebRequest webRequest, object jsonObject)
		{
			jsonObject = jsonObject ?? new object();
			var jsonData = JsonConvert.SerializeObject(jsonObject).Replace('\n', ' ');
			var body = new UTF8Encoding().GetBytes(jsonData);
			webRequest.uploadHandler = new UploadHandlerRaw(body);
		}

		private void AttachHeadersToPostRequest(UnityWebRequest webRequest, List<WebRequestHeader> requestHeaders)
		{
			AddContentTypeHeaderTo(webRequest);
			requestHeaders?.ForEach(h => webRequest.SetRequestHeader(h.Name, h.Value));
		}
	}
}

