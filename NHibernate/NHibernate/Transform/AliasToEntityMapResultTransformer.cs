// Decompiled with JetBrains decompiler
// Type: NHibernate.Transform.AliasToEntityMapResultTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Transform
{
  [Serializable]
  public class AliasToEntityMapResultTransformer : IResultTransformer
  {
    private static readonly object Hasher = new object();

    public object TransformTuple(object[] tuple, string[] aliases)
    {
      IDictionary dictionary = (IDictionary) new Hashtable();
      for (int index = 0; index < tuple.Length; ++index)
      {
        string alias = aliases[index];
        if (alias != null)
          dictionary[(object) alias] = tuple[index];
      }
      return (object) dictionary;
    }

    public IList TransformList(IList collection) => collection;

    public override bool Equals(object obj)
    {
      return obj != null && obj.GetHashCode() == AliasToEntityMapResultTransformer.Hasher.GetHashCode();
    }

    public override int GetHashCode() => AliasToEntityMapResultTransformer.Hasher.GetHashCode();
  }
}
