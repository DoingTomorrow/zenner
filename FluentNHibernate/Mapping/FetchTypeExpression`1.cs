// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.FetchTypeExpression`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class FetchTypeExpression<TParent>
  {
    private readonly TParent parent;
    private readonly Action<string> setter;

    public FetchTypeExpression(TParent parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public TParent Join()
    {
      this.setter("join");
      return this.parent;
    }

    public TParent Select()
    {
      this.setter("select");
      return this.parent;
    }

    public TParent Subselect()
    {
      this.setter("subselect");
      return this.parent;
    }
  }
}
