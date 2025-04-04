// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.SelectPathExpressionParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class SelectPathExpressionParser : PathExpressionParser
  {
    public override void End(QueryTranslator q)
    {
      if (this.CurrentProperty != null && !q.IsShallowQuery)
      {
        this.Token(".", q);
        this.Token((string) null, q);
      }
      base.End(q);
    }

    protected override void SetExpectingCollectionIndex()
    {
      throw new QueryException("expecting .elements or .indices after collection path expression in select");
    }

    public string SelectName => this.CurrentName;
  }
}
