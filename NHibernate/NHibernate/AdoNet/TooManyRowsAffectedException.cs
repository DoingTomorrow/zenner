// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.TooManyRowsAffectedException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate.AdoNet
{
  [Serializable]
  public class TooManyRowsAffectedException : HibernateException
  {
    private readonly int expectedRowCount;
    private readonly int actualRowCount;

    public TooManyRowsAffectedException(string message, int expectedRowCount, int actualRowCount)
      : base(message)
    {
      this.expectedRowCount = expectedRowCount;
      this.actualRowCount = actualRowCount;
    }

    protected TooManyRowsAffectedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.expectedRowCount = info.GetInt32(nameof (expectedRowCount));
      this.actualRowCount = info.GetInt32(nameof (actualRowCount));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("expectedRowCount", this.expectedRowCount);
      info.AddValue("actualRowCount", this.actualRowCount);
    }

    public int ExpectedRowCount => this.expectedRowCount;

    public int ActualRowCount => this.actualRowCount;
  }
}
