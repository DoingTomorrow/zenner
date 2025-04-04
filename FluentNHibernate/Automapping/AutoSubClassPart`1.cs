// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoSubClassPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoSubClassPart<T> : SubClassPart<T>, IAutoClasslike, IMappingProvider
  {
    private readonly MappingProviderStore providers;
    private readonly IList<Member> mappedMembers = (IList<Member>) new List<Member>();

    public AutoSubClassPart(DiscriminatorPart parent, string discriminatorValue)
      : this(parent, discriminatorValue, new MappingProviderStore())
    {
    }

    private AutoSubClassPart(
      DiscriminatorPart parent,
      string discriminatorValue,
      MappingProviderStore providers)
      : base(parent, (object) discriminatorValue, providers)
    {
      this.providers = providers;
    }

    public object GetMapping() => (object) ((ISubclassMappingProvider) this).GetSubclassMapping();

    void IAutoClasslike.DiscriminateSubClassesOnColumn(string column)
    {
    }

    void IAutoClasslike.AlterModel(ClassMappingBase mapping)
    {
    }

    internal override void OnMemberMapped(Member member) => this.mappedMembers.Add(member);

    public void JoinedSubClass<TSubclass>(
      string keyColumn,
      Action<AutoJoinedSubClassPart<TSubclass>> action)
    {
      AutoJoinedSubClassPart<TSubclass> instance = (AutoJoinedSubClassPart<TSubclass>) Activator.CreateInstance(typeof (AutoJoinedSubClassPart<>).MakeGenericType(typeof (TSubclass)), (object) keyColumn);
      action(instance);
      this.providers.Subclasses[typeof (TSubclass)] = (ISubclassMappingProvider) instance;
    }

    public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
    {
      ISubclassMappingProvider instance = (ISubclassMappingProvider) Activator.CreateInstance(typeof (AutoJoinedSubClassPart<>).MakeGenericType(type), (object) keyColumn);
      this.providers.Subclasses[type] = instance;
      return (IAutoClasslike) instance;
    }

    public void SubClass<TSubclass>(
      string discriminatorValue,
      Action<AutoSubClassPart<TSubclass>> action)
    {
      AutoSubClassPart<TSubclass> instance = (AutoSubClassPart<TSubclass>) Activator.CreateInstance(typeof (AutoSubClassPart<>).MakeGenericType(typeof (TSubclass)), (object) discriminatorValue);
      action(instance);
      this.providers.Subclasses[typeof (TSubclass)] = (ISubclassMappingProvider) instance;
    }

    public IAutoClasslike SubClass(Type type, string discriminatorValue)
    {
      ISubclassMappingProvider instance = (ISubclassMappingProvider) Activator.CreateInstance(typeof (AutoSubClassPart<>).MakeGenericType(type), (object) discriminatorValue);
      this.providers.Subclasses[type] = instance;
      return (IAutoClasslike) instance;
    }

    public ClassMapping GetClassMapping() => (ClassMapping) null;

    public HibernateMapping GetHibernateMapping() => (HibernateMapping) null;

    public IEnumerable<Member> GetIgnoredProperties() => (IEnumerable<Member>) this.mappedMembers;
  }
}
