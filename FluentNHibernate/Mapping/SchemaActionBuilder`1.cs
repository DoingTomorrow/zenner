// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.SchemaActionBuilder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class SchemaActionBuilder<T>
  {
    private readonly T parent;
    private readonly Action<string> setter;

    public SchemaActionBuilder(T parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public T All()
    {
      this.setter("all");
      return this.parent;
    }

    public T None()
    {
      this.setter("none");
      return this.parent;
    }

    public T Drop()
    {
      this.setter("drop");
      return this.parent;
    }

    public T Update()
    {
      this.setter("update");
      return this.parent;
    }

    public T Export()
    {
      this.setter("export");
      return this.parent;
    }

    public T Validate()
    {
      this.setter("validate");
      return this.parent;
    }

    public T Custom(string customValue)
    {
      this.setter(customValue);
      return this.parent;
    }
  }
}
