// Decompiled with JetBrains decompiler
// Type: NHibernate.LazyInitializationException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class LazyInitializationException : HibernateException
  {
    public LazyInitializationException(string entityName, object entityId, string message)
      : this(string.Format("Initializing[{0}#{1}]-{2}", (object) entityName, entityId, (object) message))
    {
      this.EntityName = entityName;
      this.EntityId = entityId;
    }

    public string EntityName { get; private set; }

    public object EntityId { get; private set; }

    public LazyInitializationException(string message)
      : this(message, (Exception) null)
    {
    }

    public LazyInitializationException(Exception innerException)
      : this("NHibernate lazy initialization problem", innerException)
    {
    }

    public LazyInitializationException(string message, Exception innerException)
      : base(message, innerException)
    {
      LoggerProvider.LoggerFor(typeof (LazyInitializationException)).Error((object) message, (Exception) this);
    }

    protected LazyInitializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
