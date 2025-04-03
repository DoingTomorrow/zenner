// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LoadedFunctions
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Data;
using System.Text;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace GMM_Handler
{
  internal class LoadedFunctions
  {
    internal ZR_HandlerFunctions MyHandler;
    internal SortedList FullLoadedFunctions;
    internal SortedList LoadedFunctionHeaders;

    public LoadedFunctions(ZR_HandlerFunctions MyHandlerIn)
    {
      this.MyHandler = MyHandlerIn;
      this.FullLoadedFunctions = new SortedList();
      this.LoadedFunctionHeaders = new SortedList();
    }

    internal bool LoadFunctionHeaders(ArrayList FunctionNumbersList)
    {
      Schema.ZRFunctionDataTable TheTable = new Schema.ZRFunctionDataTable();
      if (!this.MyHandler.MyDataBaseAccess.GetFunctionHeaders(FunctionNumbersList, TheTable))
        return false;
      try
      {
        foreach (Schema.ZRFunctionRow row in (InternalDataCollectionBase) TheTable.Rows)
          this.LoadFunctionHeader(row);
      }
      catch
      {
        return false;
      }
      return true;
    }

    internal bool LoadAllPalleteFunctions(
      bool AllVersions,
      ZR_MeterIdent TheIdent,
      out SortedList ThePalette,
      out SortedList LinearPalett,
      out SortedList OldFunctionNumbers)
    {
      ThePalette = (SortedList) null;
      LinearPalett = (SortedList) null;
      OldFunctionNumbers = new SortedList();
      ArrayList NiededFunctions;
      if (!this.MyHandler.MyDataBaseAccess.GetFunctionNumbersList(TheIdent, out NiededFunctions) || !this.GarantAllFunctionsLoaded(NiededFunctions))
        return false;
      string[] strArray = TheIdent.HardwareResource.Split(';');
      try
      {
        ThePalette = new SortedList();
        LinearPalett = new SortedList();
        string str = string.Empty;
        for (int index1 = 0; index1 < NiededFunctions.Count; ++index1)
        {
          Function fullLoadedFunction = (Function) this.FullLoadedFunctions[(object) (ushort) NiededFunctions[index1]];
          for (int index2 = 0; index2 < fullLoadedFunction.NotSupportedResources.Length; ++index2)
          {
            for (int index3 = 0; index3 < strArray.Length; ++index3)
            {
              if (fullLoadedFunction.NotSupportedResources[index2] == strArray[index3])
                goto label_21;
            }
          }
          if (fullLoadedFunction.Name == str)
          {
            OldFunctionNumbers.Add((object) fullLoadedFunction.Number, (object) "");
            if (!AllVersions)
              continue;
          }
          else
            str = fullLoadedFunction.Name;
          if (fullLoadedFunction.Name == "DefaultFunction" || fullLoadedFunction.Name.StartsWith("Hardwaretest"))
          {
            LinearPalett.Add((object) (fullLoadedFunction.FullName + "(" + fullLoadedFunction.Number.ToString() + ")"), (object) fullLoadedFunction);
            continue;
          }
          LinearPalett.Add((object) (fullLoadedFunction.FullName + "(" + fullLoadedFunction.Version.ToString() + "_" + fullLoadedFunction.Number.ToString() + ")"), (object) fullLoadedFunction);
          SortedList sortedList1 = (SortedList) ThePalette[(object) fullLoadedFunction.Group];
          if (sortedList1 == null)
          {
            sortedList1 = new SortedList();
            ThePalette.Add((object) fullLoadedFunction.Group, (object) sortedList1);
          }
          SortedList sortedList2 = (SortedList) sortedList1[(object) fullLoadedFunction.FullName];
          if (sortedList2 == null)
          {
            sortedList2 = new SortedList();
            sortedList1.Add((object) fullLoadedFunction.FullName, (object) sortedList2);
          }
          sortedList2.Add((object) (fullLoadedFunction.Version.ToString() + "_" + fullLoadedFunction.Number.ToString()), (object) fullLoadedFunction);
label_21:;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal translated function names");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
        return false;
      }
      return true;
    }

    private Function LoadFunctionHeader(Schema.ZRFunctionRow TheRow)
    {
      ushort functionNumber = (ushort) TheRow.FunctionNumber;
      Function loadedFunctionHeader = (Function) this.LoadedFunctionHeaders[(object) functionNumber];
      if (loadedFunctionHeader != null)
        return loadedFunctionHeader;
      Function function1 = new Function();
      function1.Number = functionNumber;
      function1.Name = TheRow.FunctionName;
      function1.Version = TheRow.FunctionVersion;
      function1.FirmwareVersionMin = TheRow.FirmwareVersionMin;
      function1.FirmwareVersionMax = TheRow.FirmwareVersionMax;
      switch (TheRow.FunctionType)
      {
        case 1:
          function1.Localisable = FunctionLocalisableType.NORMAL;
          break;
        case 2:
          function1.Localisable = FunctionLocalisableType.FIRST;
          break;
        case 3:
          function1.Localisable = FunctionLocalisableType.MAIN;
          break;
        case 4:
          function1.Localisable = FunctionLocalisableType.INVISIBLE;
          break;
        case 9:
          function1.Localisable = FunctionLocalisableType.SYSTEM;
          break;
        default:
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal function localisable attribute");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
          return (Function) null;
      }
      function1.LoggerType = (LoggerTypes) TheRow.LoggerType;
      function1.ShortInfo = !TheRow.IsFunctionShortInfoNull() ? TheRow.FunctionShortInfo : string.Empty;
      function1.MeterResourcesList = TheRow.HardwareResource;
      if (!TheRow.IsSoftwareResourceNull())
      {
        Function function2 = function1;
        function2.MeterResourcesList = function2.MeterResourcesList + ";" + TheRow.SoftwareResource;
      }
      string str1 = string.Empty;
      if (!TheRow.IsAccessRightsNull())
        str1 = TheRow.AccessRights;
      int num = str1.IndexOf("ALIAS=");
      if (num >= 0)
      {
        string str2 = str1.Substring(num + 6);
        int length = str2.IndexOf(" ");
        if (length >= 0)
          str2 = str2.Substring(0, length);
        if (str2.Length > 0)
          function1.AliasName = str2;
      }
      function1.FullName = TheRow.IsFullNameNull() ? string.Empty : TheRow.FullName;
      function1.Description = TheRow.IsFunctionDescriptionNull() ? string.Empty : TheRow.FunctionDescription;
      function1.Group = TheRow.IsFunctionGroupNull() ? string.Empty : TheRow.FunctionGroup;
      function1.Symbolname = TheRow.IsSymbolnameNull() ? string.Empty : TheRow.Symbolname;
      if (!TheRow.IsAccessRightNull())
        function1.UserAccessRight = TheRow.AccessRight;
      function1.WorkingAccessRights = str1;
      this.LoadedFunctionHeaders.Add((object) function1.Number, (object) function1);
      return function1;
    }

    internal bool GarantAllFunctionsLoaded(ArrayList FunctionNumbersList)
    {
      ArrayList FunctionList = new ArrayList();
      if (this.FullLoadedFunctions.Count > 0)
      {
        for (int index = 0; index < FunctionNumbersList.Count; ++index)
        {
          if (!this.FullLoadedFunctions.Contains((object) (ushort) FunctionNumbersList[index]))
            FunctionList.Add(FunctionNumbersList[index]);
        }
        if (FunctionList.Count == 0)
          return true;
      }
      else
        FunctionList = FunctionNumbersList;
      ArrayList FunctionNumbersList1 = new ArrayList();
      if (this.LoadedFunctionHeaders.Count > 0)
      {
        for (int index = 0; index < FunctionList.Count; ++index)
        {
          if (!this.LoadedFunctionHeaders.Contains((object) (ushort) FunctionList[index]))
            FunctionNumbersList1.Add(FunctionList[index]);
        }
      }
      else
        FunctionNumbersList1 = FunctionList;
      if (FunctionNumbersList1.Count <= 0 || this.LoadFunctionHeaders(FunctionNumbersList1))
      {
        Schema.ZRParameterDataTable TheTable1 = new Schema.ZRParameterDataTable();
        if (this.MyHandler.MyDataBaseAccess.GetFunctionParameters(FunctionList, TheTable1))
        {
          try
          {
            foreach (ushort key in FunctionList)
            {
              Function loadedFunctionHeader = (Function) this.LoadedFunctionHeaders[(object) key];
              Schema.ZRParameterRow[] zrParameterRowArray = (Schema.ZRParameterRow[]) TheTable1.Select("FunctionNumber = " + key.ToString(), "FunctionNumber,StructureNr,StructureIndex");
              loadedFunctionHeader.ParameterList = new ArrayList();
              StringBuilder stringBuilder = new StringBuilder();
              for (int index = 0; index < zrParameterRowArray.Length; ++index)
              {
                string parameterName = zrParameterRowArray[index].ParameterName;
                int parameterSize = zrParameterRowArray[index].ParameterSize;
                LinkBlockTypes Block = zrParameterRowArray[index].MemoryType != (short) 22 ? (LinkBlockTypes) zrParameterRowArray[index].MemoryType : LinkBlockTypes.NotLinkedReplaceParameter;
                Parameter parameter = new Parameter(parameterName, parameterSize, Block);
                parameter.FullName = loadedFunctionHeader.Name + "." + parameterName;
                parameter.ExistOnEprom = Block != LinkBlockTypes.Static;
                try
                {
                  if (!zrParameterRowArray[index].IsMeterResourceNull())
                  {
                    string meterResource = zrParameterRowArray[index].MeterResource;
                    char[] chArray = new char[1]{ ';' };
                    foreach (string str1 in meterResource.Split(chArray))
                    {
                      string str2 = str1.Trim();
                      if (str2.Length != 0)
                      {
                        if (str2.StartsWith("m:"))
                        {
                          parameter.MBusNeadedResources += str2.Substring(2);
                        }
                        else
                        {
                          parameter.MeterResource = str2;
                          stringBuilder.Append("s:" + parameter.MeterResource + ";");
                        }
                      }
                    }
                  }
                }
                catch
                {
                }
                parameter.FunctionNumber = zrParameterRowArray[index].FunctionNumber;
                parameter.StoreType = (Parameter.ParamStorageType) zrParameterRowArray[index].ParameterType;
                switch (parameter.StoreType)
                {
                  case Parameter.ParamStorageType.INTERVALPOINT:
                    parameter.ParameterFormat = Parameter.BaseParameterFormat.DateTime;
                    break;
                  case Parameter.ParamStorageType.INTERVAL:
                    parameter.ParameterFormat = Parameter.BaseParameterFormat.TimeSpan;
                    break;
                  case Parameter.ParamStorageType.INTERVALOFFSET:
                    parameter.ParameterFormat = Parameter.BaseParameterFormat.TimeSpan;
                    break;
                  case Parameter.ParamStorageType.TIMEPOINT:
                    parameter.ParameterFormat = Parameter.BaseParameterFormat.DateTime;
                    break;
                }
                parameter.DefaultValue = (long) zrParameterRowArray[index].DefaultValueLow;
                parameter.DefaultValue += (long) zrParameterRowArray[index].DefaultValueHigh << 32;
                parameter.ValueCPU = parameter.DefaultValue;
                parameter.ValueEprom = parameter.DefaultValue;
                parameter.MinValue = (long) zrParameterRowArray[index].MinValueLow;
                parameter.MinValue += (long) zrParameterRowArray[index].MinValueHigh << 32;
                parameter.MaxValue = (long) zrParameterRowArray[index].MaxValueLow;
                parameter.MaxValue += (long) zrParameterRowArray[index].MaxValueHigh << 32;
                string GroupNamesString = zrParameterRowArray[index].IsAccessrightsNull() ? string.Empty : zrParameterRowArray[index].Accessrights;
                parameter.AddParameterToGroup(GroupNamesString);
                parameter.Unit = zrParameterRowArray[index].IsUnitNull() ? string.Empty : zrParameterRowArray[index].Unit;
                string DifVifString = zrParameterRowArray[index].IsMBusDifVifValueNull() ? string.Empty : zrParameterRowArray[index].MBusDifVifValue;
                if (parameter.SetDifVifValues(DifVifString))
                {
                  if (zrParameterRowArray[index].MBusShortProt.ToString()[0] == '1')
                    parameter.MBusShortOn = true;
                  if (zrParameterRowArray[index].MBusLongProt.ToString()[0] == '1')
                    parameter.MBusOn = true;
                  parameter.MBusParameterLength = (int) zrParameterRowArray[index].MBusParamLen;
                  if (!zrParameterRowArray[index].IsMBusParamConvertNull())
                  {
                    string mbusParamConvert = zrParameterRowArray[index].MBusParamConvert;
                    if (mbusParamConvert.Length > 0)
                    {
                      switch (mbusParamConvert)
                      {
                        case "MBU_PARA_CONTROL_DATE":
                          parameter.MBusParamConvertion = Parameter.MBusParameterConversion.Date;
                          parameter.ParameterFormat = Parameter.BaseParameterFormat.DateTime;
                          break;
                        case "MBU_PARA_CONTROL_DATE_TIME":
                          parameter.MBusParamConvertion = Parameter.MBusParameterConversion.DateTime;
                          parameter.ParameterFormat = Parameter.BaseParameterFormat.DateTime;
                          break;
                        case "BCD_VALUE":
                          parameter.ParameterFormat = Parameter.BaseParameterFormat.BCD;
                          break;
                      }
                    }
                  }
                  parameter.LoggerID = zrParameterRowArray[index].LoggerID;
                  if (!zrParameterRowArray[index].IsconfiginfoNull())
                  {
                    string configinfo = zrParameterRowArray[index].configinfo;
                    parameter.MBusParameterOverride = configinfo.IndexOf("Volume") < 0 ? (configinfo.IndexOf("Energy") < 0 ? (configinfo.IndexOf("Flow") < 0 ? (configinfo.IndexOf("Power") < 0 ? (configinfo.IndexOf("INPUT_1") < 0 ? (configinfo.IndexOf("INPUT_2") < 0 ? Parameter.MBusParameterOverrideType.None : Parameter.MBusParameterOverrideType.INPUT_2) : Parameter.MBusParameterOverrideType.INPUT_1) : Parameter.MBusParameterOverrideType.Power) : Parameter.MBusParameterOverrideType.Flow) : Parameter.MBusParameterOverrideType.Energy) : Parameter.MBusParameterOverrideType.Volume;
                  }
                  else
                    parameter.MBusParameterOverride = Parameter.MBusParameterOverrideType.None;
                  parameter.NameTranslated = zrParameterRowArray[index].IsNameTranslatedNull() ? string.Empty : zrParameterRowArray[index].NameTranslated;
                  parameter.ParameterInfo = zrParameterRowArray[index].IsParameterInfoNull() ? string.Empty : zrParameterRowArray[index].ParameterInfo;
                  if (!zrParameterRowArray[index].IsStructureNrNull())
                    parameter.StructureNr = (int) zrParameterRowArray[index].StructureNr;
                  if (!zrParameterRowArray[index].IsStructureIndexNull())
                    parameter.StructureIndex = (int) zrParameterRowArray[index].StructureIndex;
                  loadedFunctionHeader.ParameterList.Add((object) parameter);
                }
                else
                  goto label_132;
              }
              stringBuilder.Append(loadedFunctionHeader.MeterResourcesList);
              int length1 = 0;
              int length2 = 0;
              int length3 = 0;
              string[] strArray = stringBuilder.ToString().Split(';');
              for (int index = 0; index < strArray.Length; ++index)
              {
                strArray[index] = strArray[index].Trim();
                if (strArray[index].Length > 0)
                {
                  if (strArray[index].StartsWith("s:"))
                    ++length2;
                  else if (strArray[index].StartsWith("n:"))
                    ++length3;
                  else
                    ++length1;
                }
              }
              loadedFunctionHeader.SuppliedResources = new string[length2];
              loadedFunctionHeader.NeadedResources = new string[length1];
              loadedFunctionHeader.NotSupportedResources = new string[length3];
              int num1 = 0;
              int num2 = 0;
              int num3 = 0;
              for (int index = 0; index < strArray.Length; ++index)
              {
                if (strArray[index].Length > 0)
                {
                  if (strArray[index].StartsWith("s:"))
                    loadedFunctionHeader.SuppliedResources[num1++] = strArray[index].Substring(2);
                  else if (strArray[index].StartsWith("n:"))
                    loadedFunctionHeader.NotSupportedResources[num3++] = strArray[index].Substring(2);
                  else
                    loadedFunctionHeader.NeadedResources[num2++] = strArray[index];
                }
              }
            }
          }
          catch (Exception ex)
          {
            ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
            goto label_132;
          }
          DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable TheTable2 = new DataSetGMM_Handler.CodeRuntimeCodeJoinedDataTable();
          if (this.MyHandler.MyDataBaseAccess.GetRuntimeCode(FunctionList, TheTable2))
          {
            try
            {
              int num = -1;
              string str = "";
              CodeBlock.CodeSequenceTypes CodeSequenceType = CodeBlock.CodeSequenceTypes.Unknown;
              FrameTypes FrameType = FrameTypes.None;
              bool flag = true;
              Function function = (Function) null;
              CodeBlock codeBlock = (CodeBlock) null;
              for (int index = 0; index < TheTable2.Rows.Count; ++index)
              {
                DataSetGMM_Handler.CodeRuntimeCodeJoinedRow runtimeCodeJoinedRow = TheTable2[index];
                int functionNumber = runtimeCodeJoinedRow.FunctionNumber;
                string codeSequenceType = runtimeCodeJoinedRow.CodeSequenceType;
                if (num != functionNumber)
                {
                  num = functionNumber;
                  function = (Function) this.LoadedFunctionHeaders[(object) (ushort) num];
                  flag = true;
                }
                if (str != codeSequenceType)
                {
                  if (!CodeBlock.GetCodeSequenceType(codeSequenceType, out CodeSequenceType, out FrameType, out string _))
                  {
                    this.MyHandler.AddErrorPointMessage("Unknown CodeType");
                    goto label_132;
                  }
                  else
                  {
                    str = codeSequenceType;
                    flag = true;
                  }
                }
                if (flag)
                {
                  if (CodeSequenceType == CodeBlock.CodeSequenceTypes.Interval_Runtime || CodeSequenceType == CodeBlock.CodeSequenceTypes.EEPROM_Interval_Runtime)
                  {
                    codeBlock = (CodeBlock) new IntervalAndLogger(CodeSequenceType, FrameType, num);
                    (codeBlock as IntervalAndLogger).Type = function.LoggerType;
                  }
                  else
                    codeBlock = new CodeBlock(CodeSequenceType, FrameType, num);
                  codeBlock.CodeSequenceName = runtimeCodeJoinedRow.IsCodeSequenceNameNull() ? string.Empty : runtimeCodeJoinedRow.CodeSequenceName;
                  codeBlock.CodeSequenceInfo = runtimeCodeJoinedRow.IsCodeSequenceInfoNull() ? string.Empty : runtimeCodeJoinedRow.CodeSequenceInfo;
                  function.RuntimeCodeBlockList.Add((object) codeBlock);
                  flag = false;
                }
                CodeObject TheCodeObject = new CodeObject(functionNumber);
                TheCodeObject.CodeValue = runtimeCodeJoinedRow.CodeValue.Trim();
                if (this.GetCodeType(runtimeCodeJoinedRow.CodeType, TheCodeObject))
                {
                  TheCodeObject.CodeID = runtimeCodeJoinedRow.CodeID;
                  TheCodeObject.LineNr = runtimeCodeJoinedRow.LineNr;
                  TheCodeObject.LineInfo = runtimeCodeJoinedRow.IsLineInfoNull() ? string.Empty : runtimeCodeJoinedRow.LineInfo;
                  codeBlock.CodeList.Add((object) TheCodeObject);
                }
                else
                  goto label_132;
              }
            }
            catch (Exception ex)
            {
              this.MyHandler.AddErrorPointMessage(ex.ToString());
              goto label_132;
            }
            Schema.MenuDataTable TheTable3 = new Schema.MenuDataTable();
            if (this.MyHandler.MyDataBaseAccess.GetMenus(FunctionList, TheTable3))
            {
              StringBuilder stringBuilder = new StringBuilder();
              for (int index = 0; index < TheTable3.Rows.Count; ++index)
                stringBuilder.Append(TheTable3[index].InterpreterCode.ToString() + ",");
              DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable TheTable4 = new DataSetGMM_Handler.CodeDisplayCodeJoinedDataTable();
              if (stringBuilder.Length > 1)
              {
                --stringBuilder.Length;
                if (!this.MyHandler.MyDataBaseAccess.GetDisplayCodes(stringBuilder.ToString(), TheTable4))
                  goto label_132;
              }
              try
              {
                int num4 = -1;
                string str = "";
                Function function = (Function) null;
                MenuItem menuItem = (MenuItem) null;
                bool flag = true;
                int num5 = -1;
                for (int index1 = 0; index1 < TheTable3.Rows.Count; ++index1)
                {
                  Schema.MenuRow menuRow = TheTable3[index1];
                  int functionNumber = menuRow.FunctionNumber;
                  if (functionNumber != num4)
                  {
                    num4 = functionNumber;
                    function = (Function) this.LoadedFunctionHeaders[(object) (ushort) num4];
                    flag = true;
                    num5 = -1;
                  }
                  string menuName = menuRow.MenuName;
                  int interpreterCode = menuRow.InterpreterCode;
                  DataSetGMM_Handler.CodeDisplayCodeJoinedRow[] displayCodeJoinedRowArray = (DataSetGMM_Handler.CodeDisplayCodeJoinedRow[]) null;
                  if (flag || menuName != str)
                  {
                    menuItem = new MenuItem((ushort) num4, menuName, interpreterCode);
                    menuItem.XPos = (int) menuRow.XPos;
                    menuItem.YPos = (int) menuRow.YPos;
                    menuItem.ClickEvent = menuRow.ClickEvent;
                    menuItem.PressEvent = menuRow.PressEvent;
                    menuItem.HoldEvent = menuRow.HoldEvent;
                    menuItem.TimeoutEvent = menuRow.TimeOutEvent;
                    menuItem.Description = menuRow.IsDescriptionNull() ? string.Empty : menuRow.Description;
                    displayCodeJoinedRowArray = (DataSetGMM_Handler.CodeDisplayCodeJoinedRow[]) TheTable4.Select("InterpreterCode = " + interpreterCode.ToString(), "InterpreterCode,SequenceNr,LineNr");
                    function.MenuList.Add((object) menuItem);
                    ++num5;
                  }
                  int num6 = -1;
                  CodeBlock.CodeSequenceTypes CodeSequenceType = CodeBlock.CodeSequenceTypes.Unknown;
                  FrameTypes FrameType = FrameTypes.None;
                  CodeBlock codeBlock = (CodeBlock) null;
                  for (int index2 = 0; index2 < displayCodeJoinedRowArray.Length; ++index2)
                  {
                    int sequenceNr = displayCodeJoinedRowArray[index2].SequenceNr;
                    if (sequenceNr != num6)
                    {
                      num6 = sequenceNr;
                      string SpecialOptions;
                      if (!CodeBlock.GetCodeSequenceType(displayCodeJoinedRowArray[index2].CodeSequenceType, out CodeSequenceType, out FrameType, out SpecialOptions))
                      {
                        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown code sequence type");
                        goto label_132;
                      }
                      else
                      {
                        codeBlock = new CodeBlock(CodeSequenceType, FrameType, functionNumber);
                        codeBlock.FunctionMenuIndex = num5;
                        codeBlock.SpecialOptions = SpecialOptions;
                        codeBlock.CodeSequenceInfo = displayCodeJoinedRowArray[index2].IsCodeSequenceInfoNull() ? string.Empty : displayCodeJoinedRowArray[index2].CodeSequenceInfo;
                        menuItem.DisplayCodeBlocks.Add((object) codeBlock);
                      }
                    }
                    CodeObject TheCodeObject = new CodeObject(functionNumber);
                    TheCodeObject.CodeValue = displayCodeJoinedRowArray[index2].CodeValue.Trim();
                    if (this.GetCodeType(displayCodeJoinedRowArray[index2].CodeType, TheCodeObject))
                    {
                      TheCodeObject.CodeID = displayCodeJoinedRowArray[index2].CodeID;
                      TheCodeObject.LineNr = displayCodeJoinedRowArray[index2].LineNr;
                      TheCodeObject.LineInfo = displayCodeJoinedRowArray[index2].IsLineInfoNull() ? string.Empty : displayCodeJoinedRowArray[index2].LineInfo;
                      codeBlock.CodeList.Add((object) TheCodeObject);
                    }
                    else
                      goto label_132;
                  }
                }
              }
              catch (Exception ex)
              {
                ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
                goto label_132;
              }
              foreach (ushort key in FunctionList)
                this.FullLoadedFunctions.Add((object) key, this.LoadedFunctionHeaders[(object) key]);
              return true;
            }
          }
        }
      }
label_132:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load function error!");
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Database error");
      return false;
    }

    private bool GetCodeType(string CodeTypeString, CodeObject TheCodeObject)
    {
      string upper = CodeTypeString.Trim().ToUpper();
      if (upper.StartsWith("BYTE"))
      {
        TheCodeObject.CodeType = CodeObject.CodeTypes.BYTE;
        TheCodeObject.Size = 1;
      }
      else if (upper.StartsWith("WORD"))
      {
        TheCodeObject.CodeType = CodeObject.CodeTypes.WORD;
        TheCodeObject.Size = 2;
      }
      else if (upper.StartsWith("LONG"))
      {
        TheCodeObject.CodeType = CodeObject.CodeTypes.LONG;
        TheCodeObject.Size = 4;
      }
      else if (upper.StartsWith("EPTR"))
      {
        TheCodeObject.CodeType = CodeObject.CodeTypes.ePTR;
        TheCodeObject.Size = 2;
      }
      else
      {
        if (!upper.StartsWith("IPTR"))
          return false;
        TheCodeObject.CodeType = CodeObject.CodeTypes.iPTR;
        TheCodeObject.Size = 2;
      }
      if (upper.Length > 4)
        TheCodeObject.OverrideMark = upper.Substring(4);
      return true;
    }

    internal short GetNewestVersion(string FunctionName)
    {
      int newestVersion = -1;
      for (int index = 0; index < this.LoadedFunctionHeaders.Count; ++index)
      {
        Function byIndex = (Function) this.LoadedFunctionHeaders.GetByIndex(index);
        if (byIndex.Name == FunctionName && byIndex.Version > newestVersion)
          newestVersion = byIndex.Version;
      }
      return (short) newestVersion;
    }

    internal void ClearCache()
    {
      this.FullLoadedFunctions = new SortedList();
      this.LoadedFunctionHeaders = new SortedList();
    }

    internal void RemoveCachedFunction(ushort FunctionNumber)
    {
      this.LoadedFunctionHeaders.Remove((object) FunctionNumber);
      this.FullLoadedFunctions.Remove((object) FunctionNumber);
    }
  }
}
