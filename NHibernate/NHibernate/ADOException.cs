// Decompiled with JetBrains decompiler
// Type: NHibernate.ADOException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class ADOException : HibernateException
  {
    private readonly string sql;

    public ADOException()
    {
    }

    public ADOException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public ADOException(string message, Exception innerException, string sql)
      : base(message + "[SQL: " + sql + "]", innerException)
    {
      this.sql = sql;
    }

    protected ADOException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.sql = (string) info.GetValue(nameof (sql), typeof (string));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("sql", (object) this.sql);
    }

    public string SqlString => this.sql;
  }
}
