// Decompiled with JetBrains decompiler
// Type: NHibernate.CallbackException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class CallbackException : HibernateException
  {
    public CallbackException(Exception innerException)
      : this("An exception occurred in a callback", innerException)
    {
    }

    public CallbackException(string message)
      : base(message)
    {
    }

    public CallbackException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected CallbackException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
