// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.HibernateConfigException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public class HibernateConfigException : MappingException
  {
    private const string baseMessage = "An exception occurred during configuration of persistence layer.";

    public HibernateConfigException()
      : base("An exception occurred during configuration of persistence layer.")
    {
    }

    public HibernateConfigException(Exception innerException)
      : base("An exception occurred during configuration of persistence layer.", innerException)
    {
    }

    public HibernateConfigException(string message)
      : base(message)
    {
    }

    public HibernateConfigException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected HibernateConfigException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
