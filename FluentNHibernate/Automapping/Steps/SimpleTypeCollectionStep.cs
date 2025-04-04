// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.SimpleTypeCollectionStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class SimpleTypeCollectionStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;
    private readonly AutoKeyMapper keys;

    public SimpleTypeCollectionStep(IAutomappingConfiguration cfg)
    {
      this.cfg = cfg;
      this.keys = new AutoKeyMapper();
    }

    public bool ShouldMap(Member member)
    {
      if (!member.PropertyType.IsGenericType || member.PropertyType.ClosesInterface(typeof (IDictionary<,>)) || member.PropertyType.Closes(typeof (IDictionary<,>)))
        return false;
      Type genericArgument = member.PropertyType.GetGenericArguments()[0];
      if (!member.PropertyType.ClosesInterface(typeof (IEnumerable<>)))
        return false;
      if (genericArgument.IsPrimitive)
        return true;
      return genericArgument.In<Type>(typeof (string), typeof (DateTime));
    }

    public void Map(ClassMappingBase classMap, Member member)
    {
      if (member.DeclaringType != classMap.Type)
        return;
      CollectionMapping collectionMapping = CollectionMapping.For(CollectionTypeResolver.Resolve(member));
      collectionMapping.ContainingEntityType = classMap.Type;
      collectionMapping.Member = member;
      collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Name), 0, member.Name);
      this.SetDefaultAccess(member, collectionMapping);
      this.keys.SetKey(member, classMap, collectionMapping);
      this.SetElement(member, classMap, collectionMapping);
      classMap.AddCollection(collectionMapping);
    }

    private void SetDefaultAccess(Member member, CollectionMapping mapping)
    {
      Access access = MemberAccessResolver.Resolve(member);
      if (access != Access.Property && access != Access.Unset)
        mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Access), 0, access.ToString());
      if (!member.IsProperty || member.CanWrite)
        return;
      mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Access), 0, this.cfg.GetAccessStrategyForReadOnlyProperty(member).ToString());
    }

    private void SetElement(Member property, ClassMappingBase classMap, CollectionMapping mapping)
    {
      ElementMapping elementMapping = new ElementMapping()
      {
        ContainingEntityType = classMap.Type
      };
      elementMapping.Set<TypeReference>((Expression<Func<ElementMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(property.PropertyType.GetGenericArguments()[0]));
      ColumnMapping mapping1 = new ColumnMapping();
      mapping1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, this.cfg.SimpleTypeCollectionValueColumn(property));
      elementMapping.AddColumn(0, mapping1);
      mapping.Set<ElementMapping>((Expression<Func<CollectionMapping, ElementMapping>>) (x => x.Element), 0, elementMapping);
    }
  }
}
