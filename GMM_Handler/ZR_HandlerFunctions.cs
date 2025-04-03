// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ZR_HandlerFunctions
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using AsyncCom;
using DeviceCollector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class ZR_HandlerFunctions : IZR_HandlerFunctions, I_ZR_Component
  {
    internal bool actualValueReadingState = false;
    internal ResourceManager MyRes;
    internal GMMConfig opConfig = (GMMConfig) null;
    private IAsyncFunctions ascom;
    public IDeviceCollector SerBus;
    internal HandlerWindow MyWindow;
    internal AllMeters MyMeters;
    internal LoadedFunctions MyLoadedFunctions;
    internal DataBaseAccess MyDataBaseAccess;
    internal InfoFlags MyInfoFlags = new InfoFlags();
    internal bool DisableChecks = false;
    internal bool ReadWithoutBackup = false;
    internal bool BackupForEachReadInternal = false;
    internal LoggerRestor LoggerRestoreState = LoggerRestor.RestoreBaseLoggers;
    internal bool ExtendedTypeEditMode = false;
    internal bool useBaseTypeTemplate = true;
    internal bool showFunctionAddDelMessages = true;
    internal bool showFunctionRemoveMessages = false;
    internal bool UseOnlyDefaultValues = false;
    internal bool checksumErrorsAsWarning = false;
    internal bool IgnoreIntervalMinutesRaster = false;
    private ERR8002_Form ErrForm;
    private string CSV_FileName = string.Empty;

    public bool BackupForEachRead
    {
      get => this.BackupForEachReadInternal;
      set => this.BackupForEachReadInternal = value;
    }

    public LoggerRestor LoggerRestoreSetup
    {
      get => this.LoggerRestoreState;
      set => this.LoggerRestoreState = value;
    }

    public bool BaseTypeEditMode
    {
      get => this.ExtendedTypeEditMode;
      set => this.ExtendedTypeEditMode = value;
    }

    public bool UseBaseTypeTemplate
    {
      get => this.useBaseTypeTemplate;
      set => this.useBaseTypeTemplate = value;
    }

    public bool ShowFunctionAddDelMessages
    {
      get => this.showFunctionAddDelMessages;
      set => this.showFunctionAddDelMessages = value;
    }

    public bool ShowFunctionRemoveMessages
    {
      get => this.showFunctionRemoveMessages;
      set => this.showFunctionRemoveMessages = value;
    }

    public bool ChecksumErrorsAsWarning
    {
      get => this.checksumErrorsAsWarning;
      set => this.checksumErrorsAsWarning = value;
    }

    public ZR_HandlerFunctions()
    {
      this.BaseConstructor((IDeviceCollector) null, (IAsyncFunctions) null);
    }

    public ZR_HandlerFunctions(IDeviceCollector TheSerialBus, IAsyncFunctions TheAsyncCom)
    {
      this.BaseConstructor(TheSerialBus, TheAsyncCom);
    }

    private void BaseConstructor(IDeviceCollector TheSerialBus, IAsyncFunctions TheAsyncCom)
    {
      this.MyRes = new ResourceManager("GMM_Handler.GMM_HandlerRes", typeof (ZR_HandlerFunctions).Assembly);
      if (TheSerialBus == null)
      {
        ZR_Component.CommonGmmInterface.GarantComponentLoaded(GMM_Components.DeviceCollector);
        this.SerBus = (IDeviceCollector) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.DeviceCollector];
      }
      else
        this.SerBus = TheSerialBus;
      this.ascom = TheAsyncCom != null ? TheAsyncCom : (IAsyncFunctions) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.AsyncCom];
      if (ZR_Component.CommonGmmInterface != null)
        this.opConfig = (GMMConfig) ZR_Component.CommonGmmInterface.LoadedComponentsList[GMM_Components.KonfigGroup];
      this.MyMeters = new AllMeters(this);
      this.MyLoadedFunctions = new LoadedFunctions(this);
      this.MyDataBaseAccess = new DataBaseAccess(this);
      BusDevice selectedDevice = this.SerBus.GetSelectedDevice();
      if (selectedDevice != null && (!(selectedDevice is Serie2MBus) || selectedDevice is Serie3MBus))
      {
        if (this.SerBus.GetBaseMode() == BusMode.MBusPointToPoint && (UserRights.GlobalUserRights.PackageName == UserRights.Packages.ConfigurationManager.ToString() || UserRights.GlobalUserRights.PackageName == UserRights.Packages.ConfigurationManagerPro.ToString()))
        {
          this.SerBus.DeleteBusInfo();
          if (this.SerBus.AddDevice(DeviceTypes.ZR_Serie2, 0))
            goto label_10;
        }
        ZR_ClassLibMessages.AddWarning(this.MyRes.GetString("IllegalBusDevice"), "IllegalBusDevice");
label_10:;
      }
      try
      {
        if (this.opConfig == null)
          return;
        string str1 = this.opConfig.GetValue("GMM_Handler", "LoggerRestoreSetup");
        if (str1 != "")
          this.LoggerRestoreSetup = (LoggerRestor) Enum.Parse(typeof (LoggerRestor), str1, true);
        string str2 = this.opConfig.GetValue("GMM_Handler", "ShowFunctionAddDelMessages");
        if (str2 != "")
          this.showFunctionAddDelMessages = bool.Parse(str2);
        string str3 = this.opConfig.GetValue("GMM_Handler", "ShowFunctionRemoveMessages");
        if (str3 != "")
          this.showFunctionRemoveMessages = bool.Parse(str3);
        string str4 = this.opConfig.GetValue("GMM_Handler", "UseBaseTypeTemplate");
        if (str4 != "")
          this.useBaseTypeTemplate = bool.Parse(str4);
      }
      catch
      {
      }
    }

    public void GMM_Dispose()
    {
      this.opConfig.SetOrUpdateValue("GMM_Handler", "LoggerRestoreSetup", this.LoggerRestoreSetup.ToString());
      this.opConfig.SetOrUpdateValue("GMM_Handler", "ShowFunctionAddDelMessages", this.showFunctionAddDelMessages.ToString());
      this.opConfig.SetOrUpdateValue("GMM_Handler", "ShowFunctionRemoveMessages", this.showFunctionRemoveMessages.ToString());
      this.opConfig.SetOrUpdateValue("GMM_Handler", "UseBaseTypeTemplate", this.useBaseTypeTemplate.ToString());
    }

    internal bool AddErrorPointMessage()
    {
      StackFrame stackFrame = new StackFrame(1, true);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Error at File: '" + stackFrame.GetFileName() + "'");
      stringBuilder.Append(" Line: '" + stackFrame.GetFileLineNumber().ToString() + "'" + ZR_Constants.SystemNewLine);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, stringBuilder.ToString());
      return false;
    }

    internal bool AddErrorPointMessage(string AdditionalMessage)
    {
      StackFrame stackFrame = new StackFrame(1, true);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Error at File: '" + stackFrame.GetFileName() + "'");
      stringBuilder.Append(" Line: '" + stackFrame.GetFileLineNumber().ToString() + "'" + ZR_Constants.SystemNewLine);
      stringBuilder.Append(ZR_Constants.SystemNewLine);
      stringBuilder.Append(AdditionalMessage + ZR_Constants.SystemNewLine);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, stringBuilder.ToString());
      return false;
    }

    public string GetLastError() => ZR_ClassLibMessages.GetLastErrorStringTranslated();

    public string getErrorMsg()
    {
      return Environment.NewLine + ZR_ClassLibMessages.GetLastErrorMessageAndClearError();
    }

    public bool Undo() => this.MyMeters.Undo();

    public void ShowHandlerWindow() => this.ShowHandlerWindow("");

    public string ShowHandlerWindow(string ComponentList)
    {
      if (this.MyWindow == null)
        this.MyWindow = new HandlerWindow(this);
      this.MyWindow.InitStartMenu(ComponentList);
      int num = (int) this.MyWindow.ShowDialog();
      return this.MyWindow.StartComponentName;
    }

    public bool setMaximumBaudrate(bool setMaxBaudrate) => true;

    public int ClearAll()
    {
      this.MyMeters = new AllMeters(this);
      ZR_ClassLibMessages.ClearErrors();
      return 0;
    }

    public void ClearConnectedReadAndWorkMeter()
    {
      this.MyMeters.ConnectedMeter = (Meter) null;
      this.MyMeters.ReadMeter = (Meter) null;
      this.MyMeters.WorkMeter = (Meter) null;
      this.MyMeters.BaseTypeMeter = (Meter) null;
      this.MyMeters.LoggerDataFromMeter = (List<LoggerInfo>) null;
      this.MyMeters.ClearUndoStack();
      ZR_ClassLibMessages.ClearErrors();
    }

    public int openDBDevice(ref ZR_MeterIdent theIdentity, DateTime theTimePoint)
    {
      if (!this.MyMeters.LoadMeter(theIdentity, theTimePoint))
        return 2;
      theIdentity = this.MyMeters.WorkMeter.MyIdent.Clone();
      return 0;
    }

    public bool GetSavedDBDeviceData(out ZR_MeterIdent theIdentity, out DateTime theTimePoint)
    {
      theIdentity = (ZR_MeterIdent) null;
      theTimePoint = DateTime.MinValue;
      if (this.MyMeters.ReadMeter == null || this.MyMeters.ReadMeter.DatabaseTime == DateTime.MinValue)
        return false;
      theIdentity = this.MyMeters.ReadMeter.MyIdent.Clone();
      theTimePoint = this.MyMeters.ReadMeter.DatabaseTime;
      return true;
    }

    public int openType(int TypeMeterInfoID) => this.openType(TypeMeterInfoID, false);

    public int openType(int TypeMeterInfoID, bool DeleteReadMeter)
    {
      return !this.MyMeters.LoadType(TypeMeterInfoID, DeleteReadMeter) ? 2 : 0;
    }

    public bool openType(int TypeMeterInfoID, int FirmwareVersion, bool DeleteReadMeter)
    {
      return this.MyMeters.LoadType(TypeMeterInfoID, FirmwareVersion, DeleteReadMeter);
    }

    public int openType(ref ZR_MeterIdent theIdentity, bool DeleteReadMeter)
    {
      if (!this.MyMeters.LoadType(theIdentity.MeterInfoID, DeleteReadMeter))
        return 2;
      theIdentity = this.MyMeters.WorkMeter.MyIdent.Clone();
      return 0;
    }

    public bool OverloadType(string OverloadSettings)
    {
      return this.MyMeters.OverloadType(OverloadSettings);
    }

    public bool OverloadIdentAndCalibrationData(ZR_HandlerFunctions.MeterObjects SourceMeterObject)
    {
      try
      {
        if (!this.MyMeters.OverloadIdentAndCalibrationData(SourceMeterObject))
          return false;
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return false;
      }
      return true;
    }

    public bool DeleteMeter(ZR_HandlerFunctions.MeterObjects MeterObject)
    {
      return this.MyMeters.DeleteMeter(MeterObject);
    }

    public bool CopyMeter(ZR_HandlerFunctions.MeterObjects SourceMeterObject)
    {
      return this.MyMeters.CopyMeter(SourceMeterObject);
    }

    public bool PastMeter(ZR_HandlerFunctions.MeterObjects MeterObject)
    {
      return this.MyMeters.PastMeter(MeterObject);
    }

    public bool IsMeterObjectAvailable(ZR_HandlerFunctions.MeterObjects MeterObject)
    {
      switch (MeterObject)
      {
        case ZR_HandlerFunctions.MeterObjects.Read:
          if (this.MyMeters.ReadMeter != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.Work:
          if (this.MyMeters.WorkMeter != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          if (this.MyMeters.TypeMeter != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.DbMeter:
          if (this.MyMeters.DbMeter != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo0:
          if (this.MyMeters.WorkMeterUndoStack[0] != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo1:
          if (this.MyMeters.WorkMeterUndoStack[1] != null)
            return true;
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo2:
          if (this.MyMeters.WorkMeterUndoStack[2] != null)
            return true;
          break;
      }
      return false;
    }

    public bool IsDatabaseSwitchTrue(string Switch)
    {
      return this.MyDataBaseAccess.IsDatabaseSwitchTrue(Switch);
    }

    public int saveType(ZR_MeterIdent TypeOverrideIdent)
    {
      return !this.MyMeters.SaveType(TypeOverrideIdent) ? 1 : 0;
    }

    public int saveAsType(ZR_MeterIdent NewTypeIdent)
    {
      return !this.MyMeters.SaveAsNewType(NewTypeIdent) ? 1 : 0;
    }

    public int saveAsType(string IdentNumber, string TypeDescription)
    {
      ZR_MeterIdent TheIdent;
      this.GetIdent(out TheIdent);
      TheIdent.PPSArtikelNr = IdentNumber;
      TheIdent.MeterInfoDescription = TypeDescription;
      TheIdent.TypeOverrideString = string.Empty;
      TheIdent.MeterInfoID = 0;
      return !this.MyMeters.SaveAsNewType(TheIdent) ? 1 : 0;
    }

    public bool SpecialFunction(SpecialFunctionSelection TheSelection, object TheParameterObject)
    {
      if (TheSelection != SpecialFunctionSelection.Err2008)
        return false;
      if (this.ErrForm == null)
        this.ErrForm = new ERR8002_Form(this);
      try
      {
        int num = (int) this.ErrForm.ShowDialog();
      }
      catch
      {
      }
      return true;
    }

    public int checkConnection() => this.checkConnection(out string _);

    public int checkConnection(out string theFirmwareVersion)
    {
      theFirmwareVersion = "0";
      return !this.MyMeters.ConnectMeter() ? 1 : 0;
    }

    public bool IdentConnectedMeter(out ZR_MeterIdent theIdentity)
    {
      return this.MyMeters.IdentConnectedMeter(out theIdentity);
    }

    public bool IsReadMeterUnchanged()
    {
      return this.MyMeters.ReadMeter != null && this.MyMeters.ReadMeter.MyCommunication.VerifyCheckSum(false);
    }

    public bool ReadConnectedDevice()
    {
      ZR_ClassLibMessages.ClearErrors();
      return this.MyMeters.ReadConnectedMeter();
    }

    public bool ReadConnectedDevice(out ZR_MeterIdent theIdentity)
    {
      ZR_ClassLibMessages.ClearErrors();
      theIdentity = this.MyMeters.ConnectedMeter.MyIdent;
      return this.MyMeters.ReadConnectedMeter();
    }

    public int progDevice() => !this.progDevice(DateTime.Now, false, false, false, false) ? 99 : 0;

    public bool progDevice(DateTime NewMeterTime)
    {
      return this.MyMeters.ProgramDevice(NewMeterTime, false, false, false, false, false);
    }

    public bool progDevice(
      DateTime NewMeterTime,
      bool SetWriteProtect,
      bool DisableReset,
      bool DisableTimeUpdate,
      bool DisableDbWrite)
    {
      return this.MyMeters.ProgramDevice(NewMeterTime, SetWriteProtect, DisableReset, DisableTimeUpdate, DisableDbWrite, false);
    }

    public bool progDevice(
      DateTime NewMeterTime,
      bool SetWriteProtect,
      bool DisableReset,
      bool DisableTimeUpdate,
      bool DisableDbWrite,
      bool HoldReadMeter)
    {
      return this.MyMeters.ProgramDevice(NewMeterTime, SetWriteProtect, DisableReset, DisableTimeUpdate, DisableDbWrite, HoldReadMeter);
    }

    public bool progDeviceOrigional() => this.MyMeters.ProgramDeviceOrigional();

    public bool ClearWriteProtection()
    {
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.ClearWriteProtection();
    }

    public bool SetWriteProtection()
    {
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.SetWriteProtection();
    }

    public bool GetFirmwareVersion(out long FirmwareVersion)
    {
      FirmwareVersion = 0L;
      if (this.MyMeters.WorkMeter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return false;
      }
      FirmwareVersion = this.MyMeters.WorkMeter.MyIdent.lFirmwareVersion;
      return true;
    }

    public int getVersionNr(out string FirmwareVersion)
    {
      FirmwareVersion = "00.00";
      if (this.MyMeters.WorkMeter == null)
        return 1;
      FirmwareVersion = this.MyMeters.WorkMeter.MyIdent.sFirmwareVersion;
      return 0;
    }

    public bool GetIdent(out ZR_MeterIdent TheIdent)
    {
      TheIdent = (ZR_MeterIdent) null;
      if (this.MyMeters.WorkMeter == null)
        return false;
      TheIdent = this.MyMeters.WorkMeter.MyIdent.Clone();
      return true;
    }

    public bool GetIdent(ZR_HandlerFunctions.MeterObjects TheObject, out ZR_MeterIdent TheIdent)
    {
      TheIdent = (ZR_MeterIdent) null;
      switch (TheObject)
      {
        case ZR_HandlerFunctions.MeterObjects.Work:
          if (this.MyMeters.WorkMeter != null)
          {
            TheIdent = this.MyMeters.WorkMeter.MyIdent.Clone();
            return true;
          }
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          if (this.MyMeters.TypeMeter != null)
          {
            TheIdent = this.MyMeters.TypeMeter.MyIdent.Clone();
            return true;
          }
          break;
      }
      return false;
    }

    public int getSerialNr(out string SerialNr)
    {
      SerialNr = "00000000";
      if (this.MyMeters.WorkMeter == null)
        return 1;
      SerialNr = this.MyMeters.WorkMeter.MyIdent.SerialNr;
      return 0;
    }

    public int setDeviceTime(DateTime newDeviceTime)
    {
      if (this.MyMeters.WorkMeter == null || this.MyMeters.WorkMeter.MyCommunication == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return 1;
      }
      return !this.MyMeters.WorkMeter.MyCommunication.setDeviceTime(newDeviceTime) ? 1 : 0;
    }

    public bool getDeviceTime(out DateTime ClockTime, out DateTime NextEventTime)
    {
      ClockTime = DateTime.MinValue;
      NextEventTime = DateTime.MinValue;
      return this.MyMeters.ReadMeter != null && this.MyMeters.ReadMeter.MyCommunication.getDeviceTime(out ClockTime, out NextEventTime);
    }

    public SortedList<MeterDBAccess.ValueTypes, string> GetActualValues()
    {
      SortedList<MeterDBAccess.ValueTypes, string> sortedList = (SortedList<MeterDBAccess.ValueTypes, string>) null;
      return this.MyMeters.WorkMeter == null ? sortedList : this.MyMeters.WorkMeter.GetActualValues();
    }

    public bool GetPalettData(bool AllVersions, out PalettData TheData)
    {
      TheData = (PalettData) null;
      if (this.MyMeters.WorkMeter == null)
        return true;
      this.MyMeters.WorkMeter.MyPalette = new FunctionPalette(this.MyMeters.WorkMeter);
      return this.MyMeters.WorkMeter.MyPalette.GetPalettData(AllVersions, out TheData);
    }

    public bool checkFunctionInPaletteDisplayFunction(int FunctionNumber)
    {
      return this.MyMeters.WorkMeter.MyPalette.IsNewestFunctionVersion((ushort) FunctionNumber);
    }

    public int getPaletteFunctionLCD(
      int FunctionNr,
      out bool[] LCDSegments,
      out string Resources,
      int x,
      int y)
    {
      Resources = string.Empty;
      if (this.MyMeters.WorkMeter.MyPalette.GetLCDList((ushort) FunctionNr, out LCDSegments))
        return 0;
      LCDSegments = new bool[128];
      return 1;
    }

    public bool GetOverrideParameterList(out SortedList TheList)
    {
      TheList = (SortedList) null;
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.GetOverrideParameterList(out TheList);
    }

    public bool GetBaudrateList(out string[] TheList)
    {
      TheList = (string[]) null;
      if (this.MyMeters.WorkMeter == null)
        return false;
      TheList = this.MyMeters.WorkMeter.GetBaudrateList();
      return true;
    }

    public GlobalDeviceId GetDeviceIdentification()
    {
      return this.MyMeters.WorkMeter == null ? (GlobalDeviceId) null : this.MyMeters.WorkMeter.GetGlobalDeviceIdentification();
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      return this.MyMeters.WorkMeter == null ? (SortedList<OverrideID, ConfigurationParameter>) null : this.MyMeters.WorkMeter.GetConfigurationParameters(ConfigurationType, SubDevice);
    }

    public bool SetConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.SetConfigurationParameter(parameterList, SubDevice);
    }

    public bool SetOverrideParameterFromList(SortedList TheParameterList)
    {
      return this.MyMeters.SetNewOverrides(TheParameterList);
    }

    public bool ResetAllData() => this.MyMeters.ResetAllData();

    public bool ChangeMeterData(List<Parameter.ParameterGroups> SelectedGroups)
    {
      return this.MyMeters.ChangeMeterData(SelectedGroups);
    }

    public bool IsStructureUnchangedToReadMeter()
    {
      return this.MyMeters.ReadMeter != null && this.MyMeters.WorkMeter != null && DataChecker.IsEqualMap(this.MyMeters.WorkMeter, this.MyMeters.ReadMeter);
    }

    public bool GetMBusVariableLists(out MBusInfo TheBusInfo)
    {
      TheBusInfo = (MBusInfo) null;
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.MyMBusList.GetMBusVariableLists(out TheBusInfo);
    }

    public bool SetMBusVariables(MBusInfo TheBusInfo) => this.MyMeters.SetMBusVariables(TheBusInfo);

    public bool GetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool DeviceRead,
      out long Value)
    {
      Value = 0L;
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.GetParameterValue(Varname, Location, DeviceRead, out Value);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public bool SetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool WriteImmediatly,
      long Value)
    {
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.SetParameterValue(Varname, Location, WriteImmediatly, Value);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public bool ChangeRamParameterBitsImmediately(string Varname, uint AndMask, uint OrMask)
    {
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.ChangeRamParameterBitsImmediately(Varname, AndMask, OrMask);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public bool GetMenuFunctionData(int x, int y, out FunctionData TheData)
    {
      TheData = (FunctionData) null;
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.MyPalette.GetMenuFunctionData(x, y, out TheData);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public int setNewMenuFunction(int x, int y, int iFunctionNr)
    {
      if (this.MyMeters.WorkMeter == null)
        return 16;
      return !this.MyMeters.AddFunction(x, y, iFunctionNr) ? 1073741824 : 0;
    }

    public int deleteMenuFunction(int x, int y)
    {
      return !this.MyMeters.DeleteFunction(x, y) ? 1073741824 : 0;
    }

    public bool DeleteFunctions(ArrayList FunctionNumbersList)
    {
      return !this.MyMeters.DeleteFunctions(FunctionNumbersList);
    }

    public int moveMenuFunction(int x1, int y1, int x2, int y2) => 0;

    public List<LoggerInfo> GetLoggerInfos(
      out int LoggerMemorySize,
      MeterObjectSelector meterObject)
    {
      LoggerMemorySize = 0;
      return meterObject == MeterObjectSelector.ReadMeter ? (this.MyMeters.ReadMeter == null ? (List<LoggerInfo>) null : this.MyMeters.ReadMeter.GetLoggerInfos(out LoggerMemorySize)) : (this.MyMeters.WorkMeter == null ? (List<LoggerInfo>) null : this.MyMeters.WorkMeter.GetLoggerInfos(out LoggerMemorySize));
    }

    public int setEmergencyMode()
    {
      if (this.SerBus.SetEmergencyMode())
        return 0;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Set emergency mode error");
      return 1;
    }

    public int resetCounter()
    {
      if (!this.SerBus.ResetDevice())
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Reset device error");
        return 1;
      }
      this.ClearAll();
      return 0;
    }

    public int DeleteMeterKey(int MeterKey)
    {
      if (this.MyMeters.WorkMeter == null || this.MyMeters.WorkMeter.MyCommunication == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return 1;
      }
      return !this.MyMeters.WorkMeter.MyCommunication.MyBus.DeleteMeterKey(MeterKey) ? 1 : 0;
    }

    public bool IsWriteEnabled()
    {
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.WriteEnable;
    }

    public bool DeleteMeterKey(long LockKey)
    {
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.DeleteMeterKey(LockKey);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public bool SetMeterKey(long DeviceKey, long DatabaseKey)
    {
      if (this.MyMeters.WorkMeter != null)
        return this.MyMeters.WorkMeter.SetMeterKey(DeviceKey, DatabaseKey);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    public bool GetWriteState(
      out bool WriteIsEnabled,
      out bool WriteProtectionIsSet,
      out bool DatabaseKeyIsAvailable,
      out bool MeterKeyIsAvailable,
      out bool DatabaseKeyIsSaved,
      out bool MeterKeyIsSaved)
    {
      WriteIsEnabled = false;
      WriteProtectionIsSet = false;
      DatabaseKeyIsAvailable = false;
      MeterKeyIsAvailable = false;
      DatabaseKeyIsSaved = false;
      MeterKeyIsSaved = false;
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.GetWriteState(out WriteIsEnabled, out WriteProtectionIsSet, out DatabaseKeyIsAvailable, out MeterKeyIsAvailable, out DatabaseKeyIsSaved, out MeterKeyIsSaved);
    }

    public bool SaveKeyToDatabase(long LockKey)
    {
      return this.MyMeters.WorkMeter != null && this.MyMeters.WorkMeter.SaveKeyToDatabase(LockKey);
    }

    public bool getActualValueReadingState() => this.actualValueReadingState;

    public void setActualValueReadingState(bool theNewState)
    {
      this.actualValueReadingState = theNewState;
    }

    internal void WriteAndShowFile(string BaseName, StringBuilder TheData)
    {
      this.WriteAndShowFile(BaseName, TheData.ToString());
    }

    internal void WriteAndShowFile(string BaseName, string TheData)
    {
      string str = this.WriteInfoFile(BaseName, TheData);
      new Process() { StartInfo = { FileName = str } }.Start();
    }

    internal void WriteFilesAndShowFileDifferences(
      string BaseName1,
      StringBuilder TheData1,
      string BaseName2,
      StringBuilder TheData2)
    {
      string str1 = this.WriteInfoFile(BaseName1, TheData1.ToString());
      string str2 = this.WriteInfoFile(BaseName2, TheData2.ToString());
      new Process()
      {
        StartInfo = {
          FileName = "TortoiseMerge",
          Arguments = ("\"" + str1 + "\" \"" + str2 + "\"")
        }
      }.Start();
    }

    internal string WriteInfoFile(string BaseName, string TheData)
    {
      string str = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "LoggData");
      Directory.CreateDirectory(str);
      DateTime now = DateTime.Now;
      string path = Path.Combine(str, now.ToString("yyMMddHHmmss") + "_Log_" + BaseName + ".txt");
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        streamWriter.WriteLine("GMM-Handler diagnostic            '" + BaseName + "'         " + now.ToLongDateString() + " " + now.ToLongTimeString());
        streamWriter.Write(TheData);
        streamWriter.Flush();
        streamWriter.Close();
      }
      return path;
    }

    internal void AppandToCSV_LogFile(string TheData)
    {
      if (this.CSV_FileName.Length < 1)
      {
        this.CSV_FileName = Path.GetDirectoryName(Application.ExecutablePath);
        this.CSV_FileName = Path.Combine(this.CSV_FileName, "LoggData");
        Directory.CreateDirectory(this.CSV_FileName);
        this.CSV_FileName = Path.Combine(this.CSV_FileName, "LogFile.csv");
      }
      using (StreamWriter streamWriter = File.AppendText(this.CSV_FileName))
      {
        streamWriter.WriteLine(DateTime.Now.ToString("yyMMddHHmmss") + "\t" + TheData);
        streamWriter.Close();
      }
    }

    internal bool GetMeterObject(string SelectionString, out Meter TheMeter)
    {
      return this.GetMeterObject((ZR_HandlerFunctions.MeterObjects) Enum.Parse(typeof (ZR_HandlerFunctions.MeterObjects), SelectionString, true), out TheMeter);
    }

    internal bool GetMeterObject(ZR_HandlerFunctions.MeterObjects SelectedMeter, out Meter TheMeter)
    {
      TheMeter = (Meter) null;
      switch (SelectedMeter)
      {
        case ZR_HandlerFunctions.MeterObjects.Read:
          TheMeter = this.MyMeters.ReadMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.Work:
          TheMeter = this.MyMeters.WorkMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          TheMeter = this.MyMeters.TypeMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.DbMeter:
          TheMeter = this.MyMeters.DbMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.SavedMeter:
          TheMeter = this.MyMeters.SavedMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo0:
          TheMeter = this.MyMeters.WorkMeterUndoStack[0];
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo1:
          TheMeter = this.MyMeters.WorkMeterUndoStack[1];
          break;
        case ZR_HandlerFunctions.MeterObjects.WorkUndo2:
          TheMeter = this.MyMeters.WorkMeterUndoStack[1];
          break;
      }
      if (TheMeter != null)
        return true;
      ZR_ClassLibMessages.AddErrorDescription("Object not available");
      return false;
    }

    public bool IsEqual(
      ZR_HandlerFunctions.IsEqualFunctions CompareFunction,
      ZR_HandlerFunctions.MeterObjects OriginalMeter,
      ZR_HandlerFunctions.MeterObjects CompareMeter)
    {
      Meter TheMeter1;
      Meter TheMeter2;
      if (!this.GetMeterObject(OriginalMeter, out TheMeter1) || !this.GetMeterObject(CompareMeter, out TheMeter2))
        return false;
      switch (CompareFunction)
      {
        case ZR_HandlerFunctions.IsEqualFunctions.Map:
          return DataChecker.IsEqualMap(TheMeter1, TheMeter2);
        case ZR_HandlerFunctions.IsEqualFunctions.ProtectedArea:
          return DataChecker.IsEqualProtectedArea(TheMeter1, TheMeter2);
        case ZR_HandlerFunctions.IsEqualFunctions.AllPointers:
          return DataChecker.IsEqualAllPointers(TheMeter1, TheMeter2);
        case ZR_HandlerFunctions.IsEqualFunctions.LoggerInit:
          return DataChecker.IsLoggerEqualToTable(TheMeter1) && DataChecker.IsLoggerEqualToTable(TheMeter2);
        case ZR_HandlerFunctions.IsEqualFunctions.Overrides:
          return DataChecker.AreOverridesEqualToDatabase(TheMeter1);
        default:
          return false;
      }
    }

    public string[] GetList(
      ZR_HandlerFunctions.GetListFunctions ListFunction,
      ZR_HandlerFunctions.MeterObjects TheMeterObj)
    {
      string[] list1 = (string[]) null;
      Meter TheMeter;
      if (!this.GetMeterObject(TheMeterObj, out TheMeter))
        return list1;
      string[] list2;
      switch (ListFunction)
      {
        case ZR_HandlerFunctions.GetListFunctions.UsedFunctions:
          list2 = DataChecker.GetFunctionList(TheMeter);
          break;
        case ZR_HandlerFunctions.GetListFunctions.UsedFunctionNumbers:
          list2 = DataChecker.GetFunctionNumbersList(TheMeter);
          break;
        case ZR_HandlerFunctions.GetListFunctions.MBusParameter:
          list2 = DataChecker.GetMBusParameterList(TheMeter);
          break;
        case ZR_HandlerFunctions.GetListFunctions.MBusParameterWhithListInfo:
          list2 = DataChecker.GetMBusParameterListWithListInfo(TheMeter);
          break;
        default:
          throw new NotImplementedException();
      }
      return list2;
    }

    public enum MeterObjects
    {
      Read,
      Work,
      Type,
      DbMeter,
      SavedMeter,
      WorkUndo0,
      WorkUndo1,
      WorkUndo2,
      Old,
    }

    public enum IsEqualFunctions
    {
      Map,
      ProtectedArea,
      AllPointers,
      LoggerInit,
      Overrides,
    }

    public enum GetListFunctions
    {
      UsedFunctions,
      UsedFunctionNumbers,
      MBusParameter,
      MBusParameterWhithListInfo,
    }
  }
}
