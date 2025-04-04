// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.NotFoundExpression`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class NotFoundExpression<TParent>
  {
    private readonly TParent parent;
    private readonly Action<string> setter;

    public NotFoundExpression(TParent parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public TParent Ignore()
    {
      this.setter("ignore");
      return this.parent;
    }

    public TParent Exception()
    {
      this.setter("exception");
      return this.parent;
    }
  }
}
