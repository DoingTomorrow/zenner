// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ClassMappingBase
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public abstract class ClassMappingBase : MappingBase, IHasMappedMembers
  {
    private readonly AttributeStore attributes;
    private readonly MappedMembers mappedMembers;
    private readonly IList<SubclassMapping> subclasses;

    protected ClassMappingBase(AttributeStore attributes)
    {
      this.attributes = attributes;
      this.mappedMembers = new MappedMembers();
      this.subclasses = (IList<SubclassMapping>) new List<SubclassMapping>();
    }

    public abstract string Name { get; }

    public abstract Type Type { get; }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      this.mappedMembers.AcceptVisitor(visitor);
      foreach (SubclassMapping subclass in this.Subclasses)
        visitor.Visit(subclass);
    }

    public IEnumerable<ManyToOneMapping> References => this.mappedMembers.References;

    public IEnumerable<CollectionMapping> Collections => this.mappedMembers.Collections;

    public IEnumerable<PropertyMapping> Properties => this.mappedMembers.Properties;

    public IEnumerable<IComponentMapping> Components => this.mappedMembers.Components;

    public IEnumerable<OneToOneMapping> OneToOnes => this.mappedMembers.OneToOnes;

    public IEnumerable<AnyMapping> Anys => this.mappedMembers.Anys;

    public IEnumerable<JoinMapping> Joins => this.mappedMembers.Joins;

    public IEnumerable<FilterMapping> Filters => this.mappedMembers.Filters;

    public IEnumerable<SubclassMapping> Subclasses
    {
      get => (IEnumerable<SubclassMapping>) this.subclasses;
    }

    public IEnumerable<StoredProcedureMapping> StoredProcedures
    {
      get => this.mappedMembers.StoredProcedures;
    }

    public void AddProperty(PropertyMapping property) => this.mappedMembers.AddProperty(property);

    public void AddOrReplaceProperty(PropertyMapping mapping)
    {
      this.mappedMembers.AddOrReplaceProperty(mapping);
    }

    public void AddCollection(CollectionMapping collection)
    {
      this.mappedMembers.AddCollection(collection);
    }

    public void AddOrReplaceCollection(CollectionMapping mapping)
    {
      this.mappedMembers.AddOrReplaceCollection(mapping);
    }

    public void AddReference(ManyToOneMapping manyToOne)
    {
      this.mappedMembers.AddReference(manyToOne);
    }

    public void AddOrReplaceReference(ManyToOneMapping manyToOne)
    {
      this.mappedMembers.AddOrReplaceReference(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
      this.mappedMembers.AddComponent(componentMapping);
    }

    public void AddOrReplaceComponent(IComponentMapping mapping)
    {
      this.mappedMembers.AddOrReplaceComponent(mapping);
    }

    public void AddOneToOne(OneToOneMapping mapping) => this.mappedMembers.AddOneToOne(mapping);

    public void AddOrReplaceOneToOne(OneToOneMapping mapping)
    {
      this.mappedMembers.AddOrReplaceOneToOne(mapping);
    }

    public void AddAny(AnyMapping mapping) => this.mappedMembers.AddAny(mapping);

    public void AddOrReplaceAny(AnyMapping mapping) => this.mappedMembers.AddOrReplaceAny(mapping);

    public void AddJoin(JoinMapping mapping) => this.mappedMembers.AddJoin(mapping);

    public void AddFilter(FilterMapping mapping) => this.mappedMembers.AddFilter(mapping);

    public void AddOrReplaceFilter(FilterMapping mapping)
    {
      this.mappedMembers.AddOrReplaceFilter(mapping);
    }

    public void AddSubclass(SubclassMapping subclass) => this.subclasses.Add(subclass);

    public void AddStoredProcedure(StoredProcedureMapping mapping)
    {
      this.mappedMembers.AddStoredProcedure(mapping);
    }

    public override string ToString()
    {
      return string.Format("ClassMapping({0})", (object) this.Type.Name);
    }

    public bool Equals(ClassMappingBase other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.mappedMembers, (object) this.mappedMembers) && other.subclasses.ContentEquals<SubclassMapping>((IEnumerable<SubclassMapping>) this.subclasses);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (ClassMappingBase) && this.Equals((ClassMappingBase) obj);
    }

    public override int GetHashCode()
    {
      return (this.mappedMembers != null ? this.mappedMembers.GetHashCode() : 0) * 397 ^ (this.subclasses != null ? this.subclasses.GetHashCode() : 0);
    }

    public void MergeAttributes(AttributeStore clone) => clone.CopyTo(this.attributes);
  }
}
