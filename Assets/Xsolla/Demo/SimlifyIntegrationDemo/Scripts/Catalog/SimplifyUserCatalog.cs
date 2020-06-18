using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
    public class SimplifyUserCatalog : MonoSingleton<SimplifyUserCatalog>
    {
        private const string CATALOG_FILE_NAME = "catalog";
        
        public event Action<List<SimplifyCatalogItem>> UpdateItemsEvent;
        public event Action<List<SimplifyCatalogItem>> UpdateVirtualCurrenciesEvent;

        private List<SimplifyCatalogItem> _items = new List<SimplifyCatalogItem>();
        private List<SimplifyCatalogItem> _currencies = new List<SimplifyCatalogItem>();

        private string _catalog;

        public void UpdateCatalog(Action<List<SimplifyCatalogItem>> onSuccess = null, Action<Error> onError = null)
        {
            List<SimplifyCatalogItem> catalog = GetCatalog();
            _items = catalog.Where(i => i.IsVirtualItem()).ToList();
            _currencies = catalog.Where(i => i.IsCurrency()).ToList();
            
            UpdateItemsEvent?.Invoke(_items);
            UpdateVirtualCurrenciesEvent?.Invoke(_currencies);
            onSuccess?.Invoke(catalog);
        }

        private List<SimplifyCatalogItem> GetCatalog()
        {
            if (string.IsNullOrEmpty(_catalog)) {
                var catalog = (TextAsset)Resources.Load(CATALOG_FILE_NAME);
                if (catalog == null)
                {
                    Debug.LogAssertion($"Can not find or load catalog file: `{CATALOG_FILE_NAME}`");
                    return new List<SimplifyCatalogItem>();
                }
                _catalog = catalog.text;
            }
            var json = _catalog;
            return string.IsNullOrEmpty(json) 
                ? new List<SimplifyCatalogItem>() 
                : json.DeserializeTo<List<SimplifyCatalogItem>>();
        }
    }
}
    