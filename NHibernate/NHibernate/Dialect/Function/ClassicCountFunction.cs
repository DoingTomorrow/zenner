// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.ClassicCountFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  public class ClassicCountFunction : ClassicAggregateFunction
  {
    public ClassicCountFunction()
      : base("count", true)
    {
    }

    public override IType ReturnType(IType columnType, IMapping mapping)
    {
      return (IType) NHibernateUtil.Int32;
    }
  }
}
