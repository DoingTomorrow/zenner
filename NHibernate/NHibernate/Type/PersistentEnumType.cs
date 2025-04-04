// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.PersistentEnumType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class PersistentEnumType : AbstractEnumType
  {
    private static readonly Dictionary<System.Type, PersistentEnumType.IEnumConverter> converters = new Dictionary<System.Type, PersistentEnumType.IEnumConverter>(8);
    private readonly PersistentEnumType.IEnumConverter converter;

    static PersistentEnumType()
    {
      PersistentEnumType.converters.Add(typeof (int), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemInt32EnumConverter());
      PersistentEnumType.converters.Add(typeof (short), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemInt16EnumConverter());
      PersistentEnumType.converters.Add(typeof (long), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemInt64EnumConverter());
      PersistentEnumType.converters.Add(typeof (byte), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemByteEnumConverter());
      PersistentEnumType.converters.Add(typeof (sbyte), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemSByteEnumConverter());
      PersistentEnumType.converters.Add(typeof (ushort), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemUInt16EnumConverter());
      PersistentEnumType.converters.Add(typeof (uint), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemUInt32EnumConverter());
      PersistentEnumType.converters.Add(typeof (ulong), (PersistentEnumType.IEnumConverter) new PersistentEnumType.SystemUInt64EnumConverter());
    }

    public PersistentEnumType(System.Type enumClass)
      : base(PersistentEnumType.GetEnumCoverter(enumClass).SqlType, enumClass)
    {
      this.converter = PersistentEnumType.GetEnumCoverter(enumClass);
    }

    public static PersistentEnumType.IEnumConverter GetEnumCoverter(System.Type enumClass)
    {
      System.Type underlyingType = Enum.GetUnderlyingType(enumClass);
      PersistentEnumType.IEnumConverter enumCoverter;
      if (!PersistentEnumType.converters.TryGetValue(underlyingType, out enumCoverter))
        throw new HibernateException("Unknown UnderlyingDbType for Enum; type:" + enumClass.FullName);
      return enumCoverter;
    }

    public override object Get(IDataReader rs, int index)
    {
      object r = rs[index];
      return r == DBNull.Value || r == null ? (object) null : this.GetInstance(r);
    }

    public virtual object GetInstance(object code)
    {
      try
      {
        return this.converter.ToObject(this.ReturnedClass, code);
      }
      catch (ArgumentException ex)
      {
        throw new HibernateException("ArgumentException occurred inside Enum.ToObject()", (Exception) ex);
      }
    }

    public virtual object GetValue(object code) => this.converter.ToEnumValue(code);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      ((IDataParameter) cmd.Parameters[index]).Value = value != null ? this.GetValue(value) : (object) DBNull.Value;
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override string Name => this.ReturnedClass.FullName;

    public override string ToString(object value)
    {
      return value != null ? this.GetValue(value).ToString() : (string) null;
    }

    public override object FromStringValue(string xml)
    {
      return this.GetInstance((object) long.Parse(xml));
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached != null ? this.GetInstance(cached) : (object) null;
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return value != null ? this.GetValue(value) : (object) null;
    }

    public override string ObjectToSQLString(object value, NHibernate.Dialect.Dialect dialect)
    {
      return this.GetValue(value).ToString();
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj) && ((AbstractType) obj).ReturnedClass == this.ReturnedClass;
    }

    public override int GetHashCode() => this.ReturnedClass.GetHashCode();

    public interface IEnumConverter
    {
      object ToObject(System.Type enumClass, object code);

      object ToEnumValue(object value);

      SqlType SqlType { get; }
    }

    [Serializable]
    private abstract class AbstractEnumConverter<T> : PersistentEnumType.IEnumConverter
    {
      public object ToObject(System.Type enumClass, object code)
      {
        return Enum.ToObject(enumClass, (object) this.Convert(code));
      }

      public object ToEnumValue(object value) => (object) this.Convert(value);

      public abstract T Convert(object input);

      public abstract SqlType SqlType { get; }
    }

    [Serializable]
    private class SystemByteEnumConverter : PersistentEnumType.AbstractEnumConverter<byte>
    {
      public override byte Convert(object input) => Convert.ToByte(input);

      public override SqlType SqlType => SqlTypeFactory.Byte;
    }

    [Serializable]
    private class SystemSByteEnumConverter : PersistentEnumType.AbstractEnumConverter<sbyte>
    {
      public override sbyte Convert(object input) => Convert.ToSByte(input);

      public override SqlType SqlType => SqlTypeFactory.SByte;
    }

    [Serializable]
    private class SystemInt16EnumConverter : PersistentEnumType.AbstractEnumConverter<short>
    {
      public override short Convert(object input) => Convert.ToInt16(input);

      public override SqlType SqlType => SqlTypeFactory.Int16;
    }

    [Serializable]
    private class SystemInt32EnumConverter : PersistentEnumType.AbstractEnumConverter<int>
    {
      public override int Convert(object input) => Convert.ToInt32(input);

      public override SqlType SqlType => SqlTypeFactory.Int32;
    }

    [Serializable]
    private class SystemInt64EnumConverter : PersistentEnumType.AbstractEnumConverter<long>
    {
      public override long Convert(object input) => Convert.ToInt64(input);

      public override SqlType SqlType => SqlTypeFactory.Int64;
    }

    [Serializable]
    private class SystemUInt16EnumConverter : PersistentEnumType.AbstractEnumConverter<ushort>
    {
      public override ushort Convert(object input) => Convert.ToUInt16(input);

      public override SqlType SqlType => SqlTypeFactory.UInt16;
    }

    [Serializable]
    private class SystemUInt32EnumConverter : PersistentEnumType.AbstractEnumConverter<uint>
    {
      public override uint Convert(object input) => Convert.ToUInt32(input);

      public override SqlType SqlType => SqlTypeFactory.UInt32;
    }

    [Serializable]
    private class SystemUInt64EnumConverter : PersistentEnumType.AbstractEnumConverter<ulong>
    {
      public override ulong Convert(object input) => Convert.ToUInt64(input);

      public override SqlType SqlType => SqlTypeFactory.UInt64;
    }
  }
}
