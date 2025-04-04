// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.ForeignKey
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Helpers.Prebuilt;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public static class ForeignKey
  {
    public static ForeignKeyConvention EndsWith(string suffix)
    {
      return (ForeignKeyConvention) new BuiltSuffixForeignKeyConvention(suffix);
    }

    public static ForeignKeyConvention Format(Func<Member, Type, string> format)
    {
      return (ForeignKeyConvention) new BuiltFuncForeignKeyConvention(format);
    }
  }
}
