// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.InvalidWithClauseException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  [Serializable]
  public class InvalidWithClauseException : QuerySyntaxException
  {
    protected InvalidWithClauseException()
    {
    }

    public InvalidWithClauseException(string message)
      : base(message)
    {
    }

    public InvalidWithClauseException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected InvalidWithClauseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
