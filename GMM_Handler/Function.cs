// Decompiled with JetBrains decompiler
// Type: GMM_Handler.Function
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.Collections;

#nullable disable
namespace GMM_Handler
{
  internal class Function
  {
    internal ushort Number;
    internal string Name;
    internal string AliasName;
    internal int Version;
    internal int FirmwareVersionMin;
    internal int FirmwareVersionMax;
    internal FunctionLocalisableType Localisable;
    internal LoggerTypes LoggerType;
    internal ArrayList ParameterList = new ArrayList();
    internal SortedList ParameterListByName;
    internal SortedList EpromCodeBlocksByName;
    internal ArrayList RuntimeCodeBlockList = new ArrayList();
    internal ArrayList MenuList = new ArrayList();
    internal SortedList MenuListByName;
    internal int ColumnNumber;
    internal int RowNumber;
    internal string MeterResourcesList;
    internal string[] SuppliedResources;
    internal string[] NeadedResources;
    internal string[] NotSupportedResources;
    internal string FullName;
    internal string ShortInfo;
    internal string Description;
    internal string Group;
    internal string Symbolname;
    internal int UserAccessRight;
    internal string WorkingAccessRights;

    internal Function Clone(Meter MyMeter)
    {
      Function function = new Function();
      function.Number = this.Number;
      function.Name = this.Name;
      function.AliasName = this.AliasName;
      function.Version = this.Version;
      function.FirmwareVersionMin = this.FirmwareVersionMin;
      function.FirmwareVersionMax = this.FirmwareVersionMax;
      function.Localisable = this.Localisable;
      function.LoggerType = this.LoggerType;
      function.MeterResourcesList = this.MeterResourcesList;
      function.NeadedResources = this.NeadedResources;
      function.SuppliedResources = this.SuppliedResources;
      function.NotSupportedResources = this.NotSupportedResources;
      function.FullName = this.FullName;
      function.ShortInfo = this.ShortInfo;
      function.Description = this.Description;
      function.Group = this.Group;
      function.Symbolname = this.Symbolname;
      function.UserAccessRight = this.UserAccessRight;
      function.WorkingAccessRights = this.WorkingAccessRights;
      foreach (Parameter parameter in this.ParameterList)
        function.ParameterList.Add((object) parameter.Clone());
      foreach (CodeBlock runtimeCodeBlock in this.RuntimeCodeBlockList)
        function.RuntimeCodeBlockList.Add((object) runtimeCodeBlock.Clone(MyMeter, function.ParameterList));
      foreach (MenuItem menu in this.MenuList)
        function.MenuList.Add((object) menu.Clone());
      return function;
    }
  }
}
