// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Prebuilt.BuiltSuffixForeignKeyConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
  public class BuiltSuffixForeignKeyConvention : ForeignKeyConvention
  {
    private readonly string suffix;

    public BuiltSuffixForeignKeyConvention(string suffix) => this.suffix = suffix;

    protected override string GetKeyName(Member property, Type type)
    {
      return (property != (Member) null ? property.Name : type.Name) + this.suffix;
    }
  }
}
