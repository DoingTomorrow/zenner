// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Prebuilt.BuiltFuncForeignKeyConvention
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
  public class BuiltFuncForeignKeyConvention : ForeignKeyConvention
  {
    private readonly Func<Member, Type, string> format;

    public BuiltFuncForeignKeyConvention(Func<Member, Type, string> format) => this.format = format;

    protected override string GetKeyName(Member property, Type type) => this.format(property, type);
  }
}
