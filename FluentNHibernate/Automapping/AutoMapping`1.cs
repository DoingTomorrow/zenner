// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMapping`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoMapping<T> : ClassMap<T>, IAutoClasslike, IMappingProvider, IPropertyIgnorer
  {
    private readonly MappingProviderStore providers;
    private readonly IList<Member> mappedMembers;

    public AutoMapping(IList<Member> mappedMembers)
      : this(mappedMembers, new AttributeStore(), new MappingProviderStore())
    {
    }

    private AutoMapping(
      IList<Member> mappedMembers,
      AttributeStore attributes,
      MappingProviderStore providers)
      : base(attributes, providers)
    {
      this.mappedMembers = mappedMembers;
      this.providers = providers;
    }

    void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
    {
      this.DiscriminateSubClassesOnColumn(column);
    }

    IEnumerable<Member> IMappingProvider.GetIgnoredProperties()
    {
      return (IEnumerable<Member>) this.mappedMembers;
    }

    void IAutoClasslike.AlterModel(ClassMappingBase mapping)
    {
      mapping.MergeAttributes(this.attributes.Clone());
      if (mapping is ClassMapping)
      {
        ClassMapping classMapping = (ClassMapping) mapping;
        if (this.providers.Id != null)
          classMapping.Set<IIdentityMapping>((Expression<Func<ClassMapping, IIdentityMapping>>) (x => x.Id), 0, (IIdentityMapping) this.providers.Id.GetIdentityMapping());
        if (this.providers.NaturalId != null)
          classMapping.Set<NaturalIdMapping>((Expression<Func<ClassMapping, NaturalIdMapping>>) (x => x.NaturalId), 0, this.providers.NaturalId.GetNaturalIdMapping());
        if (this.providers.CompositeId != null)
          classMapping.Set<IIdentityMapping>((Expression<Func<ClassMapping, IIdentityMapping>>) (x => x.Id), 0, (IIdentityMapping) this.providers.CompositeId.GetCompositeIdMapping());
        if (this.providers.Version != null)
          classMapping.Set<VersionMapping>((Expression<Func<ClassMapping, VersionMapping>>) (x => x.Version), 0, this.providers.Version.GetVersionMapping());
        if (this.providers.Discriminator != null)
          classMapping.Set<DiscriminatorMapping>((Expression<Func<ClassMapping, DiscriminatorMapping>>) (x => x.Discriminator), 0, this.providers.Discriminator.GetDiscriminatorMapping());
        if (this.Cache.IsDirty)
          classMapping.Set<CacheMapping>((Expression<Func<ClassMapping, CacheMapping>>) (x => x.Cache), 0, ((ICacheMappingProvider) this.Cache).GetCacheMapping());
        foreach (IJoinMappingProvider join in (IEnumerable<IJoinMappingProvider>) this.providers.Joins)
          classMapping.AddJoin(join.GetJoinMapping());
        classMapping.Set<TuplizerMapping>((Expression<Func<ClassMapping, TuplizerMapping>>) (x => x.Tuplizer), 0, this.providers.TuplizerMapping);
      }
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        mapping.AddOrReplaceProperty(property.GetPropertyMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        mapping.AddOrReplaceCollection(collection.GetCollectionMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        mapping.AddOrReplaceComponent(component.GetComponentMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        mapping.AddOrReplaceOneToOne(oneToOne.GetOneToOneMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        mapping.AddOrReplaceReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        mapping.AddOrReplaceAny(any.GetAnyMapping());
      foreach (IStoredProcedureMappingProvider storedProcedure in (IEnumerable<IStoredProcedureMappingProvider>) this.providers.StoredProcedures)
        mapping.AddStoredProcedure(storedProcedure.GetStoredProcedureMapping());
      foreach (IFilterMappingProvider filter in (IEnumerable<IFilterMappingProvider>) this.providers.Filters)
        mapping.AddOrReplaceFilter(filter.GetFilterMapping());
    }

    internal override void OnMemberMapped(Member member) => this.mappedMembers.Add(member);

    public void IgnoreProperty(Expression<Func<T, object>> expression)
    {
      this.mappedMembers.Add(expression.ToMember<T, object>());
    }

    IPropertyIgnorer IPropertyIgnorer.IgnoreProperty(string name)
    {
      ((IPropertyIgnorer) this).IgnoreProperties(name);
      return (IPropertyIgnorer) this;
    }

    IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(string first, params string[] others)
    {
      string[] options = ((IEnumerable<string>) (others ?? new string[0])).Concat<string>((IEnumerable<string>) new string[1]
      {
        first
      }).ToArray<string>();
      ((IPropertyIgnorer) this).IgnoreProperties((Func<Member, bool>) (x => x.Name.In<string>(options)));
      return (IPropertyIgnorer) this;
    }

    IPropertyIgnorer IPropertyIgnorer.IgnoreProperties(Func<Member, bool> predicate)
    {
      ((IEnumerable<PropertyInfo>) typeof (T).GetProperties()).Select<PropertyInfo, Member>((Func<PropertyInfo, Member>) (x => MemberExtensions.ToMember(x))).Where<Member>(predicate).Each<Member>(new Action<Member>(((ICollection<Member>) this.mappedMembers).Add));
      return (IPropertyIgnorer) this;
    }

    public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(
      string keyColumn,
      Action<AutoJoinedSubClassPart<TSubclass>> action)
      where TSubclass : T
    {
      AutoJoinedSubClassPart<TSubclass> instance = (AutoJoinedSubClassPart<TSubclass>) Activator.CreateInstance(typeof (AutoJoinedSubClassPart<>).MakeGenericType(typeof (TSubclass)), (object) keyColumn);
      if (action != null)
        action(instance);
      this.providers.Subclasses[typeof (TSubclass)] = (ISubclassMappingProvider) instance;
      return instance;
    }

    public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
    {
      ISubclassMappingProvider instance = (ISubclassMappingProvider) Activator.CreateInstance(typeof (AutoJoinedSubClassPart<>).MakeGenericType(type), (object) keyColumn);
      this.providers.Subclasses[type] = instance;
      return (IAutoClasslike) instance;
    }

    public AutoJoinedSubClassPart<TSubclass> JoinedSubClass<TSubclass>(string keyColumn) where TSubclass : T
    {
      return this.JoinedSubClass<TSubclass>(keyColumn, (Action<AutoJoinedSubClassPart<TSubclass>>) null);
    }

    public AutoSubClassPart<TSubclass> SubClass<TSubclass>(
      object discriminatorValue,
      Action<AutoSubClassPart<TSubclass>> action)
      where TSubclass : T
    {
      AutoSubClassPart<TSubclass> instance = (AutoSubClassPart<TSubclass>) Activator.CreateInstance(typeof (AutoSubClassPart<>).MakeGenericType(typeof (TSubclass)), new object[2]
      {
        null,
        discriminatorValue
      });
      if (action != null)
        action(instance);
      this.providers.Subclasses[typeof (TSubclass)] = (ISubclassMappingProvider) instance;
      return instance;
    }

    public AutoSubClassPart<TSubclass> SubClass<TSubclass>(object discriminatorValue) where TSubclass : T
    {
      return this.SubClass<TSubclass>(discriminatorValue, (Action<AutoSubClassPart<TSubclass>>) null);
    }

    public IAutoClasslike SubClass(Type type, string discriminatorValue)
    {
      ISubclassMappingProvider instance = (ISubclassMappingProvider) Activator.CreateInstance(typeof (AutoSubClassPart<>).MakeGenericType(type), new object[2]
      {
        null,
        (object) discriminatorValue
      });
      this.providers.Subclasses[type] = instance;
      return (IAutoClasslike) instance;
    }

    private new void Join(string table, Action<JoinPart<T>> action)
    {
    }

    public void Join(string table, Action<AutoJoinPart<T>> action)
    {
      AutoJoinPart<T> autoJoinPart = new AutoJoinPart<T>(this.mappedMembers, table);
      action(autoJoinPart);
      this.providers.Joins.Add((IJoinMappingProvider) autoJoinPart);
    }

    [Obsolete("Imports aren't supported in overrides.", true)]
    public new ImportPart ImportType<TImport>() => (ImportPart) null;
  }
}
