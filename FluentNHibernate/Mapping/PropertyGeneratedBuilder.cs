// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.PropertyGeneratedBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class PropertyGeneratedBuilder
  {
    private readonly PropertyPart parent;
    private readonly Action<string> setter;

    public PropertyGeneratedBuilder(PropertyPart parent, Action<string> setter)
    {
      this.parent = parent;
      this.setter = setter;
    }

    public PropertyPart Never()
    {
      this.setter("never");
      return this.parent;
    }

    public PropertyPart Insert()
    {
      this.setter("insert");
      return this.parent;
    }

    public PropertyPart Always()
    {
      this.setter("always");
      return this.parent;
    }
  }
}
