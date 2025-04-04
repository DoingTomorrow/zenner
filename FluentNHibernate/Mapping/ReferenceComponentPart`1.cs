// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ReferenceComponentPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ReferenceComponentPart<T> : 
    IReferenceComponentMappingProvider,
    IComponentMappingProvider
  {
    private readonly Member property;
    private readonly Type containingEntityType;
    private string columnPrefix;

    public ReferenceComponentPart(Member property, Type containingEntityType)
    {
      this.property = property;
      this.containingEntityType = containingEntityType;
    }

    public void ColumnPrefix(string prefix) => this.columnPrefix = prefix;

    IComponentMapping IComponentMappingProvider.GetComponentMapping()
    {
      return (IComponentMapping) new ReferenceComponentMapping(ComponentType.Component, this.property, typeof (T), this.containingEntityType, this.columnPrefix);
    }

    Type IReferenceComponentMappingProvider.Type => typeof (T);
  }
}
