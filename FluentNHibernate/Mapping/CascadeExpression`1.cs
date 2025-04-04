// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CascadeExpression`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class CascadeExpression<TParent>
  {
    private readonly TParent parent;
    private readonly Action<string> setter;

    public CascadeExpression(TParent parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public TParent All()
    {
      this.setter("all");
      return this.parent;
    }

    public TParent None()
    {
      this.setter("none");
      return this.parent;
    }

    public TParent SaveUpdate()
    {
      this.setter("save-update");
      return this.parent;
    }

    public TParent Delete()
    {
      this.setter("delete");
      return this.parent;
    }

    public TParent Merge()
    {
      this.setter("merge");
      return this.parent;
    }
  }
}
