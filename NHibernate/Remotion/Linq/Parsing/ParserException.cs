// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ParserException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

#nullable disable
namespace Remotion.Linq.Parsing
{
  [Serializable]
  public class ParserException : Exception
  {
    private static string CreateMessage(object expected, object expression, string context)
    {
      ArgumentUtility.CheckNotNull<object>(nameof (expected), expected);
      ArgumentUtility.CheckNotNull<object>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<string>(nameof (context), context);
      if (!(expression is Expression))
        return string.Format("Expected {0} for {1}, found '{2}'.", expected, (object) context, expression);
      return string.Format("Expected {0} for {1}, found '{2}' ({3}).", expected, (object) context, expression, (object) expression.GetType().Name);
    }

    public ParserException(string message)
      : this(message, (object) null, (Exception) null)
    {
    }

    public ParserException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ParserException(string message, object parsedExpression, Exception inner)
      : base(message, inner)
    {
      this.ParsedExpression = parsedExpression;
    }

    public ParserException(object expected, object expression, string context)
      : this(ParserException.CreateMessage(expected, expression, context), expression, (Exception) null)
    {
    }

    protected ParserException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.ParsedExpression = info.GetValue("ParserException.ParsedExpression", typeof (object));
    }

    public object ParsedExpression { get; private set; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ParserException.ParsedExpression", this.ParsedExpression);
    }
  }
}
