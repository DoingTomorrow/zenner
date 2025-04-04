// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.ForeignKeyConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public abstract class ForeignKeyConvention : 
    IReferenceConvention,
    IConvention<IManyToOneInspector, IManyToOneInstance>,
    IHasManyToManyConvention,
    IConvention<IManyToManyCollectionInspector, IManyToManyCollectionInstance>,
    IJoinedSubclassConvention,
    IConvention<IJoinedSubclassInspector, IJoinedSubclassInstance>,
    IJoinConvention,
    IConvention<IJoinInspector, IJoinInstance>,
    ICollectionConvention,
    IConvention<ICollectionInspector, ICollectionInstance>,
    IConvention
  {
    protected abstract string GetKeyName(Member property, Type type);

    public void Apply(IManyToOneInstance instance)
    {
      string keyName = this.GetKeyName(instance.Property, instance.Class.GetUnderlyingSystemType());
      instance.Column(keyName);
    }

    public void Apply(IManyToManyCollectionInstance instance)
    {
      string keyName1 = this.GetKeyName((Member) null, instance.EntityType);
      string keyName2 = this.GetKeyName((Member) null, instance.ChildType);
      instance.Key.Column(keyName1);
      instance.Relationship.Column(keyName2);
    }

    public void Apply(IJoinedSubclassInstance instance)
    {
      string keyName = this.GetKeyName((Member) null, instance.Type.BaseType);
      instance.Key.Column(keyName);
    }

    public void Apply(IJoinInstance instance)
    {
      string keyName = this.GetKeyName((Member) null, instance.EntityType);
      instance.Key.Column(keyName);
    }

    public void Apply(ICollectionInstance instance)
    {
      string keyName = this.GetKeyName((Member) null, instance.EntityType);
      instance.Key.Column(keyName);
    }
  }
}
