// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.SeparateSubclassVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class SeparateSubclassVisitor : DefaultMappingModelVisitor
  {
    private readonly IList<IIndeterminateSubclassMappingProvider> subclassProviders;

    public SeparateSubclassVisitor(
      IList<IIndeterminateSubclassMappingProvider> subclassProviders)
    {
      this.subclassProviders = subclassProviders;
    }

    public override void ProcessClass(ClassMapping mapping)
    {
      foreach (IIndeterminateSubclassMappingProvider closestSubclass in this.FindClosestSubclasses(mapping.Type))
        mapping.AddSubclass(closestSubclass.GetSubclassMapping(this.GetSubclassType(mapping)));
      base.ProcessClass(mapping);
    }

    public override void ProcessSubclass(SubclassMapping mapping)
    {
      foreach (IIndeterminateSubclassMappingProvider closestSubclass in this.FindClosestSubclasses(mapping.Type))
        mapping.AddSubclass(closestSubclass.GetSubclassMapping(mapping.SubclassType));
      base.ProcessSubclass(mapping);
    }

    private IEnumerable<IIndeterminateSubclassMappingProvider> FindClosestSubclasses(Type type)
    {
      IEnumerable<IIndeterminateSubclassMappingProvider> closestSubclasses = this.subclassProviders.Where<IIndeterminateSubclassMappingProvider>((Func<IIndeterminateSubclassMappingProvider, bool>) (x => x.Extends == type));
      IDictionary<int, IList<IIndeterminateSubclassMappingProvider>> dictionary = this.SortByDistanceFrom(type, this.subclassProviders.Except<IIndeterminateSubclassMappingProvider>(closestSubclasses));
      if (dictionary.Keys.Count == 0 && !closestSubclasses.Any<IIndeterminateSubclassMappingProvider>())
        return (IEnumerable<IIndeterminateSubclassMappingProvider>) new IIndeterminateSubclassMappingProvider[0];
      if (dictionary.Keys.Count == 0)
        return closestSubclasses;
      int key = dictionary.Keys.Min();
      return dictionary[key].Concat<IIndeterminateSubclassMappingProvider>(closestSubclasses);
    }

    private SubclassType GetSubclassType(ClassMapping mapping)
    {
      if (mapping.IsUnionSubclass)
        return SubclassType.UnionSubclass;
      return mapping.Discriminator == null ? SubclassType.JoinedSubclass : SubclassType.Subclass;
    }

    private bool IsMapped(
      Type type,
      IEnumerable<IIndeterminateSubclassMappingProvider> providers)
    {
      return providers.Any<IIndeterminateSubclassMappingProvider>((Func<IIndeterminateSubclassMappingProvider, bool>) (x => x.EntityType == type));
    }

    private IDictionary<int, IList<IIndeterminateSubclassMappingProvider>> SortByDistanceFrom(
      Type parentType,
      IEnumerable<IIndeterminateSubclassMappingProvider> subProviders)
    {
      Dictionary<int, IList<IIndeterminateSubclassMappingProvider>> dictionary = new Dictionary<int, IList<IIndeterminateSubclassMappingProvider>>();
      foreach (IIndeterminateSubclassMappingProvider subProvider in subProviders)
      {
        Type entityType = subProvider.EntityType;
        int level = 0;
        if (parentType.IsInterface ? this.DistanceFromParentInterface(parentType, entityType, ref level) : this.DistanceFromParentBase(parentType, entityType.BaseType, ref level))
        {
          if (!dictionary.ContainsKey(level))
            dictionary[level] = (IList<IIndeterminateSubclassMappingProvider>) new List<IIndeterminateSubclassMappingProvider>();
          dictionary[level].Add(subProvider);
        }
      }
      return (IDictionary<int, IList<IIndeterminateSubclassMappingProvider>>) dictionary;
    }

    private bool DistanceFromParentInterface(Type parentType, Type evalType, ref int level)
    {
      if (!evalType.HasInterface(parentType))
        return false;
      if (evalType != typeof (object) && this.IsMapped(evalType.BaseType, (IEnumerable<IIndeterminateSubclassMappingProvider>) this.subclassProviders))
      {
        ++level;
        this.DistanceFromParentInterface(parentType, evalType.BaseType, ref level);
      }
      return true;
    }

    private bool DistanceFromParentBase(Type parentType, Type evalType, ref int level)
    {
      bool flag = false;
      if (evalType == parentType)
        flag = true;
      if (!flag && evalType != typeof (object))
      {
        if (this.IsMapped(evalType, (IEnumerable<IIndeterminateSubclassMappingProvider>) this.subclassProviders))
          ++level;
        flag = this.DistanceFromParentBase(parentType, evalType.BaseType, ref level);
      }
      return flag;
    }
  }
}
