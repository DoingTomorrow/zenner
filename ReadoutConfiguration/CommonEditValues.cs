// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.CommonEditValues
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System.Collections.Generic;

#nullable disable
namespace ReadoutConfiguration
{
  public class CommonEditValues
  {
    public SortedList<string, List<int>> valueListAndUsing = new SortedList<string, List<int>>();
    public SortedList<string, List<int>> editByListAndUsing = new SortedList<string, List<int>>();
    private string valueName;

    internal CommonEditValues(string ValueName) => this.valueName = ValueName;

    public bool IsSettingsIDRegisterd(int settingsID)
    {
      foreach (List<int> intList in (IEnumerable<List<int>>) this.valueListAndUsing.Values)
      {
        foreach (int num in intList)
        {
          if (num == settingsID)
            return true;
        }
      }
      return false;
    }

    internal string ValueName => this.valueName;

    internal void AddValue(string theValue, int settingsID)
    {
      string key = "null";
      if (theValue != null)
      {
        key = theValue.Trim();
        if (key.Length == 0)
          key = "\"\"";
      }
      List<int> intList;
      if (!this.valueListAndUsing.ContainsKey(key))
      {
        intList = new List<int>();
        this.valueListAndUsing.Add(key, intList);
      }
      else
        intList = this.valueListAndUsing[key];
      intList.Add(settingsID);
    }

    internal void AddEditBy(string theEditByValue, int settingsID)
    {
      string key = "null";
      if (theEditByValue != null)
      {
        key = theEditByValue.Trim();
        if (key.Length == 0)
          key = "\"\"";
      }
      List<int> intList;
      if (!this.editByListAndUsing.ContainsKey(key))
      {
        intList = new List<int>();
        this.editByListAndUsing.Add(key, intList);
      }
      else
        intList = this.editByListAndUsing[key];
      intList.Add(settingsID);
    }

    internal void EscapeEdit(List<int> settingIds)
    {
      foreach (int settingId in settingIds)
      {
        if (!this.IsSettingsIDRegisterd(settingId))
          this.AddValue("NotAvailable", settingId);
      }
      foreach (KeyValuePair<string, List<int>> keyValuePair in this.valueListAndUsing)
        keyValuePair.Value.Sort();
      foreach (KeyValuePair<string, List<int>> keyValuePair in this.editByListAndUsing)
        keyValuePair.Value.Sort();
    }
  }
}
