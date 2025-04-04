// Decompiled with JetBrains decompiler
// Type: NHibernate.Transform.Transformers
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Transform
{
  public sealed class Transformers
  {
    public static readonly IResultTransformer AliasToEntityMap = (IResultTransformer) new AliasToEntityMapResultTransformer();
    public static readonly ToListResultTransformer ToList = new ToListResultTransformer();
    public static readonly IResultTransformer DistinctRootEntity = (IResultTransformer) new DistinctRootEntityResultTransformer();
    public static readonly IResultTransformer PassThrough = (IResultTransformer) new PassThroughResultTransformer();
    public static readonly IResultTransformer RootEntity = (IResultTransformer) new RootEntityResultTransformer();

    private Transformers()
    {
    }

    public static IResultTransformer AliasToBean(Type target)
    {
      return (IResultTransformer) new AliasToBeanResultTransformer(target);
    }

    public static IResultTransformer AliasToBean<T>() => Transformers.AliasToBean(typeof (T));

    public static IResultTransformer AliasToBeanConstructor(ConstructorInfo constructor)
    {
      return (IResultTransformer) new AliasToBeanConstructorResultTransformer(constructor);
    }
  }
}
