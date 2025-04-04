// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.StringEnumAttribute
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using MSS.Localisation;
using System;

#nullable disable
namespace MSS.Utils.Utils
{
  public class StringEnumAttribute : Attribute
  {
    public StringEnumAttribute(string value)
    {
      string str = CultureResources.GetValue(value);
      this.Value = !string.IsNullOrEmpty(str) ? str : value;
    }

    public string Value { get; set; }
  }
}
