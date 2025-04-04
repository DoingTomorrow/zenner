// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.QuerySyntaxException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  [Serializable]
  public class QuerySyntaxException : QueryException
  {
    protected QuerySyntaxException()
    {
    }

    public QuerySyntaxException(string message, string hql)
      : base(message, hql)
    {
    }

    public QuerySyntaxException(string message)
      : base(message)
    {
    }

    public QuerySyntaxException(string message, Exception inner)
      : base(message, inner)
    {
    }

    protected QuerySyntaxException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public static QuerySyntaxException Convert(RecognitionException e)
    {
      return QuerySyntaxException.Convert(e, (string) null);
    }

    public static QuerySyntaxException Convert(RecognitionException e, string hql)
    {
      string str1;
      if (e.Line <= 0 || e.CharPositionInLine <= 0)
        str1 = "";
      else
        str1 = " near line " + (object) e.Line + ", column " + (object) e.CharPositionInLine;
      string str2 = str1;
      return new QuerySyntaxException(e.Message + str2, hql);
    }
  }
}
