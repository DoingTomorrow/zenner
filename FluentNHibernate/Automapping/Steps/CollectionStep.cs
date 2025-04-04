// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.CollectionStep
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class CollectionStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;
    private readonly AutoKeyMapper keys;

    public CollectionStep(IAutomappingConfiguration cfg)
    {
      this.cfg = cfg;
      this.keys = new AutoKeyMapper();
    }

    public bool ShouldMap(Member member)
    {
      return member.PropertyType.Namespace.In<string>("System.Collections.Generic", "Iesi.Collections.Generic") && !member.PropertyType.HasInterface(typeof (IDictionary)) && !member.PropertyType.ClosesInterface(typeof (IDictionary<,>)) && !member.PropertyType.Closes(typeof (IDictionary<,>));
    }

    public void Map(ClassMappingBase classMap, Member member)
    {
      if (member.DeclaringType != classMap.Type)
        return;
      CollectionMapping collectionMapping = CollectionMapping.For(CollectionTypeResolver.Resolve(member));
      collectionMapping.ContainingEntityType = classMap.Type;
      collectionMapping.Member = member;
      collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Name), 0, member.Name);
      collectionMapping.Set<Type>((Expression<Func<CollectionMapping, Type>>) (x => x.ChildType), 0, member.PropertyType.GetGenericArguments()[0]);
      this.SetDefaultAccess(member, collectionMapping);
      CollectionStep.SetRelationship(member, classMap, collectionMapping);
      this.keys.SetKey(member, classMap, collectionMapping);
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

    private static void SetRelationship(
      Member property,
      ClassMappingBase classMap,
      CollectionMapping mapping)
    {
      OneToManyMapping oneToManyMapping = new OneToManyMapping()
      {
        ContainingEntityType = classMap.Type
      };
      oneToManyMapping.Set<TypeReference>((Expression<Func<OneToManyMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(property.PropertyType.GetGenericArguments()[0]));
      mapping.Set<ICollectionRelationshipMapping>((Expression<Func<CollectionMapping, ICollectionRelationshipMapping>>) (x => x.Relationship), 0, (ICollectionRelationshipMapping) oneToManyMapping);
    }
  }
}
