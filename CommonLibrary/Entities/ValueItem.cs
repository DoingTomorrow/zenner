// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ValueItem
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public sealed class ValueItem
  {
    public string Value { get; private set; }

    public Dictionary<AdditionalInfoKey, string> AdditionalInfo { get; set; }

    public string Info
    {
      get
      {
        if (this.AdditionalInfo == null || this.AdditionalInfo.Count == 0)
          return string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<AdditionalInfoKey, string> keyValuePair in this.AdditionalInfo)
          stringBuilder.Append((object) keyValuePair.Key).Append(": ").Append(keyValuePair.Value).Append(", ");
        return stringBuilder.ToString().TrimEnd(',', ' ');
      }
    }

    public ValueItem(string value) => this.Value = value;

    public void AddAdditionalInfo(AdditionalInfoKey key, string value)
    {
      if (this.AdditionalInfo == null)
        this.AdditionalInfo = new Dictionary<AdditionalInfoKey, string>();
      this.AdditionalInfo.Add(key, value);
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Value))
        return base.ToString();
      if (this.AdditionalInfo == null || this.AdditionalInfo.Count == 0)
        return this.Value;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append(this.Value).Append(" {");
      foreach (KeyValuePair<AdditionalInfoKey, string> keyValuePair in this.AdditionalInfo)
        stringBuilder1.Append(keyValuePair.Value).Append(", ");
      StringBuilder stringBuilder2 = stringBuilder1.Remove(stringBuilder1.Length - 2, 2);
      stringBuilder2.Append("}");
      return stringBuilder2.ToString();
    }
  }
}
