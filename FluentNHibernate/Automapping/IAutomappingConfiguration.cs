// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.IAutomappingConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public interface IAutomappingConfiguration
  {
    bool ShouldMap(Type type);

    bool ShouldMap(Member member);

    bool IsId(Member member);

    Access GetAccessStrategyForReadOnlyProperty(Member member);

    Type GetParentSideForManyToMany(Type left, Type right);

    bool IsConcreteBaseType(Type type);

    bool IsComponent(Type type);

    string GetComponentColumnPrefix(Member member);

    bool IsDiscriminated(Type type);

    string GetDiscriminatorColumn(Type type);

    [Obsolete("Use IsDiscriminated instead.", true)]
    SubclassStrategy GetSubclassStrategy(Type type);

    bool AbstractClassIsLayerSupertype(Type type);

    string SimpleTypeCollectionValueColumn(Member member);

    bool IsVersion(Member member);

    IEnumerable<IAutomappingStep> GetMappingSteps(
      AutoMapper mapper,
      IConventionFinder conventionFinder);
  }
}
