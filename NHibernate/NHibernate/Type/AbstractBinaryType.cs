// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.AbstractBinaryType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class AbstractBinaryType : 
    MutableType,
    IVersionType,
    IType,
    ICacheAssembler,
    IComparer
  {
    internal AbstractBinaryType()
      : this(new BinarySqlType())
    {
    }

    internal AbstractBinaryType(BinarySqlType sqlType)
      : base((SqlType) sqlType)
    {
    }

    public object Next(object current, ISessionImplementor session) => current;

    public object Seed(ISessionImplementor session) => (object) null;

    public override bool IsEqual(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && ArrayHelper.ArrayEquals(this.ToInternalFormat(x), this.ToInternalFormat(y));
    }

    public IComparer Comparator => (IComparer) this;

    public virtual int Compare(object x, object y) => this.Compare(x, y, new EntityMode?());

    public abstract override string Name { get; }

    protected internal abstract object ToExternalFormat(byte[] bytes);

    protected internal abstract byte[] ToInternalFormat(object bytes);

    public override void Set(IDbCommand cmd, object value, int index)
    {
      byte[] internalFormat = this.ToInternalFormat(value);
      IDbDataParameter parameter = (IDbDataParameter) cmd.Parameters[index];
      parameter.Value = (object) internalFormat;
      if (parameter.Size > 0 && internalFormat.Length > parameter.Size)
        throw new HibernateException("The length of the byte[] value exceeds the length configured in the mapping/parameter.");
    }

    public override object Get(IDataReader rs, int index)
    {
      int bytes = (int) rs.GetBytes(index, 0L, (byte[]) null, 0, 0);
      byte[] numArray = new byte[bytes];
      if (bytes > 0)
        rs.GetBytes(index, 0L, numArray, 0, bytes);
      return this.ToExternalFormat(numArray);
    }

    public override object Get(IDataReader rs, string name) => this.Get(rs, rs.GetOrdinal(name));

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      byte[] internalFormat = this.ToInternalFormat(x);
      int hashCode = 1;
      for (int index = 0; index < internalFormat.Length; ++index)
        hashCode = 31 * hashCode + (int) internalFormat[index];
      return hashCode;
    }

    public override int Compare(object x, object y, EntityMode? entityMode)
    {
      byte[] internalFormat1 = this.ToInternalFormat(x);
      byte[] internalFormat2 = this.ToInternalFormat(y);
      if (internalFormat1.Length < internalFormat2.Length)
        return -1;
      if (internalFormat1.Length > internalFormat2.Length)
        return 1;
      for (int index = 0; index < internalFormat1.Length; ++index)
      {
        if ((int) internalFormat1[index] < (int) internalFormat2[index])
          return -1;
        if ((int) internalFormat1[index] > (int) internalFormat2[index])
          return 1;
      }
      return 0;
    }

    public override string ToString(object val)
    {
      byte[] internalFormat = this.ToInternalFormat(val);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < internalFormat.Length; ++index)
      {
        string str = Convert.ToString((int) internalFormat[index], 16);
        if (str.Length == 1)
          stringBuilder.Append('0');
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }

    public override object DeepCopyNotNull(object value)
    {
      byte[] internalFormat = this.ToInternalFormat(value);
      byte[] numArray = new byte[internalFormat.Length];
      Array.Copy((Array) internalFormat, 0, (Array) numArray, 0, internalFormat.Length);
      return this.ToExternalFormat(numArray);
    }

    public override object FromStringValue(string xml)
    {
      if (xml == null)
        return (object) null;
      byte[] bytes = xml.Length % 2 == 0 ? new byte[xml.Length / 2] : throw new ArgumentException("The string is not a valid xml representation of a binary content.");
      for (int index = 0; index < bytes.Length; ++index)
      {
        string str = xml.Substring(index * 2, (index + 1) * 2 - index * 2);
        bytes[index] = (byte) Convert.ToInt32(str, 16);
      }
      return this.ToExternalFormat(bytes);
    }
  }
}
