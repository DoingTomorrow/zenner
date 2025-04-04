// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.DetailedSemanticException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  public class DetailedSemanticException : SemanticException
  {
    public DetailedSemanticException(string message)
      : base(message)
    {
    }

    public DetailedSemanticException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
