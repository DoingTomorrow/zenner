// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.ConnectionProfileFilter
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public class ConnectionProfileFilter
  {
    public int FilterID { get; set; }

    public string Name { get; set; }

    public List<ConnectionParameterGroup> FilterGroups { get; set; }

    public SortedList<int, ConnectionParameterGroup> SubGroups { get; set; }

    public ConnectionProfileFilter(int filterID, string name)
    {
      this.FilterID = filterID;
      this.Name = name;
      this.FilterGroups = new List<ConnectionParameterGroup>();
    }

    public bool IsSelectedByFilter(
      SortedList<ConnectionProfileParameter, string> combindeProfileParameters,
      SortedList<string, string> SetupParameterList)
    {
      if (this.FilterGroups.Count == 0)
        return true;
      foreach (ConnectionParameterGroup filterGroup in this.FilterGroups)
      {
        if (!filterGroup.GetGroupResult(combindeProfileParameters, SetupParameterList))
          return false;
      }
      return true;
    }

    public bool IsFilterExpliciteDesignedFor(
      SortedList<ConnectionProfileParameter, string> checkParameters)
    {
      if (this.FilterGroups.Count == 0)
        return false;
      bool flag1 = false;
      foreach (ConnectionParameterGroup filterGroup in this.FilterGroups)
      {
        if (filterGroup.GroupFunction == ConnectionProfileFilterGroupFunctions.AND)
        {
          flag1 = true;
          foreach (KeyValuePair<ConnectionProfileParameter, string> checkParameter in checkParameters)
          {
            bool flag2 = false;
            foreach (ConnectionProfileParameterPair parameter in filterGroup.Parameters)
            {
              if (checkParameter.Key == parameter.ParameterName && checkParameter.Value == parameter.ParameterValue)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
              return false;
          }
        }
      }
      return flag1;
    }

    public override string ToString()
    {
      StringBuilder r = new StringBuilder();
      this.AddGroupData((IList<ConnectionParameterGroup>) this.FilterGroups, r);
      if (this.SubGroups != null && this.SubGroups.Count > 0)
      {
        r.AppendLine();
        r.AppendLine(" --- Sub groups --- ");
        this.AddGroupData(this.SubGroups.Values, r);
      }
      return r.ToString();
    }

    private void AddGroupData(IList<ConnectionParameterGroup> filterGroups, StringBuilder r)
    {
      foreach (ConnectionParameterGroup filterGroup in (IEnumerable<ConnectionParameterGroup>) filterGroups)
      {
        if (r.Length > 0)
          r.AppendLine();
        r.AppendLine("FilterGroup[" + filterGroup.GroupNumber.ToString() + "]: " + filterGroup.GroupFunction.ToString());
        foreach (ConnectionProfileParameterPair parameter in filterGroup.Parameters)
        {
          r.Append("   " + parameter.ParameterName.ToString());
          if (parameter.ParameterValue == null)
            r.AppendLine();
          else
            r.AppendLine(" = " + parameter.ParameterValue);
        }
      }
    }

    public static ConnectionProfileFilter CreateHandlerFilter(string handlerName)
    {
      ConnectionProfileFilter handlerFilter = new ConnectionProfileFilter(-1, handlerName + "_Filter");
      ConnectionParameterGroup connectionParameterGroup = new ConnectionParameterGroup(1);
      handlerFilter.FilterGroups.Add(connectionParameterGroup);
      connectionParameterGroup.GroupFunction = ConnectionProfileFilterGroupFunctions.AND;
      ConnectionProfileParameterPair profileParameterPair = new ConnectionProfileParameterPair(ConnectionProfileParameter.Handler, handlerName);
      connectionParameterGroup.Parameters.Add(profileParameterPair);
      return handlerFilter;
    }
  }
}
