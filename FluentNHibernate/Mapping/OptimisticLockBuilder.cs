// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.OptimisticLockBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class OptimisticLockBuilder
  {
    private readonly Action<string> setter;

    protected OptimisticLockBuilder(Action<string> setter) => this.setter = setter;

    public void None() => this.setter("none");

    public void Version() => this.setter("version");

    public void Dirty() => this.setter("dirty");

    public void All() => this.setter("all");
  }
}
