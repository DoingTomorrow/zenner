// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SerializationException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class SerializationException : HibernateException
  {
    public SerializationException()
      : base("The Property associated with a SerializableType threw an Exception during serialization or deserialization.")
    {
    }

    public SerializationException(string message)
      : base(message)
    {
    }

    public SerializationException(string message, Exception e)
      : base(message, e)
    {
    }

    protected SerializationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
