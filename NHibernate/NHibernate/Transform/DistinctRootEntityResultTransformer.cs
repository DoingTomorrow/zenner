// Decompiled with JetBrains decompiler
// Type: NHibernate.Transform.DistinctRootEntityResultTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Transform
{
  [Serializable]
  public class DistinctRootEntityResultTransformer : IResultTransformer
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DistinctRootEntityResultTransformer));
    private static readonly object Hasher = new object();

    public object TransformTuple(object[] tuple, string[] aliases) => tuple[tuple.Length - 1];

    public IList TransformList(IList list)
    {
      IList instance = (IList) Activator.CreateInstance(list.GetType());
      ISet<DistinctRootEntityResultTransformer.Identity> set = (ISet<DistinctRootEntityResultTransformer.Identity>) new HashedSet<DistinctRootEntityResultTransformer.Identity>();
      for (int index = 0; index < list.Count; ++index)
      {
        object entity = list[index];
        if (set.Add(new DistinctRootEntityResultTransformer.Identity(entity)))
          instance.Add(entity);
      }
      if (DistinctRootEntityResultTransformer.log.IsDebugEnabled)
        DistinctRootEntityResultTransformer.log.Debug((object) string.Format("transformed: {0} rows to: {1} distinct results", (object) list.Count, (object) instance.Count));
      return instance;
    }

    public override bool Equals(object obj)
    {
      return obj != null && obj.GetHashCode() == DistinctRootEntityResultTransformer.Hasher.GetHashCode();
    }

    public override int GetHashCode() => DistinctRootEntityResultTransformer.Hasher.GetHashCode();

    internal sealed class Identity
    {
      internal readonly object entity;

      internal Identity(object entity) => this.entity = entity;

      public override bool Equals(object other)
      {
        return object.ReferenceEquals(this.entity, ((DistinctRootEntityResultTransformer.Identity) other).entity);
      }

      public override int GetHashCode() => RuntimeHelpers.GetHashCode(this.entity);
    }
  }
}
