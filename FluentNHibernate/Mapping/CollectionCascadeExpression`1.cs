// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CollectionCascadeExpression`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class CollectionCascadeExpression<TParent> : CascadeExpression<TParent>
  {
    private readonly TParent parent;
    private readonly Action<string> setter;

    public CollectionCascadeExpression(TParent parent, Action<string> setter)
      : base(parent, setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public TParent AllDeleteOrphan()
    {
      this.setter("all-delete-orphan");
      return this.parent;
    }

    public TParent DeleteOrphan()
    {
      this.setter("delete-orphan");
      return this.parent;
    }
  }
}
