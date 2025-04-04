// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.EntityEntityModeToTuplizerMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  [Serializable]
  public class EntityEntityModeToTuplizerMapping : EntityModeToTuplizerMapping
  {
    private static readonly Type[] entityTuplizerCTORSignature = new Type[2]
    {
      typeof (EntityMetamodel),
      typeof (PersistentClass)
    };

    public EntityEntityModeToTuplizerMapping(PersistentClass mappedEntity, EntityMetamodel em)
    {
      Dictionary<EntityMode, string> dictionary = mappedEntity.TuplizerMap != null ? new Dictionary<EntityMode, string>(mappedEntity.TuplizerMap) : new Dictionary<EntityMode, string>();
      string className1;
      ITuplizer tuplizer1;
      if (!dictionary.TryGetValue(EntityMode.Map, out className1))
      {
        tuplizer1 = (ITuplizer) new DynamicMapEntityTuplizer(em, mappedEntity);
      }
      else
      {
        tuplizer1 = (ITuplizer) EntityEntityModeToTuplizerMapping.BuildEntityTuplizer(className1, mappedEntity, em);
        dictionary.Remove(EntityMode.Map);
      }
      string str;
      dictionary.TryGetValue(EntityMode.Poco, out str);
      dictionary.Remove(EntityMode.Poco);
      string className2 = str;
      ITuplizer tuplizer2 = !mappedEntity.HasPocoRepresentation ? tuplizer1 : (className2 != null ? (ITuplizer) EntityEntityModeToTuplizerMapping.BuildEntityTuplizer(className2, mappedEntity, em) : (ITuplizer) new PocoEntityTuplizer(em, mappedEntity));
      if (tuplizer2 != null)
        this.AddTuplizer(EntityMode.Poco, tuplizer2);
      if (tuplizer1 != null)
        this.AddTuplizer(EntityMode.Map, tuplizer1);
      foreach (KeyValuePair<EntityMode, string> keyValuePair in dictionary)
      {
        IEntityTuplizer entityTuplizer = EntityEntityModeToTuplizerMapping.BuildEntityTuplizer(keyValuePair.Value, mappedEntity, em);
        this.AddTuplizer(keyValuePair.Key, (ITuplizer) entityTuplizer);
      }
    }

    private static IEntityTuplizer BuildEntityTuplizer(
      string className,
      PersistentClass pc,
      EntityMetamodel em)
    {
      try
      {
        return (IEntityTuplizer) ReflectHelper.ClassForName(className).GetConstructor(EntityEntityModeToTuplizerMapping.entityTuplizerCTORSignature).Invoke(new object[2]
        {
          (object) em,
          (object) pc
        });
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not build tuplizer [" + className + "]", ex);
      }
    }
  }
}
