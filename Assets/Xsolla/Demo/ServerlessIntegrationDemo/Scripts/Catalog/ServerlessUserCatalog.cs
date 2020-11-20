using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.ServerlessIntegration
{
    public class ServerlessUserCatalog : MonoSingleton<ServerlessUserCatalog>
    {
        private const string CATALOG_FILE_NAME = "catalog";
        
        public event Action<List<ServerlessCatalogItem>> UpdateItemsEvent;
        public event Action<List<ServerlessCatalogItem>> UpdateVirtualCurrenciesEvent;

        private List<ServerlessCatalogItem> _items = new List<ServerlessCatalogItem>();
        private List<ServerlessCatalogItem> _currencies = new List<ServerlessCatalogItem>();

        private string _catalog;

        public void UpdateCatalog(Action<List<ServerlessCatalogItem>> onSuccess = null, Action<Error> onError = null)
        {
            List<ServerlessCatalogItem> catalog = GetCatalog();
            _items = catalog.Where(i => i.IsVirtualItem()).ToList();
            _currencies = catalog.Where(i => i.IsCurrency()).ToList();
            
            UpdateItemsEvent?.Invoke(_items);
            UpdateVirtualCurrenciesEvent?.Invoke(_currencies);
            onSuccess?.Invoke(catalog);
        }

        private List<ServerlessCatalogItem> GetCatalog()
        {
            if (string.IsNullOrEmpty(_catalog)) {
                var catalog = (TextAsset)Resources.Load(CATALOG_FILE_NAME);
                if (catalog == null)
                {
                    Debug.LogAssertion($"Can not find or load catalog file: `{CATALOG_FILE_NAME}`");
                    return new List<ServerlessCatalogItem>();
                }
                _catalog = catalog.text;
            }
            var json = _catalog;
            return string.IsNullOrEmpty(json) 
                ? new List<ServerlessCatalogItem>() 
                : json.DeserializeTo<List<ServerlessCatalogItem>>();
        }
    }
}
    