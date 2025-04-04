// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.HolderInstantiator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transform;
using System.Reflection;

#nullable disable
namespace NHibernate.Hql
{
  public sealed class HolderInstantiator
  {
    public static readonly HolderInstantiator NoopInstantiator = new HolderInstantiator((IResultTransformer) null, (string[]) null);
    private readonly IResultTransformer transformer;
    private readonly string[] queryReturnAliases;

    public static HolderInstantiator GetHolderInstantiator(
      IResultTransformer selectNewTransformer,
      IResultTransformer customTransformer,
      string[] queryReturnAliases)
    {
      return selectNewTransformer != null ? new HolderInstantiator(selectNewTransformer, queryReturnAliases) : new HolderInstantiator(customTransformer, queryReturnAliases);
    }

    public static IResultTransformer CreateSelectNewTransformer(
      ConstructorInfo constructor,
      bool returnMaps,
      bool returnLists)
    {
      if (constructor != null)
        return (IResultTransformer) new AliasToBeanConstructorResultTransformer(constructor);
      if (returnMaps)
        return Transformers.AliasToEntityMap;
      return returnLists ? (IResultTransformer) Transformers.ToList : (IResultTransformer) null;
    }

    public static HolderInstantiator CreateClassicHolderInstantiator(
      ConstructorInfo constructor,
      IResultTransformer transformer)
    {
      return constructor != null ? new HolderInstantiator((IResultTransformer) new AliasToBeanConstructorResultTransformer(constructor), (string[]) null) : new HolderInstantiator(transformer, (string[]) null);
    }

    public HolderInstantiator(IResultTransformer transformer, string[] queryReturnAliases)
    {
      this.transformer = transformer;
      this.queryReturnAliases = queryReturnAliases;
    }

    public bool IsRequired => this.transformer != null;

    public object Instantiate(object[] row)
    {
      return this.transformer == null ? (object) row : this.transformer.TransformTuple(row, this.queryReturnAliases);
    }

    public string[] QueryReturnAliases => this.queryReturnAliases;

    public IResultTransformer ResultTransformer => this.transformer;
  }
}
