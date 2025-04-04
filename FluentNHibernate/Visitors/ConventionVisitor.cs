// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ConventionVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class ConventionVisitor : DefaultMappingModelVisitor
  {
    private readonly Dictionary<Collection, Action<CollectionMapping>> collections;
    private readonly IConventionFinder finder;
    private Type currentType;

    public ConventionVisitor(IConventionFinder finder)
    {
      this.collections = new Dictionary<Collection, Action<CollectionMapping>>()
      {
        {
          Collection.Array,
          new Action<CollectionMapping>(this.ProcessArray)
        },
        {
          Collection.Bag,
          new Action<CollectionMapping>(this.ProcessBag)
        },
        {
          Collection.Map,
          new Action<CollectionMapping>(this.ProcessMap)
        },
        {
          Collection.List,
          new Action<CollectionMapping>(this.ProcessList)
        },
        {
          Collection.Set,
          new Action<CollectionMapping>(this.ProcessSet)
        }
      };
      this.finder = finder;
    }

    public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
    {
      ConventionVisitor.Apply<IHibernateMappingInspector, IHibernateMappingInstance>((IEnumerable) this.finder.Find<IHibernateMappingConvention>(), (IHibernateMappingInstance) new HibernateMappingInstance(hibernateMapping));
    }

    public override void ProcessId(IdMapping idMapping)
    {
      ConventionVisitor.Apply<IIdentityInspector, IIdentityInstance>((IEnumerable) this.finder.Find<IIdConvention>(), (IIdentityInstance) new IdentityInstance(idMapping));
    }

    public override void ProcessCompositeId(CompositeIdMapping idMapping)
    {
      ConventionVisitor.Apply<ICompositeIdentityInspector, ICompositeIdentityInstance>((IEnumerable) this.finder.Find<ICompositeIdentityConvention>(), (ICompositeIdentityInstance) new CompositeIdentityInstance(idMapping));
    }

    public override void ProcessClass(ClassMapping classMapping)
    {
      IEnumerable<IClassConvention> conventions = this.finder.Find<IClassConvention>();
      this.currentType = classMapping.Type;
      ConventionVisitor.Apply<IClassInspector, IClassInstance>((IEnumerable) conventions, (IClassInstance) new ClassInstance(classMapping));
    }

    public override void ProcessProperty(PropertyMapping propertyMapping)
    {
      ConventionVisitor.Apply<IPropertyInspector, IPropertyInstance>((IEnumerable) this.finder.Find<IPropertyConvention>(), (IPropertyInstance) new PropertyInstance(propertyMapping));
    }

    public override void ProcessColumn(ColumnMapping columnMapping)
    {
      ConventionVisitor.Apply<IColumnInspector, IColumnInstance>((IEnumerable) this.finder.Find<IColumnConvention>(), (IColumnInstance) new ColumnInstance(this.currentType, columnMapping));
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<ICollectionInspector, ICollectionInstance>((IEnumerable) this.finder.Find<ICollectionConvention>(), (ICollectionInstance) new CollectionInstance(mapping));
      if (mapping.Relationship is ManyToManyMapping)
        ConventionVisitor.Apply<IManyToManyCollectionInspector, IManyToManyCollectionInstance>((IEnumerable) this.finder.Find<IHasManyToManyConvention>(), (IManyToManyCollectionInstance) new ManyToManyCollectionInstance(mapping));
      else
        ConventionVisitor.Apply<IOneToManyCollectionInspector, IOneToManyCollectionInstance>((IEnumerable) this.finder.Find<IHasManyConvention>(), (IOneToManyCollectionInstance) new OneToManyCollectionInstance(mapping));
      this.collections[mapping.Collection](mapping);
    }

    private void ProcessArray(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<IArrayInspector, IArrayInstance>((IEnumerable) this.finder.Find<IArrayConvention>(), (IArrayInstance) new CollectionInstance(mapping));
    }

    private void ProcessBag(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<IBagInspector, IBagInstance>((IEnumerable) this.finder.Find<IBagConvention>(), (IBagInstance) new CollectionInstance(mapping));
    }

    private void ProcessList(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<IListInspector, IListInstance>((IEnumerable) this.finder.Find<IListConvention>(), (IListInstance) new CollectionInstance(mapping));
    }

    private void ProcessMap(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<IMapInspector, IMapInstance>((IEnumerable) this.finder.Find<IMapConvention>(), (IMapInstance) new CollectionInstance(mapping));
    }

    private void ProcessSet(CollectionMapping mapping)
    {
      ConventionVisitor.Apply<ISetInspector, ISetInstance>((IEnumerable) this.finder.Find<ISetConvention>(), (ISetInstance) new CollectionInstance(mapping));
    }

    public override void ProcessManyToOne(ManyToOneMapping mapping)
    {
      ConventionVisitor.Apply<IManyToOneInspector, IManyToOneInstance>((IEnumerable) this.finder.Find<IReferenceConvention>(), (IManyToOneInstance) new ManyToOneInstance(mapping));
    }

    public override void ProcessVersion(VersionMapping mapping)
    {
      ConventionVisitor.Apply<IVersionInspector, IVersionInstance>((IEnumerable) this.finder.Find<IVersionConvention>(), (IVersionInstance) new VersionInstance(mapping));
    }

    public override void ProcessOneToOne(OneToOneMapping mapping)
    {
      ConventionVisitor.Apply<IOneToOneInspector, IOneToOneInstance>((IEnumerable) this.finder.Find<IHasOneConvention>(), (IOneToOneInstance) new OneToOneInstance(mapping));
    }

    public override void ProcessSubclass(SubclassMapping subclassMapping)
    {
      if (subclassMapping.SubclassType == SubclassType.Subclass)
        ConventionVisitor.Apply<ISubclassInspector, ISubclassInstance>((IEnumerable) this.finder.Find<ISubclassConvention>(), (ISubclassInstance) new SubclassInstance(subclassMapping));
      else
        ConventionVisitor.Apply<IJoinedSubclassInspector, IJoinedSubclassInstance>((IEnumerable) this.finder.Find<IJoinedSubclassConvention>(), (IJoinedSubclassInstance) new JoinedSubclassInstance(subclassMapping));
    }

    public override void ProcessComponent(ComponentMapping mapping)
    {
      if (mapping.ComponentType == ComponentType.Component)
        ConventionVisitor.Apply<IComponentInspector, IComponentInstance>((IEnumerable) this.finder.Find<IComponentConvention>(), (IComponentInstance) new ComponentInstance(mapping));
      else
        ConventionVisitor.Apply<IDynamicComponentInspector, IDynamicComponentInstance>((IEnumerable) this.finder.Find<IDynamicComponentConvention>(), (IDynamicComponentInstance) new DynamicComponentInstance(mapping));
    }

    public override void ProcessIndex(IndexMapping indexMapping)
    {
      ConventionVisitor.Apply<IIndexInspector, IIndexInstance>((IEnumerable) this.finder.Find<IIndexConvention>(), (IIndexInstance) new IndexInstance(indexMapping));
    }

    public override void ProcessIndex(IndexManyToManyMapping indexMapping)
    {
      ConventionVisitor.Apply<IIndexManyToManyInspector, IIndexManyToManyInstance>((IEnumerable) this.finder.Find<IIndexManyToManyConvention>(), (IIndexManyToManyInstance) new IndexManyToManyInstance(indexMapping));
    }

    public override void ProcessJoin(JoinMapping joinMapping)
    {
      ConventionVisitor.Apply<IJoinInspector, IJoinInstance>((IEnumerable) this.finder.Find<IJoinConvention>(), (IJoinInstance) new JoinInstance(joinMapping));
    }

    public override void ProcessKeyProperty(KeyPropertyMapping mapping)
    {
      ConventionVisitor.Apply<IKeyPropertyInspector, IKeyPropertyInstance>((IEnumerable) this.finder.Find<IKeyPropertyConvention>(), (IKeyPropertyInstance) new KeyPropertyInstance(mapping));
    }

    public override void ProcessKeyManyToOne(KeyManyToOneMapping mapping)
    {
      ConventionVisitor.Apply<IKeyManyToOneInspector, IKeyManyToOneInstance>((IEnumerable) this.finder.Find<IKeyManyToOneConvention>(), (IKeyManyToOneInstance) new KeyManyToOneInstance(mapping));
    }

    public override void ProcessAny(AnyMapping mapping)
    {
      ConventionVisitor.Apply<IAnyInspector, IAnyInstance>((IEnumerable) this.finder.Find<IAnyConvention>(), (IAnyInstance) new AnyInstance(mapping));
    }

    private static void Apply<TInspector, TInstance>(IEnumerable conventions, TInstance instance)
      where TInspector : IInspector
      where TInstance : TInspector
    {
      foreach (IConvention<TInspector, TInstance> convention in conventions)
      {
        ConcreteAcceptanceCriteria<TInspector> criteria = new ConcreteAcceptanceCriteria<TInspector>();
        if (convention is IConventionAcceptance<TInspector> conventionAcceptance)
          conventionAcceptance.Accept((IAcceptanceCriteria<TInspector>) criteria);
        if (criteria.Matches((IInspector) instance))
          convention.Apply(instance);
      }
    }
  }
}
