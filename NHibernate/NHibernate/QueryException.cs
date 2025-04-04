// Decompiled with JetBrains decompiler
// Type: NHibernate.QueryException
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
  public class QueryException : HibernateException, ISerializable
  {
    private string queryString;

    protected QueryException()
    {
    }

    public QueryException(string message)
      : base(message)
    {
    }

    public QueryException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public QueryException(string message, string queryString)
      : base(message)
    {
      this.queryString = queryString;
    }

    public QueryException(Exception innerException)
      : base(innerException)
    {
    }

    public string QueryString
    {
      get => this.queryString;
      set => this.queryString = value;
    }

    public override string Message
    {
      get
      {
        string message = base.Message;
        if (this.queryString != null)
          message = message + " [" + this.queryString + "]";
        return message;
      }
    }

    protected QueryException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.queryString = info.GetString(nameof (queryString));
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("queryString", (object) this.queryString, typeof (string));
    }
  }
}
