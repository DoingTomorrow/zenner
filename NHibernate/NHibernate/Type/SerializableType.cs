// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SerializableType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class SerializableType : MutableType
  {
    private readonly System.Type serializableClass;
    private readonly BinaryType binaryType;

    internal SerializableType()
      : this(typeof (object))
    {
    }

    internal SerializableType(System.Type serializableClass)
      : base((SqlType) new BinarySqlType())
    {
      this.serializableClass = serializableClass;
      this.binaryType = (BinaryType) NHibernateUtil.Binary;
    }

    internal SerializableType(System.Type serializableClass, BinarySqlType sqlType)
      : base((SqlType) sqlType)
    {
      this.serializableClass = serializableClass;
      this.binaryType = (BinaryType) TypeFactory.GetBinaryType(sqlType.Length);
    }

    public override void Set(IDbCommand st, object value, int index)
    {
      this.binaryType.Set(st, (object) this.ToBytes(value), index);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override object Get(IDataReader rs, int index)
    {
      byte[] bytes = (byte[]) this.binaryType.Get(rs, index);
      return bytes == null ? (object) null : this.FromBytes(bytes);
    }

    public override System.Type ReturnedClass => this.serializableClass;

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      if (x == null || y == null)
        return false;
      return x.Equals(y) || this.binaryType.IsEqual((object) this.ToBytes(x), (object) this.ToBytes(y));
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      return this.binaryType.GetHashCode((object) this.ToBytes(x), entityMode);
    }

    public override string ToString(object value)
    {
      return this.binaryType.ToString((object) this.ToBytes(value));
    }

    public override object FromStringValue(string xml)
    {
      return this.FromBytes((byte[]) this.binaryType.FromStringValue(xml));
    }

    public override string Name
    {
      get
      {
        return this.serializableClass != typeof (ISerializable) ? this.serializableClass.FullName : "serializable";
      }
    }

    public override object DeepCopyNotNull(object value) => this.FromBytes(this.ToBytes(value));

    private byte[] ToBytes(object obj)
    {
      try
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream serializationStream = new MemoryStream();
        binaryFormatter.Serialize((Stream) serializationStream, obj);
        return serializationStream.ToArray();
      }
      catch (Exception ex)
      {
        throw new SerializationException("Could not serialize a serializable property: ", ex);
      }
    }

    public object FromBytes(byte[] bytes)
    {
      try
      {
        return new BinaryFormatter().Deserialize((Stream) new MemoryStream(bytes));
      }
      catch (Exception ex)
      {
        throw new SerializationException("Could not deserialize a serializable property: ", ex);
      }
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached != null ? this.FromBytes((byte[]) cached) : (object) null;
    }

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      return value != null ? (object) this.ToBytes(value) : (object) null;
    }
  }
}
