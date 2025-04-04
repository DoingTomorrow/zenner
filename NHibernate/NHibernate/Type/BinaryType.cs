// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.BinaryType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class BinaryType : AbstractBinaryType
  {
    internal BinaryType()
      : this(new BinarySqlType())
    {
    }

    internal BinaryType(BinarySqlType sqlType)
      : base(sqlType)
    {
    }

    public override System.Type ReturnedClass => typeof (byte[]);

    public override string Name => "Byte[]";

    protected internal override object ToExternalFormat(byte[] bytes) => (object) bytes;

    protected internal override byte[] ToInternalFormat(object bytes) => (byte[]) bytes;

    public override int Compare(object x, object y) => 0;
  }
}
