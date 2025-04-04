// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.MappedMembers
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  internal class MappedMembers : IMapping, IHasMappedMembers
  {
    private readonly List<PropertyMapping> properties;
    private readonly List<CollectionMapping> collections;
    private readonly List<ManyToOneMapping> references;
    private readonly List<IComponentMapping> components;
    private readonly List<OneToOneMapping> oneToOnes;
    private readonly List<AnyMapping> anys;
    private readonly List<JoinMapping> joins;
    private readonly List<FilterMapping> filters;
    private readonly List<StoredProcedureMapping> storedProcedures;

    public MappedMembers()
    {
      this.properties = new List<PropertyMapping>();
      this.collections = new List<CollectionMapping>();
      this.references = new List<ManyToOneMapping>();
      this.components = new List<IComponentMapping>();
      this.oneToOnes = new List<OneToOneMapping>();
      this.anys = new List<AnyMapping>();
      this.joins = new List<JoinMapping>();
      this.filters = new List<FilterMapping>();
      this.storedProcedures = new List<StoredProcedureMapping>();
    }

    public IEnumerable<PropertyMapping> Properties
    {
      get => (IEnumerable<PropertyMapping>) this.properties;
    }

    public IEnumerable<CollectionMapping> Collections
    {
      get => (IEnumerable<CollectionMapping>) this.collections;
    }

    public IEnumerable<ManyToOneMapping> References
    {
      get => (IEnumerable<ManyToOneMapping>) this.references;
    }

    public IEnumerable<IComponentMapping> Components
    {
      get => (IEnumerable<IComponentMapping>) this.components;
    }

    public IEnumerable<OneToOneMapping> OneToOnes => (IEnumerable<OneToOneMapping>) this.oneToOnes;

    public IEnumerable<AnyMapping> Anys => (IEnumerable<AnyMapping>) this.anys;

    public IEnumerable<JoinMapping> Joins => (IEnumerable<JoinMapping>) this.joins;

    public IEnumerable<FilterMapping> Filters => (IEnumerable<FilterMapping>) this.filters;

    public void AddOrReplaceFilter(FilterMapping mapping)
    {
      FilterMapping filterMapping = this.filters.Find((Predicate<FilterMapping>) (x => x.Name == mapping.Name));
      if (filterMapping != null)
        this.filters.Remove(filterMapping);
      this.filters.Add(mapping);
    }

    public IEnumerable<StoredProcedureMapping> StoredProcedures
    {
      get => (IEnumerable<StoredProcedureMapping>) this.storedProcedures;
    }

    public void AddProperty(PropertyMapping property)
    {
      if (this.properties.Exists((Predicate<PropertyMapping>) (x => x.Name == property.Name)))
        throw new InvalidOperationException("Tried to add property '" + property.Name + "' when already added.");
      this.properties.Add(property);
    }

    public void AddOrReplaceProperty(PropertyMapping mapping)
    {
      this.properties.RemoveAll((Predicate<PropertyMapping>) (x => x.Name == mapping.Name));
      this.properties.Add(mapping);
    }

    public void AddCollection(CollectionMapping collection)
    {
      if (this.collections.Exists((Predicate<CollectionMapping>) (x => x.Name == collection.Name)))
        throw new InvalidOperationException("Tried to add collection '" + collection.Name + "' when already added.");
      this.collections.Add(collection);
    }

    public void AddOrReplaceCollection(CollectionMapping mapping)
    {
      this.collections.RemoveAll((Predicate<CollectionMapping>) (x => x.Name == mapping.Name));
      this.collections.Add(mapping);
    }

    public void AddReference(ManyToOneMapping manyToOne)
    {
      if (this.references.Exists((Predicate<ManyToOneMapping>) (x => x.Name == manyToOne.Name)))
        throw new InvalidOperationException("Tried to add many-to-one '" + manyToOne.Name + "' when already added.");
      this.references.Add(manyToOne);
    }

    public void AddOrReplaceReference(ManyToOneMapping manyToOne)
    {
      this.references.RemoveAll((Predicate<ManyToOneMapping>) (x => x.Name == manyToOne.Name));
      this.references.Add(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
      if (this.components.Exists((Predicate<IComponentMapping>) (x => x.Name == componentMapping.Name)))
        throw new InvalidOperationException("Tried to add component '" + componentMapping.Name + "' when already added.");
      this.components.Add(componentMapping);
    }

    public void AddOrReplaceComponent(IComponentMapping componentMapping)
    {
      this.components.RemoveAll((Predicate<IComponentMapping>) (x => x.Name == componentMapping.Name));
      this.components.Add(componentMapping);
    }

    public void AddOneToOne(OneToOneMapping mapping)
    {
      if (this.oneToOnes.Exists((Predicate<OneToOneMapping>) (x => x.Name == mapping.Name)))
        throw new InvalidOperationException("Tried to add one-to-one '" + mapping.Name + "' when already added.");
      this.oneToOnes.Add(mapping);
    }

    public void AddOrReplaceOneToOne(OneToOneMapping mapping)
    {
      this.oneToOnes.RemoveAll((Predicate<OneToOneMapping>) (x => x.Name == mapping.Name));
      this.oneToOnes.Add(mapping);
    }

    public void AddAny(AnyMapping mapping)
    {
      if (this.anys.Exists((Predicate<AnyMapping>) (x => x.Name == mapping.Name)))
        throw new InvalidOperationException("Tried to add any '" + mapping.Name + "' when already added.");
      this.anys.Add(mapping);
    }

    public void AddOrReplaceAny(AnyMapping mapping)
    {
      this.anys.RemoveAll((Predicate<AnyMapping>) (x => x.Name == mapping.Name));
      this.anys.Add(mapping);
    }

    public void AddJoin(JoinMapping mapping)
    {
      if (this.joins.Exists((Predicate<JoinMapping>) (x => x.TableName == mapping.TableName)))
        throw new InvalidOperationException("Tried to add join to table '" + mapping.TableName + "' when already added.");
      this.joins.Add(mapping);
    }

    public void AddFilter(FilterMapping mapping)
    {
      if (this.filters.Exists((Predicate<FilterMapping>) (x => x.Name == mapping.Name)))
        throw new InvalidOperationException("Tried to add filter with name '" + mapping.Name + "' when already added.");
      this.filters.Add(mapping);
    }

    public virtual void AcceptVisitor(IMappingModelVisitor visitor)
    {
      foreach (CollectionMapping collection in this.Collections)
        visitor.Visit(collection);
      foreach (PropertyMapping property in this.Properties)
        visitor.Visit(property);
      foreach (ManyToOneMapping reference in this.References)
        visitor.Visit(reference);
      foreach (IComponentMapping component in this.Components)
        visitor.Visit(component);
      foreach (OneToOneMapping oneToOne in this.oneToOnes)
        visitor.Visit(oneToOne);
      foreach (AnyMapping any in this.anys)
        visitor.Visit(any);
      foreach (JoinMapping join in this.joins)
        visitor.Visit(join);
      foreach (FilterMapping filter in this.filters)
        visitor.Visit(filter);
      foreach (StoredProcedureMapping storedProcedure in this.storedProcedures)
        visitor.Visit(storedProcedure);
    }

    public bool IsSpecified(string property) => false;

    public void Set(string attribute, int layer, object value)
    {
    }

    public void AddStoredProcedure(StoredProcedureMapping mapping)
    {
      this.storedProcedures.Add(mapping);
    }

    public bool Equals(MappedMembers other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return other.properties.ContentEquals<PropertyMapping>((IEnumerable<PropertyMapping>) this.properties) && other.collections.ContentEquals<CollectionMapping>((IEnumerable<CollectionMapping>) this.collections) && other.references.ContentEquals<ManyToOneMapping>((IEnumerable<ManyToOneMapping>) this.references) && other.components.ContentEquals<IComponentMapping>((IEnumerable<IComponentMapping>) this.components) && other.oneToOnes.ContentEquals<OneToOneMapping>((IEnumerable<OneToOneMapping>) this.oneToOnes) && other.anys.ContentEquals<AnyMapping>((IEnumerable<AnyMapping>) this.anys) && other.joins.ContentEquals<JoinMapping>((IEnumerable<JoinMapping>) this.joins) && other.filters.ContentEquals<FilterMapping>((IEnumerable<FilterMapping>) this.filters) && other.storedProcedures.ContentEquals<StoredProcedureMapping>((IEnumerable<StoredProcedureMapping>) this.storedProcedures);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (MappedMembers) && this.Equals((MappedMembers) obj);
    }

    public override int GetHashCode()
    {
      return ((((((((this.properties != null ? this.properties.GetHashCode() : 0) * 397 ^ (this.collections != null ? this.collections.GetHashCode() : 0)) * 397 ^ (this.references != null ? this.references.GetHashCode() : 0)) * 397 ^ (this.components != null ? this.components.GetHashCode() : 0)) * 397 ^ (this.oneToOnes != null ? this.oneToOnes.GetHashCode() : 0)) * 397 ^ (this.anys != null ? this.anys.GetHashCode() : 0)) * 397 ^ (this.joins != null ? this.joins.GetHashCode() : 0)) * 397 ^ (this.filters != null ? this.filters.GetHashCode() : 0)) * 397 ^ (this.storedProcedures != null ? this.storedProcedures.GetHashCode() : 0);
    }
  }
}
