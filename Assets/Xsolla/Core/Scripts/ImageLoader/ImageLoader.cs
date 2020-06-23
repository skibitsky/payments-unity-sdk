using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Xsolla.Core
{
	public class ImageLoader : MonoSingleton<ImageLoader>
	{
		private Dictionary<string, Sprite> _images;
		private List<string> _pendingImages;

		public override void Init()
		{
			base.Init();
			_images = new Dictionary<string, Sprite>();
			_pendingImages = new List<string>();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			StopAllCoroutines();
			_pendingImages.Clear();
			_images.Clear();
		}

		public void GetImageAsync(string url, Action<string, Sprite> callback)
		{
			if (_images.ContainsKey(url))
			{
				callback?.Invoke(url, _images[url]);
				return;
			}

			if (!_pendingImages.Contains(url))
			{
				_pendingImages.Add(url);
				StartCoroutine(LoadImage(url));
			}

			if (callback != null)
			{
				StartCoroutine(WaitImage(url, callback));
			}
		}

		IEnumerator LoadImage(string url)
		{
			yield return WebRequestHelper.Instance.ImageRequestCoroutine(url, sprite =>
			{
				_images.Add(url, sprite);
				_pendingImages.Remove(url);
			});
		}

		IEnumerator WaitImage(string url, Action<string, Sprite> callback)
		{
			yield return new WaitUntil(() => _images.ContainsKey(url));
			callback?.Invoke(url, _images[url]);
		}
	}
}