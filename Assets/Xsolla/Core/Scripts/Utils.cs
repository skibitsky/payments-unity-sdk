using System;
using System.Runtime.Serialization.Formatters;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Xsolla.Core
{
	public static class Utils
	{
		public static Error ParseError(string json)
		{
			// if json is a simple array return null to avoid raising exception while trying to parse it as an error
			return JsonConvert.DeserializeObject(json) is JArray ? null : json.DeserializeTo<Error>();
		}

		public static string SerializeToJson(this object serializable, bool useDefaultSettings = true)
		{
			string json;
			try
			{
				json = useDefaultSettings
					? JsonConvert.SerializeObject(serializable)
					: JsonConvert.SerializeObject(serializable, GetJsonSettings());
			}
			catch (Exception e)
			{
				Debug.LogError($"Can't serialize object! Exception: {e.Message}");
				json = string.Empty;
			}

			return json;
		}

		public static T DeserializeTo<T>(this string json, bool useDefaultSettings = true)
		{
			T result;
			try
			{
				result = useDefaultSettings
					? JsonConvert.DeserializeObject<T>(json)
					: JsonConvert.DeserializeObject<T>(json, GetJsonSettings());
			}
			catch (Exception e)
			{
				Debug.LogError($"Can't deserialize json = {json} !!! Exception: {e.Message}");
				result = default;
			}

			return result;
		}

		private static JsonSerializerSettings GetJsonSettings()
		{
			return new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				ContractResolver = new UnderscorePropertyNamesContractResolver(),
				TypeNameHandling = TypeNameHandling.All,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
			};
		}

		private class UnderscorePropertyNamesContractResolver : DefaultContractResolver
		{
			protected override string ResolvePropertyName(string propertyName)
			{
				return Regex.Replace(propertyName, "_", "");
			}
		}
	}
}