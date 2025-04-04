// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_SmartFunctionManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using SmartFunctionCompiler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_SmartFunctionManager
  {
    public static string[] HardCodedFunctions = new string[5]
    {
      LoggerNames.Month.ToString(),
      LoggerNames.Event.ToString(),
      LoggerNames.KeyDate.ToString(),
      LoggerNames.DayVolumes.ToString(),
      LoggerNames.NearPast.ToString()
    };
    public const string ReloadAllKey = "RELOAD_ALL_FUNCTIONS";
    internal static Logger S4_SmartFunctionLogger = LogManager.GetLogger(nameof (S4_SmartFunctionLogger));
    internal static List<SmartFunctionIdentAndCode> DatabaseSmartFunctions;
    private static List<string> databaseSmartFunctionGroups;
    public const string NotDefinedGroup = "NotDefined";
    internal NfcDeviceCommands NfcCmd;
    private bool functionReloadRequired = false;

    public static List<string> DatabaseSmartFunctionGroups
    {
      get
      {
        if (S4_SmartFunctionManager.databaseSmartFunctionGroups != null)
          S4_SmartFunctionManager.LoadSmartFunctions();
        return S4_SmartFunctionManager.databaseSmartFunctionGroups;
      }
    }

    public S4_SmartFunctionInfo SmartFunctionInfo { get; private set; }

    public List<SmartFunctionIdentAndCode> UsableFunctions { get; private set; }

    public List<SmartFunctionIdent> UsableFunctionIdent { get; private set; }

    public List<SmartFunctionIdentAndCode> FunctionsToDevice { get; private set; }

    public List<string> ActiveFunctionsToDevice { get; private set; }

    internal List<SmartFunctionIdentResultAndCalls> FunctionsInDevice { get; private set; }

    internal List<SmartFunctionIdentResultAndCalls> LoadedFunctionsInDevice { get; private set; }

    internal List<SmartFunctionIdentAndFlashParams> FunctionsFromMemory { get; private set; }

    internal SortedList<string, List<SmartFunctionConfigParam>> FunctionParametersForConfiguration { get; private set; }

    internal bool FunctionReloadRequired
    {
      get => this.functionReloadRequired;
      set => this.functionReloadRequired |= value;
    }

    internal static void Dispose()
    {
      S4_SmartFunctionManager.DatabaseSmartFunctions = (List<SmartFunctionIdentAndCode>) null;
    }

    internal S4_SmartFunctionManager(NfcDeviceCommands nfcCmd)
    {
      this.SmartFunctionInfo = new S4_SmartFunctionInfo((byte) 0);
      this.NfcCmd = nfcCmd;
    }

    internal S4_SmartFunctionManager(S4_DeviceMemory meterMemory)
    {
      if (meterMemory.SmartFunctionFlashRange == null)
        throw new Exception("SmartFunctionFlashRange not defined");
      this.SmartFunctionInfo = meterMemory.IsParameterAvailable(S4_Params.InterpreterVersion) ? new S4_SmartFunctionInfo(meterMemory.GetParameterValue<byte>(S4_Params.InterpreterVersion)) : throw new Exception("Old map. InterpreterVersion not available");
      this.RefreshDatabaseFunctions();
      this.SetFunctionIdentsFromMemory(meterMemory);
    }

    public static string[] GetSmartFunctionNamesOfGroup(string groupName)
    {
      if (!S4_SmartFunctionManager.DatabaseSmartFunctionGroups.Contains(groupName))
        throw new Exception("Group name not defined: " + groupName);
      List<string> stringList = new List<string>();
      foreach (SmartFunctionIdent databaseSmartFunction in S4_SmartFunctionManager.DatabaseSmartFunctions)
      {
        string[] source;
        if (databaseSmartFunction.MemberOfGroups != null)
          source = databaseSmartFunction.MemberOfGroups.Split(new char[1]
          {
            ';'
          }, StringSplitOptions.RemoveEmptyEntries);
        else
          source = new string[1]{ "NotDefined" };
        if (((IEnumerable<string>) source).Contains<string>(groupName))
          stringList.Add(databaseSmartFunction.Name);
      }
      return stringList.ToArray();
    }

    internal async Task<S4_SmartFunctionInfo> ReadSmartFunctionInfoAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.NfcCmd.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetSmartFunctionInfo);
      this.SmartFunctionInfo = new S4_SmartFunctionInfo(result);
      this.UsableFunctions = S4_SmartFunctionManager.GetNewestDatabaseSmartFunctionCodes(this.SmartFunctionInfo.InterpreterVersion);
      this.UsableFunctionIdent = S4_SmartFunctionManager.GetNewestDatabaseSmartFunctionIdents(this.UsableFunctions);
      S4_SmartFunctionInfo smartFunctionInfo = this.SmartFunctionInfo;
      result = (byte[]) null;
      return smartFunctionInfo;
    }

    internal void RefreshDatabaseFunctions()
    {
      this.UsableFunctions = S4_SmartFunctionManager.GetNewestDatabaseSmartFunctionCodes(this.SmartFunctionInfo.InterpreterVersion);
      this.UsableFunctionIdent = S4_SmartFunctionManager.GetNewestDatabaseSmartFunctionIdents(this.UsableFunctions);
    }

    internal async Task ReadLoadedFunctionsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<SmartFunctionIdentResultAndCalls> identResultAndCallsList = await this.NfcCmd.GetSmartFunctionsList(progress, cancelToken);
      this.FunctionsInDevice = identResultAndCallsList;
      identResultAndCallsList = (List<SmartFunctionIdentResultAndCalls>) null;
      this.LoadedFunctionsInDevice = new List<SmartFunctionIdentResultAndCalls>();
      foreach (SmartFunctionIdentResultAndCalls func in this.FunctionsInDevice)
      {
        if (!((IEnumerable<string>) S4_SmartFunctionManager.HardCodedFunctions).Contains<string>(func.Name))
          this.LoadedFunctionsInDevice.Add(func);
      }
    }

    internal async Task DeleteAllFunctionsAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.DeleteAllSmartFunctions, NfcDeviceCommands.FillData);
      this.FunctionsInDevice = (List<SmartFunctionIdentResultAndCalls>) null;
      this.LoadedFunctionsInDevice = (List<SmartFunctionIdentResultAndCalls>) null;
      result = (byte[]) null;
    }

    internal async Task<SmartFunctionRuntimeResult> LoadFunctionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte[] functionCode)
    {
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.LoadSmartFunction, functionCode);
      SmartFunctionRuntimeResult functionRuntimeResult = new SmartFunctionRuntimeResult(result);
      result = (byte[]) null;
      return functionRuntimeResult;
    }

    internal async Task<byte[]> GetFunctionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName)
    {
      byte[] functionNameBytes = new byte[functionName.Length + 1];
      int offset = 0;
      ByteArrayScanner.ScanInString(functionNameBytes, functionName, ref offset);
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.GetSmartFunction, functionNameBytes);
      byte[] functionCode = new byte[result.Length - 4];
      if (result[0] < byte.MaxValue)
      {
        functionCode = new byte[result.Length - 4];
        Array.Copy((Array) result, 2, (Array) functionCode, 0, functionCode.Length);
      }
      else
      {
        functionCode = new byte[result.Length - 6];
        Array.Copy((Array) result, 4, (Array) functionCode, 0, functionCode.Length);
      }
      byte[] functionAsync = functionCode;
      functionNameBytes = (byte[]) null;
      result = (byte[]) null;
      functionCode = (byte[]) null;
      return functionAsync;
    }

    internal async Task<List<SmartFunctionParameter>> GetFunctionParametersAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName)
    {
      byte[] functionNameBytes = new byte[functionName.Length + 1];
      int offset = 0;
      ByteArrayScanner.ScanInString(functionNameBytes, functionName, ref offset);
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.GetSmartFunctionParameters, functionNameBytes);
      List<SmartFunctionParameter> functionParameters = new List<SmartFunctionParameter>();
      int scanOffset = 2;
      if (result[0] == byte.MaxValue)
        scanOffset += 2;
      while (scanOffset < result.Length - 2)
        functionParameters.Add(new SmartFunctionParameter(result, ref scanOffset));
      List<SmartFunctionParameter> functionParametersAsync = functionParameters;
      functionNameBytes = (byte[]) null;
      result = (byte[]) null;
      functionParameters = (List<SmartFunctionParameter>) null;
      return functionParametersAsync;
    }

    internal async Task<List<SmartFunctionParameter>> GetFunctionParametersFromCodeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName)
    {
      byte[] functionCode = await this.GetFunctionAsync(progress, cancelToken, functionName);
      int offset = functionName.Length + 1 + 3;
      byte numberOfParameters = ByteArrayScanner.ScanByte(functionCode, ref offset);
      List<SmartFunctionParameter> functionParameters = new List<SmartFunctionParameter>();
      for (int i = 0; i < (int) numberOfParameters - 1; ++i)
        functionParameters.Add(new SmartFunctionParameter(functionCode, ref offset, true));
      List<SmartFunctionParameter> parametersFromCodeAsync = functionParameters;
      functionCode = (byte[]) null;
      functionParameters = (List<SmartFunctionParameter>) null;
      return parametersFromCodeAsync;
    }

    internal async Task<List<SmartFunctionParameter>> GetLoggerParametersFromCodeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName)
    {
      byte[] functionCode = await this.GetFunctionAsync(progress, cancelToken, functionName);
      int offset = functionName.Length + 1 + 3;
      byte numberOfParameters = ByteArrayScanner.ScanByte(functionCode, ref offset);
      SmartFunctionParameter loggerParameter = new SmartFunctionParameter(functionCode, ref offset, true);
      if (loggerParameter.ParameterName != "Logger")
        throw new Exception("Function is not a logger");
      List<SmartFunctionParameter> functionParameters = new List<SmartFunctionParameter>();
      for (int i = 0; i < (int) numberOfParameters - 1; ++i)
        functionParameters.Add(new SmartFunctionParameter(functionCode, ref offset, true));
      List<SmartFunctionParameter> parametersFromCodeAsync = functionParameters;
      functionCode = (byte[]) null;
      loggerParameter = (SmartFunctionParameter) null;
      functionParameters = (List<SmartFunctionParameter>) null;
      return parametersFromCodeAsync;
    }

    internal async Task<SmartFunctionRuntimeResult> SetFunctionParametersAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string functionName,
      List<SmartFunctionParameter> changedParameters)
    {
      byte[] byteData = new byte[1000];
      int offset = 0;
      ByteArrayScanner.ScanInString(byteData, functionName, ref offset);
      foreach (SmartFunctionParameter theParameter in changedParameters)
        theParameter.ScanIn(byteData, ref offset);
      Array.Resize<byte>(ref byteData, offset);
      byte[] result = await this.NfcCmd.StandardCommandAsync(progress, cancelToken, NfcCommands.SetSmartFunctionParameters, byteData);
      SmartFunctionRuntimeResult functionRuntimeResult = new SmartFunctionRuntimeResult(result);
      byteData = (byte[]) null;
      result = (byte[]) null;
      return functionRuntimeResult;
    }

    public static List<SmartFunctionIdent> GetNewestDatabaseSmartFunctionIdents(
      List<SmartFunctionIdentAndCode> newestFunctions)
    {
      List<SmartFunctionIdent> smartFunctionIdents = new List<SmartFunctionIdent>();
      foreach (SmartFunctionIdentAndCode newestFunction in newestFunctions)
      {
        if (!((IEnumerable<string>) S4_SmartFunctionManager.HardCodedFunctions).Contains<string>(newestFunction.Name))
          smartFunctionIdents.Add((SmartFunctionIdent) newestFunction);
      }
      return smartFunctionIdents;
    }

    public static List<SmartFunctionIdentAndCode> GetNewestDatabaseSmartFunctionCodes(
      byte interpreterVersion)
    {
      S4_SmartFunctionManager.LoadSmartFunctions();
      List<SmartFunctionIdentAndCode> smartFunctionCodes = new List<SmartFunctionIdentAndCode>();
      foreach (SmartFunctionIdentAndCode databaseSmartFunction in S4_SmartFunctionManager.DatabaseSmartFunctions)
      {
        SmartFunctionIdentAndCode theFunction = databaseSmartFunction;
        if ((int) theFunction.InterpreterVersion >= (int) interpreterVersion)
        {
          int index = smartFunctionCodes.FindIndex((Predicate<SmartFunctionIdentAndCode>) (x => x.Name == theFunction.Name));
          if (index >= 0 && (int) theFunction.InterpreterVersion > (int) smartFunctionCodes[index].InterpreterVersion)
          {
            smartFunctionCodes.RemoveAt(index);
            smartFunctionCodes.Add(theFunction);
          }
          smartFunctionCodes.Add(theFunction);
        }
      }
      return smartFunctionCodes;
    }

    public string[] GetPreparedSmartFunctionNames()
    {
      return this.FunctionsToDevice != null ? S4_SmartFunctionManager.GetSmartFunctionNames(this.FunctionsToDevice) : S4_SmartFunctionManager.GetSmartFunctionNames(this.FunctionsFromMemory);
    }

    public string[] GetPreparedActiveFunctionNames()
    {
      string[] source = this.ActiveFunctionsToDevice == null ? S4_SmartFunctionManager.GetActiveSmartFunctionNames(this.FunctionsFromMemory) : this.ActiveFunctionsToDevice.ToArray();
      if (this.FunctionsToDevice != null)
      {
        List<string> stringList = new List<string>();
        foreach (SmartFunctionIdentAndCode functionIdentAndCode in this.FunctionsToDevice)
        {
          if (((IEnumerable<string>) source).Contains<string>(functionIdentAndCode.Name))
            stringList.Add(functionIdentAndCode.Name);
        }
        source = stringList.ToArray();
      }
      return source;
    }

    public string[] GetUsableSmartFunctionNames()
    {
      return S4_SmartFunctionManager.GetSmartFunctionNames(this.UsableFunctions);
    }

    public static string[] GetSmartFunctionNames(List<SmartFunctionIdentAndFlashParams> functions)
    {
      if (functions == null)
        return new string[0];
      string[] smartFunctionNames = new string[functions.Count];
      for (int index = 0; index < smartFunctionNames.Length; ++index)
        smartFunctionNames[index] = functions[index].Name;
      return smartFunctionNames;
    }

    public static string[] GetSmartFunctionNames(List<SmartFunctionIdentAndCode> functions)
    {
      if (functions == null)
        return new string[0];
      string[] smartFunctionNames = new string[functions.Count];
      for (int index = 0; index < smartFunctionNames.Length; ++index)
        smartFunctionNames[index] = functions[index].Name;
      return smartFunctionNames;
    }

    public static string[] GetSmartFunctionNames(List<SmartFunctionIdentResultAndCalls> functions)
    {
      if (functions == null)
        return new string[0];
      string[] smartFunctionNames = new string[functions.Count];
      for (int index = 0; index < smartFunctionNames.Length; ++index)
        smartFunctionNames[index] = functions[index].Name;
      return smartFunctionNames;
    }

    public static string[] GetActiveSmartFunctionNames(
      List<SmartFunctionIdentResultAndCalls> functions)
    {
      if (functions == null)
        throw new ArgumentNullException(nameof (functions));
      List<string> stringList = new List<string>();
      foreach (SmartFunctionIdentResultAndCalls function in functions)
      {
        if (function.FunctionResult == SmartFunctionResult.NoError)
          stringList.Add(function.Name);
      }
      return stringList.ToArray();
    }

    public static string[] GetActiveSmartFunctionNames(
      List<SmartFunctionIdentAndFlashParams> functions)
    {
      if (functions == null)
        return new string[0];
      List<string> stringList = new List<string>();
      foreach (SmartFunctionIdentAndFlashParams function in functions)
      {
        if (function.FunctionResult == SmartFunctionResult.NoError)
          stringList.Add(function.Name);
      }
      return stringList.ToArray();
    }

    public static string[] GetReadOnlySmartFunctionNames(
      List<SmartFunctionIdentResultAndCalls> functions)
    {
      if (functions == null)
        throw new ArgumentNullException(nameof (functions));
      List<string> stringList = new List<string>();
      foreach (SmartFunctionIdentResultAndCalls function in functions)
      {
        if (((IEnumerable<string>) S4_SmartFunctionManager.HardCodedFunctions).Contains<string>(function.Name))
          stringList.Add(function.Name);
        else if (function.FunctionResult == SmartFunctionResult.NoError && function.FunctionResult == SmartFunctionResult.DeactivatedByCommand)
          stringList.Add(function.Name);
      }
      return stringList.ToArray();
    }

    internal static byte[] GetSelectedFunctionCodeFromDatabase(SmartFunctionIdent selectedFunction)
    {
      S4_SmartFunctionManager.LoadSmartFunctions();
      return (S4_SmartFunctionManager.DatabaseSmartFunctions.FirstOrDefault<SmartFunctionIdentAndCode>((System.Func<SmartFunctionIdentAndCode, bool>) (x => x.Name == selectedFunction.Name && (int) x.Version == (int) selectedFunction.Version)) ?? throw new Exception("Smart function not found: " + selectedFunction.ToString())).Code;
    }

    internal static void DeleteFunctionFromDatabase(SmartFunctionIdent selectedFunction)
    {
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbCommand command = newConnection.CreateCommand();
          command.CommandText = "DELETE FROM SmartFunctions WHERE FunctionName=@FunctionName AND FunctionVersion=@FunctionVersion;";
          DbUtil.AddParameter((IDbCommand) command, "@FunctionName", selectedFunction.Name);
          DbUtil.AddParameter((IDbCommand) command, "@FunctionVersion", (int) selectedFunction.Version);
          if (command.ExecuteNonQuery() != 1)
            throw new Exception("Error on delete function: " + selectedFunction.Name);
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Delete function from database");
      }
      finally
      {
        S4_SmartFunctionManager.DatabaseSmartFunctions = (List<SmartFunctionIdentAndCode>) null;
        S4_SmartFunctionManager.LoadSmartFunctions();
      }
    }

    private static void LoadSmartFunctions()
    {
      if (S4_SmartFunctionManager.DatabaseSmartFunctions != null)
        return;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT * FROM SmartFunctions ORDER BY LoadOrder,FunctionName");
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HandlerTables.SmartFunctionsDataTable functionsDataTable = new HandlerTables.SmartFunctionsDataTable();
          dataAdapter.Fill((DataTable) functionsDataTable);
          S4_SmartFunctionManager.DatabaseSmartFunctions = new List<SmartFunctionIdentAndCode>();
          foreach (HandlerTables.SmartFunctionsRow smartFunctionsRow in (TypedTableBase<HandlerTables.SmartFunctionsRow>) functionsDataTable)
          {
            SmartFunctionIdentAndCode functionIdentAndCode = new SmartFunctionIdentAndCode(smartFunctionsRow.FunctionName, smartFunctionsRow.FunctionVersion, smartFunctionsRow.InterpreterVersion, smartFunctionsRow.FunctionEvent, smartFunctionsRow.FunctionCode);
            if (!smartFunctionsRow.IsFunctionDescriptionNull())
              functionIdentAndCode.FunctionDescription = smartFunctionsRow.FunctionDescription;
            if (!smartFunctionsRow.IsRequiredFunctionsNull())
              functionIdentAndCode.RequiredFunctions = smartFunctionsRow.RequiredFunctions;
            if (!smartFunctionsRow.IsMemberOfGroupsNull())
              functionIdentAndCode.MemberOfGroups = smartFunctionsRow.MemberOfGroups;
            S4_SmartFunctionManager.DatabaseSmartFunctions.Add(functionIdentAndCode);
          }
        }
        S4_SmartFunctionManager.databaseSmartFunctionGroups = new List<string>();
        foreach (SmartFunctionIdent databaseSmartFunction in S4_SmartFunctionManager.DatabaseSmartFunctions)
        {
          if (databaseSmartFunction.MemberOfGroups != null)
          {
            string memberOfGroups = databaseSmartFunction.MemberOfGroups;
            char[] separator = new char[1]{ ';' };
            foreach (string str in memberOfGroups.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
              if (!S4_SmartFunctionManager.databaseSmartFunctionGroups.Contains(str))
                S4_SmartFunctionManager.databaseSmartFunctionGroups.Add(str);
            }
          }
          else if (!S4_SmartFunctionManager.databaseSmartFunctionGroups.Contains("NotDefined"))
            S4_SmartFunctionManager.databaseSmartFunctionGroups.Add("NotDefined");
        }
      }
      catch (Exception ex)
      {
        S4_SmartFunctionManager.DatabaseSmartFunctions = (List<SmartFunctionIdentAndCode>) null;
        throw new Exception("Load smart functions from database error", ex);
      }
    }

    internal static bool SaveSmartFunction(
      SmartFunction smartFunction,
      string functionDescription,
      string requiredFunctions,
      string memberOfGroups)
    {
      bool flag = false;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT * FROM SmartFunctions WHERE FunctionName = '" + smartFunction.FunctionName + "' AND FunctionVersion = " + smartFunction.FunctionVersion.ToString());
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
          HandlerTables.SmartFunctionsDataTable functionsDataTable = new HandlerTables.SmartFunctionsDataTable();
          dataAdapter.Fill((DataTable) functionsDataTable);
          if (functionsDataTable.Count == 0)
          {
            HandlerTables.SmartFunctionsRow row = functionsDataTable.NewSmartFunctionsRow();
            row.FunctionName = smartFunction.FunctionName;
            row.FunctionVersion = smartFunction.FunctionVersion;
            row.InterpreterVersion = smartFunction.InterpreterVersion;
            row.FunctionEvent = smartFunction.StartEvent.ToString();
            row.FunctionCode = smartFunction.Code;
            if (functionDescription != null)
              row.FunctionDescription = functionDescription;
            if (requiredFunctions != null)
              row.RequiredFunctions = requiredFunctions;
            if (memberOfGroups != null)
              row.MemberOfGroups = memberOfGroups;
            row.LoadOrder = (short) 17;
            functionsDataTable.AddSmartFunctionsRow(row);
            flag = true;
          }
          else
          {
            if (functionsDataTable.Count != 1)
              throw new Exception("More than one row exists.");
            functionsDataTable[0].InterpreterVersion = smartFunction.InterpreterVersion;
            functionsDataTable[0].FunctionEvent = smartFunction.StartEvent.ToString();
            functionsDataTable[0].FunctionCode = smartFunction.Code;
            if (functionDescription != null)
              functionsDataTable[0].FunctionDescription = functionDescription;
            else
              functionsDataTable[0].SetFunctionDescriptionNull();
            if (requiredFunctions != null)
              functionsDataTable[0].RequiredFunctions = requiredFunctions;
            else
              functionsDataTable[0].SetRequiredFunctionsNull();
            if (memberOfGroups != null)
              functionsDataTable[0].MemberOfGroups = memberOfGroups;
            else
              functionsDataTable[0].SetMemberOfGroupsNull();
            flag = false;
          }
          dataAdapter.Update((DataTable) functionsDataTable);
          S4_SmartFunctionManager.LoadSmartFunctions();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Save function from editor to database");
      }
      return flag;
    }

    internal void SetFunctionIdentsFromMemory(S4_DeviceMemory meterMemory)
    {
      byte[] numArray1 = new byte[(int) meterMemory.SmartFunctionFlashRange.ByteSize];
      meterMemory.InsertAvailableData(numArray1, meterMemory.SmartFunctionFlashRange.StartAddress);
      this.FunctionsFromMemory = new List<SmartFunctionIdentAndFlashParams>();
      if (numArray1 == null || numArray1.Length < 2)
        return;
      ushort storageOffset = 0;
      while (true)
      {
        FunctionAccessStorage functionAccessStorage = new FunctionAccessStorage(numArray1, storageOffset);
        if (functionAccessStorage.StorageOffsetNextFunction != (ushort) 0)
        {
          storageOffset += functionAccessStorage.StorageOffsetNextFunction;
          byte[] numArray2 = new byte[(int) functionAccessStorage.NumberOfFunctionBytes];
          Array.Copy((Array) numArray1, (int) functionAccessStorage.RuntimeCodeStorageOffset, (Array) numArray2, 0, numArray2.Length);
          SmartFunction smartFunction = new SmartFunction(numArray2);
          SmartFunctionIdentAndFlashParams identAndFlashParams = new SmartFunctionIdentAndFlashParams(smartFunction.FunctionName, smartFunction.FunctionVersion, functionAccessStorage.SmartFunctionResult);
          try
          {
            for (byte index = 0; (int) index < smartFunction.AllParameters.Count; ++index)
            {
              RuntimeParameter allParameter = smartFunction.AllParameters[(int) index];
              if (allParameter.StorageCode == StorageTypeCodes.flash)
              {
                string name = allParameter.Name;
                if (!name.StartsWith("L_") && !(name == "Logger"))
                {
                  Register register = new Register();
                  ushort offset = (ushort) ((uint) numArray1.Length - (uint) functionAccessStorage.GetRuntimeParameterDataOffset(index));
                  register.LoadValue(allParameter.TypeCode, numArray1, (int) offset);
                  string str = this.Value4Digits(register.GetValue());
                  identAndFlashParams.FlashParameters.Add(name, str);
                }
              }
            }
          }
          catch
          {
          }
          this.FunctionsFromMemory.Add(identAndFlashParams);
        }
        else
          break;
      }
    }

    private string Value4Digits(string valueString)
    {
      bool flag = false;
      int num = 0;
      for (int index = 0; index < valueString.Length; ++index)
      {
        if (valueString[index] == '.' || valueString[index] == ',')
          flag = true;
        if (flag && num >= 4)
          return valueString.Substring(0, index);
        if (char.IsDigit(valueString[index]))
          ++num;
      }
      return valueString;
    }

    internal void CheckReadAgainstMemory()
    {
      int num = 0;
      foreach (SmartFunctionIdentResultAndCalls identResultAndCalls in this.FunctionsInDevice)
      {
        SmartFunctionIdentResultAndCalls theFunction = identResultAndCalls;
        if (!((IEnumerable<string>) S4_SmartFunctionManager.HardCodedFunctions).Contains<string>(theFunction.Name))
        {
          ++num;
          if (((IEnumerable<SmartFunctionIdent>) this.FunctionsFromMemory).FirstOrDefault<SmartFunctionIdent>((System.Func<SmartFunctionIdent, bool>) (x => x.Name == theFunction.Name)) == null)
            throw new Exception("Functions missed in memory: " + theFunction.Name);
        }
      }
      if (num != this.FunctionsFromMemory.Count)
        throw new Exception("Functions count different!");
    }

    internal void PrepareRequiredFunctions(string[] requiredFunctionNames)
    {
      this.GetPreparedSmartFunctionNames();
      this.FunctionsToDevice = new List<SmartFunctionIdentAndCode>();
      this.ActiveFunctionsToDevice = (List<string>) null;
      foreach (SmartFunctionIdentAndCode usableFunction in this.UsableFunctions)
      {
        SmartFunctionIdentAndCode theFunction = usableFunction;
        int num = ((IEnumerable<string>) requiredFunctionNames).Count<string>((System.Func<string, bool>) (x => x == theFunction.Name));
        if (num > 1)
          throw new Exception("Redundant smart function name: " + theFunction.Name);
        if (num == 1)
          this.FunctionsToDevice.Add(theFunction);
      }
      if (this.FunctionsToDevice.Count != requiredFunctionNames.Length)
      {
        foreach (string requiredFunctionName in requiredFunctionNames)
        {
          string theName = requiredFunctionName;
          if (theName == "RELOAD_ALL_FUNCTIONS")
            this.functionReloadRequired = true;
          else if (this.FunctionsToDevice.FirstOrDefault<SmartFunctionIdentAndCode>((System.Func<SmartFunctionIdentAndCode, bool>) (x => x.Name == theName)) == null)
          {
            this.FunctionsToDevice = (List<SmartFunctionIdentAndCode>) null;
            throw new Exception("Required smart function not available: " + theName);
          }
        }
      }
      if (!this.functionReloadRequired)
      {
        if (this.LoadedFunctionsInDevice != null && this.LoadedFunctionsInDevice.Count == this.FunctionsToDevice.Count)
        {
          for (int index = 0; index < this.FunctionsToDevice.Count; ++index)
          {
            if (this.LoadedFunctionsInDevice[index].Name != this.FunctionsToDevice[index].Name || (int) this.LoadedFunctionsInDevice[index].Version != (int) this.FunctionsToDevice[index].Version)
            {
              this.functionReloadRequired = true;
              break;
            }
          }
        }
        else
          this.functionReloadRequired = true;
      }
      if (this.functionReloadRequired)
      {
        this.ActiveFunctionsToDevice = new List<string>();
        foreach (SmartFunctionIdent smartFunctionIdent in this.FunctionsToDevice)
          this.ActiveFunctionsToDevice.Add(smartFunctionIdent.Name);
      }
      else
        this.FunctionsToDevice = (List<SmartFunctionIdentAndCode>) null;
    }

    internal void PrepareActiveFunctions(string[] activeFunctionNames)
    {
      this.ActiveFunctionsToDevice = new List<string>();
      foreach (string smartFunctionName in this.GetPreparedSmartFunctionNames())
      {
        if (((IEnumerable<string>) activeFunctionNames).Contains<string>(smartFunctionName))
          this.ActiveFunctionsToDevice.Add(smartFunctionName);
      }
    }

    internal void PrepareParameterOverwrite(
      DeviceCharacteristics deviceCharacteristics,
      string[] owerwriteDefinitions)
    {
      this.FunctionParametersForConfiguration = new SortedList<string, List<SmartFunctionConfigParam>>();
      foreach (string owerwriteDefinition in owerwriteDefinitions)
      {
        SmartFunctionConfigParam functionConfigParam = new SmartFunctionConfigParam(deviceCharacteristics, this.UsableFunctions, owerwriteDefinition);
        if (this.FunctionParametersForConfiguration.ContainsKey(functionConfigParam.FunctionName))
          this.FunctionParametersForConfiguration[functionConfigParam.FunctionName].Add(functionConfigParam);
        else
          this.FunctionParametersForConfiguration.Add(functionConfigParam.FunctionName, new List<SmartFunctionConfigParam>()
          {
            functionConfigParam
          });
      }
    }
  }
}
