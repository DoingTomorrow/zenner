// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.DynamicMapInstantiator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Mapping;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public class DynamicMapInstantiator : IInstantiator
  {
    public const string KEY = "$type$";
    private readonly string entityName;
    private readonly HashedSet<string> isInstanceEntityNames = new HashedSet<string>();

    public DynamicMapInstantiator() => this.entityName = (string) null;

    public DynamicMapInstantiator(PersistentClass mappingInfo)
    {
      this.entityName = mappingInfo.EntityName;
      this.isInstanceEntityNames.Add(this.entityName);
      if (!mappingInfo.HasSubclasses)
        return;
      foreach (PersistentClass persistentClass in mappingInfo.SubclassClosureIterator)
        this.isInstanceEntityNames.Add(persistentClass.EntityName);
    }

    public object Instantiate(object id) => this.Instantiate();

    public object Instantiate()
    {
      IDictionary map = this.GenerateMap();
      if (this.entityName != null)
        map[(object) "$type$"] = (object) this.entityName;
      return (object) map;
    }

    protected virtual IDictionary GenerateMap() => (IDictionary) new Hashtable();

    public bool IsInstance(object obj)
    {
      if (!(obj is IDictionary dictionary))
        return false;
      if (this.entityName == null)
        return true;
      string o = (string) dictionary[(object) "$type$"];
      return o == null || this.isInstanceEntityNames.Contains(o);
    }
  }
}
