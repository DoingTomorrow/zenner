// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.CacheException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class CacheException : HibernateException
  {
    public CacheException()
      : base("There was an Exception in the Cache.")
    {
    }

    public CacheException(string message)
      : base(message)
    {
    }

    public CacheException(Exception innerException)
      : base(innerException)
    {
    }

    public CacheException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected CacheException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
