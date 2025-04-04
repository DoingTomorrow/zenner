// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.PolymorphismBuilder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class PolymorphismBuilder<T>
  {
    private readonly T parent;
    private readonly Action<string> setter;

    public PolymorphismBuilder(T parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public T Implicit()
    {
      this.setter("implicit");
      return this.parent;
    }

    public T Explicit()
    {
      this.setter("explicit");
      return this.parent;
    }
  }
}
