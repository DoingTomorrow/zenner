// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.UniqueIdentification
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class UniqueIdentification
  {
    public int ShortKey => Util.GetStableHash(this.Key);

    public string Key
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this.Manufacturer);
        stringBuilder.Append(this.Version);
        stringBuilder.Append(this.Medium);
        foreach (string parameter in this.ParameterList)
          stringBuilder.Append(parameter);
        return stringBuilder.ToString();
      }
    }

    public string Manufacturer { get; set; }

    public string Version { get; set; }

    public string Medium { get; set; }

    public List<string> ParameterList { get; set; }

    public string ParameterListStringWithSeparator
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (string parameter in this.ParameterList)
        {
          ++num;
          if (stringBuilder.Length > 0)
            stringBuilder.Append("\r\n");
          stringBuilder.Append(string.Format("[{0:00}]  ", (object) num));
          stringBuilder.Append(parameter);
        }
        return stringBuilder.ToString();
      }
    }

    public string ParameterListString
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        foreach (string parameter in this.ParameterList)
        {
          ++num;
          if (stringBuilder.Length > 0)
            stringBuilder.Append(" ");
          stringBuilder.Append(parameter);
        }
        return stringBuilder.ToString();
      }
    }

    public TranslationRuleCollection TranslationRules
    {
      get
      {
        return TranslationRulesManager.Instance.LoadRules(this.Manufacturer, this.Medium, new int?(Convert.ToInt32(this.Version)), new int?(Convert.ToInt32(this.Version)));
      }
    }

    public UniqueIdentification() => this.ParameterList = new List<string>();

    public override string ToString() => this.Key;
  }
}
