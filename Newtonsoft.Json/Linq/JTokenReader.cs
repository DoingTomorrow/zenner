﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenReader
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JTokenReader : JsonReader, IJsonLineInfo
  {
    private readonly JToken _root;
    private string _initialPath;
    private JToken _parent;
    private JToken _current;

    public JToken CurrentToken => this._current;

    public JTokenReader(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, nameof (token));
      this._root = token;
    }

    internal JTokenReader(JToken token, string initialPath)
      : this(token)
    {
      this._initialPath = initialPath;
    }

    public override bool Read()
    {
      if (this.CurrentState != JsonReader.State.Start)
      {
        if (this._current == null)
          return false;
        return this._current is JContainer current && this._parent != current ? this.ReadInto(current) : this.ReadOver(this._current);
      }
      this._current = this._root;
      this.SetToken(this._current);
      return true;
    }

    private bool ReadOver(JToken t)
    {
      if (t == this._root)
        return this.ReadToEnd();
      JToken next = t.Next;
      if (next == null || next == t || t == t.Parent.Last)
        return t.Parent == null ? this.ReadToEnd() : this.SetEnd(t.Parent);
      this._current = next;
      this.SetToken(this._current);
      return true;
    }

    private bool ReadToEnd()
    {
      this._current = (JToken) null;
      this.SetToken(JsonToken.None);
      return false;
    }

    private JsonToken? GetEndToken(JContainer c)
    {
      switch (c.Type)
      {
        case JTokenType.Object:
          return new JsonToken?(JsonToken.EndObject);
        case JTokenType.Array:
          return new JsonToken?(JsonToken.EndArray);
        case JTokenType.Constructor:
          return new JsonToken?(JsonToken.EndConstructor);
        case JTokenType.Property:
          return new JsonToken?();
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) c.Type, "Unexpected JContainer type.");
      }
    }

    private bool ReadInto(JContainer c)
    {
      JToken first = c.First;
      if (first == null)
        return this.SetEnd(c);
      this.SetToken(first);
      this._current = first;
      this._parent = (JToken) c;
      return true;
    }

    private bool SetEnd(JContainer c)
    {
      JsonToken? endToken = this.GetEndToken(c);
      if (!endToken.HasValue)
        return this.ReadOver((JToken) c);
      this.SetToken(endToken.GetValueOrDefault());
      this._current = (JToken) c;
      this._parent = (JToken) c;
      return true;
    }

    private void SetToken(JToken token)
    {
      switch (token.Type)
      {
        case JTokenType.Object:
          this.SetToken(JsonToken.StartObject);
          break;
        case JTokenType.Array:
          this.SetToken(JsonToken.StartArray);
          break;
        case JTokenType.Constructor:
          this.SetToken(JsonToken.StartConstructor, (object) ((JConstructor) token).Name);
          break;
        case JTokenType.Property:
          this.SetToken(JsonToken.PropertyName, (object) ((JProperty) token).Name);
          break;
        case JTokenType.Comment:
          this.SetToken(JsonToken.Comment, ((JValue) token).Value);
          break;
        case JTokenType.Integer:
          this.SetToken(JsonToken.Integer, ((JValue) token).Value);
          break;
        case JTokenType.Float:
          this.SetToken(JsonToken.Float, ((JValue) token).Value);
          break;
        case JTokenType.String:
          this.SetToken(JsonToken.String, ((JValue) token).Value);
          break;
        case JTokenType.Boolean:
          this.SetToken(JsonToken.Boolean, ((JValue) token).Value);
          break;
        case JTokenType.Null:
          this.SetToken(JsonToken.Null, ((JValue) token).Value);
          break;
        case JTokenType.Undefined:
          this.SetToken(JsonToken.Undefined, ((JValue) token).Value);
          break;
        case JTokenType.Date:
          this.SetToken(JsonToken.Date, ((JValue) token).Value);
          break;
        case JTokenType.Raw:
          this.SetToken(JsonToken.Raw, ((JValue) token).Value);
          break;
        case JTokenType.Bytes:
          this.SetToken(JsonToken.Bytes, ((JValue) token).Value);
          break;
        case JTokenType.Guid:
          this.SetToken(JsonToken.String, (object) this.SafeToString(((JValue) token).Value));
          break;
        case JTokenType.Uri:
          object obj = ((JValue) token).Value;
          if ((object) (obj as Uri) != null)
          {
            this.SetToken(JsonToken.String, (object) ((Uri) obj).OriginalString);
            break;
          }
          this.SetToken(JsonToken.String, (object) this.SafeToString(obj));
          break;
        case JTokenType.TimeSpan:
          this.SetToken(JsonToken.String, (object) this.SafeToString(((JValue) token).Value));
          break;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) token.Type, "Unexpected JTokenType.");
      }
    }

    private string SafeToString(object value) => value?.ToString();

    bool IJsonLineInfo.HasLineInfo()
    {
      if (this.CurrentState == JsonReader.State.Start)
        return false;
      IJsonLineInfo current = (IJsonLineInfo) this._current;
      return current != null && current.HasLineInfo();
    }

    int IJsonLineInfo.LineNumber
    {
      get
      {
        if (this.CurrentState == JsonReader.State.Start)
          return 0;
        IJsonLineInfo current = (IJsonLineInfo) this._current;
        return current != null ? current.LineNumber : 0;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        if (this.CurrentState == JsonReader.State.Start)
          return 0;
        IJsonLineInfo current = (IJsonLineInfo) this._current;
        return current != null ? current.LinePosition : 0;
      }
    }

    public override string Path
    {
      get
      {
        string source = base.Path;
        if (this._initialPath == null)
          this._initialPath = this._root.Path;
        if (!string.IsNullOrEmpty(this._initialPath))
        {
          if (string.IsNullOrEmpty(source))
            return this._initialPath;
          source = !source.StartsWith('[') ? this._initialPath + "." + source : this._initialPath + source;
        }
        return source;
      }
    }
  }
}
