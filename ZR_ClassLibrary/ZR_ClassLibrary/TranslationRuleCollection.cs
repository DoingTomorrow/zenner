// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TranslationRuleCollection
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  [XmlRoot("Rules")]
  [Serializable]
  public sealed class TranslationRuleCollection : List<TranslationRule>
  {
    internal TranslationRuleCollection GetRules(
      string manufacturer,
      string medium,
      int? versionMin,
      int? versionMax)
    {
      TranslationRuleCollection rules = new TranslationRuleCollection();
      medium = TranslationRule.CorrectMedium(medium);
      for (int index = 0; index < this.Count; ++index)
      {
        TranslationRule translationRule = this[index];
        if ((!(manufacturer != string.Empty) || !(translationRule.Manufacturer != manufacturer) || !(manufacturer != "DEFAULT")) && (string.IsNullOrEmpty(medium) || !(medium != "UNKNOWN") || !(translationRule.Medium != medium)))
        {
          int num1 = versionMin.HasValue ? versionMin.Value : 0;
          int num2 = versionMax.HasValue ? versionMax.Value : (int) byte.MaxValue;
          if (translationRule.VersionMin >= num1 && translationRule.VersionMin <= num2 && translationRule.VersionMax <= num2 && translationRule.VersionMax >= num1)
            rules.Add(translationRule);
          else if (num1 >= translationRule.VersionMin && num2 >= translationRule.VersionMin && num2 <= translationRule.VersionMax && num1 <= translationRule.VersionMax)
            rules.Add(translationRule);
        }
      }
      return rules;
    }

    internal TranslationRuleCollection GetSubDeviceRules(
      string manufacturer,
      int version,
      int? subDeviceIndex)
    {
      TranslationRuleCollection subDeviceRules = new TranslationRuleCollection();
      foreach (TranslationRule translationRule in (List<TranslationRule>) this)
      {
        if (translationRule.SubDeviceIndex > 0 && translationRule.Manufacturer == manufacturer && version >= translationRule.VersionMin && version <= translationRule.VersionMax && (!subDeviceIndex.HasValue || subDeviceIndex.Value == translationRule.SubDeviceIndex))
          subDeviceRules.Add(translationRule);
      }
      return subDeviceRules;
    }

    private bool ContainsRule(
      string manufacturer,
      string medium,
      int versionMin,
      int versionMax,
      string zdfValue,
      double multiplier,
      SpecialTranslation specialTranslation)
    {
      foreach (TranslationRule translationRule in (List<TranslationRule>) this)
      {
        if (translationRule.Manufacturer == manufacturer && translationRule.Medium == medium && translationRule.MBusZDF == zdfValue && translationRule.Multiplier == multiplier && translationRule.SpecialTranslation == specialTranslation && translationRule.VersionMin >= versionMin && translationRule.VersionMax <= versionMax)
          return true;
      }
      return false;
    }
  }
}
