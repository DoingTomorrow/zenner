// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCaseNamingStrategy
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class CamelCaseNamingStrategy : NamingStrategy
  {
    public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
    {
      this.ProcessDictionaryKeys = processDictionaryKeys;
      this.OverrideSpecifiedNames = overrideSpecifiedNames;
    }

    public CamelCaseNamingStrategy()
    {
    }

    protected override string ResolvePropertyName(string name) => StringUtils.ToCamelCase(name);
  }
}
