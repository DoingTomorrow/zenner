// Decompiled with JetBrains decompiler
// Type: GMM_Handler.IZR_HandlerFunctions
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public interface IZR_HandlerFunctions : I_ZR_Component
  {
    bool Undo();

    LoggerRestor LoggerRestoreSetup { get; set; }

    bool BackupForEachRead { get; set; }

    bool BaseTypeEditMode { get; set; }

    bool UseBaseTypeTemplate { get; set; }

    bool ShowFunctionAddDelMessages { get; set; }

    bool ShowFunctionRemoveMessages { get; set; }

    bool ChecksumErrorsAsWarning { get; set; }

    bool setMaximumBaudrate(bool setMaxBaudrate);

    int ClearAll();

    void ClearConnectedReadAndWorkMeter();

    int openDBDevice(ref ZR_MeterIdent theIdentity, DateTime theTimePoint);

    bool GetSavedDBDeviceData(out ZR_MeterIdent theIdentity, out DateTime theTimePoint);

    int openType(int theMeterInfoID);

    bool OverloadType(string OverloadSettings);

    bool OverloadIdentAndCalibrationData(ZR_HandlerFunctions.MeterObjects SourceMeterObject);

    bool DeleteMeter(ZR_HandlerFunctions.MeterObjects MeterObject);

    bool CopyMeter(ZR_HandlerFunctions.MeterObjects SourceMeterObject);

    bool PastMeter(ZR_HandlerFunctions.MeterObjects MeterObject);

    bool IsWriteEnabled();

    bool IsMeterObjectAvailable(ZR_HandlerFunctions.MeterObjects MeterObject);

    bool IsDatabaseSwitchTrue(string Switch);

    int openType(int TypeMeterInfoID, bool ignoreCompilerError);

    bool openType(int TypeMeterInfoID, int FirmwareVersion, bool DeleteReadMeter);

    int openType(ref ZR_MeterIdent theIdentity, bool DeleteReadMeter);

    int saveType(ZR_MeterIdent TypeOverrideIdent);

    int checkConnection();

    int checkConnection(out string theFirmwareVersion);

    bool IdentConnectedMeter(out ZR_MeterIdent theIdentity);

    bool IsReadMeterUnchanged();

    bool ReadConnectedDevice();

    bool ReadConnectedDevice(out ZR_MeterIdent theIdentity);

    int progDevice();

    bool progDevice(DateTime NewMeterTime);

    bool progDevice(
      DateTime NewMeterTime,
      bool SetWriteProtect,
      bool DisableReset,
      bool DisableTimeUpdate,
      bool DisableDbWrite);

    bool progDevice(
      DateTime NewMeterTime,
      bool SetWriteProtect,
      bool DisableReset,
      bool DisableTimeUpdate,
      bool DisableDbWrite,
      bool HoldReadMeter);

    bool progDeviceOrigional();

    bool ClearWriteProtection();

    bool SetWriteProtection();

    bool GetFirmwareVersion(out long FirmwareVersion);

    int getVersionNr(out string FirmwareVersion);

    bool GetIdent(out ZR_MeterIdent TheIdent);

    bool GetIdent(ZR_HandlerFunctions.MeterObjects TheObject, out ZR_MeterIdent TheIdent);

    int getSerialNr(out string SerialNr);

    bool getDeviceTime(out DateTime ClockTime, out DateTime NextEventTime);

    bool IsStructureUnchangedToReadMeter();

    List<LoggerInfo> GetLoggerInfos(out int LoggerMemorySize, MeterObjectSelector meterObject);

    SortedList<MeterDBAccess.ValueTypes, string> GetActualValues();

    bool GetMBusVariableLists(out MBusInfo TheBusInfo);

    bool SetMBusVariables(MBusInfo TheBusInfo);

    bool GetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool DeviceRead,
      out long Value);

    bool SetParameterValue(
      string Varname,
      MemoryLocation Location,
      bool WriteImmediatly,
      long Value);

    bool ChangeRamParameterBitsImmediately(string Varname, uint AndMask, uint OrMask);

    bool DeleteMeterKey(long LockKey);

    bool SetMeterKey(long DeviceKey, long DatabaseKey);

    bool GetWriteState(
      out bool WriteIsEnabled,
      out bool WriteProtectionIsSet,
      out bool DatabaseKeyIsAvailable,
      out bool MeterKeyIsAvailable,
      out bool DatabaseKeyIsSaved,
      out bool MeterKeyIsSaved);

    bool SaveKeyToDatabase(long LockKey);

    int resetCounter();

    int setEmergencyMode();

    bool GetPalettData(bool AllVersions, out PalettData TheData);

    bool checkFunctionInPaletteDisplayFunction(int FunctionNumber);

    int getPaletteFunctionLCD(
      int FunctionNr,
      out bool[] LCDSegments,
      out string Resources,
      int x,
      int y);

    int setDeviceTime(DateTime newDeviceTime);

    bool GetOverrideParameterList(out SortedList TheList);

    bool SetOverrideParameterFromList(SortedList TheParameterList);

    GlobalDeviceId GetDeviceIdentification();

    SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice);

    bool SetConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice);

    bool ResetAllData();

    bool ChangeMeterData(List<Parameter.ParameterGroups> SelectedGroups);

    bool GetBaudrateList(out string[] TheList);

    bool GetMenuFunctionData(int x, int y, out FunctionData TheData);

    int setNewMenuFunction(int x, int y, int iFunctionNr);

    int deleteMenuFunction(int x, int y);

    bool DeleteFunctions(ArrayList FunctionNumbersList);

    int moveMenuFunction(int x1, int y1, int x2, int y2);

    bool getActualValueReadingState();

    void setActualValueReadingState(bool theNewState);

    string GetLastError();

    string getErrorMsg();

    void ShowHandlerWindow();

    string ShowHandlerWindow(string ComponentList);

    int DeleteMeterKey(int MeterKey);

    int saveAsType(ZR_MeterIdent NewTypeIdent);

    int saveAsType(string IdentNumber, string TypeDescription);

    bool SpecialFunction(SpecialFunctionSelection TheSelection, object TheParameterObject);

    bool IsEqual(
      ZR_HandlerFunctions.IsEqualFunctions CompareFunction,
      ZR_HandlerFunctions.MeterObjects OriginalMeter,
      ZR_HandlerFunctions.MeterObjects CompareMeter);

    string[] GetList(
      ZR_HandlerFunctions.GetListFunctions ListFunction,
      ZR_HandlerFunctions.MeterObjects TheMeterObj);
  }
}
