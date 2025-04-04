// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.CollectionCascadeInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class CollectionCascadeInstance : 
    CascadeInstance,
    ICollectionCascadeInstance,
    ICascadeInstance
  {
    private readonly Action<string> setter;

    public CollectionCascadeInstance(Action<string> setter)
      : base(setter)
    {
      this.setter = setter;
    }

    public void AllDeleteOrphan() => this.setter("all-delete-orphan");

    public void DeleteOrphan() => this.setter("delete-orphan");
  }
}
