// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.InspectorModelMapper`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class InspectorModelMapper<TInspector, TMapping>
  {
    private readonly IDictionary<string, string> mappings = (IDictionary<string, string>) new Dictionary<string, string>();

    public void Map(
      Expression<Func<TInspector, object>> inspectorProperty,
      Expression<Func<TMapping, object>> mappingProperty)
    {
      this.Map(inspectorProperty.ToMember<TInspector, object>(), mappingProperty);
    }

    public void Map(
      Expression<Func<TInspector, object>> inspectorProperty,
      string mappingProperty)
    {
      this.mappings[inspectorProperty.ToMember<TInspector, object>().Name] = mappingProperty;
    }

    private void Map(Member inspectorProperty, Expression<Func<TMapping, object>> mappingProperty)
    {
      this.mappings[inspectorProperty.Name] = mappingProperty.ToMember<TMapping, object>().Name;
    }

    public string Get(Member property)
    {
      return this.mappings.ContainsKey(property.Name) ? this.mappings[property.Name] : property.Name;
    }
  }
}
