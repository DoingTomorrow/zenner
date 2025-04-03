// Decompiled with JetBrains decompiler
// Type: HandlerLib.ConfigurationParameterListSupport
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class ConfigurationParameterListSupport
  {
    private SortedList<OverrideID, ConfigurationParameter> InitialParameterList;
    private HashSet<OverrideID> WorkedParameters;

    public ConfigurationParameterListSupport(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      this.InitialParameterList = parameterList;
      this.WorkedParameters = new HashSet<OverrideID>();
    }

    public ConfigurationParameter GetWorkParameterFromList(OverrideID overrideID)
    {
      int index = this.InitialParameterList.IndexOfKey(overrideID);
      if (index < 0)
        return (ConfigurationParameter) null;
      if (this.WorkedParameters.Contains(overrideID))
        throw new Exception("Second work of ConfigurationParameter: " + overrideID.ToString());
      this.WorkedParameters.Add(overrideID);
      return this.InitialParameterList.Values[index];
    }

    public void CheckAllParametersWorked()
    {
      if (this.WorkedParameters.Count != this.InitialParameterList.Count)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Not worked ConfigurationParameter: ");
        bool flag = true;
        foreach (OverrideID key in (IEnumerable<OverrideID>) this.InitialParameterList.Keys)
        {
          if (flag)
            flag = false;
          else
            stringBuilder.Append(",");
          if (!this.WorkedParameters.Contains(key))
            stringBuilder.Append(key.ToString());
        }
        throw new Exception(stringBuilder.ToString());
      }
    }
  }
}
