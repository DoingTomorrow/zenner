﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JProperty
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JProperty : JContainer
  {
    private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();
    private readonly string _name;

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._content;

    public string Name
    {
      [DebuggerStepThrough] get => this._name;
    }

    public JToken Value
    {
      [DebuggerStepThrough] get => this._content._token;
      set
      {
        this.CheckReentrancy();
        JToken jtoken = value ?? (JToken) JValue.CreateNull();
        if (this._content._token == null)
          this.InsertItem(0, jtoken, false);
        else
          this.SetItem(0, jtoken);
      }
    }

    public JProperty(JProperty other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    internal override JToken GetItem(int index)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      return this.Value;
    }

    internal override void SetItem(int index, JToken item)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      if (JContainer.IsTokenUnchanged(this.Value, item))
        return;
      if (this.Parent != null)
        ((JObject) this.Parent).InternalPropertyChanging(this);
      base.SetItem(0, item);
      if (this.Parent == null)
        return;
      ((JObject) this.Parent).InternalPropertyChanged(this);
    }

    internal override bool RemoveItem(JToken item)
    {
      throw new JsonException("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override void RemoveItemAt(int index)
    {
      throw new JsonException("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override int IndexOfItem(JToken item) => this._content.IndexOf(item);

    internal override void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (item != null && item.Type == JTokenType.Comment)
        return;
      if (this.Value != null)
        throw new JsonException("{0} cannot have multiple values.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
      base.InsertItem(0, item, false);
    }

    internal override bool ContainsItem(JToken item) => this.Value == item;

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JProperty jproperty) || jproperty.Value == null || jproperty.Value.Type == JTokenType.Null)
        return;
      this.Value = jproperty.Value;
    }

    internal override void ClearItems()
    {
      throw new JsonException("Cannot add or remove items from {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override bool DeepEquals(JToken node)
    {
      return node is JProperty container && this._name == container.Name && this.ContentsEqual((JContainer) container);
    }

    internal override JToken CloneToken() => (JToken) new JProperty(this);

    public override JTokenType Type
    {
      [DebuggerStepThrough] get => JTokenType.Property;
    }

    internal JProperty(string name)
    {
      ValidationUtils.ArgumentNotNull((object) name, nameof (name));
      this._name = name;
    }

    public JProperty(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JProperty(string name, object content)
    {
      ValidationUtils.ArgumentNotNull((object) name, nameof (name));
      this._name = name;
      this.Value = this.IsMultiContent(content) ? (JToken) new JArray(content) : JContainer.CreateFromContent(content);
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WritePropertyName(this._name);
      JToken jtoken = this.Value;
      if (jtoken != null)
        jtoken.WriteTo(writer, converters);
      else
        writer.WriteNull();
    }

    internal override int GetDeepHashCode()
    {
      return this._name.GetHashCode() ^ (this.Value != null ? this.Value.GetDeepHashCode() : 0);
    }

    public static JProperty Load(JsonReader reader)
    {
      return JProperty.Load(reader, (JsonLoadSettings) null);
    }

    public static JProperty Load(JsonReader reader, JsonLoadSettings settings)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
      reader.MoveToContent();
      JProperty jproperty = reader.TokenType == JsonToken.PropertyName ? new JProperty((string) reader.Value) : throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
      jproperty.ReadTokenFrom(reader, settings);
      return jproperty;
    }

    private class JPropertyList : 
      IList<JToken>,
      ICollection<JToken>,
      IEnumerable<JToken>,
      IEnumerable
    {
      internal JToken _token;

      public IEnumerator<JToken> GetEnumerator()
      {
        if (this._token != null)
          yield return this._token;
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      public void Add(JToken item) => this._token = item;

      public void Clear() => this._token = (JToken) null;

      public bool Contains(JToken item) => this._token == item;

      public void CopyTo(JToken[] array, int arrayIndex)
      {
        if (this._token == null)
          return;
        array[arrayIndex] = this._token;
      }

      public bool Remove(JToken item)
      {
        if (this._token != item)
          return false;
        this._token = (JToken) null;
        return true;
      }

      public int Count => this._token == null ? 0 : 1;

      public bool IsReadOnly => false;

      public int IndexOf(JToken item) => this._token != item ? -1 : 0;

      public void Insert(int index, JToken item)
      {
        if (index != 0)
          return;
        this._token = item;
      }

      public void RemoveAt(int index)
      {
        if (index != 0)
          return;
        this._token = (JToken) null;
      }

      public JToken this[int index]
      {
        get => index != 0 ? (JToken) null : this._token;
        set
        {
          if (index != 0)
            return;
          this._token = value;
        }
      }
    }
  }
}
