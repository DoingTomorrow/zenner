// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.Steps.HasManyToManyStep
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Automapping.Steps
{
  public class HasManyToManyStep : IAutomappingStep
  {
    private readonly IAutomappingConfiguration cfg;

    public HasManyToManyStep(IAutomappingConfiguration cfg) => this.cfg = cfg;

    public bool ShouldMap(Member member)
    {
      Type propertyType = member.PropertyType;
      return (!(propertyType.Namespace != "Iesi.Collections.Generic") || !(propertyType.Namespace != "System.Collections.Generic")) && !propertyType.HasInterface(typeof (IDictionary)) && !propertyType.ClosesInterface(typeof (IDictionary<,>)) && !propertyType.Closes(typeof (IDictionary<,>)) && HasManyToManyStep.GetInverseProperty(member) != (Member) null;
    }

    private static Member GetInverseProperty(Member member)
    {
      Type propertyType = member.PropertyType;
      Type expectedInversePropertyType = propertyType.GetGenericTypeDefinition().MakeGenericType(member.DeclaringType);
      return ((IEnumerable<PropertyInfo>) propertyType.GetGenericArguments()[0].GetProperties()).Select<PropertyInfo, Member>((Func<PropertyInfo, Member>) (x => MemberExtensions.ToMember(x))).Where<Member>((Func<Member, bool>) (x => x.PropertyType == expectedInversePropertyType && x != member)).FirstOrDefault<Member>();
    }

    private static CollectionMapping GetCollection(Member property)
    {
      return CollectionMapping.For(CollectionTypeResolver.Resolve(property));
    }

    private void ConfigureModel(
      Member member,
      CollectionMapping mapping,
      ClassMappingBase classMap,
      Type parentSide)
    {
      mapping.ContainingEntityType = classMap.Type;
      mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Name), 0, member.Name);
      mapping.Set<ICollectionRelationshipMapping>((Expression<Func<CollectionMapping, ICollectionRelationshipMapping>>) (x => x.Relationship), 0, HasManyToManyStep.CreateManyToMany(member, member.PropertyType.GetGenericArguments()[0], classMap.Type));
      mapping.Set<Type>((Expression<Func<CollectionMapping, Type>>) (x => x.ChildType), 0, member.PropertyType.GetGenericArguments()[0]);
      mapping.Member = member;
      this.SetDefaultAccess(member, mapping);
      HasManyToManyStep.SetKey(member, classMap, mapping);
      if (parentSide == member.DeclaringType)
        return;
      mapping.Set<bool>((Expression<Func<CollectionMapping, bool>>) (x => x.Inverse), 0, true);
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

    private static ICollectionRelationshipMapping CreateManyToMany(
      Member property,
      Type child,
      Type parent)
    {
      ManyToManyMapping manyToMany = new ManyToManyMapping()
      {
        ContainingEntityType = parent
      };
      manyToMany.Set<TypeReference>((Expression<Func<ManyToManyMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(property.PropertyType.GetGenericArguments()[0]));
      ColumnMapping mapping = new ColumnMapping();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, child.Name + "_id");
      manyToMany.AddColumn(0, mapping);
      return (ICollectionRelationshipMapping) manyToMany;
    }

    private static void SetKey(
      Member property,
      ClassMappingBase classMap,
      CollectionMapping mapping)
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

    public void Map(ClassMappingBase classMap, Member member)
    {
      Member inverseProperty = HasManyToManyStep.GetInverseProperty(member);
      Type sideForManyToMany = this.cfg.GetParentSideForManyToMany(member.DeclaringType, inverseProperty.DeclaringType);
      CollectionMapping collection = HasManyToManyStep.GetCollection(member);
      this.ConfigureModel(member, collection, classMap, sideForManyToMany);
      classMap.AddCollection(collection);
    }
  }
}
