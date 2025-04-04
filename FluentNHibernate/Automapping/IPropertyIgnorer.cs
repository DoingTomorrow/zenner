// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.IPropertyIgnorer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public interface IPropertyIgnorer
  {
    IPropertyIgnorer IgnoreProperty(string name);

    IPropertyIgnorer IgnoreProperties(string first, params string[] others);

    IPropertyIgnorer IgnoreProperties(Func<Member, bool> predicate);
  }
}
