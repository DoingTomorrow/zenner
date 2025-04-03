// Decompiled with JetBrains decompiler
// Type: GMM_Handler.FunctionPalette
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  internal class FunctionPalette
  {
    private Meter MyMeter;
    private SortedList MyPalette;
    private SortedList MyLinearPalette;
    private string PaletteID_Name = string.Empty;
    private long PaletteID_Version;
    private SortedList SelectedGroup;
    private SortedList SelectedFunction;

    public FunctionPalette(Meter MyMeterIn) => this.MyMeter = MyMeterIn;

    internal FunctionPalette Clone(Meter MyMeterIn)
    {
      FunctionPalette functionPalette = new FunctionPalette(MyMeterIn);
      functionPalette.MyPalette = this.MyPalette;
      functionPalette.PaletteID_Name = this.PaletteID_Name;
      functionPalette.PaletteID_Version = this.PaletteID_Version;
      if (this.MyPalette.Count > 0)
      {
        functionPalette.SelectedGroup = (SortedList) this.MyPalette.GetByIndex(0);
        functionPalette.SelectedFunction = (SortedList) this.SelectedGroup.GetByIndex(0);
      }
      else
      {
        functionPalette.SelectedGroup = (SortedList) null;
        functionPalette.SelectedFunction = (SortedList) null;
      }
      return functionPalette;
    }

    internal bool LoadPalette(bool AllVersions, out SortedList OldFunctionNumbers)
    {
      OldFunctionNumbers = (SortedList) null;
      if (this.PaletteID_Name != this.MyMeter.MyIdent.HardwareName || this.PaletteID_Version != this.MyMeter.MyIdent.lFirmwareVersion)
      {
        if (!this.MyMeter.MyHandler.MyLoadedFunctions.LoadAllPalleteFunctions(AllVersions, this.MyMeter.MyIdent, out this.MyPalette, out this.MyLinearPalette, out OldFunctionNumbers))
          return false;
        this.PaletteID_Name = this.MyMeter.MyIdent.HardwareName;
        this.PaletteID_Version = this.MyMeter.MyIdent.lFirmwareVersion;
      }
      if (this.MyPalette.Count > 0)
      {
        this.SelectedGroup = (SortedList) this.MyPalette.GetByIndex(0);
        this.SelectedFunction = (SortedList) this.SelectedGroup.GetByIndex(0);
      }
      else
      {
        this.SelectedGroup = (SortedList) null;
        this.SelectedFunction = (SortedList) null;
      }
      return true;
    }

    internal bool GetMenuFunctionData(int x, int y, out FunctionData OutFunction)
    {
      OutFunction = (FunctionData) null;
      Function TheFunction;
      if (!this.MyMeter.MyFunctionTable.GetFunctionXY(x, y, out TheFunction))
        return false;
      OutFunction = this.GetFunctionData(TheFunction);
      return true;
    }

    internal bool IsNewestFunctionVersion(ushort FunctionNumber)
    {
      Function function = (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) FunctionNumber];
      return (int) this.MyMeter.MyHandler.MyLoadedFunctions.GetNewestVersion(function.Name) <= (int) (short) function.Version;
    }

    internal bool GetLCDList(ushort FunctionNumber, out bool[] LCDSegments)
    {
      if (this.MyMeter.MyCommunication == null)
        this.MyMeter.MyCommunication = new MeterCommunication(this.MyMeter, true);
      LCDSegments = (bool[]) null;
      foreach (CodeBlock displayCodeBlock in ((MenuItem) ((Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) FunctionNumber]).MenuList[0]).DisplayCodeBlocks)
      {
        if (displayCodeBlock.CodeSequenceType == CodeBlock.CodeSequenceTypes.Displaycode)
          return this.MyMeter.MyMath.GetDisplay(this.MyMeter.Eprom, (uint) this.MyMeter.MyFunctionTable.StartAddressOfNextBlock, (uint) ((LinkObj) displayCodeBlock.CodeList[0]).Address, out LCDSegments);
      }
      return false;
    }

    internal bool GetPalettData(bool AllVersions, out PalettData ThePalettData)
    {
      ThePalettData = new PalettData();
      ThePalettData.AvailableResources = (SortedList) this.MyMeter.AvailableMeterResouces.Clone();
      OverrideParameter overrides = (OverrideParameter) this.MyMeter.MyFunctionTable.OverridesList[(object) OverrideID.ModuleType];
      if (overrides != null)
      {
        ModuleTypeValues moduleTypeValues1 = (ModuleTypeValues) overrides.Value;
        string str1 = (moduleTypeValues1 & ModuleTypeValues.IO1Mask).ToString();
        if (str1 != ModuleTypeValues.NoValue.ToString() && this.MyMeter.AvailableMeterResouces[(object) str1] == null)
        {
          MeterResource meterResource = new MeterResource(str1, (ushort) 0);
          ThePalettData.AvailableResources.Add((object) str1, (object) meterResource);
        }
        ModuleTypeValues moduleTypeValues2 = moduleTypeValues1 & ModuleTypeValues.IO2Mask;
        string str2 = moduleTypeValues2.ToString();
        string str3 = str2;
        moduleTypeValues2 = ModuleTypeValues.NoValue;
        string str4 = moduleTypeValues2.ToString();
        if (str3 != str4 && this.MyMeter.AvailableMeterResouces[(object) str2] == null)
        {
          MeterResource meterResource = new MeterResource(str2, (ushort) 0);
          ThePalettData.AvailableResources.Add((object) str2, (object) meterResource);
        }
      }
      SortedList OldFunctionNumbers;
      if (!this.LoadPalette(AllVersions, out OldFunctionNumbers))
        return false;
      for (int index1 = 0; index1 < this.MyPalette.Count; ++index1)
      {
        SortedList byIndex1 = (SortedList) this.MyPalette.GetByIndex(index1);
        SortedList sortedList = new SortedList();
        ThePalettData.GroupsAndFunctions.Add(this.MyPalette.GetKey(index1), (object) sortedList);
        for (int index2 = 0; index2 < byIndex1.Count; ++index2)
        {
          SortedList byIndex2 = (SortedList) byIndex1.GetByIndex(index2);
          for (int index3 = 0; index3 < byIndex2.Count; ++index3)
          {
            Function byIndex3 = (Function) byIndex2.GetByIndex(index3);
            string str = "?";
            switch (byIndex3.Localisable)
            {
              case FunctionLocalisableType.NORMAL:
                str = "N";
                break;
              case FunctionLocalisableType.FIRST:
                str = "F";
                break;
              case FunctionLocalisableType.MAIN:
                str = "M";
                break;
              case FunctionLocalisableType.INVISIBLE:
                str = "I";
                break;
            }
            try
            {
              FunctionData functionData = this.GetFunctionData(byIndex3);
              if (AllVersions)
                sortedList.Add((object) (functionData.FullName + "(V:" + byIndex3.Version.ToString() + " T:" + str + " F:" + byIndex3.Number.ToString() + ")"), (object) functionData);
              else if ((FunctionData) sortedList[(object) functionData.FullName] == null)
                sortedList.Add((object) functionData.FullName, (object) functionData);
              else
                sortedList.Add((object) (functionData.FullName + " F:" + byIndex3.Number.ToString()), (object) functionData);
              ThePalettData.PalettFunctions.Add((object) functionData.Number, (object) functionData);
            }
            catch (Exception ex)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal function data on function: '" + byIndex3.Name);
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
              return false;
            }
          }
        }
      }
      int index4 = 0;
      int num = 0;
      ArrayList arrayList = (ArrayList) null;
      for (int index5 = 0; index5 < this.MyMeter.MyFunctionTable.FunctionList.Count; ++index5)
      {
        while ((int) (short) this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[index4] == index5)
        {
          if (index4 < this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList.Count - 1)
          {
            ++index4;
            num = 0;
            arrayList = new ArrayList();
            ThePalettData.MenuFunctions.Add((object) arrayList);
          }
          else
            goto label_39;
        }
        ++num;
        FunctionData functionData = this.GetFunctionData((Function) this.MyMeter.MyFunctionTable.FunctionList[index5]);
        functionData.IsVisible = true;
        functionData.Column = index4 - 1;
        functionData.Row = num - 1;
        if (OldFunctionNumbers[(object) functionData.Number] != null)
          functionData.NewestVersion = false;
        arrayList.Add((object) functionData);
        ThePalettData.LoadedFunctions.Add((object) functionData.Number, (object) functionData);
      }
label_39:
      for (short indexOfMenuColumn = (short) this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[this.MyMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList.Count - 1]; (int) indexOfMenuColumn < this.MyMeter.MyFunctionTable.FunctionList.Count; ++indexOfMenuColumn)
      {
        FunctionData functionData = this.GetFunctionData((Function) this.MyMeter.MyFunctionTable.FunctionList[(int) indexOfMenuColumn]);
        functionData.IsVisible = false;
        ThePalettData.NonMenuFunctions.Add((object) functionData);
        ThePalettData.LoadedFunctions.Add((object) functionData.Number, (object) functionData);
      }
      return true;
    }

    private FunctionData GetFunctionData(Function TheFunction)
    {
      FunctionData functionData = new FunctionData();
      functionData.Number = TheFunction.Number;
      functionData.Name = TheFunction.Name;
      functionData.FullName = TheFunction.FullName;
      functionData.Group = TheFunction.Group;
      functionData.ShortInfo = TheFunction.ShortInfo;
      functionData.Description = TheFunction.Description;
      functionData.Symbolname = TheFunction.Symbolname;
      functionData.FunctionType = TheFunction.Localisable;
      functionData.Version = TheFunction.Version;
      functionData.NewestVersion = true;
      functionData.FirmwareVersionMin = TheFunction.FirmwareVersionMin;
      functionData.FirmwareVersionMax = TheFunction.FirmwareVersionMax;
      string meterResourcesList = TheFunction.MeterResourcesList;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str1 in meterResourcesList.Split(chArray1))
      {
        string str2 = str1.Trim();
        if (str2.Length >= 1)
        {
          SortedList sortedList;
          string key;
          if (str2.StartsWith("s:"))
          {
            sortedList = functionData.SuppliedResources;
            key = str2.Substring(2);
          }
          else
          {
            sortedList = functionData.NeadedResources;
            key = str2;
          }
          if (sortedList.IndexOfKey((object) key) < 0)
            sortedList.Add((object) key, (object) (ushort) 0);
        }
      }
      foreach (Parameter parameter in TheFunction.ParameterList)
      {
        string meterResource = parameter.MeterResource;
        char[] chArray2 = new char[1]{ ';' };
        foreach (string str in meterResource.Split(chArray2))
        {
          string key = str.Trim();
          if (key.Length >= 1 && functionData.SuppliedResources.IndexOfKey((object) key) < 0)
            functionData.SuppliedResources.Add((object) key, (object) (ushort) 0);
        }
      }
      return functionData;
    }
  }
}
