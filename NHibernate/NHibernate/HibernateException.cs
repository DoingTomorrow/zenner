// Decompiled with JetBrains decompiler
// Type: NHibernate.HibernateException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class HibernateException : ApplicationException
  {
    public HibernateException()
      : base("An exception occurred in the persistence layer.")
    {
    }

    public HibernateException(string message)
      : base(message)
    {
    }

    public HibernateException(Exception innerException)
      : base(innerException.Message, innerException)
    {
    }

    public HibernateException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected HibernateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
