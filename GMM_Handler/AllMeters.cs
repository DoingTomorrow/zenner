// Decompiled with JetBrains decompiler
// Type: GMM_Handler.AllMeters
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class AllMeters
  {
    internal AllMeters.RunningFunctions RunningFunction = AllMeters.RunningFunctions.NoFunction;
    internal ZR_HandlerFunctions MyHandler;
    internal Meter ConnectedMeter;
    internal Meter BaseTypeMeter;
    internal Meter TypeMeter;
    internal Meter ReadMeter;
    internal Meter DbMeter;
    internal Meter SavedMeter;
    internal Meter WorkMeter;
    internal byte[] DbMeterReadEEProm;
    private const int UndoStackLength = 5;
    internal Meter[] WorkMeterUndoStack = new Meter[5];
    internal List<LoggerInfo> LoggerDataFromMeter;
    private static string[] IdentVars = new string[4]
    {
      "EEP_Header.EEP_HEADER_SerialNr",
      "EEP_Header.EEP_HEADER_MBusSerialNr",
      "EEP_Header.EEP_HEADER_MeterID",
      "EEP_Header.EEP_HEADER_MeterKey"
    };
    private static string[] CalibrationVarsC2 = new string[42]
    {
      "DefaultFunction.kf_rl_exp_1",
      "DefaultFunction.kf_rl_exp_2",
      "DefaultFunction.kf_rl_exp_3",
      "DefaultFunction.kf_rl_exp_4",
      "DefaultFunction.kf_rl_exp_5",
      "DefaultFunction.kf_rl_exp_6",
      "DefaultFunction.kf_rl_man_1",
      "DefaultFunction.kf_rl_man_2",
      "DefaultFunction.kf_rl_man_3",
      "DefaultFunction.kf_rl_man_4",
      "DefaultFunction.kf_rl_man_5",
      "DefaultFunction.kf_rl_man_6",
      "DefaultFunction.kf_vl_exp_1",
      "DefaultFunction.kf_vl_exp_2",
      "DefaultFunction.kf_vl_exp_3",
      "DefaultFunction.kf_vl_exp_4",
      "DefaultFunction.kf_vl_exp_5",
      "DefaultFunction.kf_vl_exp_6",
      "DefaultFunction.kf_vl_man_1",
      "DefaultFunction.kf_vl_man_2",
      "DefaultFunction.kf_vl_man_3",
      "DefaultFunction.kf_vl_man_4",
      "DefaultFunction.kf_vl_man_5",
      "DefaultFunction.kf_vl_man_6",
      "DefaultFunction.tf_exp_1",
      "DefaultFunction.tf_exp_2",
      "DefaultFunction.tf_exp_3",
      "DefaultFunction.tf_exp_4",
      "DefaultFunction.tf_exp_5",
      "DefaultFunction.tf_exp_6",
      "DefaultFunction.tf_man_1",
      "DefaultFunction.tf_man_2",
      "DefaultFunction.tf_man_3",
      "DefaultFunction.tf_man_4",
      "DefaultFunction.tf_man_5",
      "DefaultFunction.tf_man_6",
      "DefaultFunction.v_cal_exp",
      "DefaultFunction.v_cal_man",
      "DefaultFunction.n_ref_exp",
      "DefaultFunction.n_ref_man",
      "DefaultFunction.o_cal_exp",
      "DefaultFunction.o_cal_man"
    };
    private static string[] CalibrationVarsWR3 = new string[48]
    {
      "DefaultFunction.n_ref_man_1",
      "DefaultFunction.n_ref_man_2",
      "DefaultFunction.n_ref_exp_1",
      "DefaultFunction.n_ref_exp_2",
      "DefaultFunction.v_cal_man_1",
      "DefaultFunction.v_cal_man_2",
      "DefaultFunction.v_cal_exp_1",
      "DefaultFunction.v_cal_exp_2",
      "DefaultFunction.o_cal_man_1",
      "DefaultFunction.o_cal_man_2",
      "DefaultFunction.o_cal_exp_1",
      "DefaultFunction.o_cal_exp_2",
      "DefaultFunction.tf_man_1",
      "DefaultFunction.tf_man_2",
      "DefaultFunction.tf_man_3",
      "DefaultFunction.tf_man_4",
      "DefaultFunction.tf_man_5",
      "DefaultFunction.tf_man_6",
      "DefaultFunction.kf_rl_man_1",
      "DefaultFunction.kf_rl_man_2",
      "DefaultFunction.kf_rl_man_3",
      "DefaultFunction.kf_rl_man_4",
      "DefaultFunction.kf_rl_man_5",
      "DefaultFunction.kf_rl_man_6",
      "DefaultFunction.kf_vl_man_1",
      "DefaultFunction.kf_vl_man_2",
      "DefaultFunction.kf_vl_man_3",
      "DefaultFunction.kf_vl_man_4",
      "DefaultFunction.kf_vl_man_5",
      "DefaultFunction.kf_vl_man_6",
      "DefaultFunction.tf_exp_1",
      "DefaultFunction.tf_exp_2",
      "DefaultFunction.tf_exp_3",
      "DefaultFunction.tf_exp_4",
      "DefaultFunction.tf_exp_5",
      "DefaultFunction.tf_exp_6",
      "DefaultFunction.kf_rl_exp_1",
      "DefaultFunction.kf_rl_exp_2",
      "DefaultFunction.kf_rl_exp_3",
      "DefaultFunction.kf_rl_exp_4",
      "DefaultFunction.kf_rl_exp_5",
      "DefaultFunction.kf_rl_exp_6",
      "DefaultFunction.kf_vl_exp_1",
      "DefaultFunction.kf_vl_exp_2",
      "DefaultFunction.kf_vl_exp_3",
      "DefaultFunction.kf_vl_exp_4",
      "DefaultFunction.kf_vl_exp_5",
      "DefaultFunction.kf_vl_exp_6"
    };

    public AllMeters(ZR_HandlerFunctions MyHandlerIn) => this.MyHandler = MyHandlerIn;

    internal bool DeleteMeter(ZR_HandlerFunctions.MeterObjects MeterObject)
    {
      switch (MeterObject)
      {
        case ZR_HandlerFunctions.MeterObjects.Read:
          this.ReadMeter = (Meter) null;
          break;
        case ZR_HandlerFunctions.MeterObjects.Work:
          this.WorkMeter = (Meter) null;
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          this.TypeMeter = (Meter) null;
          break;
        case ZR_HandlerFunctions.MeterObjects.DbMeter:
          this.DbMeter = (Meter) null;
          break;
        default:
          return false;
      }
      return true;
    }

    internal bool CopyMeter(ZR_HandlerFunctions.MeterObjects SourceMeterObject)
    {
      switch (SourceMeterObject)
      {
        case ZR_HandlerFunctions.MeterObjects.Read:
          this.SavedMeter = this.ReadMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.Work:
          this.SavedMeter = this.WorkMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          this.SavedMeter = this.TypeMeter;
          break;
        case ZR_HandlerFunctions.MeterObjects.DbMeter:
          this.SavedMeter = this.DbMeter;
          break;
        default:
          return false;
      }
      return this.SavedMeter != null;
    }

    internal bool PastMeter(ZR_HandlerFunctions.MeterObjects SourceMeterObject)
    {
      if (this.SavedMeter == null)
        return false;
      Meter MyMeterIn = this.SavedMeter.BaseClone();
      if (!MyMeterIn.CompleteTheClone(this.SavedMeter.AllParameters, true) || !MyMeterIn.GenerateEprom())
        return false;
      if (MyMeterIn.MyCommunication == null)
        MyMeterIn.MyCommunication = new MeterCommunication(MyMeterIn);
      switch (SourceMeterObject)
      {
        case ZR_HandlerFunctions.MeterObjects.Read:
          this.ReadMeter = MyMeterIn;
          break;
        case ZR_HandlerFunctions.MeterObjects.Work:
          this.WorkMeter = MyMeterIn;
          break;
        case ZR_HandlerFunctions.MeterObjects.Type:
          this.TypeMeter = MyMeterIn;
          break;
        case ZR_HandlerFunctions.MeterObjects.DbMeter:
          this.DbMeter = MyMeterIn;
          break;
        default:
          return false;
      }
      return true;
    }

    internal bool ConnectMeter()
    {
      this.ReadMeter = (Meter) null;
      this.LoggerDataFromMeter = (List<LoggerInfo>) null;
      this.ConnectedMeter = new Meter(this.MyHandler);
      return this.ConnectedMeter.ConnectMeter();
    }

    internal bool IdentConnectedMeter(out ZR_MeterIdent TheIdent)
    {
      TheIdent = (ZR_MeterIdent) null;
      if (this.ConnectedMeter == null || !this.ConnectedMeter.IdentConnectedMeter())
        return false;
      TheIdent = this.ConnectedMeter.MyIdent;
      return true;
    }

    internal bool ReadConnectedMeter()
    {
      this.RunningFunction = AllMeters.RunningFunctions.WorkMeter;
      if (this.ConnectedMeter == null)
        return false;
      this.ReadMeter = this.ConnectedMeter;
      this.ConnectedMeter = (Meter) null;
      this.BaseTypeMeter = (Meter) null;
      if (!this.ReadMeter.ReadConnectedMeter())
      {
        this.ReadMeter = (Meter) null;
        this.WorkMeter = (Meter) null;
        this.LoggerDataFromMeter = (List<LoggerInfo>) null;
        return false;
      }
      this.MyHandler.MyDataBaseAccess.WriteDailyMeterData(this.ReadMeter);
      if (this.MyHandler.LoggerRestoreState == LoggerRestor.RestoreBaseLoggers)
        this.LoggerDataFromMeter = this.ReadMeter.ReadFixedLoggers();
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.ReadMeter.BaseClone();
      this.WorkMeter.MyCommunication = new MeterCommunication(this.WorkMeter);
      if (!this.WorkMeter.CompleteTheClone(this.ReadMeter.AllParameters, false))
      {
        this.WorkMeter = (Meter) null;
        return false;
      }
      Parameter allParameter1 = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"];
      if (allParameter1.ValueEprom > 0L)
      {
        Parameter allParameter2 = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MBusSerialNr"];
        if (allParameter2.ValueEprom == 0L)
        {
          allParameter2.ValueEprom = allParameter1.ValueEprom;
          allParameter2.UpdateByteList();
          ((OverrideParameter) this.WorkMeter.MyFunctionTable.OverridesList[(object) OverrideID.MBusIdentificationNo]).Value = (ulong) allParameter2.ValueEprom;
        }
        if (this.WorkMeter.WriteEnable)
        {
          Parameter allParameter3 = (Parameter) this.WorkMeter.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
          if (allParameter3.ValueEprom != (long) byte.MaxValue && !UserRights.GlobalUserRights.CheckRight(UserRights.Rights.ProfessionalConfig))
          {
            allParameter3.ValueEprom = (long) byte.MaxValue;
            allParameter3.UpdateByteList();
          }
        }
      }
      this.WorkMeter.GenerateEprom();
      this.ReadMeter.OverrideAllLinkerObjectsWithEpromData();
      return true;
    }

    internal bool LoadMeter(ZR_MeterIdent TheMeterIdent, DateTime StorageTime)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.RunningFunction = AllMeters.RunningFunctions.WorkDBMeter;
      this.ReadMeter = (Meter) null;
      this.TypeMeter = (Meter) null;
      this.DbMeter = new Meter(this.MyHandler);
      if (!this.DbMeter.LoadMeter(TheMeterIdent, StorageTime))
      {
        this.DbMeter = (Meter) null;
        return false;
      }
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.DbMeter.BaseClone();
      if (!this.WorkMeter.CompleteTheClone(this.DbMeter.AllParameters, false))
      {
        this.WorkMeter = (Meter) null;
        return false;
      }
      this.WorkMeter.GenerateEprom();
      this.DbMeter.OverrideAllLinkerObjectsWithEpromData();
      return this.WorkMeter.AreBlocksizesUnchanged(this.DbMeter.Eprom);
    }

    internal bool LoadType(int MeterInfoID, bool DeleteReadMeter)
    {
      return this.LoadType(MeterInfoID, 0, DeleteReadMeter);
    }

    internal bool LoadType(int MeterInfoID, int FirmwareVersion, bool DeleteReadMeter)
    {
      this.RunningFunction = AllMeters.RunningFunctions.WorkType;
      ZR_ClassLibMessages.ClearErrors();
      if (DeleteReadMeter)
        this.ReadMeter = (Meter) null;
      try
      {
        if (MeterInfoID != 0 && (this.TypeMeter == null || this.TypeMeter.MyIdent.MeterInfoID != MeterInfoID || FirmwareVersion > 0 && this.TypeMeter.MyIdent.lFirmwareVersion != (long) FirmwareVersion))
        {
          this.TypeMeter = new Meter(this.MyHandler);
          bool extendedTypeEditMode = this.MyHandler.ExtendedTypeEditMode;
          this.MyHandler.ExtendedTypeEditMode = true;
          bool flag = this.TypeMeter.LoadType(MeterInfoID, FirmwareVersion);
          this.MyHandler.ExtendedTypeEditMode = extendedTypeEditMode;
          if (flag)
          {
            if (!this.MyHandler.UseOnlyDefaultValues && !this.TypeMeter.MyLinker.AreBlockAdressesUnchanged(this.TypeMeter.Eprom))
              return false;
          }
          else
            goto label_16;
        }
        if (this.TypeMeter == null)
        {
          if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.NoError)
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
          return false;
        }
        this.SaveWorkMeterToUndoStack();
        this.WorkMeter = this.TypeMeter.BaseClone();
        if (this.WorkMeter.AddDatabaseOverridesToBaseClone())
        {
          if (this.WorkMeter.CompleteTheClone(this.TypeMeter.AllParameters, false))
          {
            this.WorkMeter.MeterTime = DateTime.Now;
            this.WorkMeter.InitialiseAllTimes();
            this.WorkMeter.GenerateEprom();
          }
          else
            goto label_16;
        }
        else
          goto label_16;
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load type exception");
        goto label_16;
      }
      return true;
label_16:
      this.TypeMeter = (Meter) null;
      ZR_ClassLibMessages.AddErrorDescription("Load type error");
      return false;
    }

    internal bool LoadBaseType(ZR_MeterIdent TheIdent)
    {
      if (TheIdent.MeterInfoBaseID == 0)
        return false;
      try
      {
        if (this.TypeMeter != null && this.TypeMeter.MyIdent.MeterInfoID == TheIdent.MeterInfoBaseID)
        {
          this.BaseTypeMeter = this.TypeMeter;
        }
        else
        {
          this.BaseTypeMeter = new Meter(this.MyHandler);
          bool extendedTypeEditMode = this.MyHandler.ExtendedTypeEditMode;
          this.MyHandler.ExtendedTypeEditMode = true;
          bool flag = false;
          try
          {
            flag = this.BaseTypeMeter.LoadTypeToFunctionTable(TheIdent.MeterInfoBaseID);
          }
          catch
          {
          }
          this.MyHandler.ExtendedTypeEditMode = extendedTypeEditMode;
          if (!flag)
            goto label_10;
        }
      }
      catch
      {
        goto label_10;
      }
      return true;
label_10:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Load basetype error");
      return false;
    }

    internal bool OverloadType(string OverloadSettings)
    {
      ZR_ClassLibMessages.ClearErrors();
      try
      {
        this.SaveWorkMeterToUndoStack();
        this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.MenuOverride))
        {
          SortedList overridesList = this.WorkMeter.MyFunctionTable.OverridesList;
          this.WorkMeter.MyFunctionTable = this.TypeMeter.MyFunctionTable.Clone(this.WorkMeter);
          this.WorkMeter.MyFunctionTable.OverridesList = overridesList;
        }
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.MBusListOverride))
          this.WorkMeter.MyMBusList = this.TypeMeter.MyMBusList.Clone(this.WorkMeter);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.MBusAddress))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.MBusAddress);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.ReadingDate))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.ReadingDate);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.ReadingDate))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.ReadingDate);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.CustomID))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.CustomID);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.ModuleType))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.ModuleType);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.BaseConfig))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.BaseConfig);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.WarmerPipe))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.WarmerPipe);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.EnergyResolution))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.EnergyResolution);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.VolumeResolution))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.VolumeResolution);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.VolumePulsValue))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.VolumePulsValue);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input1Type))
          OverrideParameter.CopyIOFunctionOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, 15UL);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input1IdNumber))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input1IdNumber);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input1Unit))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input1Unit);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input1PulsValue))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input1PulsValue);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input2Type))
          OverrideParameter.CopyIOFunctionOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, 240UL);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input2IdNumber))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input2IdNumber);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input2Unit))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input2Unit);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.Input2PulsValue))
          OverrideParameter.CopyOverrideParameter(this.WorkMeter.MyFunctionTable.OverridesList, this.TypeMeter.MyFunctionTable.OverridesList, OverrideID.Input2PulsValue);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.ClearNotProtectedValues))
          OverrideParameter.ClearNotProtectedValues(this.WorkMeter.MyFunctionTable.OverridesList);
        if (OverrideParameter.OverrideIdAtString(this.TypeMeter.MyIdent.TypeOverrideString, OverrideID.ClearProtectedValues))
          OverrideParameter.ClearProtectedValues(this.WorkMeter.MyFunctionTable.OverridesList);
        if (this.WorkMeter.CompleteTheClone(this.ReadMeter.AllParameters, false))
        {
          this.WorkMeter.GenerateEprom();
          ZR_ClassLibMessages.ClearErrors();
        }
        else
          goto label_49;
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Overload type exception");
        goto label_49;
      }
      return true;
label_49:
      this.TypeMeter = (Meter) null;
      return false;
    }

    internal bool ChangeConfigLogger(Function ReplaceFunction)
    {
      try
      {
        if (this.WorkMeter.ConfigLoggers == null)
        {
          this.WorkMeter.ConfigLoggers = new SortedList<uint, Function>();
        }
        else
        {
          int index = this.WorkMeter.ConfigLoggers.IndexOfKey((uint) ReplaceFunction.Number);
          if (index >= 0)
            this.WorkMeter.ConfigLoggers.RemoveAt(index);
        }
        this.WorkMeter.ConfigLoggers.Add((uint) ReplaceFunction.Number, ReplaceFunction);
        SortedList BaseAllParameters = new SortedList();
        foreach (DictionaryEntry allParameter in this.WorkMeter.AllParameters)
        {
          Parameter parameter = (Parameter) allParameter.Value;
          if (parameter.FunctionNumber != (int) ReplaceFunction.Number)
            BaseAllParameters.Add((object) parameter.FullName, (object) parameter);
        }
        this.SaveWorkMeterToUndoStack();
        this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
        if (BaseAllParameters == null)
        {
          if (!this.WorkMeter.CompleteTheCloneToCompiledFunctions(this.WorkMeterUndoStack[0].AllParameters, false))
            return false;
        }
        else if (!this.WorkMeter.CompleteTheCloneToCompiledFunctions(BaseAllParameters, false))
          return false;
        if (!this.WorkMeter.MyMBusList.GenerateNewList() || !this.WorkMeter.CompleteTheCloneFromCreateFunctionTable())
          return false;
        this.WorkMeter.GenerateEprom();
        ZR_ClassLibMessages.ClearErrors();
      }
      catch
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Change logger configuration exception");
        this.Undo();
        goto label_25;
      }
      return true;
label_25:
      return false;
    }

    internal bool OverloadIdentAndCalibrationData(ZR_HandlerFunctions.MeterObjects SourceMeterObject)
    {
      if (SourceMeterObject == ZR_HandlerFunctions.MeterObjects.Read && this.ReadMeter != null)
      {
        Meter readMeter = this.ReadMeter;
        string key = string.Empty;
        try
        {
          this.WorkMeter.MyIdent.MeterID = readMeter.MyIdent.MeterID;
          this.WorkMeter.MyIdent.SerialNr = readMeter.MyIdent.SerialNr;
          this.WorkMeter.MyIdent.MBus_SerialNumber = readMeter.MyIdent.MBus_SerialNumber;
          this.WorkMeter.MyIdent.MBusSerialNr = readMeter.MyIdent.MBusSerialNr;
          for (int index = 0; index < AllMeters.IdentVars.Length; ++index)
          {
            key = AllMeters.IdentVars[index];
            Parameter allParameter1 = (Parameter) readMeter.AllParameters[(object) key];
            Parameter allParameter2 = (Parameter) this.WorkMeter.AllParameters[(object) key];
            allParameter2.ValueEprom = allParameter1.ValueEprom;
            allParameter2.UpdateByteList();
          }
          string[] strArray;
          if ((readMeter.MyIdent.lFirmwareVersion & 61440L) == 0L)
            strArray = AllMeters.CalibrationVarsC2;
          else if ((readMeter.MyIdent.lFirmwareVersion & 4095L) == 1L)
            strArray = AllMeters.CalibrationVarsC2;
          else if ((readMeter.MyIdent.lFirmwareVersion & 4095L) == 8L)
          {
            strArray = AllMeters.CalibrationVarsWR3;
          }
          else
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Calibration data unknown");
            return false;
          }
          for (int index = 0; index < strArray.Length; ++index)
          {
            key = strArray[index];
            Parameter allParameter3 = (Parameter) readMeter.AllParameters[(object) key];
            Parameter allParameter4 = (Parameter) this.WorkMeter.AllParameters[(object) key];
            allParameter4.ValueEprom = allParameter3.ValueEprom;
            allParameter4.UpdateByteList();
          }
        }
        catch
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Missing variable: " + key);
          return false;
        }
        return true;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
      return false;
    }

    internal bool SaveAsNewType(ZR_MeterIdent NewTypeIdent)
    {
      if (this.WorkMeter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing work meter");
        return false;
      }
      return this.WorkMeter.DeleteIdentData() && this.MyHandler.MyDataBaseAccess.SaveAsNewType(this.WorkMeter, NewTypeIdent);
    }

    internal bool SaveType(ZR_MeterIdent TypeOverrideIdent)
    {
      ZR_ClassLibMessages.ClearErrors();
      try
      {
        if (this.WorkMeter == null)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing work meter");
          goto label_17;
        }
        else if (this.TypeMeter == null)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing type meter");
          goto label_17;
        }
        else
        {
          if (this.WorkMeter.MyIdent.PPSArtikelNr == "BASETYPE")
          {
            if (!this.MyHandler.BaseTypeEditMode)
            {
              int num = (int) GMM_MessageBox.ShowMessage("Save BASETYPE", "Save type for BASETYPE's is only availible from base type edit mode!", true);
              return false;
            }
            if (GMM_MessageBox.ShowMessage("Save BASETYPE", "Are you sure to override this BASETYPE", MessageBoxButtons.OKCancel) != DialogResult.OK)
              return false;
          }
          if (this.WorkMeter.DeleteIdentData())
          {
            ZR_MeterIdent TheIdent = this.TypeMeter.MyIdent.Clone();
            if (TypeOverrideIdent != null)
            {
              TheIdent.PPSArtikelNr = TypeOverrideIdent.PPSArtikelNr;
              TheIdent.MeterInfoDescription = TypeOverrideIdent.MeterInfoDescription;
              TheIdent.TypeOverrideString = TypeOverrideIdent.TypeOverrideString;
            }
            if (this.MyHandler.MyDataBaseAccess.SaveType(this.WorkMeter, TheIdent))
              this.MyHandler.MyLoadedFunctions.ClearCache();
            else
              goto label_17;
          }
          else
            goto label_17;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Save type exception");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        goto label_17;
      }
      return true;
label_17:
      return false;
    }

    internal bool ResetAllData()
    {
      if (this.WorkMeter == null)
        return true;
      HandlerLists.GarantVarsListExists();
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
      {
        for (int index = 0; index < this.WorkMeter.AllParameters.Count; ++index)
        {
          Parameter byIndex = (Parameter) this.WorkMeter.AllParameters.GetByIndex(index);
          string key = byIndex.Name;
          if (byIndex.FunctionNumber >= 0)
            key = ((Function) this.WorkMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) byIndex.FunctionNumber]).Name + "." + byIndex.Name;
          if (HandlerLists.ConsumationDataParameters.ContainsKey(key))
          {
            byIndex.ValueCPU = 0L;
            byIndex.ValueEprom = 0L;
            byIndex.UpdateByteList();
          }
        }
        if (this.WorkMeter.MyFunctionTable.AddOverridesFromParameter())
        {
          this.WorkMeter.GenerateEprom();
          return true;
        }
      }
      this.Undo();
      return false;
    }

    internal bool ChangeMeterData(List<Parameter.ParameterGroups> SelectedGroups)
    {
      if (this.WorkMeter == null)
        return true;
      HandlerLists.GarantVarsListExists();
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
      {
        if (!this.WorkMeter.WriteEnable)
          this.WorkMeter.GenerateWriteEnableLists(true);
        for (int index = 0; index < this.WorkMeter.AllParameters.Count; ++index)
        {
          Parameter byIndex = (Parameter) this.WorkMeter.AllParameters.GetByIndex(index);
          if (this.WorkMeter.WriteEnable || (!byIndex.ExistOnEprom || this.WorkMeter.EpromWriteEnable[byIndex.Address]) && (!byIndex.ExistOnCPU || this.WorkMeter.RamWriteEnable[byIndex.AddressCPU]))
          {
            string str = byIndex.Name;
            if (byIndex.FunctionNumber >= 0)
              str = ((Function) this.WorkMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) byIndex.FunctionNumber]).Name + "." + byIndex.Name;
            foreach (Parameter.ParameterGroups selectedGroup in SelectedGroups)
            {
              if (byIndex.GroupMember[(int) selectedGroup])
              {
                switch (selectedGroup)
                {
                  case Parameter.ParameterGroups.CONSUMATION:
                    byIndex.ValueCPU = 0L;
                    byIndex.ValueEprom = 0L;
                    byIndex.UpdateByteList();
                    break;
                  case Parameter.ParameterGroups.EXTERNAL_IDENT:
                    if (str == "EEP_HEADER_MBusSerialNr")
                    {
                      Parameter allParameter = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"];
                      byIndex.ValueEprom = allParameter.ValueEprom;
                      byIndex.UpdateByteList();
                      break;
                    }
                    byIndex.ValueCPU = 0L;
                    byIndex.ValueEprom = 0L;
                    byIndex.UpdateByteList();
                    break;
                }
              }
            }
          }
        }
        if (this.WorkMeter.MyFunctionTable.AddOverridesFromParameter())
        {
          this.WorkMeter.GenerateEprom();
          return true;
        }
      }
      this.Undo();
      return false;
    }

    internal bool SetNewOverrides(SortedList NewOverrides)
    {
      if (this.WorkMeter == null)
        return true;
      if (this.MyHandler.useBaseTypeTemplate && this.BaseTypeMeter == null && !this.LoadBaseType(this.WorkMeter.MyIdent))
        return false;
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.MyHandler.useBaseTypeTemplate)
      {
        FunctionTable functionTable = this.BaseTypeMeter.MyFunctionTable.Clone(this.WorkMeter);
        this.WorkMeter.MyFunctionTable.FunctionNumbersList = functionTable.FunctionNumbersList;
        this.WorkMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList = functionTable.FunctionStartIndexOfMenuColumnList;
        if ((NewOverrides == null || this.WorkMeter.SetOverrideParameterFromList(NewOverrides)) && this.WorkMeter.CompleteTheCloneToCompiledFunctions(this.WorkMeterUndoStack[0].AllParameters, false))
        {
          MBusInfo TheInfo = (MBusInfo) null;
          if (!this.BaseTypeMeter.MyMBusList.GetMBusVariableLists(out TheInfo) || !this.WorkMeter.MyMBusList.SetMBusVariables(TheInfo) || !this.WorkMeter.CompleteTheCloneFromCreateFunctionTable())
            goto label_40;
        }
        else
          goto label_40;
      }
      else if (NewOverrides != null && !this.WorkMeter.SetOverrideParameterFromList(NewOverrides) || !this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
        goto label_40;
      this.WorkMeter.GenerateEprom();
      if (this.MyHandler.showFunctionAddDelMessages)
      {
        SortedList<ushort, int> sortedList1 = new SortedList<ushort, int>();
        foreach (ushort functionNumbers in this.WorkMeterUndoStack[0].MyFunctionTable.FunctionNumbersList)
          sortedList1.Add(functionNumbers, 0);
        SortedList<ushort, int> sortedList2 = new SortedList<ushort, int>();
        foreach (ushort functionNumbers in this.WorkMeter.MyFunctionTable.FunctionNumbersList)
          sortedList2.Add(functionNumbers, 0);
        foreach (ushort key in (IEnumerable<ushort>) sortedList1.Keys)
        {
          if (!sortedList2.ContainsKey(key))
          {
            Function fullLoadedFunction = (Function) this.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) key];
            ZR_ClassLibMessages.AddInfo(this.MyHandler.MyRes.GetString("FuncDel") + ": " + fullLoadedFunction.FullName);
          }
        }
        foreach (ushort key in (IEnumerable<ushort>) sortedList2.Keys)
        {
          if (!sortedList1.ContainsKey(key))
          {
            Function fullLoadedFunction = (Function) this.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) key];
            ZR_ClassLibMessages.AddInfo(this.MyHandler.MyRes.GetString("FuncAdd") + ": " + fullLoadedFunction.FullName);
          }
        }
      }
      return true;
label_40:
      this.Undo();
      return false;
    }

    internal bool SetMeterKey(uint MeterKey)
    {
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.CompleteTheCloneAndSetMeterKey(this.WorkMeterUndoStack[0].AllParameters, MeterKey))
      {
        this.WorkMeter.GenerateEprom();
        return true;
      }
      this.Undo();
      return false;
    }

    internal bool ProgramDevice(
      DateTime NewMeterTime,
      bool SetWriteProtect,
      bool DisableReset,
      bool DisableTimeUpdate,
      bool DisableDbWrite,
      bool HoldRead)
    {
      if (this.IfProgrammDevicePossible())
      {
        this.WorkMeter.MeterTime = NewMeterTime;
        if (!DisableTimeUpdate)
        {
          if (!this.WorkMeter.SetParameterValue("DefaultFunction.Sta_Secounds", MemoryLocation.RAM, true, (long) ZR_Calendar.Cal_GetMeterTime(NewMeterTime)))
            goto label_78;
        }
        else
        {
          long TheValue;
          if (this.WorkMeter.GetParameterValue("DefaultFunction.Sta_Secounds", MemoryLocation.RAM, true, out TheValue))
          {
            NewMeterTime = ZR_Calendar.Cal_GetDateTime((uint) TheValue);
            this.WorkMeter.MeterTime = NewMeterTime;
          }
          else
            goto label_78;
        }
        if (this.WorkMeter.InitialiseAllTimes())
        {
          if (SetWriteProtect)
          {
            Parameter allParameter = (Parameter) this.WorkMeter.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
            allParameter.ValueEprom = 0L;
            allParameter.UpdateByteList();
          }
          this.WorkMeter.GenerateEprom();
          if (this.ReadMeter != null)
          {
            for (int blockStartAddress = this.ReadMeter.MyLoggerStore.BlockStartAddress; blockStartAddress < this.WorkMeter.MyLoggerStore.BlockStartAddress; ++blockStartAddress)
              this.ReadMeter.Eprom[blockStartAddress] = ~this.WorkMeter.Eprom[blockStartAddress];
            if (!this.ReadMeter.WriteEnable)
            {
              if (this.ReadMeter.GenerateWriteEnableLists(false))
              {
                Parameter allParameter = (Parameter) this.ReadMeter.AllParameters[(object) "DefaultFunction.RAM_WriteLimit"];
                if (this.ReadMeter.EpromWriteEnable[allParameter.Address])
                {
                  ByteField data = new ByteField(1);
                  data.Add((int) byte.MaxValue);
                  if (!this.MyHandler.SerBus.WriteMemory(MemoryLocation.RAM, allParameter.AddressCPU, data))
                  {
                    ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write to ram error");
                    goto label_78;
                  }
                }
              }
              else
                goto label_78;
            }
            if (!this.WorkMeter.WriteEnableWithOpen && !this.AreAllEpromChangesProgrammable())
            {
              ZR_ClassLibMessages.ClearErrors();
              DataChecker.TryReloadWrongOldData(this.ReadMeter, this.WorkMeter);
              if (!this.AreAllEpromChangesProgrammable())
                goto label_78;
            }
            if (this.MyHandler.SerBus.SetEmergencyMode())
            {
              if (this.ReadMeter.WriteEnable || this.ReadMeter.WriteEnableWithOpen)
              {
                if (!this.MyHandler.SerBus.UpdateMemory(MemoryLocation.EEPROM, 0, new ByteField(this.ReadMeter.Eprom), new ByteField(this.WorkMeter.Eprom)))
                  goto label_78;
              }
              else
              {
                ByteField OldData = new ByteField(this.ReadMeter.Eprom.Length);
                ByteField NewData = new ByteField(this.WorkMeter.Eprom.Length);
                int extEepSize = this.ReadMeter.MyIdent.extEEPSize;
                if (extEepSize < this.WorkMeter.MyIdent.extEEPSize)
                  extEepSize = this.WorkMeter.MyIdent.extEEPSize;
                int index;
                for (index = 6; index < extEepSize; ++index)
                {
                  try
                  {
                    if (this.ReadMeter.EpromWriteEnable[index])
                    {
                      OldData.Add(this.ReadMeter.Eprom[index]);
                      NewData.Add(this.WorkMeter.Eprom[index]);
                    }
                    else if (OldData.Count > 0)
                    {
                      if (!this.MyHandler.SerBus.UpdateMemory(MemoryLocation.EEPROM, index - OldData.Count, OldData, NewData))
                      {
                        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Update eeprom memory error");
                        goto label_78;
                      }
                      else
                      {
                        OldData.Count = 0;
                        NewData.Count = 0;
                      }
                    }
                  }
                  catch
                  {
                  }
                }
                if (OldData.Count > 0 && !this.MyHandler.SerBus.UpdateMemory(MemoryLocation.EEPROM, index - OldData.Count, OldData, NewData))
                {
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Update eeprom memory error");
                  goto label_78;
                }
                else
                {
                  Parameter allParameter1 = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"];
                  Parameter allParameter2 = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FixedParamAdr"];
                  for (int valueEprom = (int) allParameter1.ValueEprom; valueEprom < (int) allParameter2.ValueEprom; ++valueEprom)
                  {
                    if ((int) this.ReadMeter.Eprom[valueEprom] != (int) this.WorkMeter.Eprom[valueEprom])
                    {
                      Parameter TheParameter;
                      while (true)
                      {
                        TheParameter = (Parameter) this.WorkMeter.AllEpromParametersByAddress[(object) valueEprom];
                        if (TheParameter == null)
                          --valueEprom;
                        else
                          break;
                      }
                      if (TheParameter.Name != "Sta_Secounds")
                      {
                        if (this.ReadMeter.RamWriteEnable[TheParameter.AddressCPU])
                        {
                          TheParameter.ValueCPU = TheParameter.ValueEprom;
                          if (!this.WorkMeter.MyCommunication.WriteParameterValue(TheParameter, MemoryLocation.RAM))
                            ZR_ClassLibMessages.AddWarning("RAM variable '" + TheParameter.FullName + "' not written.");
                        }
                        else
                          ZR_ClassLibMessages.AddWarning("Variable '" + TheParameter.FullName + "' not written! Permission not available.");
                      }
                      valueEprom += TheParameter.Size - 1;
                    }
                  }
                }
              }
            }
            else
              goto label_78;
          }
          else if (!this.MyHandler.SerBus.SetEmergencyMode())
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Ser emergency mode error");
            goto label_78;
          }
          else if (!this.MyHandler.SerBus.WriteMemory(MemoryLocation.EEPROM, 0, new ByteField(this.WorkMeter.Eprom)
          {
            Count = this.WorkMeter.MyLoggerStore.BlockStartAddress
          }))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write Error");
            goto label_78;
          }
          if (this.MyHandler.LoggerRestoreState != 0 && this.LoggerDataFromMeter != null)
            this.WorkMeter.ReprogramLoggerData(this.LoggerDataFromMeter);
          if (!DisableReset)
          {
            if (!this.MyHandler.SerBus.ResetDevice(this.WorkMeter.GetBaudrate()))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "No answer after reset");
              goto label_78;
            }
            else if (!DisableTimeUpdate && this.WorkMeter.MyLinker.AllIntervallCodes.Count > 0)
            {
              Parameter allParameter = (Parameter) this.WorkMeter.AllParameters[(object) "Itr_NextIntervalTime"];
              if (this.WorkMeter.MyCommunication.ReadParameterValue(allParameter, MemoryLocation.RAM))
              {
                if (ZR_Calendar.Cal_GetDateTime((uint) allParameter.ValueCPU) < this.WorkMeter.MeterTime)
                {
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Time initialising error!");
                  goto label_78;
                }
              }
              else
                goto label_78;
            }
          }
          if (!HoldRead)
            this.ReadMeter = (Meter) null;
          else if (this.ReadMeter != null)
          {
            if (this.ReadMeter.Eprom.Length != this.WorkMeter.Eprom.Length)
              return this.MyHandler.AddErrorPointMessage("Different eprom sizees");
            for (int index = 0; index < this.WorkMeter.Eprom.Length; ++index)
              this.ReadMeter.Eprom[index] = this.WorkMeter.Eprom[index];
          }
          if (!DisableDbWrite)
          {
            Parameter allParameter = (Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_MeterKey"];
            allParameter.ValueEprom = 0L;
            allParameter.UpdateByteList();
            allParameter.CopyToEprom(this.WorkMeter.Eprom);
            this.WorkMeter.MyIdent.SerialNr = ((Parameter) this.WorkMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).ValueEprom.ToString("X8");
            if (!this.MyHandler.MyDataBaseAccess.WriteMeterData(this.WorkMeter.Eprom, this.WorkMeter.MyLoggerStore.BlockStartAddress, this.WorkMeter.MyIdent, DateTime.Now))
              return false;
          }
          return true;
        }
      }
label_78:
      return false;
    }

    internal bool ProgramDeviceOrigional()
    {
      if (this.DbMeter == null || this.DbMeter.MeterDataState != Meter.MeterDataStates.EpromDataReloaded)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
        return false;
      }
      if (this.ConnectMeter())
      {
        if (this.DbMeter.MyIdent.lFirmwareVersion != this.ConnectedMeter.MyIdent.lFirmwareVersion)
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal Firmware");
        else if (!this.MyHandler.SerBus.SetEmergencyMode())
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Set emergency mode error.");
        else if (!this.MyHandler.SerBus.WriteMemory(MemoryLocation.EEPROM, 0, new ByteField(this.MyHandler.MyMeters.DbMeterReadEEProm)))
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write Error");
        else if (!this.MyHandler.SerBus.ResetDevice())
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Reset device error.");
        }
        else
        {
          this.ConnectedMeter = (Meter) null;
          return true;
        }
      }
      this.ConnectedMeter = (Meter) null;
      this.ReadMeter = (Meter) null;
      return false;
    }

    private bool IfProgrammDevicePossible()
    {
      Meter meter;
      if (this.ReadMeter == null)
      {
        if (!this.ConnectMeter())
          return false;
        meter = this.ConnectedMeter;
      }
      else
      {
        if (!this.ReadMeter.MyCommunication.VerifyCheckSum(true))
        {
          if (!this.WorkMeter.SecoundWriteRunning)
            return false;
          ZR_ClassLibMessages.ClearErrors();
          if (!this.ReadMeter.MyCommunication.VerifyMeterID())
            return false;
        }
        meter = this.ReadMeter;
      }
      if (this.WorkMeter.MyIdent.lFirmwareVersion != meter.MyIdent.lFirmwareVersion)
      {
        ZR_MeterIdent CompatibleIdent = this.MyHandler.MyDataBaseAccess.IsFirmwareCompatible(this.WorkMeter.MyIdent, meter.MyIdent.lFirmwareVersion);
        if (CompatibleIdent == null)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal Firmware");
          return false;
        }
        this.SaveWorkMeterToUndoStack();
        this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
        if (!this.WorkMeter.ChangeBaseCloneToCompatibeleType(CompatibleIdent) || !this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, true))
          return false;
      }
      if (this.WorkMeter.MyCommunication == null)
        this.WorkMeter.MyCommunication = new MeterCommunication(this.WorkMeter);
      this.WorkMeter.SecoundWriteRunning = true;
      return true;
    }

    private bool AreAllEpromChangesProgrammable()
    {
      if (this.ReadMeter.WriteEnable)
        return true;
      for (ushort index = 6; (int) index < this.WorkMeter.MyLoggerStore.BlockStartAddress; ++index)
      {
        if ((int) this.ReadMeter.Eprom[(int) index] != (int) this.WorkMeter.Eprom[(int) index] && !this.ReadMeter.EpromWriteEnable[(int) index])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Protected data changed");
          return false;
        }
      }
      if (this.WorkMeter.MyIdent.lFirmwareVersion < 33554432L)
      {
        try
        {
          if (((Parameter) this.WorkMeter.AllParameters[(object) "DefaultFunction.MesurementCodeLocation"]).ValueEprom == ((Parameter) this.ReadMeter.AllParameters[(object) "DefaultFunction.MesurementCodeLocation"]).ValueEprom)
          {
            if (((Parameter) this.WorkMeter.AllParameters[(object) "DefaultFunction.RuntimeCodeLocation"]).ValueEprom == ((Parameter) this.ReadMeter.AllParameters[(object) "DefaultFunction.RuntimeCodeLocation"]).ValueEprom)
            {
              if (((Parameter) this.WorkMeter.AllParameters[(object) "DefaultFunction.RuntimeVarsLocation"]).ValueEprom == ((Parameter) this.ReadMeter.AllParameters[(object) "DefaultFunction.RuntimeVarsLocation"]).ValueEprom)
                goto label_12;
            }
          }
        }
        catch
        {
        }
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Runtime size changed");
        return false;
      }
label_12:
      return true;
    }

    private void SaveWorkMeterToUndoStack()
    {
      if (this.WorkMeter == null)
        return;
      for (int index = this.WorkMeterUndoStack.Length - 2; index >= 0; --index)
        this.WorkMeterUndoStack[index + 1] = this.WorkMeterUndoStack[index];
      this.WorkMeterUndoStack[0] = this.WorkMeter;
      this.WorkMeter = (Meter) null;
    }

    internal bool Undo()
    {
      if (this.WorkMeterUndoStack[0] == null)
        return false;
      this.WorkMeter = this.WorkMeterUndoStack[0];
      for (int index = 0; index < this.WorkMeterUndoStack.Length - 1; ++index)
        this.WorkMeterUndoStack[index] = this.WorkMeterUndoStack[index + 1];
      this.WorkMeterUndoStack[this.WorkMeterUndoStack.Length - 1] = (Meter) null;
      return true;
    }

    internal void ClearUndoStack() => this.WorkMeterUndoStack = new Meter[5];

    internal bool DeleteFunction(int x, int y)
    {
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.MyFunctionTable.DeleteFunction(x, y) && this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
      {
        this.WorkMeter.MeterTime = DateTime.Now;
        this.WorkMeter.InitialiseAllTimes();
        this.WorkMeter.GenerateEprom();
        return true;
      }
      this.Undo();
      return false;
    }

    public bool DeleteFunctions(ArrayList FunctionNumbersList)
    {
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      foreach (ushort functionNumbers in FunctionNumbersList)
      {
        if (!this.WorkMeter.MyFunctionTable.DeleteFunction(functionNumbers))
          goto label_9;
      }
      if (this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
      {
        this.WorkMeter.MeterTime = DateTime.Now;
        this.WorkMeter.InitialiseAllTimes();
        this.WorkMeter.GenerateEprom();
        return true;
      }
label_9:
      this.Undo();
      return false;
    }

    internal bool RepareAndCompress()
    {
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.MyFunctionTable.RepareAndCompress() && this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
      {
        this.WorkMeter.MeterTime = DateTime.Now;
        this.WorkMeter.InitialiseAllTimes();
        this.WorkMeter.GenerateEprom();
        return true;
      }
      this.Undo();
      return false;
    }

    internal bool AddFunction(int x, int y, int FunctionNumber)
    {
      try
      {
        this.SaveWorkMeterToUndoStack();
        this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
        if (this.WorkMeter.MyFunctionTable.AddFunction(x, y, FunctionNumber))
        {
          if (this.WorkMeter.CompleteTheClone(this.WorkMeterUndoStack[0].AllParameters, false))
          {
            this.WorkMeter.MeterTime = DateTime.Now;
            this.WorkMeter.InitialiseAllTimes();
            this.WorkMeter.GenerateEprom();
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Add function exception");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
      }
      this.Undo();
      return false;
    }

    public bool SetMBusVariables(MBusInfo TheMBusInfo)
    {
      this.SaveWorkMeterToUndoStack();
      this.WorkMeter = this.WorkMeterUndoStack[0].BaseClone();
      if (this.WorkMeter.CompleteTheCloneToCompiledFunctions(this.WorkMeterUndoStack[0].AllParameters, false) && this.WorkMeter.MyMBusList.SetMBusVariables(TheMBusInfo) && this.WorkMeter.CompleteTheCloneFromCreateFunctionTable())
      {
        this.WorkMeter.MeterTime = DateTime.Now;
        this.WorkMeter.InitialiseAllTimes();
        this.WorkMeter.GenerateEprom();
        return true;
      }
      this.Undo();
      return false;
    }

    internal enum RunningFunctions
    {
      NoFunction,
      WorkType,
      WorkDBMeter,
      WorkMeter,
    }

    private enum VersionDeviceInfo : long
    {
      C2 = 1,
      C3 = 2,
      C4 = 4,
      WR3 = 8,
      DevMask = 4095, // 0x0000000000000FFF
      RevMask = 61440, // 0x000000000000F000
    }
  }
}
