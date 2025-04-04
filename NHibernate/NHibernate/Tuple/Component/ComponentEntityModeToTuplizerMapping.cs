// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Component.ComponentEntityModeToTuplizerMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tuple.Component
{
  [Serializable]
  public class ComponentEntityModeToTuplizerMapping : EntityModeToTuplizerMapping
  {
    private static readonly Type[] componentTuplizerCTORSignature = new Type[1]
    {
      typeof (NHibernate.Mapping.Component)
    };

    public ComponentEntityModeToTuplizerMapping(NHibernate.Mapping.Component component)
    {
      PersistentClass owner = component.Owner;
      Dictionary<EntityMode, string> dictionary = component.TuplizerMap == null ? new Dictionary<EntityMode, string>() : new Dictionary<EntityMode, string>(component.TuplizerMap);
      string tuplizerImpl1;
      ITuplizer tuplizer1;
      if (!dictionary.TryGetValue(EntityMode.Map, out tuplizerImpl1))
      {
        tuplizer1 = (ITuplizer) new DynamicMapComponentTuplizer(component);
      }
      else
      {
        tuplizer1 = (ITuplizer) this.BuildComponentTuplizer(tuplizerImpl1, component);
        dictionary.Remove(EntityMode.Map);
      }
      string str;
      dictionary.TryGetValue(EntityMode.Poco, out str);
      dictionary.Remove(EntityMode.Poco);
      string tuplizerImpl2 = str;
      ITuplizer tuplizer2 = !owner.HasPocoRepresentation || !component.HasPocoRepresentation ? tuplizer1 : (tuplizerImpl2 != null ? (ITuplizer) this.BuildComponentTuplizer(tuplizerImpl2, component) : (ITuplizer) new PocoComponentTuplizer(component));
      if (tuplizer2 != null)
        this.AddTuplizer(EntityMode.Poco, tuplizer2);
      if (tuplizer1 != null)
        this.AddTuplizer(EntityMode.Map, tuplizer1);
      foreach (KeyValuePair<EntityMode, string> keyValuePair in dictionary)
      {
        IComponentTuplizer componentTuplizer = this.BuildComponentTuplizer(keyValuePair.Value, component);
        this.AddTuplizer(keyValuePair.Key, (ITuplizer) componentTuplizer);
      }
    }

    private IComponentTuplizer BuildComponentTuplizer(string tuplizerImpl, NHibernate.Mapping.Component component)
    {
      try
      {
        return (IComponentTuplizer) ReflectHelper.ClassForName(tuplizerImpl).GetConstructor(ComponentEntityModeToTuplizerMapping.componentTuplizerCTORSignature).Invoke(new object[1]
        {
          (object) component
        });
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not build tuplizer [" + tuplizerImpl + "]", ex);
      }
    }
  }
}
