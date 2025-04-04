// Decompiled with JetBrains decompiler
// Type: NHibernate.MappingException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class MappingException : HibernateException
  {
    public MappingException(string message)
      : base(message)
    {
    }

    public MappingException(Exception innerException)
      : base(innerException)
    {
    }

    public MappingException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected MappingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
