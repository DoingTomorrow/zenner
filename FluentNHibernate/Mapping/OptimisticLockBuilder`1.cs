// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.OptimisticLockBuilder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class OptimisticLockBuilder<TParent> : OptimisticLockBuilder
  {
    private readonly TParent parent;

    public OptimisticLockBuilder(TParent parent, Action<string> setter)
      : base(setter)
    {
      this.parent = parent;
    }

    public TParent None()
    {
      base.None();
      return this.parent;
    }

    public TParent Version()
    {
      base.Version();
      return this.parent;
    }

    public TParent Dirty()
    {
      base.Dirty();
      return this.parent;
    }

    public TParent All()
    {
      base.All();
      return this.parent;
    }
  }
}
