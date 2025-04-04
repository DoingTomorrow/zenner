// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.FromPathExpressionParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class FromPathExpressionParser : PathExpressionParser
  {
    public override void End(QueryTranslator q)
    {
      if (!this.IsCollectionValued)
      {
        IType propertyType = this.PropertyType;
        if (propertyType.IsEntityType)
        {
          this.Token(".", q);
          this.Token((string) null, q);
        }
        else if (propertyType.IsCollectionType)
        {
          this.Token(".", q);
          this.Token("elements", q);
        }
      }
      base.End(q);
    }

    protected override void SetExpectingCollectionIndex()
    {
      throw new QueryException("illegal syntax near collection-valued path expression in from: " + this.CollectionName);
    }
  }
}
