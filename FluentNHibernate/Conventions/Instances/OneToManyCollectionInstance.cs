// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.OneToManyCollectionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class OneToManyCollectionInstance : 
    CollectionInstance,
    IOneToManyCollectionInstance,
    IOneToManyCollectionInspector,
    ICollectionInstance,
    ICollectionInspector,
    IInspector
  {
    private readonly CollectionMapping mapping;

    public OneToManyCollectionInstance(CollectionMapping mapping)
      : base(mapping)
    {
      this.nextBool = true;
      this.mapping = mapping;
    }

    IOneToManyInspector IOneToManyCollectionInspector.Relationship
    {
      get => (IOneToManyInspector) this.Relationship;
    }

    IManyToOneInspector IOneToManyCollectionInspector.OtherSide
    {
      get => (IManyToOneInspector) this.OtherSide;
    }

    public IManyToOneInstance OtherSide
    {
      get
      {
        return !(this.mapping.OtherSide is ManyToOneMapping otherSide) ? (IManyToOneInstance) null : (IManyToOneInstance) new ManyToOneInstance(otherSide);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IOneToManyCollectionInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IOneToManyCollectionInstance) this;
      }
    }

    public IOneToManyInstance Relationship
    {
      get
      {
        return (IOneToManyInstance) new OneToManyInstance((OneToManyMapping) this.mapping.Relationship);
      }
    }
  }
}
