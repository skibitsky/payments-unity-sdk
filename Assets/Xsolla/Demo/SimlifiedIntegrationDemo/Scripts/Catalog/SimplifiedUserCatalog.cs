using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifiedIntegration
{
    public class SimplifiedUserCatalog : MonoSingleton<SimplifiedUserCatalog>
    {
        private const string CATALOG_FILE_NAME = "catalog";
        
        public event Action<List<SimplifiedCatalogItem>> UpdateItemsEvent;
        public event Action<List<SimplifiedCatalogItem>> UpdateVirtualCurrenciesEvent;

        private List<SimplifiedCatalogItem> _items = new List<SimplifiedCatalogItem>();
        private List<SimplifiedCatalogItem> _currencies = new List<SimplifiedCatalogItem>();

        private string _catalog;

        public void UpdateCatalog(Action<List<SimplifiedCatalogItem>> onSuccess = null, Action<Error> onError = null)
        {
            List<SimplifiedCatalogItem> catalog = GetCatalog();
            _items = catalog.Where(i => i.IsVirtualItem()).ToList();
            _currencies = catalog.Where(i => i.IsCurrency()).ToList();
            
            UpdateItemsEvent?.Invoke(_items);
            UpdateVirtualCurrenciesEvent?.Invoke(_currencies);
            onSuccess?.Invoke(catalog);
        }

        private List<SimplifiedCatalogItem> GetCatalog()
        {
            if (string.IsNullOrEmpty(_catalog)) {
                var catalog = (TextAsset)Resources.Load(CATALOG_FILE_NAME);
                if (catalog == null)
                {
                    Debug.LogAssertion($"Can not find or load catalog file: `{CATALOG_FILE_NAME}`");
                    return new List<SimplifiedCatalogItem>();
                }
                _catalog = catalog.text;
            }
            var json = _catalog;
            return string.IsNullOrEmpty(json) 
                ? new List<SimplifiedCatalogItem>() 
                : json.DeserializeTo<List<SimplifiedCatalogItem>>();
        }
    }
}
    