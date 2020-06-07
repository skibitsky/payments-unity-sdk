using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Xsolla.Core
{
	public partial class WebRequestHelper : MonoSingleton<WebRequestHelper>
	{
		/// <summary>
		/// Processing request and invoke no data callback by success
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="onComplete"></param>
		/// <param name="onError"></param>
		/// <param name="errorsToCheck"></param>
		void ProcessRequest(UnityWebRequest webRequest, Action onComplete, Action<Error> onError)
		{
			Error error = CheckResponseForErrors(webRequest);
			if (error == null)
				onComplete?.Invoke();
			else
				TriggerOnError(onError, error);
		}

		/// <summary>
		/// Processing request and invoke raw data (Action<string>) callback by success
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="onComplete"></param>
		/// <param name="onError"></param>
		/// <param name="errorsToCheck"></param>
		void ProcessRequest(UnityWebRequest webRequest, Action<string> onComplete, Action<Error> onError)
		{
			Error error;
			try {
				error = CheckResponseForErrors(webRequest);
			}catch(Exception e) {
				Debug.LogWarning(e.Message);
				error = null;
			}
			if (error == null) {
				string data = webRequest.downloadHandler.text;
				if (data != null) {
					onComplete?.Invoke(data);
				} else {
					error = Error.UnknownError;
				}
			}
			if (error != null) {
				TriggerOnError(onError, error);
			}
		}
		
		/// <summary>
		/// Processing request and invoke Texture2D (Action<Texture2D>) callback by success
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="onComplete"></param>
		/// <param name="onError"></param>
		void ProcessRequest(UnityWebRequest webRequest, Action<Texture2D> onComplete, Action<Error> onError)
		{
			Error error = null;
			if (webRequest.isNetworkError || webRequest.isHttpError)
				error = Error.NetworkError;
			if (error == null)
			{
				var texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
				if (texture != null) {
					onComplete?.Invoke(texture);
				} else {
					error = Error.UnknownError;
				}
			}
			if (error != null) {
				TriggerOnError(onError, error);
			}
		}

		/// <summary>
		/// Processing request and invoke Action<T> callback by success
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="webRequest"></param>
		/// <param name="onComplete"></param>
		/// <param name="onError"></param>
		void ProcessRequest<T>(UnityWebRequest webRequest, Action<T> onComplete, Action<Error> onError) where T : class
		{
			Error error = CheckResponseForErrors(webRequest);
			if (error == null) {
				T data = GetResponsePayload<T>(webRequest);
				if(data != null) {
					onComplete?.Invoke(data);
				} else {
					error = Error.UnknownError;
				}
			}
			if (error != null) {
				TriggerOnError(onError, error);
			}
		}

		T GetResponsePayload<T>(UnityWebRequest webRequest) where T: class
		{
			string responseData = webRequest.downloadHandler.text;
			return string.IsNullOrEmpty(responseData) ? null : responseData.DeserializeTo<T>();
		}

		Error CheckResponseForErrors(UnityWebRequest webRequest)
		{
			if (webRequest.isNetworkError) {
				return Error.NetworkError;
			}
			return CheckResponsePayloadForErrors(webRequest);
		}

		Error CheckResponsePayloadForErrors(UnityWebRequest webRequest)
		{
			var responseData = webRequest.downloadHandler.text;
			Debug.Log(
				"URL: " + webRequest.url + Environment.NewLine +
				"RESPONSE: " + responseData
				);
			return (responseData != null) ? TryParseErrorMessage(responseData) : null;
		}

		Error TryParseErrorMessage(string json)
		{
			var error = Utils.ParseError(json);
			return (error != null && error.IsValid()) ? error : null;
		}

		void TriggerOnError(Action<Error> onError, Error error)
		{
			onError?.Invoke(error);
		}
	}
}

