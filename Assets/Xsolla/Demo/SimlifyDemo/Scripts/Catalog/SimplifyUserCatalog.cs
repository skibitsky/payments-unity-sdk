using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Store
{
    public class SimplifyUserCatalog : MonoSingleton<SimplifyUserCatalog>
    {
        //public const string CATALOG_FILE_PATH = "/Xsolla/Demo/SimplifyDemo/Resources/";
        public const string CATALOG_FILE_NAME = "catalog";
        
        public event Action<List<SimplifyCatalogItem>> UpdateItemsEvent;
        public event Action<List<SimplifyCatalogItem>> UpdateVirtualCurrenciesEvent;

        private List<SimplifyCatalogItem> _items = new List<SimplifyCatalogItem>();
        private List<SimplifyCatalogItem> _currencies = new List<SimplifyCatalogItem>();

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
            TextAsset catalog = (TextAsset)Resources.Load(CATALOG_FILE_NAME);
            string json = catalog.text;
            //var json = LoadJsonFromFile("file.json");
            
            return string.IsNullOrEmpty(json) 
                ? new List<SimplifyCatalogItem>() 
                : json.DeserializeTo<List<SimplifyCatalogItem>>();
        }

        private string LoadJsonFromFile(string path)
        {
            string json;
            try {
                using (var r = new StreamReader("file.json")) {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception e) {
                Debug.Log($"Can not load file = {path}. Exception: {e.Message}");
                json = string.Empty;
            }
            return json;
        }
    }
}
    