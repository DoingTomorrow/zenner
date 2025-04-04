// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.Naming
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public static class Naming
  {
    private static readonly List<string> Invalids = new List<string>()
    {
      "camelcase-m",
      "camelcase-m-underscore",
      "lowercase-m",
      "lowercase-m-underscore"
    };

    public static NamingStrategy Determine(string name)
    {
      string strategy = Naming.GuessNamingStrategy(name);
      return Naming.Invalids.Contains(strategy) ? NamingStrategy.Unknown : NamingStrategy.FromString(strategy);
    }

    private static string GuessNamingStrategy(string name)
    {
      if (name.StartsWith("_"))
        return Naming.GuessNamingStrategy(name.Substring(1)) + "-underscore";
      if (name.StartsWith("m_"))
        return Naming.GuessNamingStrategy(name.Substring(2)) + "-m-underscore";
      if (name.StartsWith("m") && char.IsUpper(name[1]))
        return Naming.GuessNamingStrategy(name.Substring(1)) + "-m";
      if (name.All<char>(new Func<char, bool>(char.IsLower)))
        return "lowercase";
      return char.IsUpper(name[0]) ? "pascalcase" : "camelcase";
    }
  }
}
