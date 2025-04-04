// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.MultipleQueriesCacheAssembler
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate.Impl
{
  internal class MultipleQueriesCacheAssembler : ICacheAssembler
  {
    private readonly IList assemblersList;

    public MultipleQueriesCacheAssembler(IList assemblers) => this.assemblersList = assemblers;

    public object Disassemble(object value, ISessionImplementor session, object owner)
    {
      IList list1 = (IList) value;
      ArrayList arrayList1 = new ArrayList();
      for (int index = 0; index < list1.Count; ++index)
      {
        ICacheAssembler[] assemblers = (ICacheAssembler[]) this.assemblersList[index];
        IList list2 = (IList) list1[index];
        ArrayList arrayList2 = new ArrayList();
        foreach (object row in (IEnumerable) list2)
        {
          if (assemblers.Length == 1)
            arrayList2.Add(assemblers[0].Disassemble(row, session, owner));
          else
            arrayList2.Add((object) TypeHelper.Disassemble((object[]) row, assemblers, (bool[]) null, session, (object) null));
        }
        arrayList1.Add((object) arrayList2);
      }
      return (object) arrayList1;
    }

    public object Assemble(object cached, ISessionImplementor session, object owner)
    {
      IList list1 = (IList) cached;
      ArrayList arrayList1 = new ArrayList();
      for (int index = 0; index < this.assemblersList.Count; ++index)
      {
        ICacheAssembler[] assemblers = (ICacheAssembler[]) this.assemblersList[index];
        IList list2 = (IList) list1[index];
        ArrayList arrayList2 = new ArrayList();
        foreach (object obj in (IEnumerable) list2)
        {
          if (assemblers.Length == 1)
            arrayList2.Add(assemblers[0].Assemble(obj, session, owner));
          else
            arrayList2.Add((object) TypeHelper.Assemble((object[]) obj, assemblers, session, owner));
        }
        arrayList1.Add((object) arrayList2);
      }
      return (object) arrayList1;
    }

    public void BeforeAssemble(object cached, ISessionImplementor session)
    {
    }

    public IList GetResultFromQueryCache(
      ISessionImplementor session,
      QueryParameters queryParameters,
      ISet<string> querySpaces,
      IQueryCache queryCache,
      QueryKey key)
    {
      if (queryParameters.ForceCacheRefresh)
        return (IList) null;
      IList resultFromQueryCache = queryCache.Get(key, new ICacheAssembler[1]
      {
        (ICacheAssembler) this
      }, (queryParameters.NaturalKeyLookup ? 1 : 0) != 0, querySpaces, session);
      if (resultFromQueryCache != null)
        resultFromQueryCache = (IList) resultFromQueryCache[0];
      return resultFromQueryCache;
    }
  }
}
