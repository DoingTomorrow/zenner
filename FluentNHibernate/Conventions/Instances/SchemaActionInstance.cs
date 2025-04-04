// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.SchemaActionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class SchemaActionInstance : ISchemaActionInstance
  {
    private readonly Action<string> setter;

    public SchemaActionInstance(Action<string> setter) => this.setter = setter;

    public void None() => this.setter("none");

    public void All() => this.setter("all");

    public void Drop() => this.setter("drop");

    public void Update() => this.setter("update");

    public void Validate() => this.setter("validate");

    public void Export() => this.setter("export");

    public void Custom(string customValue) => this.setter(customValue);
  }
}
