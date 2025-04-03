// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.CriteriaAliasesInfo
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class CriteriaAliasesInfo
  {
    private Dictionary<string, string> propertyAliases = new Dictionary<string, string>();

    public string GetPropertyForAlias(string alias)
    {
      return this.propertyAliases.ContainsKey(alias) ? this.propertyAliases[alias] : (string) null;
    }

    public CriteriaAliasesInfo Add(string alias, string propertyPath)
    {
      if (this.propertyAliases.ContainsKey(alias))
        throw new ArgumentException("The aliased key already exists.");
      this.propertyAliases.Add(alias, propertyPath);
      return this;
    }
  }
}
