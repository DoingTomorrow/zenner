// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.InlineOverride
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class InlineOverride
  {
    private readonly Type type;
    private readonly Action<object> action;

    public InlineOverride(Type type, Action<object> action)
    {
      this.type = type;
      this.action = action;
    }

    public bool CanOverride(Type otherType)
    {
      return this.type == otherType || otherType.IsSubclassOf(this.type);
    }

    public void Apply(object mapping) => this.action(mapping);
  }
}
