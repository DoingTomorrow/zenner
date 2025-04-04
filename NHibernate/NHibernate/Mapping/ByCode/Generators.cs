// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Generators
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class Generators
  {
    static Generators()
    {
      Generators.Native = (IGeneratorDef) new NativeGeneratorDef();
      Generators.HighLow = (IGeneratorDef) new HighLowGeneratorDef();
      Generators.Guid = (IGeneratorDef) new GuidGeneratorDef();
      Generators.GuidComb = (IGeneratorDef) new GuidCombGeneratorDef();
      Generators.Sequence = (IGeneratorDef) new SequenceGeneratorDef();
      Generators.Identity = (IGeneratorDef) new IdentityGeneratorDef();
      Generators.Assigned = (IGeneratorDef) new AssignedGeneratorDef();
    }

    public static IGeneratorDef Assigned { get; private set; }

    public static IGeneratorDef Native { get; private set; }

    public static IGeneratorDef HighLow { get; private set; }

    public static IGeneratorDef Guid { get; private set; }

    public static IGeneratorDef GuidComb { get; private set; }

    public static IGeneratorDef Sequence { get; private set; }

    public static IGeneratorDef Identity { get; private set; }

    public static IGeneratorDef Foreign<TEntity>(Expression<Func<TEntity, object>> property)
    {
      return (IGeneratorDef) new ForeignGeneratorDef(TypeExtensions.DecodeMemberAccessExpression<TEntity>(property));
    }

    public static IGeneratorDef Foreign(MemberInfo property)
    {
      return (IGeneratorDef) new ForeignGeneratorDef(property);
    }
  }
}
