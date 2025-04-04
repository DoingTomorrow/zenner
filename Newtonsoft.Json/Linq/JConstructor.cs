// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JConstructor
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JConstructor : JContainer
  {
    private string _name;
    private readonly List<JToken> _values = new List<JToken>();

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._values;

    internal override int IndexOfItem(JToken item) => this._values.IndexOfReference<JToken>(item);

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JConstructor content1))
        return;
      if (content1.Name != null)
        this.Name = content1.Name;
      JContainer.MergeEnumerableContent((JContainer) this, (IEnumerable) content1, settings);
    }

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public override JTokenType Type => JTokenType.Constructor;

    public JConstructor()
    {
    }

    public JConstructor(JConstructor other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    public JConstructor(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JConstructor(string name, object content)
      : this(name)
    {
      this.Add(content);
    }

    public JConstructor(string name)
    {
      switch (name)
      {
        case null:
          throw new ArgumentNullException(nameof (name));
        case "":
          throw new ArgumentException("Constructor name cannot be empty.", nameof (name));
        default:
          this._name = name;
          break;
      }
    }

    internal override bool DeepEquals(JToken node)
    {
      return node is JConstructor container && this._name == container.Name && this.ContentsEqual((JContainer) container);
    }

    internal override JToken CloneToken() => (JToken) new JConstructor(this);

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartConstructor(this._name);
      foreach (JToken child in this.Children())
        child.WriteTo(writer, converters);
      writer.WriteEndConstructor();
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        return key is int index ? this.GetItem(index) : throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is int index))
          throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem(index, value);
      }
    }

    internal override int GetDeepHashCode() => this._name.GetHashCode() ^ this.ContentsHashCode();

    public static JConstructor Load(JsonReader reader)
    {
      return JConstructor.Load(reader, (JsonLoadSettings) null);
    }

    public static JConstructor Load(JsonReader reader, JsonLoadSettings settings)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      reader.MoveToContent();
      JConstructor jconstructor = reader.TokenType == JsonToken.StartConstructor ? new JConstructor((string) reader.Value) : throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
      jconstructor.ReadTokenFrom(reader, settings);
      return jconstructor;
    }
  }
}
