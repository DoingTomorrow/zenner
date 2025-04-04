// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ModelExplicitDeclarationsHolderExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public static class ModelExplicitDeclarationsHolderExtensions
  {
    public static void Merge(
      this IModelExplicitDeclarationsHolder destination,
      IModelExplicitDeclarationsHolder source)
    {
      if (destination == null || source == null)
        return;
      System.Array.ForEach<Type>(source.RootEntities.ToArray<Type>(), new Action<Type>(destination.AddAsRootEntity));
      System.Array.ForEach<Type>(source.Components.ToArray<Type>(), new Action<Type>(destination.AddAsComponent));
      System.Array.ForEach<Type>(source.TablePerClassEntities.ToArray<Type>(), new Action<Type>(destination.AddAsTablePerClassEntity));
      System.Array.ForEach<Type>(source.TablePerClassHierarchyEntities.ToArray<Type>(), new Action<Type>(destination.AddAsTablePerClassHierarchyEntity));
      System.Array.ForEach<Type>(source.TablePerConcreteClassEntities.ToArray<Type>(), new Action<Type>(destination.AddAsTablePerConcreteClassEntity));
      System.Array.ForEach<MemberInfo>(source.OneToOneRelations.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsOneToOneRelation));
      System.Array.ForEach<MemberInfo>(source.ManyToOneRelations.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsManyToOneRelation));
      System.Array.ForEach<MemberInfo>(source.ManyToManyRelations.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsManyToManyRelation));
      System.Array.ForEach<MemberInfo>(source.ManyToAnyRelations.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsManyToAnyRelation));
      System.Array.ForEach<MemberInfo>(source.OneToManyRelations.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsOneToManyRelation));
      System.Array.ForEach<MemberInfo>(source.Any.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsAny));
      System.Array.ForEach<MemberInfo>(source.Poids.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsPoid));
      System.Array.ForEach<MemberInfo>(source.ComposedIds.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsPartOfComposedId));
      System.Array.ForEach<MemberInfo>(source.VersionProperties.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsVersionProperty));
      System.Array.ForEach<MemberInfo>(source.NaturalIds.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsNaturalId));
      System.Array.ForEach<MemberInfo>(source.Sets.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsSet));
      System.Array.ForEach<MemberInfo>(source.Bags.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsBag));
      System.Array.ForEach<MemberInfo>(source.IdBags.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsIdBag));
      System.Array.ForEach<MemberInfo>(source.Lists.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsList));
      System.Array.ForEach<MemberInfo>(source.Arrays.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsArray));
      System.Array.ForEach<MemberInfo>(source.Dictionaries.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsMap));
      System.Array.ForEach<MemberInfo>(source.Properties.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsProperty));
      System.Array.ForEach<MemberInfo>(source.PersistentMembers.ToArray<MemberInfo>(), new Action<MemberInfo>(destination.AddAsPersistentMember));
      System.Array.ForEach<SplitDefinition>(source.SplitDefinitions.ToArray<SplitDefinition>(), new Action<SplitDefinition>(destination.AddAsPropertySplit));
      foreach (MemberInfo dynamicComponent in source.DynamicComponents)
      {
        Type componentTemplate = source.GetDynamicComponentTemplate(dynamicComponent);
        destination.AddAsDynamicComponent(dynamicComponent, componentTemplate);
      }
    }
  }
}
