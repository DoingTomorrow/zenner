﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonPropertyCollection
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
  {
    private readonly Type _type;
    private readonly List<JsonProperty> _list;

    public JsonPropertyCollection(Type type)
      : base((IEqualityComparer<string>) StringComparer.Ordinal)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      this._type = type;
      this._list = (List<JsonProperty>) this.Items;
    }

    protected override string GetKeyForItem(JsonProperty item) => item.PropertyName;

    public void AddProperty(JsonProperty property)
    {
      if (this.Contains(property.PropertyName))
      {
        if (property.Ignored)
          return;
        JsonProperty jsonProperty = this[property.PropertyName];
        bool flag = true;
        if (jsonProperty.Ignored)
        {
          this.Remove(jsonProperty);
          flag = false;
        }
        else if (property.DeclaringType != (Type) null && jsonProperty.DeclaringType != (Type) null)
        {
          if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType) || jsonProperty.DeclaringType.IsInterface() && property.DeclaringType.ImplementInterface(jsonProperty.DeclaringType))
          {
            this.Remove(jsonProperty);
            flag = false;
          }
          if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType) || property.DeclaringType.IsInterface() && jsonProperty.DeclaringType.ImplementInterface(property.DeclaringType))
            return;
        }
        if (flag)
          throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) property.PropertyName, (object) this._type));
      }
      this.Add(property);
    }

    public JsonProperty GetClosestMatchProperty(string propertyName)
    {
      return this.GetProperty(propertyName, StringComparison.Ordinal) ?? this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
    }

    private bool TryGetValue(string key, out JsonProperty item)
    {
      if (this.Dictionary != null)
        return this.Dictionary.TryGetValue(key, out item);
      item = (JsonProperty) null;
      return false;
    }

    public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
    {
      if (comparisonType == StringComparison.Ordinal)
      {
        JsonProperty jsonProperty;
        return this.TryGetValue(propertyName, out jsonProperty) ? jsonProperty : (JsonProperty) null;
      }
      for (int index = 0; index < this._list.Count; ++index)
      {
        JsonProperty property = this._list[index];
        if (string.Equals(propertyName, property.PropertyName, comparisonType))
          return property;
      }
      return (JsonProperty) null;
    }
  }
}
