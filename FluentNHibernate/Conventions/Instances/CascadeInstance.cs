// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.CascadeInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class CascadeInstance : ICascadeInstance
  {
    private readonly Action<string> setter;

    public CascadeInstance(Action<string> setter) => this.setter = setter;

    public void All() => this.setter("all");

    public void None() => this.setter("none");

    public void SaveUpdate() => this.setter("save-update");

    public void Delete() => this.setter("delete");

    public void Merge() => this.setter("merge");
  }
}
