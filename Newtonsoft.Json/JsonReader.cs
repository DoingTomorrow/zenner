﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonReader
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

#nullable disable
namespace Newtonsoft.Json
{
  public abstract class JsonReader : IDisposable
  {
    private JsonToken _tokenType;
    private object _value;
    internal char _quoteChar;
    internal JsonReader.State _currentState;
    private JsonPosition _currentPosition;
    private CultureInfo _culture;
    private DateTimeZoneHandling _dateTimeZoneHandling;
    private int? _maxDepth;
    private bool _hasExceededMaxDepth;
    internal DateParseHandling _dateParseHandling;
    internal FloatParseHandling _floatParseHandling;
    private string _dateFormatString;
    private List<JsonPosition> _stack;

    protected JsonReader.State CurrentState => this._currentState;

    public bool CloseInput { get; set; }

    public bool SupportMultipleContent { get; set; }

    public virtual char QuoteChar
    {
      get => this._quoteChar;
      protected internal set => this._quoteChar = value;
    }

    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get => this._dateTimeZoneHandling;
      set
      {
        this._dateTimeZoneHandling = value >= DateTimeZoneHandling.Local && value <= DateTimeZoneHandling.RoundtripKind ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public DateParseHandling DateParseHandling
    {
      get => this._dateParseHandling;
      set
      {
        this._dateParseHandling = value >= DateParseHandling.None && value <= DateParseHandling.DateTimeOffset ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public FloatParseHandling FloatParseHandling
    {
      get => this._floatParseHandling;
      set
      {
        this._floatParseHandling = value >= FloatParseHandling.Double && value <= FloatParseHandling.Decimal ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public string DateFormatString
    {
      get => this._dateFormatString;
      set => this._dateFormatString = value;
    }

    public int? MaxDepth
    {
      get => this._maxDepth;
      set
      {
        int? nullable = value;
        int num = 0;
        if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
          throw new ArgumentException("Value must be positive.", nameof (value));
        this._maxDepth = value;
      }
    }

    public virtual JsonToken TokenType => this._tokenType;

    public virtual object Value => this._value;

    public virtual Type ValueType => this._value == null ? (Type) null : this._value.GetType();

    public virtual int Depth
    {
      get
      {
        int count = this._stack != null ? this._stack.Count : 0;
        return JsonTokenUtils.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None ? count : count + 1;
      }
    }

    public virtual string Path
    {
      get
      {
        return this._currentPosition.Type == JsonContainerType.None ? string.Empty : JsonPosition.BuildPath(this._stack, (this._currentState == JsonReader.State.ArrayStart || this._currentState == JsonReader.State.ConstructorStart ? 0 : (this._currentState != JsonReader.State.ObjectStart ? 1 : 0)) != 0 ? new JsonPosition?(this._currentPosition) : new JsonPosition?());
      }
    }

    public CultureInfo Culture
    {
      get => this._culture ?? CultureInfo.InvariantCulture;
      set => this._culture = value;
    }

    internal JsonPosition GetPosition(int depth)
    {
      return this._stack != null && depth < this._stack.Count ? this._stack[depth] : this._currentPosition;
    }

    protected JsonReader()
    {
      this._currentState = JsonReader.State.Start;
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this._dateParseHandling = DateParseHandling.DateTime;
      this._floatParseHandling = FloatParseHandling.Double;
      this.CloseInput = true;
    }

    private void Push(JsonContainerType value)
    {
      this.UpdateScopeWithFinishedValue();
      if (this._currentPosition.Type == JsonContainerType.None)
      {
        this._currentPosition = new JsonPosition(value);
      }
      else
      {
        if (this._stack == null)
          this._stack = new List<JsonPosition>();
        this._stack.Add(this._currentPosition);
        this._currentPosition = new JsonPosition(value);
        if (!this._maxDepth.HasValue)
          return;
        int num = this.Depth + 1;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if ((num > valueOrDefault ? (maxDepth.HasValue ? 1 : 0) : 0) != 0 && !this._hasExceededMaxDepth)
        {
          this._hasExceededMaxDepth = true;
          throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._maxDepth));
        }
      }
    }

    private JsonContainerType Pop()
    {
      JsonPosition currentPosition;
      if (this._stack != null && this._stack.Count > 0)
      {
        currentPosition = this._currentPosition;
        this._currentPosition = this._stack[this._stack.Count - 1];
        this._stack.RemoveAt(this._stack.Count - 1);
      }
      else
      {
        currentPosition = this._currentPosition;
        this._currentPosition = new JsonPosition();
      }
      if (this._maxDepth.HasValue)
      {
        int depth = this.Depth;
        int? maxDepth = this._maxDepth;
        int valueOrDefault = maxDepth.GetValueOrDefault();
        if ((depth <= valueOrDefault ? (maxDepth.HasValue ? 1 : 0) : 0) != 0)
          this._hasExceededMaxDepth = false;
      }
      return currentPosition.Type;
    }

    private JsonContainerType Peek() => this._currentPosition.Type;

    public abstract bool Read();

    public virtual int? ReadAsInt32()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new int?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is int))
            this.SetToken(JsonToken.Integer, (object) Convert.ToInt32(this.Value, (IFormatProvider) CultureInfo.InvariantCulture), false);
          return new int?((int) this.Value);
        case JsonToken.String:
          return this.ReadInt32String((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal int? ReadInt32String(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new int?();
      }
      int result;
      if (int.TryParse(s, NumberStyles.Integer, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Integer, (object) result, false);
        return new int?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual string ReadAsString()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (string) null;
        case JsonToken.String:
          return (string) this.Value;
        default:
          if (!JsonTokenUtils.IsPrimitiveToken(contentToken) || this.Value == null)
            throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
          string str = !(this.Value is IFormattable) ? (!(this.Value is Uri) ? this.Value.ToString() : ((Uri) this.Value).OriginalString) : ((IFormattable) this.Value).ToString((string) null, (IFormatProvider) this.Culture);
          this.SetToken(JsonToken.String, (object) str, false);
          return str;
      }
    }

    public virtual byte[] ReadAsBytes()
    {
      JsonToken contentToken = this.GetContentToken();
      if (contentToken == JsonToken.None)
        return (byte[]) null;
      if (this.TokenType == JsonToken.StartObject)
      {
        this.ReadIntoWrappedTypeObject();
        byte[] numArray = this.ReadAsBytes();
        this.ReaderReadAndAssert();
        if (this.TokenType != JsonToken.EndObject)
          throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
        this.SetToken(JsonToken.Bytes, (object) numArray, false);
        return numArray;
      }
      switch (contentToken)
      {
        case JsonToken.StartArray:
          return this.ReadArrayIntoByteArray();
        case JsonToken.String:
          string s = (string) this.Value;
          Guid g;
          byte[] numArray1 = s.Length != 0 ? (!ConvertUtils.TryConvertGuid(s, out g) ? Convert.FromBase64String(s) : g.ToByteArray()) : new byte[0];
          this.SetToken(JsonToken.Bytes, (object) numArray1, false);
          return numArray1;
        case JsonToken.Null:
        case JsonToken.EndArray:
          return (byte[]) null;
        case JsonToken.Bytes:
          if (!(this.ValueType == typeof (Guid)))
            return (byte[]) this.Value;
          byte[] byteArray = ((Guid) this.Value).ToByteArray();
          this.SetToken(JsonToken.Bytes, (object) byteArray, false);
          return byteArray;
        default:
          throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal byte[] ReadArrayIntoByteArray()
    {
      List<byte> byteList = new List<byte>();
      JsonToken contentToken;
      while (true)
      {
        contentToken = this.GetContentToken();
        switch (contentToken)
        {
          case JsonToken.None:
            goto label_2;
          case JsonToken.Integer:
            byteList.Add(Convert.ToByte(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            goto label_4;
          default:
            goto label_5;
        }
      }
label_2:
      throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
label_4:
      byte[] array = byteList.ToArray();
      this.SetToken(JsonToken.Bytes, (object) array, false);
      return array;
label_5:
      throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
    }

    public virtual double? ReadAsDouble()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new double?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is double))
            this.SetToken(JsonToken.Float, (object) (!(this.Value is BigInteger) ? Convert.ToDouble(this.Value, (IFormatProvider) CultureInfo.InvariantCulture) : (double) (BigInteger) this.Value), false);
          return new double?((double) this.Value);
        case JsonToken.String:
          return this.ReadDoubleString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading double. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal double? ReadDoubleString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new double?();
      }
      double result;
      if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new double?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to double: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual bool? ReadAsBoolean()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new bool?();
        case JsonToken.Integer:
        case JsonToken.Float:
          bool flag = !(this.Value is BigInteger) ? Convert.ToBoolean(this.Value, (IFormatProvider) CultureInfo.InvariantCulture) : (BigInteger) this.Value != 0L;
          this.SetToken(JsonToken.Boolean, (object) flag, false);
          return new bool?(flag);
        case JsonToken.String:
          return this.ReadBooleanString((string) this.Value);
        case JsonToken.Boolean:
          return new bool?((bool) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading boolean. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal bool? ReadBooleanString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new bool?();
      }
      bool result;
      if (bool.TryParse(s, out result))
      {
        this.SetToken(JsonToken.Boolean, (object) result, false);
        return new bool?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to boolean: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual Decimal? ReadAsDecimal()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new Decimal?();
        case JsonToken.Integer:
        case JsonToken.Float:
          if (!(this.Value is Decimal))
            this.SetToken(JsonToken.Float, (object) Convert.ToDecimal(this.Value, (IFormatProvider) CultureInfo.InvariantCulture), false);
          return new Decimal?((Decimal) this.Value);
        case JsonToken.String:
          return this.ReadDecimalString((string) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal Decimal? ReadDecimalString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new Decimal?();
      }
      Decimal result;
      if (Decimal.TryParse(s, NumberStyles.Number, (IFormatProvider) this.Culture, out result))
      {
        this.SetToken(JsonToken.Float, (object) result, false);
        return new Decimal?(result);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    public virtual DateTime? ReadAsDateTime()
    {
      switch (this.GetContentToken())
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTime?();
        case JsonToken.String:
          return this.ReadDateTimeString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTimeOffset)
            this.SetToken(JsonToken.Date, (object) ((DateTimeOffset) this.Value).DateTime, false);
          return new DateTime?((DateTime) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
      }
    }

    internal DateTime? ReadDateTimeString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTime?();
      }
      DateTime dateTime;
      if (DateTimeUtils.TryParseDateTime(s, this.DateTimeZoneHandling, this._dateFormatString, this.Culture, out dateTime))
      {
        dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
        this.SetToken(JsonToken.Date, (object) dateTime, false);
        return new DateTime?(dateTime);
      }
      if (!DateTime.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTime))
        throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
      dateTime = DateTimeUtils.EnsureDateTime(dateTime, this.DateTimeZoneHandling);
      this.SetToken(JsonToken.Date, (object) dateTime, false);
      return new DateTime?(dateTime);
    }

    public virtual DateTimeOffset? ReadAsDateTimeOffset()
    {
      JsonToken contentToken = this.GetContentToken();
      switch (contentToken)
      {
        case JsonToken.None:
        case JsonToken.Null:
        case JsonToken.EndArray:
          return new DateTimeOffset?();
        case JsonToken.String:
          return this.ReadDateTimeOffsetString((string) this.Value);
        case JsonToken.Date:
          if (this.Value is DateTime)
            this.SetToken(JsonToken.Date, (object) new DateTimeOffset((DateTime) this.Value), false);
          return new DateTimeOffset?((DateTimeOffset) this.Value);
        default:
          throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) contentToken));
      }
    }

    internal DateTimeOffset? ReadDateTimeOffsetString(string s)
    {
      if (string.IsNullOrEmpty(s))
      {
        this.SetToken(JsonToken.Null, (object) null, false);
        return new DateTimeOffset?();
      }
      DateTimeOffset dateTimeOffset;
      if (DateTimeUtils.TryParseDateTimeOffset(s, this._dateFormatString, this.Culture, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      if (DateTimeOffset.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
      {
        this.SetToken(JsonToken.Date, (object) dateTimeOffset, false);
        return new DateTimeOffset?(dateTimeOffset);
      }
      this.SetToken(JsonToken.String, (object) s, false);
      throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) s));
    }

    internal void ReaderReadAndAssert()
    {
      if (!this.Read())
        throw this.CreateUnexpectedEndException();
    }

    internal JsonReaderException CreateUnexpectedEndException()
    {
      return JsonReaderException.Create(this, "Unexpected end when reading JSON.");
    }

    internal void ReadIntoWrappedTypeObject()
    {
      this.ReaderReadAndAssert();
      if (this.Value.ToString() == "$type")
      {
        this.ReaderReadAndAssert();
        if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
        {
          this.ReaderReadAndAssert();
          if (this.Value.ToString() == "$value")
            return;
        }
      }
      throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JsonToken.StartObject));
    }

    public void Skip()
    {
      if (this.TokenType == JsonToken.PropertyName)
        this.Read();
      if (!JsonTokenUtils.IsStartToken(this.TokenType))
        return;
      int depth = this.Depth;
      do
        ;
      while (this.Read() && depth < this.Depth);
    }

    protected void SetToken(JsonToken newToken) => this.SetToken(newToken, (object) null, true);

    protected void SetToken(JsonToken newToken, object value)
    {
      this.SetToken(newToken, value, true);
    }

    internal void SetToken(JsonToken newToken, object value, bool updateIndex)
    {
      this._tokenType = newToken;
      this._value = value;
      switch (newToken)
      {
        case JsonToken.StartObject:
          this._currentState = JsonReader.State.ObjectStart;
          this.Push(JsonContainerType.Object);
          break;
        case JsonToken.StartArray:
          this._currentState = JsonReader.State.ArrayStart;
          this.Push(JsonContainerType.Array);
          break;
        case JsonToken.StartConstructor:
          this._currentState = JsonReader.State.ConstructorStart;
          this.Push(JsonContainerType.Constructor);
          break;
        case JsonToken.PropertyName:
          this._currentState = JsonReader.State.Property;
          this._currentPosition.PropertyName = (string) value;
          break;
        case JsonToken.Raw:
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          this.SetPostValueState(updateIndex);
          break;
        case JsonToken.EndObject:
          this.ValidateEnd(JsonToken.EndObject);
          break;
        case JsonToken.EndArray:
          this.ValidateEnd(JsonToken.EndArray);
          break;
        case JsonToken.EndConstructor:
          this.ValidateEnd(JsonToken.EndConstructor);
          break;
      }
    }

    internal void SetPostValueState(bool updateIndex)
    {
      if (this.Peek() != JsonContainerType.None)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
      if (!updateIndex)
        return;
      this.UpdateScopeWithFinishedValue();
    }

    private void UpdateScopeWithFinishedValue()
    {
      if (!this._currentPosition.HasIndex)
        return;
      ++this._currentPosition.Position;
    }

    private void ValidateEnd(JsonToken endToken)
    {
      JsonContainerType jsonContainerType = this.Pop();
      if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
        throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) endToken, (object) jsonContainerType));
      if (this.Peek() != JsonContainerType.None)
        this._currentState = JsonReader.State.PostValue;
      else
        this.SetFinished();
    }

    protected void SetStateBasedOnCurrent()
    {
      JsonContainerType jsonContainerType = this.Peek();
      switch (jsonContainerType)
      {
        case JsonContainerType.None:
          this.SetFinished();
          break;
        case JsonContainerType.Object:
          this._currentState = JsonReader.State.Object;
          break;
        case JsonContainerType.Array:
          this._currentState = JsonReader.State.Array;
          break;
        case JsonContainerType.Constructor:
          this._currentState = JsonReader.State.Constructor;
          break;
        default:
          throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jsonContainerType));
      }
    }

    private void SetFinished()
    {
      if (this.SupportMultipleContent)
        this._currentState = JsonReader.State.Start;
      else
        this._currentState = JsonReader.State.Finished;
    }

    private JsonContainerType GetTypeForCloseToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return JsonContainerType.Object;
        case JsonToken.EndArray:
          return JsonContainerType.Array;
        case JsonToken.EndConstructor:
          return JsonContainerType.Constructor;
        default:
          throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token));
      }
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!(this._currentState != JsonReader.State.Closed & disposing))
        return;
      this.Close();
    }

    public virtual void Close()
    {
      this._currentState = JsonReader.State.Closed;
      this._tokenType = JsonToken.None;
      this._value = (object) null;
    }

    internal void ReadAndAssert()
    {
      if (!this.Read())
        throw JsonSerializationException.Create(this, "Unexpected end when reading JSON.");
    }

    internal bool ReadAndMoveToContent() => this.Read() && this.MoveToContent();

    internal bool MoveToContent()
    {
      for (JsonToken tokenType = this.TokenType; tokenType == JsonToken.None || tokenType == JsonToken.Comment; tokenType = this.TokenType)
      {
        if (!this.Read())
          return false;
      }
      return true;
    }

    private JsonToken GetContentToken()
    {
      while (this.Read())
      {
        JsonToken tokenType = this.TokenType;
        if (tokenType != JsonToken.Comment)
          return tokenType;
      }
      this.SetToken(JsonToken.None);
      return JsonToken.None;
    }

    protected internal enum State
    {
      Start,
      Complete,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      Closed,
      PostValue,
      ConstructorStart,
      Constructor,
      Error,
      Finished,
    }
  }
}
