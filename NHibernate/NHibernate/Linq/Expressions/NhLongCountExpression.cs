// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Expressions.NhLongCountExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Expressions
{
  public class NhLongCountExpression(Expression expression) : NhCountExpression(expression, typeof (long))
  {
    public override NhCountExpression CreateNew(Expression expression)
    {
      return (NhCountExpression) new NhLongCountExpression(expression);
    }
  }
}
