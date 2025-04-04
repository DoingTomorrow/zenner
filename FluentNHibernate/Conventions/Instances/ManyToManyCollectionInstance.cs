// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ManyToManyCollectionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ManyToManyCollectionInstance : 
    CollectionInstance,
    IManyToManyCollectionInstance,
    IManyToManyCollectionInspector,
    ICollectionInstance,
    ICollectionInspector,
    IInspector
  {
    private readonly CollectionMapping mapping;

    public ManyToManyCollectionInstance(CollectionMapping mapping)
      : base(mapping)
    {
      this.nextBool = true;
      this.mapping = mapping;
    }

    IManyToManyInspector IManyToManyCollectionInspector.Relationship
    {
      get => (IManyToManyInspector) this.Relationship;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IManyToManyCollectionInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IManyToManyCollectionInstance) this;
      }
    }

    public IManyToManyCollectionInstance OtherSide
    {
      get
      {
        return !(this.mapping.OtherSide is CollectionMapping otherSide) ? (IManyToManyCollectionInstance) null : (IManyToManyCollectionInstance) new ManyToManyCollectionInstance(otherSide);
      }
    }

    public IManyToManyInstance Relationship
    {
      get
      {
        return (IManyToManyInstance) new ManyToManyInstance((ManyToManyMapping) this.mapping.Relationship);
      }
    }

    public new Type ChildType => this.mapping.ChildType;

    IManyToManyCollectionInspector IManyToManyCollectionInspector.OtherSide
    {
      get => (IManyToManyCollectionInspector) this.OtherSide;
    }
  }
}
