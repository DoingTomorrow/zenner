// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.GeneratedInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class GeneratedInstance : IGeneratedInstance
  {
    private readonly Action<string> setter;

    public GeneratedInstance(Action<string> setter) => this.setter = setter;

    public void Never() => this.setter("never");

    public void Insert() => this.setter("insert");

    public void Always() => this.setter("always");
  }
}
