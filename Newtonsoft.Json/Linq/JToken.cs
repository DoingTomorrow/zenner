﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JToken
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public abstract class JToken : 
    IJEnumerable<JToken>,
    IEnumerable<JToken>,
    IEnumerable,
    IJsonLineInfo,
    ICloneable,
    IDynamicMetaObjectProvider
  {
    private static JTokenEqualityComparer _equalityComparer;
    private JContainer _parent;
    private JToken _previous;
    private JToken _next;
    private object _annotations;
    private static readonly JTokenType[] BooleanTypes = new JTokenType[6]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean
    };
    private static readonly JTokenType[] NumberTypes = new JTokenType[6]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean
    };
    private static readonly JTokenType[] BigIntegerTypes = new JTokenType[7]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean,
      JTokenType.Bytes
    };
    private static readonly JTokenType[] StringTypes = new JTokenType[11]
    {
      JTokenType.Date,
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Boolean,
      JTokenType.Bytes,
      JTokenType.Guid,
      JTokenType.TimeSpan,
      JTokenType.Uri
    };
    private static readonly JTokenType[] GuidTypes = new JTokenType[5]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Guid,
      JTokenType.Bytes
    };
    private static readonly JTokenType[] TimeSpanTypes = new JTokenType[4]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.TimeSpan
    };
    private static readonly JTokenType[] UriTypes = new JTokenType[4]
    {
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Uri
    };
    private static readonly JTokenType[] CharTypes = new JTokenType[5]
    {
      JTokenType.Integer,
      JTokenType.Float,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw
    };
    private static readonly JTokenType[] DateTimeTypes = new JTokenType[4]
    {
      JTokenType.Date,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw
    };
    private static readonly JTokenType[] BytesTypes = new JTokenType[5]
    {
      JTokenType.Bytes,
      JTokenType.String,
      JTokenType.Comment,
      JTokenType.Raw,
      JTokenType.Integer
    };

    public static JTokenEqualityComparer EqualityComparer
    {
      get
      {
        if (JToken._equalityComparer == null)
          JToken._equalityComparer = new JTokenEqualityComparer();
        return JToken._equalityComparer;
      }
    }

    public JContainer Parent
    {
      [DebuggerStepThrough] get => this._parent;
      internal set => this._parent = value;
    }

    public JToken Root
    {
      get
      {
        JContainer parent = this.Parent;
        if (parent == null)
          return this;
        while (parent.Parent != null)
          parent = parent.Parent;
        return (JToken) parent;
      }
    }

    internal abstract JToken CloneToken();

    internal abstract bool DeepEquals(JToken node);

    public abstract JTokenType Type { get; }

    public abstract bool HasValues { get; }

    public static bool DeepEquals(JToken t1, JToken t2)
    {
      if (t1 == t2)
        return true;
      return t1 != null && t2 != null && t1.DeepEquals(t2);
    }

    public JToken Next
    {
      get => this._next;
      internal set => this._next = value;
    }

    public JToken Previous
    {
      get => this._previous;
      internal set => this._previous = value;
    }

    public string Path
    {
      get
      {
        if (this.Parent == null)
          return string.Empty;
        List<JsonPosition> positions = new List<JsonPosition>();
        JToken jtoken1 = (JToken) null;
        for (JToken jtoken2 = this; jtoken2 != null; jtoken2 = (JToken) jtoken2.Parent)
        {
          JsonPosition jsonPosition1;
          switch (jtoken2.Type)
          {
            case JTokenType.Array:
            case JTokenType.Constructor:
              if (jtoken1 != null)
              {
                int num = ((IList<JToken>) jtoken2).IndexOf(jtoken1);
                List<JsonPosition> jsonPositionList = positions;
                jsonPosition1 = new JsonPosition(JsonContainerType.Array);
                jsonPosition1.Position = num;
                JsonPosition jsonPosition2 = jsonPosition1;
                jsonPositionList.Add(jsonPosition2);
                break;
              }
              break;
            case JTokenType.Property:
              JProperty jproperty = (JProperty) jtoken2;
              List<JsonPosition> jsonPositionList1 = positions;
              jsonPosition1 = new JsonPosition(JsonContainerType.Object);
              jsonPosition1.PropertyName = jproperty.Name;
              JsonPosition jsonPosition3 = jsonPosition1;
              jsonPositionList1.Add(jsonPosition3);
              break;
          }
          jtoken1 = jtoken2;
        }
        positions.Reverse();
        return JsonPosition.BuildPath(positions, new JsonPosition?());
      }
    }

    internal JToken()
    {
    }

    public void AddAfterSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this) + 1, content, false);
    }

    public void AddBeforeSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this), content, false);
    }

    public IEnumerable<JToken> Ancestors() => this.GetAncestors(false);

    public IEnumerable<JToken> AncestorsAndSelf() => this.GetAncestors(true);

    internal IEnumerable<JToken> GetAncestors(bool self)
    {
      JToken current;
      for (current = self ? this : (JToken) this.Parent; current != null; current = (JToken) current.Parent)
        yield return current;
      current = (JToken) null;
    }

    public IEnumerable<JToken> AfterSelf()
    {
      if (this.Parent != null)
      {
        JToken o;
        for (o = this.Next; o != null; o = o.Next)
          yield return o;
        o = (JToken) null;
      }
    }

    public IEnumerable<JToken> BeforeSelf()
    {
      JToken o;
      for (o = this.Parent.First; o != this; o = o.Next)
        yield return o;
      o = (JToken) null;
    }

    public virtual JToken this[object key]
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
      set
      {
        throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    public virtual T Value<T>(object key)
    {
      JToken token = this[key];
      return token != null ? token.Convert<JToken, T>() : default (T);
    }

    public virtual JToken First
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    public virtual JToken Last
    {
      get
      {
        throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    public virtual JEnumerable<JToken> Children() => JEnumerable<JToken>.Empty;

    public JEnumerable<T> Children<T>() where T : JToken
    {
      return new JEnumerable<T>(this.Children().OfType<T>());
    }

    public virtual IEnumerable<T> Values<T>()
    {
      throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    public void Remove()
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.RemoveItem(this);
    }

    public void Replace(JToken value)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.ReplaceItem(this, value);
    }

    public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

    public override string ToString() => this.ToString(Formatting.Indented);

    public string ToString(Formatting formatting, params JsonConverter[] converters)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        JsonTextWriter writer = new JsonTextWriter((TextWriter) stringWriter);
        writer.Formatting = formatting;
        this.WriteTo((JsonWriter) writer, converters);
        return stringWriter.ToString();
      }
    }

    private static JValue EnsureValue(JToken value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (value is JProperty)
        value = ((JProperty) value).Value;
      return value as JValue;
    }

    private static string GetType(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, nameof (token));
      if (token is JProperty)
        token = ((JProperty) token).Value;
      return token.Type.ToString();
    }

    private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
    {
      if (Array.IndexOf<JTokenType>(validTypes, o.Type) != -1)
        return true;
      if (!nullable)
        return false;
      return o.Type == JTokenType.Null || o.Type == JTokenType.Undefined;
    }

    public static explicit operator bool(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.BooleanTypes, false))
        throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? Convert.ToBoolean((int) (BigInteger) o.Value) : Convert.ToBoolean(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator DateTimeOffset(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.DateTimeTypes, false))
        throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is DateTimeOffset)
        return (DateTimeOffset) o.Value;
      return o.Value is string ? DateTimeOffset.Parse((string) o.Value, (IFormatProvider) CultureInfo.InvariantCulture) : new DateTimeOffset(Convert.ToDateTime(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator bool?(JToken value)
    {
      if (value == null)
        return new bool?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.BooleanTypes, true))
        throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new bool?(Convert.ToBoolean((int) (BigInteger) o.Value));
      return o.Value == null ? new bool?() : new bool?(Convert.ToBoolean(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator long(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (long) (BigInteger) o.Value : Convert.ToInt64(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator DateTime?(JToken value)
    {
      if (value == null)
        return new DateTime?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.DateTimeTypes, true))
        throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is DateTimeOffset)
        return new DateTime?(((DateTimeOffset) o.Value).DateTime);
      return o.Value == null ? new DateTime?() : new DateTime?(Convert.ToDateTime(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator DateTimeOffset?(JToken value)
    {
      if (value == null)
        return new DateTimeOffset?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.DateTimeTypes, true))
        throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value == null)
        return new DateTimeOffset?();
      if (o.Value is DateTimeOffset)
        return (DateTimeOffset?) o.Value;
      return o.Value is string ? new DateTimeOffset?(DateTimeOffset.Parse((string) o.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : new DateTimeOffset?(new DateTimeOffset(Convert.ToDateTime(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    public static explicit operator Decimal?(JToken value)
    {
      if (value == null)
        return new Decimal?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new Decimal?((Decimal) (BigInteger) o.Value);
      return o.Value == null ? new Decimal?() : new Decimal?(Convert.ToDecimal(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator double?(JToken value)
    {
      if (value == null)
        return new double?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new double?((double) (BigInteger) o.Value);
      return o.Value == null ? new double?() : new double?(Convert.ToDouble(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator char?(JToken value)
    {
      if (value == null)
        return new char?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.CharTypes, true))
        throw new ArgumentException("Can not convert {0} to Char.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new char?((char) (ushort) (BigInteger) o.Value);
      return o.Value == null ? new char?() : new char?(Convert.ToChar(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator int(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (int) (BigInteger) o.Value : Convert.ToInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator short(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (short) (BigInteger) o.Value : Convert.ToInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ushort(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (ushort) (BigInteger) o.Value : Convert.ToUInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator char(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.CharTypes, false))
        throw new ArgumentException("Can not convert {0} to Char.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (char) (ushort) (BigInteger) o.Value : Convert.ToChar(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator byte(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Byte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (byte) (BigInteger) o.Value : Convert.ToByte(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator sbyte(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to SByte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (sbyte) (BigInteger) o.Value : Convert.ToSByte(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator int?(JToken value)
    {
      if (value == null)
        return new int?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new int?((int) (BigInteger) o.Value);
      return o.Value == null ? new int?() : new int?(Convert.ToInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator short?(JToken value)
    {
      if (value == null)
        return new short?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new short?((short) (BigInteger) o.Value);
      return o.Value == null ? new short?() : new short?(Convert.ToInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator ushort?(JToken value)
    {
      if (value == null)
        return new ushort?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new ushort?((ushort) (BigInteger) o.Value);
      return o.Value == null ? new ushort?() : new ushort?(Convert.ToUInt16(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator byte?(JToken value)
    {
      if (value == null)
        return new byte?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Byte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new byte?((byte) (BigInteger) o.Value);
      return o.Value == null ? new byte?() : new byte?(Convert.ToByte(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator sbyte?(JToken value)
    {
      if (value == null)
        return new sbyte?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to SByte.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new sbyte?((sbyte) (BigInteger) o.Value);
      return o.Value == null ? new sbyte?() : new sbyte?(Convert.ToSByte(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator DateTime(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.DateTimeTypes, false))
        throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is DateTimeOffset ? ((DateTimeOffset) o.Value).DateTime : Convert.ToDateTime(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator long?(JToken value)
    {
      if (value == null)
        return new long?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Int64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new long?((long) (BigInteger) o.Value);
      return o.Value == null ? new long?() : new long?(Convert.ToInt64(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator float?(JToken value)
    {
      if (value == null)
        return new float?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new float?((float) (BigInteger) o.Value);
      return o.Value == null ? new float?() : new float?(Convert.ToSingle(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator Decimal(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (Decimal) (BigInteger) o.Value : Convert.ToDecimal(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator uint?(JToken value)
    {
      if (value == null)
        return new uint?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new uint?((uint) (BigInteger) o.Value);
      return o.Value == null ? new uint?() : new uint?(Convert.ToUInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator ulong?(JToken value)
    {
      if (value == null)
        return new ulong?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, true))
        throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is BigInteger)
        return new ulong?((ulong) (BigInteger) o.Value);
      return o.Value == null ? new ulong?() : new ulong?(Convert.ToUInt64(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator double(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Double.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (double) (BigInteger) o.Value : Convert.ToDouble(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator float(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to Single.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (float) (BigInteger) o.Value : Convert.ToSingle(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator string(JToken value)
    {
      if (value == null)
        return (string) null;
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.StringTypes, true))
        throw new ArgumentException("Can not convert {0} to String.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value == null)
        return (string) null;
      if (o.Value is byte[])
        return Convert.ToBase64String((byte[]) o.Value);
      return o.Value is BigInteger ? ((BigInteger) o.Value).ToString((IFormatProvider) CultureInfo.InvariantCulture) : Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator uint(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (uint) (BigInteger) o.Value : Convert.ToUInt32(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ulong(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.NumberTypes, false))
        throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value is BigInteger ? (ulong) (BigInteger) o.Value : Convert.ToUInt64(o.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator byte[](JToken value)
    {
      if (value == null)
        return (byte[]) null;
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.BytesTypes, false))
        throw new ArgumentException("Can not convert {0} to byte array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is string)
        return Convert.FromBase64String(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (o.Value is BigInteger)
        return ((BigInteger) o.Value).ToByteArray();
      return o.Value is byte[] ? (byte[]) o.Value : throw new ArgumentException("Can not convert {0} to byte array.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    public static explicit operator Guid(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.GuidTypes, false))
        throw new ArgumentException("Can not convert {0} to Guid.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value is byte[])
        return new Guid((byte[]) o.Value);
      return !(o.Value is Guid) ? new Guid(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (Guid) o.Value;
    }

    public static explicit operator Guid?(JToken value)
    {
      if (value == null)
        return new Guid?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.GuidTypes, true))
        throw new ArgumentException("Can not convert {0} to Guid.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value == null)
        return new Guid?();
      return o.Value is byte[] ? new Guid?(new Guid((byte[]) o.Value)) : new Guid?(o.Value is Guid ? (Guid) o.Value : new Guid(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    public static explicit operator TimeSpan(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.TimeSpanTypes, false))
        throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return !(o.Value is TimeSpan) ? ConvertUtils.ParseTimeSpan(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (TimeSpan) o.Value;
    }

    public static explicit operator TimeSpan?(JToken value)
    {
      if (value == null)
        return new TimeSpan?();
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.TimeSpanTypes, true))
        throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new TimeSpan?() : new TimeSpan?(o.Value is TimeSpan ? (TimeSpan) o.Value : ConvertUtils.ParseTimeSpan(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    public static explicit operator Uri(JToken value)
    {
      if (value == null)
        return (Uri) null;
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.UriTypes, true))
        throw new ArgumentException("Can not convert {0} to Uri.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (o.Value == null)
        return (Uri) null;
      return !(o.Value is Uri) ? new Uri(Convert.ToString(o.Value, (IFormatProvider) CultureInfo.InvariantCulture)) : (Uri) o.Value;
    }

    private static BigInteger ToBigInteger(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      return o != null && JToken.ValidateToken((JToken) o, JToken.BigIntegerTypes, false) ? ConvertUtils.ToBigInteger(o.Value) : throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
    }

    private static BigInteger? ToBigIntegerNullable(JToken value)
    {
      JValue o = JToken.EnsureValue(value);
      if (o == null || !JToken.ValidateToken((JToken) o, JToken.BigIntegerTypes, true))
        throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      return o.Value == null ? new BigInteger?() : new BigInteger?(ConvertUtils.ToBigInteger(o.Value));
    }

    public static implicit operator JToken(bool value) => (JToken) new JValue(value);

    public static implicit operator JToken(DateTimeOffset value) => (JToken) new JValue(value);

    public static implicit operator JToken(byte value) => (JToken) new JValue((long) value);

    public static implicit operator JToken(byte? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(sbyte value) => (JToken) new JValue((long) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(sbyte? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(bool? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(long value) => (JToken) new JValue(value);

    public static implicit operator JToken(DateTime? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(DateTimeOffset? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(Decimal? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(double? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(short value) => (JToken) new JValue((long) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort value) => (JToken) new JValue((long) value);

    public static implicit operator JToken(int value) => (JToken) new JValue((long) value);

    public static implicit operator JToken(int? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(DateTime value) => (JToken) new JValue(value);

    public static implicit operator JToken(long? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(float? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(Decimal value) => (JToken) new JValue(value);

    [CLSCompliant(false)]
    public static implicit operator JToken(short? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(uint? value) => (JToken) new JValue((object) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(double value) => (JToken) new JValue(value);

    public static implicit operator JToken(float value) => (JToken) new JValue(value);

    public static implicit operator JToken(string value) => (JToken) new JValue(value);

    [CLSCompliant(false)]
    public static implicit operator JToken(uint value) => (JToken) new JValue((long) value);

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong value) => (JToken) new JValue(value);

    public static implicit operator JToken(byte[] value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(Uri value) => (JToken) new JValue(value);

    public static implicit operator JToken(TimeSpan value) => (JToken) new JValue(value);

    public static implicit operator JToken(TimeSpan? value) => (JToken) new JValue((object) value);

    public static implicit operator JToken(Guid value) => (JToken) new JValue(value);

    public static implicit operator JToken(Guid? value) => (JToken) new JValue((object) value);

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<JToken>) this).GetEnumerator();
    }

    IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator() => this.Children().GetEnumerator();

    internal abstract int GetDeepHashCode();

    IJEnumerable<JToken> IJEnumerable<JToken>.this[object key] => (IJEnumerable<JToken>) this[key];

    public JsonReader CreateReader() => (JsonReader) new JTokenReader(this);

    internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull(o, nameof (o));
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jsonSerializer.Serialize((JsonWriter) jtokenWriter, o);
        return jtokenWriter.Token;
      }
    }

    public static JToken FromObject(object o)
    {
      return JToken.FromObjectInternal(o, JsonSerializer.CreateDefault());
    }

    public static JToken FromObject(object o, JsonSerializer jsonSerializer)
    {
      return JToken.FromObjectInternal(o, jsonSerializer);
    }

    public T ToObject<T>() => (T) this.ToObject(typeof (T));

    public object ToObject(System.Type objectType)
    {
      if (JsonConvert.DefaultSettings == null)
      {
        bool isEnum;
        PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out isEnum);
        if (isEnum)
        {
          if (this.Type == JTokenType.String)
          {
            try
            {
              return this.ToObject(objectType, JsonSerializer.CreateDefault());
            }
            catch (Exception ex)
            {
              throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (string) this, (object) (objectType.IsEnum() ? (MemberInfo) objectType : (MemberInfo) Nullable.GetUnderlyingType(objectType)).Name), ex);
            }
          }
          else if (this.Type == JTokenType.Integer)
            return Enum.ToObject(objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType), ((JValue) this).Value);
        }
        switch (typeCode)
        {
          case PrimitiveTypeCode.Char:
            return (object) (char) this;
          case PrimitiveTypeCode.CharNullable:
            return (object) (char?) this;
          case PrimitiveTypeCode.Boolean:
            return (object) (bool) this;
          case PrimitiveTypeCode.BooleanNullable:
            return (object) (bool?) this;
          case PrimitiveTypeCode.SByte:
            return (object) (sbyte?) this;
          case PrimitiveTypeCode.SByteNullable:
            return (object) (sbyte) this;
          case PrimitiveTypeCode.Int16:
            return (object) (short) this;
          case PrimitiveTypeCode.Int16Nullable:
            return (object) (short?) this;
          case PrimitiveTypeCode.UInt16:
            return (object) (ushort) this;
          case PrimitiveTypeCode.UInt16Nullable:
            return (object) (ushort?) this;
          case PrimitiveTypeCode.Int32:
            return (object) (int) this;
          case PrimitiveTypeCode.Int32Nullable:
            return (object) (int?) this;
          case PrimitiveTypeCode.Byte:
            return (object) (byte) this;
          case PrimitiveTypeCode.ByteNullable:
            return (object) (byte?) this;
          case PrimitiveTypeCode.UInt32:
            return (object) (uint) this;
          case PrimitiveTypeCode.UInt32Nullable:
            return (object) (uint?) this;
          case PrimitiveTypeCode.Int64:
            return (object) (long) this;
          case PrimitiveTypeCode.Int64Nullable:
            return (object) (long?) this;
          case PrimitiveTypeCode.UInt64:
            return (object) (ulong) this;
          case PrimitiveTypeCode.UInt64Nullable:
            return (object) (ulong?) this;
          case PrimitiveTypeCode.Single:
            return (object) (float) this;
          case PrimitiveTypeCode.SingleNullable:
            return (object) (float?) this;
          case PrimitiveTypeCode.Double:
            return (object) (double) this;
          case PrimitiveTypeCode.DoubleNullable:
            return (object) (double?) this;
          case PrimitiveTypeCode.DateTime:
            return (object) (DateTime) this;
          case PrimitiveTypeCode.DateTimeNullable:
            return (object) (DateTime?) this;
          case PrimitiveTypeCode.DateTimeOffset:
            return (object) (DateTimeOffset) this;
          case PrimitiveTypeCode.DateTimeOffsetNullable:
            return (object) (DateTimeOffset?) this;
          case PrimitiveTypeCode.Decimal:
            return (object) (Decimal) this;
          case PrimitiveTypeCode.DecimalNullable:
            return (object) (Decimal?) this;
          case PrimitiveTypeCode.Guid:
            return (object) (Guid) this;
          case PrimitiveTypeCode.GuidNullable:
            return (object) (Guid?) this;
          case PrimitiveTypeCode.TimeSpan:
            return (object) (TimeSpan) this;
          case PrimitiveTypeCode.TimeSpanNullable:
            return (object) (TimeSpan?) this;
          case PrimitiveTypeCode.BigInteger:
            return (object) JToken.ToBigInteger(this);
          case PrimitiveTypeCode.BigIntegerNullable:
            return (object) JToken.ToBigIntegerNullable(this);
          case PrimitiveTypeCode.Uri:
            return (object) (Uri) this;
          case PrimitiveTypeCode.String:
            return (object) (string) this;
        }
      }
      return this.ToObject(objectType, JsonSerializer.CreateDefault());
    }

    public T ToObject<T>(JsonSerializer jsonSerializer)
    {
      return (T) this.ToObject(typeof (T), jsonSerializer);
    }

    public object ToObject(System.Type objectType, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, nameof (jsonSerializer));
      using (JTokenReader reader = new JTokenReader(this))
        return jsonSerializer.Deserialize((JsonReader) reader, objectType);
    }

    public static JToken ReadFrom(JsonReader reader)
    {
      return JToken.ReadFrom(reader, (JsonLoadSettings) null);
    }

    public static JToken ReadFrom(JsonReader reader, JsonLoadSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && (settings == null || settings.CommentHandling != CommentHandling.Ignore ? (reader.Read() ? 1 : 0) : (reader.ReadAndMoveToContent() ? 1 : 0)) == 0)
        throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
      IJsonLineInfo lineInfo = reader as IJsonLineInfo;
      switch (reader.TokenType)
      {
        case JsonToken.StartObject:
          return (JToken) JObject.Load(reader, settings);
        case JsonToken.StartArray:
          return (JToken) JArray.Load(reader, settings);
        case JsonToken.StartConstructor:
          return (JToken) JConstructor.Load(reader, settings);
        case JsonToken.PropertyName:
          return (JToken) JProperty.Load(reader, settings);
        case JsonToken.Comment:
          JValue comment = JValue.CreateComment(reader.Value.ToString());
          comment.SetLineInfo(lineInfo, settings);
          return (JToken) comment;
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Date:
        case JsonToken.Bytes:
          JValue jvalue1 = new JValue(reader.Value);
          jvalue1.SetLineInfo(lineInfo, settings);
          return (JToken) jvalue1;
        case JsonToken.Null:
          JValue jvalue2 = JValue.CreateNull();
          jvalue2.SetLineInfo(lineInfo, settings);
          return (JToken) jvalue2;
        case JsonToken.Undefined:
          JValue undefined = JValue.CreateUndefined();
          undefined.SetLineInfo(lineInfo, settings);
          return (JToken) undefined;
        default:
          throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      }
    }

    public static JToken Parse(string json) => JToken.Parse(json, (JsonLoadSettings) null);

    public static JToken Parse(string json, JsonLoadSettings settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
      {
        JToken jtoken = JToken.Load(reader, settings);
        if (reader.Read() && reader.TokenType != JsonToken.Comment)
          throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
        return jtoken;
      }
    }

    public static JToken Load(JsonReader reader, JsonLoadSettings settings)
    {
      return JToken.ReadFrom(reader, settings);
    }

    public static JToken Load(JsonReader reader) => JToken.Load(reader, (JsonLoadSettings) null);

    internal void SetLineInfo(IJsonLineInfo lineInfo, JsonLoadSettings settings)
    {
      if (settings != null && settings.LineInfoHandling == LineInfoHandling.Load || lineInfo == null || !lineInfo.HasLineInfo())
        return;
      this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
    }

    internal void SetLineInfo(int lineNumber, int linePosition)
    {
      this.AddAnnotation((object) new JToken.LineInfoAnnotation(lineNumber, linePosition));
    }

    bool IJsonLineInfo.HasLineInfo() => this.Annotation<JToken.LineInfoAnnotation>() != null;

    int IJsonLineInfo.LineNumber
    {
      get
      {
        JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.LineNumber : 0;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        JToken.LineInfoAnnotation lineInfoAnnotation = this.Annotation<JToken.LineInfoAnnotation>();
        return lineInfoAnnotation != null ? lineInfoAnnotation.LinePosition : 0;
      }
    }

    public JToken SelectToken(string path) => this.SelectToken(path, false);

    public JToken SelectToken(string path, bool errorWhenNoMatch)
    {
      JPath jpath = new JPath(path);
      JToken jtoken1 = (JToken) null;
      int num = errorWhenNoMatch ? 1 : 0;
      foreach (JToken jtoken2 in jpath.Evaluate(this, num != 0))
        jtoken1 = jtoken1 == null ? jtoken2 : throw new JsonException("Path returned multiple tokens.");
      return jtoken1;
    }

    public IEnumerable<JToken> SelectTokens(string path) => this.SelectTokens(path, false);

    public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
    {
      return new JPath(path).Evaluate(this, errorWhenNoMatch);
    }

    protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
    {
      return (DynamicMetaObject) new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>(), true);
    }

    DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
    {
      return this.GetMetaObject(parameter);
    }

    object ICloneable.Clone() => (object) this.DeepClone();

    public JToken DeepClone() => this.CloneToken();

    public void AddAnnotation(object annotation)
    {
      if (annotation == null)
        throw new ArgumentNullException(nameof (annotation));
      if (this._annotations == null)
      {
        object obj;
        if (!(annotation is object[]))
        {
          obj = annotation;
        }
        else
        {
          obj = (object) new object[1];
          obj[0] = annotation;
        }
        this._annotations = obj;
      }
      else if (!(this._annotations is object[] annotations))
      {
        this._annotations = (object) new object[2]
        {
          this._annotations,
          annotation
        };
      }
      else
      {
        int index = 0;
        while (index < annotations.Length && annotations[index] != null)
          ++index;
        if (index == annotations.Length)
        {
          Array.Resize<object>(ref annotations, index * 2);
          this._annotations = (object) annotations;
        }
        annotations[index] = annotation;
      }
    }

    public T Annotation<T>() where T : class
    {
      if (this._annotations != null)
      {
        if (!(this._annotations is object[] annotations))
          return this._annotations as T;
        for (int index = 0; index < annotations.Length; ++index)
        {
          object obj1 = annotations[index];
          if (obj1 != null)
          {
            if (obj1 is T obj2)
              return obj2;
          }
          else
            break;
        }
      }
      return default (T);
    }

    public object Annotation(System.Type type)
    {
      if (type == (System.Type) null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations != null)
      {
        if (!(this._annotations is object[] annotations))
        {
          if (type.IsInstanceOfType(this._annotations))
            return this._annotations;
        }
        else
        {
          for (int index = 0; index < annotations.Length; ++index)
          {
            object o = annotations[index];
            if (o != null)
            {
              if (type.IsInstanceOfType(o))
                return o;
            }
            else
              break;
          }
        }
      }
      return (object) null;
    }

    public IEnumerable<T> Annotations<T>() where T : class
    {
      if (this._annotations != null)
      {
        if (this._annotations is object[] annotations2)
        {
          for (int i = 0; i < annotations2.Length; ++i)
          {
            object obj1 = annotations2[i];
            if (obj1 == null)
              break;
            if (obj1 is T obj2)
              yield return obj2;
          }
        }
        else if (this._annotations is T annotations1)
          yield return annotations1;
      }
    }

    public IEnumerable<object> Annotations(System.Type type)
    {
      if (type == (System.Type) null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations != null)
      {
        if (this._annotations is object[] annotations)
        {
          for (int i = 0; i < annotations.Length; ++i)
          {
            object o = annotations[i];
            if (o == null)
              break;
            if (type.IsInstanceOfType(o))
              yield return o;
          }
        }
        else if (type.IsInstanceOfType(this._annotations))
          yield return this._annotations;
      }
    }

    public void RemoveAnnotations<T>() where T : class
    {
      if (this._annotations == null)
        return;
      if (!(this._annotations is object[] annotations))
      {
        if (!(this._annotations is T))
          return;
        this._annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object obj = annotations[index];
          if (obj != null)
          {
            if (!(obj is T))
              annotations[num++] = obj;
          }
          else
            break;
        }
        if (num != 0)
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
        else
          this._annotations = (object) null;
      }
    }

    public void RemoveAnnotations(System.Type type)
    {
      if (type == (System.Type) null)
        throw new ArgumentNullException(nameof (type));
      if (this._annotations == null)
        return;
      if (!(this._annotations is object[] annotations))
      {
        if (!type.IsInstanceOfType(this._annotations))
          return;
        this._annotations = (object) null;
      }
      else
      {
        int index = 0;
        int num = 0;
        for (; index < annotations.Length; ++index)
        {
          object o = annotations[index];
          if (o != null)
          {
            if (!type.IsInstanceOfType(o))
              annotations[num++] = o;
          }
          else
            break;
        }
        if (num != 0)
        {
          while (num < index)
            annotations[num++] = (object) null;
        }
        else
          this._annotations = (object) null;
      }
    }

    private class LineInfoAnnotation
    {
      internal readonly int LineNumber;
      internal readonly int LinePosition;

      public LineInfoAnnotation(int lineNumber, int linePosition)
      {
        this.LineNumber = lineNumber;
        this.LinePosition = linePosition;
      }
    }
  }
}
