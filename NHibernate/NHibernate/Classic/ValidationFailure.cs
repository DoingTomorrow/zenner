// Decompiled with JetBrains decompiler
// Type: NHibernate.Classic.ValidationFailure
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Classic
{
  [Serializable]
  public class ValidationFailure : HibernateException
  {
    public ValidationFailure()
      : base("A validation failure occurred")
    {
    }

    public ValidationFailure(string message)
      : base(message)
    {
    }

    public ValidationFailure(Exception innerException)
      : base("A validation failure occurred", innerException)
    {
    }

    public ValidationFailure(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ValidationFailure(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
