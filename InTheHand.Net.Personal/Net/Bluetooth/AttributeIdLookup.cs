// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.AttributeIdLookup
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Reflection;

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public static class AttributeIdLookup
  {
    public static string GetName(ServiceAttributeId id, Type[] attributeIdDefiningClasses)
    {
      return AttributeIdLookup.GetName(id, attributeIdDefiningClasses, new LanguageBaseItem[0], out LanguageBaseItem _);
    }

    public static string GetName(
      ServiceAttributeId id,
      Type[] attributeIdDefiningClasses,
      LanguageBaseItem[] langBaseList,
      out LanguageBaseItem applicableLangBase)
    {
      if (attributeIdDefiningClasses == null)
        throw new ArgumentNullException(nameof (attributeIdDefiningClasses));
      if (langBaseList == null)
        throw new ArgumentNullException(nameof (langBaseList));
      foreach (Type attributeIdDefiningClass in attributeIdDefiningClasses)
      {
        foreach (FieldInfo field in attributeIdDefiningClass.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
          if (field.FieldType == typeof (ServiceAttributeId))
          {
            if (field.GetCustomAttributes(typeof (StringWithLanguageBaseAttribute), false).Length != 0)
            {
              string matchesMultiLang = AttributeIdLookup._GetNameIfMatchesMultiLang(id, field, langBaseList, out applicableLangBase);
              if (matchesMultiLang != null)
                return matchesMultiLang;
            }
            else
            {
              string nameIfMatches = AttributeIdLookup._GetNameIfMatches(id, field);
              if (nameIfMatches != null)
              {
                applicableLangBase = (LanguageBaseItem) null;
                return nameIfMatches;
              }
            }
          }
        }
      }
      applicableLangBase = (LanguageBaseItem) null;
      return (string) null;
    }

    private static string _GetNameIfMatchesMultiLang(
      ServiceAttributeId id,
      FieldInfo curField,
      LanguageBaseItem[] langBaseList,
      out LanguageBaseItem applicableLangBase)
    {
      foreach (LanguageBaseItem langBase in langBaseList)
      {
        ServiceAttributeId attributeIdBase = langBase.AttributeIdBase;
        string nameIfMatches = AttributeIdLookup._GetNameIfMatches((ServiceAttributeId) (id - attributeIdBase), curField);
        if (nameIfMatches != null)
        {
          applicableLangBase = langBase;
          return nameIfMatches;
        }
      }
      applicableLangBase = (LanguageBaseItem) null;
      return (string) null;
    }

    private static string _GetNameIfMatches(ServiceAttributeId id, FieldInfo curField)
    {
      return (ServiceAttributeId) curField.GetRawConstantValue() == id ? curField.Name : (string) null;
    }
  }
}
