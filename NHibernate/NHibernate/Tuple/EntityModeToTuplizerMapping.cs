// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.EntityModeToTuplizerMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tuple
{
  [Serializable]
  public abstract class EntityModeToTuplizerMapping
  {
    private readonly IDictionary<EntityMode, ITuplizer> tuplizers = (IDictionary<EntityMode, ITuplizer>) new LinkedHashMap<EntityMode, ITuplizer>(5, (IEqualityComparer<EntityMode>) new EntityModeEqualityComparer());

    protected internal void AddTuplizer(EntityMode entityMode, ITuplizer tuplizer)
    {
      this.tuplizers[entityMode] = tuplizer;
    }

    public virtual EntityMode? GuessEntityMode(object obj)
    {
      foreach (KeyValuePair<EntityMode, ITuplizer> tuplizer in (IEnumerable<KeyValuePair<EntityMode, ITuplizer>>) this.tuplizers)
      {
        if (tuplizer.Value.IsInstance(obj))
          return new EntityMode?(tuplizer.Key);
      }
      return new EntityMode?();
    }

    public virtual ITuplizer GetTuplizerOrNull(EntityMode entityMode)
    {
      ITuplizer tuplizerOrNull;
      this.tuplizers.TryGetValue(entityMode, out tuplizerOrNull);
      return tuplizerOrNull;
    }

    public virtual ITuplizer GetTuplizer(EntityMode entityMode)
    {
      return this.GetTuplizerOrNull(entityMode) ?? throw new HibernateException("No tuplizer found for entity-mode [" + (object) entityMode + "]");
    }
  }
}
