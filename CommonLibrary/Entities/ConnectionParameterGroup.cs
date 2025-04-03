// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionParameterGroup
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public class ConnectionParameterGroup
  {
    public int GroupNumber;
    public ConnectionProfileFilterGroupFunctions GroupFunction;
    public List<ConnectionProfileParameterPair> Parameters = new List<ConnectionProfileParameterPair>();
    public List<ConnectionParameterGroup> SubGroups;

    public ConnectionParameterGroup(int groupNumber) => this.GroupNumber = groupNumber;

    internal bool GetGroupResult(
      SortedList<ConnectionProfileParameter, string> combindeProfileParameters,
      SortedList<string, string> SetupParameterList)
    {
      switch (this.GroupFunction)
      {
        case ConnectionProfileFilterGroupFunctions.OR:
          foreach (ConnectionProfileParameterPair parameter in this.Parameters)
          {
            if (parameter.ParameterName == ConnectionProfileParameter.ConfigParam)
            {
              if (parameter.ParameterValue != null)
              {
                string[] strArray = parameter.ParameterValue.Split('=');
                if (strArray.Length == 2 && SetupParameterList.Keys.Contains(strArray[0]))
                {
                  if (!(strArray[1] == "*"))
                  {
                    if (SetupParameterList[strArray[0]] == strArray[1])
                      goto label_58;
                  }
                  else
                    goto label_58;
                }
              }
            }
            else if (combindeProfileParameters.ContainsKey(parameter.ParameterName))
            {
              if (parameter.ParameterValue == null || !(parameter.ParameterValue == "*"))
              {
                if (combindeProfileParameters[parameter.ParameterName] == parameter.ParameterValue)
                  goto label_58;
              }
              else
                goto label_58;
            }
          }
          if (this.SubGroups != null)
          {
            using (List<ConnectionParameterGroup>.Enumerator enumerator = this.SubGroups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.GetGroupResult(combindeProfileParameters, SetupParameterList))
                  goto label_58;
              }
              break;
            }
          }
          else
            break;
        case ConnectionProfileFilterGroupFunctions.AND:
          foreach (ConnectionProfileParameterPair parameter in this.Parameters)
          {
            if (parameter.ParameterName == ConnectionProfileParameter.ConfigParam)
            {
              if (parameter.ParameterValue != null)
              {
                string[] strArray = parameter.ParameterValue.Split('=');
                if (strArray.Length == 2)
                {
                  if (SetupParameterList.Keys.Contains(strArray[0]))
                  {
                    if (strArray[1] != "*" && strArray[1] != SetupParameterList[strArray[0]])
                      goto label_57;
                  }
                  else
                    goto label_57;
                }
              }
            }
            else if (combindeProfileParameters.ContainsKey(parameter.ParameterName))
            {
              string profileParameter = combindeProfileParameters[parameter.ParameterName];
              if (parameter.ParameterValue == null || !(parameter.ParameterValue == "*"))
              {
                if (profileParameter != parameter.ParameterValue)
                  goto label_57;
              }
            }
            else
              goto label_57;
          }
          if (this.SubGroups != null)
          {
            using (List<ConnectionParameterGroup>.Enumerator enumerator = this.SubGroups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (!enumerator.Current.GetGroupResult(combindeProfileParameters, SetupParameterList))
                  goto label_57;
              }
              goto label_58;
            }
          }
          else
            goto label_58;
        case ConnectionProfileFilterGroupFunctions.NOT:
          foreach (ConnectionProfileParameterPair parameter in this.Parameters)
          {
            if (parameter.ParameterName == ConnectionProfileParameter.ConfigParam)
            {
              if (parameter.ParameterValue != null)
              {
                string[] strArray = parameter.ParameterValue.Split('=');
                if (strArray.Length == 2 && SetupParameterList.Keys.Contains(strArray[0]))
                {
                  if (!(strArray[1] == "*"))
                  {
                    if (SetupParameterList[strArray[0]] == strArray[1])
                      goto label_57;
                  }
                  else
                    goto label_57;
                }
              }
            }
            else if (combindeProfileParameters.ContainsKey(parameter.ParameterName))
            {
              if (parameter.ParameterValue == null || !(parameter.ParameterValue == "*"))
              {
                if (combindeProfileParameters[parameter.ParameterName] == parameter.ParameterValue)
                  goto label_57;
              }
              else
                goto label_57;
            }
          }
          if (this.SubGroups != null)
          {
            using (List<ConnectionParameterGroup>.Enumerator enumerator = this.SubGroups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.GetGroupResult(combindeProfileParameters, SetupParameterList))
                  goto label_57;
              }
              goto label_58;
            }
          }
          else
            goto label_58;
        default:
          throw new Exception("Illegal filter function");
      }
label_57:
      return false;
label_58:
      return true;
    }

    internal bool GetGroupEnabledResult(
      SortedList<ConnectionProfileParameter, string> checkParameters)
    {
      switch (this.GroupFunction)
      {
        case ConnectionProfileFilterGroupFunctions.OR:
        case ConnectionProfileFilterGroupFunctions.AND:
          if (this.SubGroups != null)
          {
            using (List<ConnectionParameterGroup>.Enumerator enumerator = this.SubGroups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.GetGroupEnabledResult(checkParameters))
                  goto label_22;
              }
              break;
            }
          }
          else
            break;
        case ConnectionProfileFilterGroupFunctions.NOT:
          foreach (ConnectionProfileParameterPair parameter in this.Parameters)
          {
            if (checkParameters.ContainsKey(parameter.ParameterName))
            {
              if (parameter.ParameterValue == null || !(parameter.ParameterValue == "*"))
              {
                if (checkParameters[parameter.ParameterName] == parameter.ParameterValue)
                  goto label_21;
              }
              else
                goto label_21;
            }
          }
          if (this.SubGroups != null)
          {
            using (List<ConnectionParameterGroup>.Enumerator enumerator = this.SubGroups.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.GetGroupEnabledResult(checkParameters))
                  goto label_21;
              }
              goto label_22;
            }
          }
          else
            goto label_22;
        default:
          throw new Exception("Illegal filter function");
      }
label_21:
      return false;
label_22:
      return true;
    }

    public override string ToString()
    {
      int num = 0;
      if (this.SubGroups != null)
        num = this.SubGroups.Count;
      return this.GroupNumber.ToString() + ";" + this.GroupFunction.ToString() + "; Params:" + this.Parameters.Count.ToString() + "; SGroups:" + num.ToString();
    }
  }
}
