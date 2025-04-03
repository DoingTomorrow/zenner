// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ChangeableParameter
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
  public class ChangeableParameter
  {
    public string Key { get; set; }

    public string Value { get; set; }

    public Type Type { get; set; }

    public string KeyTranslated { get; set; }

    public string KeyTranslatedDescription { get; set; }

    public List<ValueItem> AvailableValues { get; set; }

    public object ValueMin { get; set; }

    public object ValueMax { get; set; }

    public ChangeableParameterUsings ParameterUsing { get; set; }

    public HashSet<ConfigurationParameterEnvironment> ParameterEnvironment { get; set; }

    public bool UpdateAvailableValues()
    {
      if (this.UpdateAvailableValuesHandler == null)
        return false;
      this.AvailableValues = this.UpdateAvailableValuesHandler();
      return true;
    }

    public ChangeableParameter.UpdateAvailableValuesDelegate UpdateAvailableValuesHandler { get; set; }

    public ChangeableParameter DeepCopy()
    {
      return new ChangeableParameter()
      {
        Key = this.Key,
        Value = this.Value,
        Type = this.Type,
        KeyTranslated = this.KeyTranslated,
        KeyTranslatedDescription = this.KeyTranslatedDescription,
        ValueMin = this.ValueMin,
        ValueMax = this.ValueMax,
        AvailableValues = this.AvailableValues,
        ParameterUsing = this.ParameterUsing,
        ParameterEnvironment = this.ParameterEnvironment,
        UpdateAvailableValuesHandler = this.UpdateAvailableValuesHandler
      };
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Key))
        return base.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Key).Append(" ");
      if (this.Value != null)
        stringBuilder.Append("= ").Append(this.Value).Append(", ");
      if (!string.IsNullOrEmpty(this.KeyTranslated))
        stringBuilder.Append("'").Append(this.KeyTranslated).Append("', ");
      if (this.Type != (Type) null)
        stringBuilder.Append(this.Type.ToString()).Append(", ");
      if (this.ValueMin != null)
        stringBuilder.Append("Min: ").Append(this.ValueMin.ToString()).Append(", ");
      if (this.ValueMax != null)
        stringBuilder.Append("Max: ").Append(this.ValueMax.ToString()).Append(", ");
      if (this.AvailableValues != null)
      {
        stringBuilder.Append("Possible values {");
        foreach (ValueItem availableValue in this.AvailableValues)
          stringBuilder.Append((object) availableValue).Append(" ");
        stringBuilder.Append("}");
      }
      return stringBuilder.ToString();
    }

    public static bool TryGet<T>(List<ChangeableParameter> list, string name, out T result)
    {
      if (list == null || list.Count == 0)
        throw new Exception(nameof (list));
      if (string.IsNullOrEmpty(name))
        throw new Exception(nameof (name));
      result = default (T);
      ChangeableParameter changeableParameter = list.Find((Predicate<ChangeableParameter>) (x => x.Key == name));
      if (changeableParameter == null || string.IsNullOrEmpty(changeableParameter.Value))
        return false;
      result = (T) Convert.ChangeType((object) changeableParameter.Value, typeof (T));
      return true;
    }

    public static void Set(List<ChangeableParameter> list, string name, object value)
    {
      if (list == null || list.Count == 0)
        throw new Exception(nameof (list));
      if (string.IsNullOrEmpty(name))
        throw new Exception(nameof (name));
      ChangeableParameter changeableParameter = list.Find((Predicate<ChangeableParameter>) (x => x.Key == name));
      if (changeableParameter == null)
        throw new Exception("The changeable parameter '" + name + "' does not exist!");
      if (value == null)
        changeableParameter.Value = string.Empty;
      else
        changeableParameter.Value = value.ToString();
    }

    public delegate List<ValueItem> UpdateAvailableValuesDelegate();
  }
}
