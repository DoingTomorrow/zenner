// Decompiled with JetBrains decompiler
// Type: NHibernate.Exceptions.ConstraintViolationException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Exceptions
{
  [Serializable]
  public class ConstraintViolationException : ADOException
  {
    private readonly string constraintName;

    public ConstraintViolationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public ConstraintViolationException(
      string message,
      Exception innerException,
      string sql,
      string constraintName)
      : base(message, innerException, sql)
    {
      this.constraintName = constraintName;
    }

    public ConstraintViolationException(
      string message,
      Exception innerException,
      string constraintName)
      : base(message, innerException)
    {
      this.constraintName = constraintName;
    }

    public string ConstraintName => this.constraintName;
  }
}
