// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.GenericADOException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Exceptions
{
  [Serializable]
  public class GenericADOException : ADOException
  {
    public GenericADOException()
    {
    }

    public GenericADOException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public GenericADOException(string message, Exception innerException, string sql)
      : base(message, innerException, sql)
    {
    }

    public GenericADOException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
