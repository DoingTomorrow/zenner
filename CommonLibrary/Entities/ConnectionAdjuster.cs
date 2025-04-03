// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionAdjuster
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public class ConnectionAdjuster
  {
    public int ConnectionProfileID { get; private set; }

    public string Name { get; private set; }

    public List<ChangeableParameter> SetupParameters { get; private set; }

    public ConnectionAdjuster(
      int connectionProfileID,
      string name,
      List<ChangeableParameter> setupParameters)
    {
      setupParameters.Sort((IComparer<ChangeableParameter>) new ChangeableParameterComparer());
      this.ConnectionProfileID = connectionProfileID;
      this.Name = name;
      this.SetupParameters = setupParameters;
    }

    public ConfigList GetMergedConfiguration(ConnectionProfile theProfile)
    {
      SortedList<string, string> configList = new SortedList<string, string>();
      configList.Add(ParameterKey.ConnectionProfileID.ToString(), this.ConnectionProfileID.ToString());
      foreach (KeyValuePair<string, string> setupParameter in theProfile.ConnectionSettings.SetupParameterList)
      {
        KeyValuePair<string, string> defaultParam = setupParameter;
        ChangeableParameter changeableParameter = this.SetupParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == defaultParam.Key));
        if (changeableParameter != null)
          configList.Add(changeableParameter.Key, changeableParameter.Value);
        else
          configList.Add(defaultParam.Key, defaultParam.Value);
      }
      return new ConfigList(configList);
    }
  }
}
