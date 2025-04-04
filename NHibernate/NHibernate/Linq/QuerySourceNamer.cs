// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.QuerySourceNamer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq
{
  public class QuerySourceNamer
  {
    private readonly IDictionary<IQuerySource, string> _map = (IDictionary<IQuerySource, string>) new Dictionary<IQuerySource, string>();
    private readonly IList<string> _names = (IList<string>) new List<string>();
    private int _differentiator = 1;

    public void Add(IQuerySource querySource)
    {
      if (this._map.ContainsKey(querySource))
        return;
      this._map.Add(querySource, this.CreateUniqueName(querySource.ItemName));
    }

    public string GetName(IQuerySource querySource)
    {
      return this._map.ContainsKey(querySource) ? this._map[querySource] : throw new HibernateException(string.Format("Query Source could not be identified: ItemName = {0}, ItemType = {1}, Expression = {2}", (object) querySource.ItemName, (object) querySource.ItemType, (object) querySource));
    }

    private string CreateUniqueName(string proposedName)
    {
      string uniqueName = proposedName;
      if (this._names.Contains(proposedName))
      {
        uniqueName = string.Format("{0}{1:000}", (object) proposedName, (object) this._differentiator);
        ++this._differentiator;
      }
      this._names.Add(uniqueName);
      return uniqueName;
    }
  }
}
