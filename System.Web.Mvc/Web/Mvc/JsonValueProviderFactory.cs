// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.JsonValueProviderFactory
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.Mvc.Properties;
using System.Web.Script.Serialization;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class JsonValueProviderFactory : ValueProviderFactory
  {
    private static void AddToBackingStore(
      JsonValueProviderFactory.EntryLimitedDictionary backingStore,
      string prefix,
      object value)
    {
      switch (value)
      {
        case IDictionary<string, object> dictionary:
          using (IEnumerator<KeyValuePair<string, object>> enumerator = dictionary.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, object> current = enumerator.Current;
              JsonValueProviderFactory.AddToBackingStore(backingStore, JsonValueProviderFactory.MakePropertyKey(prefix, current.Key), current.Value);
            }
            break;
          }
        case IList list:
          for (int index = 0; index < list.Count; ++index)
            JsonValueProviderFactory.AddToBackingStore(backingStore, JsonValueProviderFactory.MakeArrayKey(prefix, index), list[index]);
          break;
        default:
          backingStore.Add(prefix, value);
          break;
      }
    }

    private static object GetDeserializedObject(ControllerContext controllerContext)
    {
      if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
        return (object) null;
      string end = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
      return string.IsNullOrEmpty(end) ? (object) null : new JavaScriptSerializer().DeserializeObject(end);
    }

    public override IValueProvider GetValueProvider(ControllerContext controllerContext)
    {
      object obj = controllerContext != null ? JsonValueProviderFactory.GetDeserializedObject(controllerContext) : throw new ArgumentNullException(nameof (controllerContext));
      if (obj == null)
        return (IValueProvider) null;
      Dictionary<string, object> innerDictionary = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      JsonValueProviderFactory.AddToBackingStore(new JsonValueProviderFactory.EntryLimitedDictionary((IDictionary<string, object>) innerDictionary), string.Empty, obj);
      return (IValueProvider) new DictionaryValueProvider<object>((IDictionary<string, object>) innerDictionary, CultureInfo.CurrentCulture);
    }

    private static string MakeArrayKey(string prefix, int index)
    {
      return prefix + "[" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "]";
    }

    private static string MakePropertyKey(string prefix, string propertyName)
    {
      return !string.IsNullOrEmpty(prefix) ? prefix + "." + propertyName : propertyName;
    }

    private class EntryLimitedDictionary
    {
      private static int _maximumDepth = JsonValueProviderFactory.EntryLimitedDictionary.GetMaximumDepth();
      private readonly IDictionary<string, object> _innerDictionary;
      private int _itemCount;

      public EntryLimitedDictionary(IDictionary<string, object> innerDictionary)
      {
        this._innerDictionary = innerDictionary;
      }

      public void Add(string key, object value)
      {
        if (++this._itemCount > JsonValueProviderFactory.EntryLimitedDictionary._maximumDepth)
          throw new InvalidOperationException(MvcResources.JsonValueProviderFactory_RequestTooLarge);
        this._innerDictionary.Add(key, value);
      }

      private static int GetMaximumDepth()
      {
        NameValueCollection appSettings = ConfigurationManager.AppSettings;
        if (appSettings != null)
        {
          string[] values = appSettings.GetValues("aspnet:MaxJsonDeserializerMembers");
          int result;
          if (values != null && values.Length > 0 && int.TryParse(values[0], out result))
            return result;
        }
        return 1000;
      }
    }
  }
}
