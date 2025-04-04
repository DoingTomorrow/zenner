// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.IQueryCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public interface IQueryCache
  {
    ICache Cache { get; }

    string RegionName { get; }

    void Clear();

    bool Put(
      QueryKey key,
      ICacheAssembler[] returnTypes,
      IList result,
      bool isNaturalKeyLookup,
      ISessionImplementor session);

    IList Get(
      QueryKey key,
      ICacheAssembler[] returnTypes,
      bool isNaturalKeyLookup,
      ISet<string> spaces,
      ISessionImplementor session);

    void Destroy();
  }
}
