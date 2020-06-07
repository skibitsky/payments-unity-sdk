using Playfab.Catalog;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;

public class VirtualCurrencyBalanceUI : MonoBehaviour
{
	[SerializeField]
	public Image Image;
	[SerializeField]
	public Text Text;

	public void Initialize(VirtualCurrencyItem item)
	{
		if(Image != null) {
			ImageLoader.Instance.GetImageAsync(item.imageUrl, (_, sprite) => Image.sprite = sprite);
		} else {
			Debug.LogWarning($"Your Virtual Currency with sku = `{item.sku}` created without Image component!");
		}
	}

	public void SetBalance(uint balance)
	{
		if (Text)
			Text.text = balance.ToString();
	}
}
