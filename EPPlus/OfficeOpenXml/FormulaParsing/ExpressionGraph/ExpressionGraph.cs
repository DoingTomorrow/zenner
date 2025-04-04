// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExpressionGraph
  {
    private List<Expression> _expressions = new List<Expression>();

    public IEnumerable<Expression> Expressions => (IEnumerable<Expression>) this._expressions;

    public Expression Current { get; private set; }

    public Expression Add(Expression expression)
    {
      this._expressions.Add(expression);
      if (this.Current != null)
      {
        this.Current.Next = expression;
        expression.Prev = this.Current;
      }
      this.Current = expression;
      return expression;
    }

    public void Reset()
    {
      this._expressions.Clear();
      this.Current = (Expression) null;
    }

    public void Remove(Expression item)
    {
      if (item == this.Current)
        this.Current = item.Prev ?? item.Next;
      this._expressions.Remove(item);
    }
  }
}
