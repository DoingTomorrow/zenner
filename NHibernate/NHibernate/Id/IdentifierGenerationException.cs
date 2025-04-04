// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.IdentifierGenerationException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Id
{
  [Serializable]
  public class IdentifierGenerationException : HibernateException
  {
    public IdentifierGenerationException()
      : base("An exception occurred during ID generation.")
    {
    }

    public IdentifierGenerationException(string message)
      : base(message)
    {
    }

    public IdentifierGenerationException(string message, Exception e)
      : base(message, e)
    {
    }

    protected IdentifierGenerationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
