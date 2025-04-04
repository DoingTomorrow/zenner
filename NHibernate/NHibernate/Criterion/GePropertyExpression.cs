// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.GePropertyExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class GePropertyExpression : PropertyExpression
  {
    public GePropertyExpression(string lhsPropertyName, IProjection rhsProjection)
      : base(lhsPropertyName, rhsProjection)
    {
    }

    public GePropertyExpression(IProjection lhsProjection, IProjection rhsProjection)
      : base(lhsProjection, rhsProjection)
    {
    }

    public GePropertyExpression(IProjection lhsProjection, string rhsPropertyName)
      : base(lhsProjection, rhsPropertyName)
    {
    }

    public GePropertyExpression(string lhsPropertyName, string rhsPropertyName)
      : base(lhsPropertyName, rhsPropertyName)
    {
    }

    protected override string Op => " >= ";
  }
}
