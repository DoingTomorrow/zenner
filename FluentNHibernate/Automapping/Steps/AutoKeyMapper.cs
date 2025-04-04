// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.AutoKeyMapper
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class AutoKeyMapper
  {
    public void SetKey(Member property, ClassMappingBase classMap, CollectionMapping mapping)
    {
      string str = property.DeclaringType.Name + "_id";
      KeyMapping keyMapping = new KeyMapping()
      {
        ContainingEntityType = classMap.Type
      };
      ColumnMapping mapping1 = new ColumnMapping();
      mapping1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, str);
      keyMapping.AddColumn(0, mapping1);
      mapping.Set<KeyMapping>((Expression<Func<CollectionMapping, KeyMapping>>) (x => x.Key), 0, keyMapping);
    }
  }
}
