using System.Collections.Generic;
using System.Linq;
using Playfab.Catalog;
using Playfab.Inventory;
using UnityEngine;

public class VirtualCurrencyContainer : MonoBehaviour
{
	public GameObject VirtualCurrencyBalancePrefab;
	private Dictionary<string, VirtualCurrencyBalanceUI> _currencies;

	private void Awake()
	{
		if(VirtualCurrencyBalancePrefab == null) {
			Debug.LogAssertion("VirtualCurrencyBalancePrefab is missing!");
			Destroy(gameObject);
			return;
		}
		_currencies = new Dictionary<string, VirtualCurrencyBalanceUI>();
	}

	private VirtualCurrencyBalanceUI AddCurrency(VirtualCurrencyItem item)
	{
		if (_currencies.ContainsKey(item.sku)) return _currencies[item.sku];
		if (string.IsNullOrEmpty(item.imageUrl)) return null;
		GameObject currencyBalance = Instantiate(VirtualCurrencyBalancePrefab, transform);
		VirtualCurrencyBalanceUI balanceUi = currencyBalance.GetComponent<VirtualCurrencyBalanceUI>();
		balanceUi.Initialize(item);
		_currencies.Add(item.sku, balanceUi);
		return balanceUi;
	}
	
	public void SetCurrencies(List<VirtualCurrencyItem> items)
	{
		_currencies.Values.ToList().ForEach(c =>
		{
			if(c.gameObject != null)
				Destroy(c.gameObject);
		});
		_currencies.Clear();
		items.ForEach(i => AddCurrency(i));
	}
	
	public void SetCurrenciesBalance(List<VirtualCurrencyBalance> balance)
	{
		balance.ForEach(SetCurrencyBalance);
	}
	
	private void SetCurrencyBalance(VirtualCurrencyBalance balance)
	{
		AddCurrency(new VirtualCurrencyItem
		{
			sku = balance.sku,
			imageUrl = balance.image_url
		})?.SetBalance(balance.amount);
	}
}
