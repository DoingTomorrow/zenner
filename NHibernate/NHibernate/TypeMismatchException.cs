// Decompiled with JetBrains decompiler
// Type: NHibernate.TypeMismatchException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public class TypeMismatchException : HibernateException
  {
    public TypeMismatchException(string message)
      : base(message)
    {
    }

    public TypeMismatchException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected TypeMismatchException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
