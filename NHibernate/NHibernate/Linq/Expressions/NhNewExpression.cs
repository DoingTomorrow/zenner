// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Expressions.NhNewExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Expressions
{
  public class NhNewExpression : Expression
  {
    private readonly ReadOnlyCollection<string> _members;
    private readonly ReadOnlyCollection<Expression> _arguments;

    public NhNewExpression(IList<string> members, IList<Expression> arguments)
      : base((ExpressionType) 10006, typeof (object))
    {
      this._members = new ReadOnlyCollection<string>(members);
      this._arguments = new ReadOnlyCollection<Expression>(arguments);
    }

    public ReadOnlyCollection<Expression> Arguments => this._arguments;

    public ReadOnlyCollection<string> Members => this._members;
  }
}
