// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_AllMeters
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using AsyncCom;
using CommonWPF;
using DeviceCollector;
using GmmDbLib;
using GmmDbLib.DataSets;
using GmmDbLib.TableManagers;
using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  internal class S3_AllMeters
  {
    internal static Logger S3_AllMetersLogger = LogManager.GetLogger(nameof (S3_AllMeters));
    private S3_HandlerFunctions MyFunctions;
    internal S3_Meter TypeMeter;
    internal S3_Meter TypeMeterOriginal;
    internal S3_Meter ConnectedMeter;
    internal S3_Meter DbMeter;
    internal S3_Meter SavedMeter;
    internal S3_Meter WorkMeter;
    internal List<S3_Meter> UndoList;
    internal OverwriteWorkMeter OverwriteFromType;
    internal bool SetSleepModeOnWrite = false;
    internal bool SetWriteProtectionOnWrite = false;
    internal string openNlogLineHeader = string.Empty;
    internal SortedList<int, S3_Meter> TypeMeterCache;

    internal S3_Meter CheckedWorkMeter
    {
      get => this.WorkMeter != null ? this.WorkMeter : throw new Exception("WorkMeter not defined");
    }

    public event EventHandlerEx<int> OnProgress;

    public S3_AllMeters(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.OverwriteFromType = new OverwriteWorkMeter(this.MyFunctions, this);
    }

    internal bool NewWorkMeter(string cloneInfo) => this.NewWorkMeter((S3_Meter) null, cloneInfo);

    internal bool NewWorkMeter(S3_Meter meterToClone, string cloneInfo)
    {
      bool flag = false;
      S3_Meter s3Meter1;
      string str1;
      if (meterToClone == null)
      {
        if (this.WorkMeter != null)
        {
          flag = true;
          meterToClone = this.WorkMeter;
          s3Meter1 = this.WorkMeter.Clone(this.MyFunctions);
          str1 = "Work";
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "New WorkMeter! Clone from WorkMeter: " + cloneInfo);
          s3Meter1.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from WorkMeter"));
        }
        else if (this.ConnectedMeter != null)
        {
          meterToClone = this.ConnectedMeter;
          s3Meter1 = this.ConnectedMeter.Clone(this.MyFunctions);
          str1 = "Connected";
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "New WorkMeter! Clone from ConnectedMeter: " + cloneInfo);
          s3Meter1.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from ConnectedMeter"));
        }
        else if (this.DbMeter != null)
        {
          meterToClone = this.DbMeter;
          s3Meter1 = this.DbMeter.Clone(this.MyFunctions);
          str1 = "Db";
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "New WorkMeter! Clone from DbMeter: " + cloneInfo);
          s3Meter1.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from DbMeter"));
        }
        else if (this.TypeMeter != null)
        {
          meterToClone = this.TypeMeter;
          s3Meter1 = this.TypeMeter.Clone(this.MyFunctions);
          str1 = "Type";
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "New WorkMeter! Clone from TypeMeter: " + cloneInfo);
          s3Meter1.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from TypeMeter"));
        }
        else
        {
          string TheDescription = "Base object for clone not availabel";
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, TheDescription);
          S3_AllMeters.S3_AllMetersLogger.Info(TheDescription + "; CloneInfo: " + cloneInfo);
          return false;
        }
      }
      else
      {
        string str2 = "Special";
        if (meterToClone == this.WorkMeter)
          str2 = "Work";
        else if (meterToClone == this.ConnectedMeter)
          str2 = "Connected";
        else if (meterToClone == this.DbMeter)
          str2 = "Connected";
        else if (meterToClone == this.TypeMeter)
          str2 = "Type";
        s3Meter1 = meterToClone.Clone(this.MyFunctions);
        s3Meter1.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from: " + str2));
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "New WorkMeter! Clone from " + str2 + ": " + cloneInfo);
      }
      if (s3Meter1 == null)
        return false;
      if (flag)
        this.SaveWorkMeterForUndo();
      if (cloneInfo == null)
      {
        cloneInfo = "no undo info available";
        ZR_ClassLibMessages.AddWarning("No undo info available");
      }
      s3Meter1.CloneIndex = meterToClone != this.WorkMeter ? 0 : meterToClone.CloneIndex + 1;
      s3Meter1.CloneFlowName = "Work";
      if (s3Meter1.CloneIndex > 0)
      {
        S3_Meter s3Meter2 = s3Meter1;
        s3Meter2.CloneFlowName = s3Meter2.CloneFlowName + "[" + s3Meter1.CloneIndex.ToString() + "]";
      }
      if (meterToClone.CloneFlowName != null)
      {
        int length = meterToClone.CloneFlowName.IndexOf('<');
        if (length < 0)
        {
          S3_Meter s3Meter3 = s3Meter1;
          s3Meter3.CloneFlowName = s3Meter3.CloneFlowName + "<" + meterToClone.CloneFlowName;
        }
        else
        {
          S3_Meter s3Meter4 = s3Meter1;
          s3Meter4.CloneFlowName = s3Meter4.CloneFlowName + "<" + meterToClone.CloneFlowName.Substring(0, length);
        }
      }
      s3Meter1.CloneInfo = cloneInfo;
      this.WorkMeter = s3Meter1;
      return true;
    }

    internal bool SetConnectedMeterFromWorkMeter()
    {
      if (this.WorkMeter == null || this.ConnectedMeter == null || this.MyFunctions.WriteEnabled)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "SetConnectedMeterFromWorkMeter: Write state not complete");
        return false;
      }
      this.ConnectedMeter = this.WorkMeter;
      this.NewWorkMeter(nameof (SetConnectedMeterFromWorkMeter));
      this.MyFunctions.WriteEnabled = true;
      return true;
    }

    internal string[] GetCloneInfo()
    {
      string[] cloneInfo = (string[]) null;
      if (this.WorkMeter != null)
      {
        if (this.UndoList != null && this.UndoList.Count > 0)
        {
          cloneInfo = new string[this.UndoList.Count + 1];
          for (int index = 0; index < this.UndoList.Count; ++index)
            cloneInfo[index + 1] = this.UndoList[index].CloneInfo;
        }
        else
          cloneInfo = new string[1];
        cloneInfo[0] = this.WorkMeter.CloneInfo;
      }
      return cloneInfo;
    }

    internal int UndoCount
    {
      get
      {
        return this.WorkMeter != null && this.UndoList != null && this.UndoList.Count > 0 ? this.UndoList.Count : 0;
      }
    }

    internal bool Undo()
    {
      if (this.UndoList == null)
      {
        if (this.WorkMeter == null)
          return false;
        this.WorkMeter = (S3_Meter) null;
      }
      else
      {
        this.WorkMeter = this.UndoList[0];
        this.UndoList.RemoveAt(0);
      }
      return true;
    }

    internal void SaveWorkMeterForUndo()
    {
      if (this.UndoList == null)
        this.UndoList = new List<S3_Meter>();
      this.UndoList.Insert(0, this.WorkMeter);
      while (this.UndoList.Count > 10)
        this.UndoList.RemoveAt(10);
    }

    internal bool SaveMeter(S3_Meter meterToSave)
    {
      if (meterToSave == null)
        return false;
      this.SavedMeter = meterToSave;
      return true;
    }

    internal S3_Meter GetMeterObject(MeterObjects TheSelect)
    {
      switch (TheSelect)
      {
        case MeterObjects.WorkMeter:
          return this.MyFunctions.MyMeters.WorkMeter;
        case MeterObjects.ConnectedMeter:
          return this.MyFunctions.MyMeters.ConnectedMeter;
        case MeterObjects.DbMeter:
          return this.MyFunctions.MyMeters.DbMeter;
        case MeterObjects.TypeMeter:
          return this.MyFunctions.MyMeters.TypeMeter;
        case MeterObjects.TypeMeterOriginal:
          return this.MyFunctions.MyMeters.TypeMeterOriginal;
        case MeterObjects.SavedMeter:
          return this.MyFunctions.MyMeters.SavedMeter;
        case MeterObjects.Undo1Meter:
          return this.MyFunctions.MyMeters.UndoList != null && this.MyFunctions.MyMeters.UndoList.Count > 0 ? this.MyFunctions.MyMeters.UndoList[0] : (S3_Meter) null;
        case MeterObjects.Undo2Meter:
          return this.MyFunctions.MyMeters.UndoList != null && this.MyFunctions.MyMeters.UndoList.Count > 1 ? this.MyFunctions.MyMeters.UndoList[1] : (S3_Meter) null;
        default:
          return (S3_Meter) null;
      }
    }

    internal string GetMeterObjectName(S3_Meter theObject)
    {
      if (theObject == this.WorkMeter)
        return "Work";
      if (theObject == this.TypeMeter)
        return "Type";
      if (theObject == this.TypeMeterOriginal)
        return "TypeOriginal";
      if (theObject == this.ConnectedMeter)
        return "Connected";
      if (theObject == this.DbMeter)
        return "Database";
      if (theObject == this.SavedMeter)
        return "Saved";
      if (this.UndoList != null)
      {
        for (int index = 0; index < this.UndoList.Count; ++index)
        {
          if (theObject == this.UndoList[index])
            return "Undo" + index.ToString();
        }
      }
      return "???";
    }

    internal void Clear()
    {
      this.TypeMeterOriginal = (S3_Meter) null;
      this.TypeMeter = (S3_Meter) null;
      this.DbMeter = (S3_Meter) null;
      this.ConnectedMeter = (S3_Meter) null;
      this.WorkMeter = (S3_Meter) null;
      this.UndoList = (List<S3_Meter>) null;
      this.MyFunctions.SendMessageSwitchThread(21, GMM_EventArgs.MessageType.StatusChanged);
    }

    internal void ClearWorkLine()
    {
      this.WorkMeter = (S3_Meter) null;
      this.UndoList = (List<S3_Meter>) null;
    }

    internal void ClearReadLine()
    {
      this.ConnectedMeter = (S3_Meter) null;
      this.WorkMeter = (S3_Meter) null;
      this.UndoList = (List<S3_Meter>) null;
    }

    internal S3_Meter GetActiveMeter()
    {
      if (this.WorkMeter != null)
        return this.WorkMeter;
      if (this.ConnectedMeter != null)
        return this.ConnectedMeter;
      if (this.DbMeter != null)
        return this.DbMeter;
      return this.TypeMeter != null ? this.TypeMeter : (S3_Meter) null;
    }

    internal bool GarantBaseTypeLoaded(out int changedBaseTypeId)
    {
      changedBaseTypeId = -1;
      int intValue;
      if (this.MyFunctions.lockLoadedTypeAsBaseType && this.TypeMeter != null)
      {
        intValue = this.TypeMeter.MyParameters.ParameterByName["Con_MeterInfoId"].GetIntValue();
        changedBaseTypeId = intValue;
      }
      else
        intValue = this.WorkMeter.MyParameters.ParameterByName["Con_BaseTypeId"].GetIntValue();
      if (this.TypeMeter != null && ((long) this.TypeMeter.MyIdentification.BaseTypeId == (long) intValue || (long) this.TypeMeter.MyIdentification.MeterInfoId == (long) intValue) || this.OpenType(intValue))
        return true;
      if (!this.MyFunctions.noAdditionalBaseTypeMissingMessages)
      {
        ZR_ClassLibMessages.ClearErrors();
        ZR_ClassLibMessages.AddWarning("Base type not found! baseTypeId: " + intValue.ToString());
        this.MyFunctions.noAdditionalBaseTypeMissingMessages = true;
      }
      return false;
    }

    internal bool OpenType(string TypeCreationString) => this.OpenType(TypeCreationString, true);

    private bool OpenType(string TypeCreationString, bool CreateWorkMeter)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + ">== Open type from type creation string: " + TypeCreationString);
      this.AddOpenNlogLineHeader();
      if (TypeCreationString == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing TypeCreationString");
      string str = (string) null;
      string[] strArray1 = TypeCreationString.Split(';');
      S3_Meter s3Meter = (S3_Meter) null;
      bool baseTypeEditMode = this.MyFunctions.baseTypeEditMode;
      try
      {
        this.MyFunctions.baseTypeEditMode = true;
        if (this.WorkMeter != null)
          this.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Work meter saved to prepare TypeMeter"));
        s3Meter = this.WorkMeter;
        this.WorkMeter = (S3_Meter) null;
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Set tempSavedWorkMeter = WorkMeter; WorkMeter = null");
        this.AddOpenNlogLineHeader();
        int MeterInfoId1 = int.Parse(strArray1[0]);
        if (!this.OpenType(MeterInfoId1))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "BaseType open error. MeterInfoId: " + MeterInfoId1.ToString());
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "--> BaseType opend");
        this.WorkMeter.TypeCreationString = TypeCreationString;
        this.WorkMeter.TypeCreationStringReplaced = str;
        this.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Continue on TypeCreationString: " + TypeCreationString));
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Insert groups from all parameter types");
        for (int index1 = 1; index1 < strArray1.Length; ++index1)
        {
          string[] strArray2 = strArray1[index1].Split('=');
          try
          {
            string s;
            if (strArray2[0].Contains(":"))
            {
              string[] strArray3 = strArray2[0].Split(':');
              if (!(strArray3[0] == "SP"))
                s = strArray3[1];
              else
                continue;
            }
            else
              s = strArray2[0];
            int MeterInfoId2 = int.Parse(s);
            string[] strArray4 = strArray2[1].Split(',');
            if (!this.OpenType(MeterInfoId2))
              return ZR_ClassLibMessages.AddErrorDescription("ParameterType open error. MeterInfoId: " + MeterInfoId2.ToString());
            S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "--> ParameterType opend");
            bool[] OverwriteSelection = new bool[21];
            for (int index2 = 0; index2 < strArray4.Length; ++index2)
              OverwriteSelection[(int) OverwriteWorkMeter.OverwriteSelectionShortcutsList[strArray4[index2]].overwriteSelection] = true;
            if (!this.OverwriteWorkFromType(OverwriteSelection, OverwriteOptions.None))
            {
              ZR_ClassLibMessages.AddErrorDescription(this.openNlogLineHeader + "Overwrite error on ParameterType. MeterInfoId: " + MeterInfoId2.ToString());
              return false;
            }
            S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "--> BaseType data changed from ParameterType");
          }
          catch (Exception ex)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, ex.ToString());
            ZR_ClassLibMessages.AddErrorDescription("Illegal ParameterType defination. Number: " + index1.ToString());
            return false;
          }
        }
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Insert groups finished -> compile");
        this.WorkMeter.Compile();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Illegal BaseType defination");
        return false;
      }
      finally
      {
        this.MyFunctions.baseTypeEditMode = baseTypeEditMode;
        this.TypeMeter = this.WorkMeter;
        this.TypeMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("TypeMeter finished. MeterInfoId: " + this.TypeMeter.MyIdentification.MeterInfoId.ToString()));
        this.WorkMeter = s3Meter;
        if (this.WorkMeter != null)
          this.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Work meter restored"));
        this.SubOpenNlogLineHeader();
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Set TypeMeter == WorkMeter ; WorkMeter = tempSavedWorkMeter.");
        this.SubOpenNlogLineHeader();
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "<== Open type from type creation string finished");
      }
      bool flag = true;
      if (CreateWorkMeter)
        flag = this.SetWorkMeterFromType();
      return flag;
    }

    internal bool OpenType(int MeterInfoId)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + ">== Open type from MeterInfoId: " + MeterInfoId.ToString());
      this.AddOpenNlogLineHeader();
      try
      {
        if (!this.GetDbType(MeterInfoId, out this.TypeMeter))
          return false;
        if (this.TypeMeter.TypeCreationString != null && !this.MyFunctions.baseTypeEditMode)
        {
          this.TypeMeterOriginal = this.MyFunctions.MyMeters.TypeMeter;
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "TypeMeterOriginal = TypeMeter");
          if (!this.OpenType(this.TypeMeter.TypeCreationString, false))
            return false;
          this.TypeMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Open type by TypeCreationString out of MeterInfoId finished: " + MeterInfoId.ToString()));
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "localSavedMeter = this.WorkMeter ; WorkMeter = TypeMeter.Clone()");
          S3_Meter workMeter = this.WorkMeter;
          this.WorkMeter = this.TypeMeter.Clone(this.MyFunctions);
          this.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("WorkMeter cloned from TypeMeter"));
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Copy type ident data from TypeMeterOriginal");
          S3_Parameter s3Parameter1 = this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()];
          SortedList<string, S3_Parameter> parameterByName1 = this.TypeMeterOriginal.MyParameters.ParameterByName;
          S3_ParameterNames s3ParameterNames = S3_ParameterNames.Bak_HardwareAndRestrictions;
          string key1 = s3ParameterNames.ToString();
          int ushortValue = (int) parameterByName1[key1].GetUshortValue();
          s3Parameter1.SetUshortValue((ushort) ushortValue);
          SortedList<string, S3_Parameter> parameterByName2 = this.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_HardwareTypeId;
          string key2 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter2 = parameterByName2[key2];
          SortedList<string, S3_Parameter> parameterByName3 = this.TypeMeterOriginal.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_HardwareTypeId;
          string key3 = s3ParameterNames.ToString();
          int uintValue1 = (int) parameterByName3[key3].GetUintValue();
          s3Parameter2.SetUintValue((uint) uintValue1);
          SortedList<string, S3_Parameter> parameterByName4 = this.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_MeterInfoId;
          string key4 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter3 = parameterByName4[key4];
          SortedList<string, S3_Parameter> parameterByName5 = this.TypeMeterOriginal.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_MeterInfoId;
          string key5 = s3ParameterNames.ToString();
          int uintValue2 = (int) parameterByName5[key5].GetUintValue();
          s3Parameter3.SetUintValue((uint) uintValue2);
          SortedList<string, S3_Parameter> parameterByName6 = this.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_MeterTypeId;
          string key6 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter4 = parameterByName6[key6];
          SortedList<string, S3_Parameter> parameterByName7 = this.TypeMeterOriginal.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_MeterTypeId;
          string key7 = s3ParameterNames.ToString();
          int uintValue3 = (int) parameterByName7[key7].GetUintValue();
          s3Parameter4.SetUintValue((uint) uintValue3);
          SortedList<string, S3_Parameter> parameterByName8 = this.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_SAP_MaterialNumber;
          string key8 = s3ParameterNames.ToString();
          S3_Parameter s3Parameter5 = parameterByName8[key8];
          SortedList<string, S3_Parameter> parameterByName9 = this.TypeMeterOriginal.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_SAP_MaterialNumber;
          string key9 = s3ParameterNames.ToString();
          int uintValue4 = (int) parameterByName9[key9].GetUintValue();
          s3Parameter5.SetUintValue((uint) uintValue4);
          this.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
          this.WorkMeter.MyResources.ReloadResources();
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Load all configuration Parameter from TypeMeterOriginal");
          SortedList<OverrideID, ConfigurationParameter>[] parameterLists = new SortedList<OverrideID, ConfigurationParameter>[4]
          {
            this.TypeMeterOriginal.GetConfigurationParameters(0, (SortedList<OverrideID, ConfigurationParameter>) null, true),
            this.TypeMeterOriginal.GetConfigurationParameters(1, (SortedList<OverrideID, ConfigurationParameter>) null, true),
            this.TypeMeterOriginal.GetConfigurationParameters(2, (SortedList<OverrideID, ConfigurationParameter>) null, true),
            this.TypeMeterOriginal.GetConfigurationParameters(3, (SortedList<OverrideID, ConfigurationParameter>) null, true)
          };
          int index1 = parameterLists[0].IndexOfKey(OverrideID.EndOfBattery);
          if (index1 >= 0)
            parameterLists[0].RemoveAt(index1);
          int index2 = parameterLists[0].IndexOfKey(OverrideID.EndOfCalibration);
          if (index2 >= 0)
            parameterLists[0].RemoveAt(index2);
          int index3 = parameterLists[0].IndexOfKey(OverrideID.OperatingHours);
          if (index3 >= 0)
            parameterLists[0].RemoveAt(index3);
          if (parameterLists[0].IndexOfKey(OverrideID.ShowVolumeAsMass) < 0)
          {
            ConfigurationParameter configurationParameter = new ConfigurationParameter(OverrideID.ShowVolumeAsMass);
            configurationParameter.ParameterValue = (object) false;
            parameterLists[0].Add(configurationParameter.ParameterID, configurationParameter);
          }
          if (parameterLists[0].IndexOfKey(OverrideID.ShowEnergyChecker) < 0)
          {
            ConfigurationParameter configurationParameter = new ConfigurationParameter(OverrideID.ShowEnergyChecker);
            configurationParameter.ParameterValue = (object) false;
            parameterLists[0].Add(configurationParameter.ParameterID, configurationParameter);
          }
          if (parameterLists[0].IndexOfKey(OverrideID.ShowGCAL) < 0)
          {
            ConfigurationParameter configurationParameter = new ConfigurationParameter(OverrideID.ShowGCAL);
            configurationParameter.ParameterValue = (object) false;
            parameterLists[0].Add(configurationParameter.ParameterID, configurationParameter);
          }
          if (parameterLists[0].IndexOfKey(OverrideID.Glycol) < 0)
          {
            ConfigurationParameter configurationParameter = new ConfigurationParameter(OverrideID.Glycol);
            configurationParameter.ParameterValue = (object) "off";
            parameterLists[0].Add(configurationParameter.ParameterID, configurationParameter);
          }
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "localSavedMeter = WorkMeter; WorkMeter = TypeMeter. Use WorkMeter for type configuration. ");
          bool flag = this.SetAllConfigurationParameter(parameterLists, false);
          this.TypeMeter = this.WorkMeter;
          this.TypeMeter.CloneFlowName = "TypeByTCS";
          this.TypeMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("TypeMeter ident changed to MeterInfoID: " + this.TypeMeter.MyIdentification.MeterInfoId.ToString()));
          this.WorkMeter = workMeter;
          S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "TypeMeter = WorkMeter; WorkMeter = localSavedMeter. Reload old WorkMeter");
          if (!flag)
            return false;
        }
        return this.SetWorkMeterFromType();
      }
      catch (Exception ex)
      {
        S3_AllMeters.S3_AllMetersLogger.Info(ex.ToString());
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Open type exception:");
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Open type exception");
        return false;
      }
      finally
      {
        this.SubOpenNlogLineHeader();
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "<== Open type from MeterInfoId finished");
      }
    }

    private bool SetWorkMeterFromType()
    {
      bool flag = true;
      if (this.WorkMeter == null)
      {
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "Create WorkMeter object as clone from type");
        flag = this.MyFunctions.MyMeters.NewWorkMeter(this.TypeMeter, "Work from type");
        this.MyFunctions.MyMeters.TypeMeter.MeterInfoDescription = (string) null;
        this.MyFunctions.MyMeters.TypeMeter.MeterTypeDescription = (string) null;
        if (flag)
        {
          SortedList<string, S3_Parameter> parameterByName1 = this.WorkMeter.MyParameters.ParameterByName;
          S3_ParameterNames s3ParameterNames = S3_ParameterNames.Con_HardwareTypeId;
          string key1 = s3ParameterNames.ToString();
          parameterByName1[key1].SetUintValue(this.WorkMeter.MyIdentification.HardwareTypeId);
          SortedList<string, S3_Parameter> parameterByName2 = this.WorkMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Bak_HardwareAndRestrictions;
          string key2 = s3ParameterNames.ToString();
          parameterByName2[key2].SetUshortValue((ushort) this.WorkMeter.MyIdentification.HardwareMask);
          flag = this.WorkMeter.MyResources.ReloadResources();
        }
        if (flag)
          flag = this.MyFunctions.MyMeters.WorkMeter.Compile();
        if (!flag)
        {
          this.WorkMeter = (S3_Meter) null;
          S3_AllMeters.S3_AllMetersLogger.Error(this.openNlogLineHeader + "Create clone error");
        }
      }
      else
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "WorkMeter exits. TypeMeter is not cloned to WorkMeter");
      return flag;
    }

    internal bool GetDbType(int MeterInfoId, out S3_Meter typeMeter)
    {
      S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + ">== GetDbType from MeterInfoId: " + MeterInfoId.ToString());
      this.AddOpenNlogLineHeader();
      try
      {
        typeMeter = (S3_Meter) null;
        if (this.TypeMeterCache == null)
        {
          this.TypeMeterCache = new SortedList<int, S3_Meter>();
        }
        else
        {
          int index = this.TypeMeterCache.IndexOfKey(MeterInfoId);
          if (index >= 0)
          {
            S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "+++ Load TypeMeter from cache ...");
            typeMeter = this.TypeMeterCache.Values[index];
            this.TypeMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Load TypeMeter from cache. MeterInfoId: " + this.TypeMeter.MyIdentification.MeterInfoId.ToString() + "; Adapted to MeterInfoID: " + typeMeter.MyIdentification.MeterInfoId.ToString()));
            return true;
          }
        }
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "+++ Try Load TypeMeter from data base ...");
        DataTable Table = new DataTable();
        DataRow row;
        byte[] PackedByteList;
        try
        {
          bool flag = false;
          string SQLCommand = "SELECT MeterInfo.HardwareTypeID AS HardwareTypeID ,MeterInfo.Description AS MeterInfoDescription ,MeterType.GenerateDate AS GenerateDate ,MeterType.Description AS MeterTypeDescription ,MTypeZelsius.EEPdata AS EEPdata ,MTypeZelsius.TypeOverrideString AS TypeOverrideString ,MTypeZelsius.TypeCreationString AS TypeCreationString ,HardwareType.FirmwareVersion AS FirmwareVersion ,HardwareType.HardwareVersion AS HardwareVersion ,HardwareType.MapID AS MapID FROM MeterInfo,MeterType,MTypeZelsius,HardwareType WHERE MeterInfo.MeterInfoID = " + MeterInfoId.ToString() + " AND MeterInfo.MeterTypeID = MTypeZelsius.MeterTypeID AND MeterInfo.MeterTypeID = MeterType.MeterTypeID AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID";
          try
          {
            this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, Table);
          }
          catch
          {
            flag = true;
          }
          if (flag)
            this.MyFunctions.MyDatabase.GetDataTableBySQLCommand("SELECT MeterInfo.HardwareTypeID AS HardwareTypeID ,MeterInfo.Description AS MeterInfoDescription ,MeterType.GenerateDate AS GenerateDate ,MeterType.Description AS MeterTypeDescription ,MTypeZelsius.EEPdata AS EEPdata ,MTypeZelsius.TypeOverrideString AS TypeOverrideString ,HardwareType.FirmwareVersion AS FirmwareVersion ,HardwareType.HardwareVersion AS HardwareVersion ,HardwareType.MapID AS MapID FROM MeterInfo,MeterType,MTypeZelsius,HardwareType WHERE MeterInfo.MeterInfoID = " + MeterInfoId.ToString() + " AND MeterInfo.MeterTypeID = MTypeZelsius.MeterTypeID AND MeterInfo.MeterTypeID = MeterType.MeterTypeID AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID", Table);
          if (Table.Rows.Count != 1)
          {
            S3_AllMeters.S3_AllMetersLogger.Error(this.openNlogLineHeader + "Type not found. MeterInfoID = " + MeterInfoId.ToString());
            return false;
          }
          row = Table.Rows[0];
          object obj = row["EEPdata"];
          PackedByteList = (byte[]) row["EEPdata"];
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Read type data" + ZR_Constants.SystemNewLine + ex.ToString());
          return false;
        }
        typeMeter = new S3_Meter(this.MyFunctions, PackedByteList);
        typeMeter.CloneInfo = "DbType_" + MeterInfoId.ToString();
        this.TypeMeter.CloneFlowName = "Type" + MeterInfoId.ToString();
        typeMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Create from database eeprom data. MeterInfoId: " + MeterInfoId.ToString()));
        typeMeter.MyIdentification.AddIdsFromTypeData((uint) (int) row["FirmwareVersion"], (uint) (int) row["HardwareVersion"], (uint) (int) row["HardwareTypeID"], (uint) (int) row["MapID"]);
        if (!typeMeter.CreateCompleteFromMemory())
          return false;
        if (!row.IsNull("GenerateDate"))
          typeMeter.MeterObjectGeneratedTimeStamp = ((DateTime) row["GenerateDate"]).ToLocalTime();
        if (!row.IsNull("MeterInfoDescription"))
          typeMeter.MeterInfoDescription = (string) row["MeterInfoDescription"];
        if (!row.IsNull("MeterTypeDescription"))
          typeMeter.MeterTypeDescription = (string) row["MeterTypeDescription"];
        if (!row.IsNull("TypeOverrideString"))
          typeMeter.TypeOverrideString = (string) row["TypeOverrideString"];
        if (row.Table.Columns.IndexOf("TypeCreationString") >= 0 && !row.IsNull("TypeCreationString"))
          typeMeter.TypeCreationString = (string) row["TypeCreationString"];
        this.TypeMeterCache.Add(MeterInfoId, typeMeter);
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "+++ Load TypeMeter from database done. MeterInfoId: " + MeterInfoId.ToString());
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        this.SubOpenNlogLineHeader();
        S3_AllMeters.S3_AllMetersLogger.Info(this.openNlogLineHeader + "<== GetDbType finished");
      }
    }

    private void AddOpenNlogLineHeader() => this.openNlogLineHeader += "|  ";

    private void SubOpenNlogLineHeader()
    {
      if (this.openNlogLineHeader.Length < 3)
        return;
      this.openNlogLineHeader = this.openNlogLineHeader.Substring(0, this.openNlogLineHeader.Length - 3);
    }

    internal bool SaveType(
      ref uint meterInfoId,
      string sapNumberRequest,
      DeviceHardwareIdentification hardwareIdentification,
      SaveOptions saveOption,
      string meterInfoDescription,
      string meterTypeDescription,
      string typeOverrideString)
    {
      if (this.MyFunctions.MyMeters.WorkMeter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "WorkMeter object not available");
        return false;
      }
      if (sapNumberRequest == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "SAP Number not defined");
        return false;
      }
      bool flag = false;
      this.MyFunctions.MyDatabase.DbConn.Open();
      DbTransaction TheTransaction = this.MyFunctions.MyDatabase.DbConn.BeginTransaction();
      try
      {
        DbDataAdapter dbDataAdapter = (DbDataAdapter) null;
        Schema.MeterInfoDataTable Table1 = (Schema.MeterInfoDataTable) null;
        Schema.MeterInfoRow meterInfoRow1 = (Schema.MeterInfoRow) null;
        int result = -1;
        if (meterInfoId > 0U)
        {
          string SQLCommand = "SELECT * FROM MeterInfo WHERE MeterInfoID = " + meterInfoId.ToString();
          Table1 = new Schema.MeterInfoDataTable();
          dbDataAdapter = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table1, TheTransaction);
          if (Table1.Count == 1)
          {
            meterInfoRow1 = Table1[0];
          }
          else
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Type not available (MeterInfoID)");
            goto label_68;
          }
        }
        else if (Util.TryParse(sapNumberRequest, out result))
        {
          string SQLCommand = "SELECT * FROM MeterInfo WHERE PPSArtikelNr = '" + result.ToString() + "'";
          Table1 = new Schema.MeterInfoDataTable();
          dbDataAdapter = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table1, TheTransaction);
          if (Table1.Count > 1)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "More then one identical SAP number: " + sapNumberRequest);
            goto label_68;
          }
          else if (Table1.Count == 1)
            meterInfoRow1 = Table1[0];
        }
        DateTime dateTime;
        switch (saveOption)
        {
          case SaveOptions.ErrorIfExists:
            if (meterInfoRow1 != null)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Old data exists!");
              break;
            }
            goto case SaveOptions.OverrideExists;
          case SaveOptions.OverrideExists:
            Schema.MeterTypeDataTable Table2;
            DbDataAdapter tableBySqlCommand1;
            Schema.MTypeZelsiusDataTable Table3;
            DbDataAdapter tableBySqlCommand2;
            Schema.MeterInfoRow row1;
            Schema.MTypeZelsiusRow row2;
            Schema.MeterTypeRow row3;
            if (meterInfoRow1 == null)
            {
              string SQLCommand1 = "SELECT * FROM MeterInfo WHERE 1=0";
              Table1 = new Schema.MeterInfoDataTable();
              dbDataAdapter = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand1, (DataTable) Table1, TheTransaction);
              string SQLCommand2 = "SELECT * FROM MeterType WHERE 1=0";
              Table2 = new Schema.MeterTypeDataTable();
              tableBySqlCommand1 = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand2, (DataTable) Table2, TheTransaction);
              tableBySqlCommand1.Fill((DataTable) Table2);
              string SQLCommand3 = "SELECT * FROM MTypeZelsius WHERE 1=0";
              Table3 = new Schema.MTypeZelsiusDataTable();
              tableBySqlCommand2 = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand3, (DataTable) Table3, TheTransaction);
              tableBySqlCommand2.Fill((DataTable) Table3);
              row1 = Table1.NewMeterInfoRow();
              row2 = Table3.NewMTypeZelsiusRow();
              row3 = Table2.NewMeterTypeRow();
              long newId;
              if (sapNumberRequest == SapNumberRequestValues.S3_BASETYPE.ToString() || sapNumberRequest == SapNumberRequestValues.PARAMETER_TYPE.ToString())
              {
                meterInfoId = (uint) this.MyFunctions.MyDatabase.BaseDb.GetNewId("MeterInfo_BASE");
                newId = (long) this.MyFunctions.MyDatabase.BaseDb.GetNewId("MeterType_BASE");
              }
              else
              {
                meterInfoId = (uint) this.MyFunctions.MyDatabase.BaseDb.GetNewId("MeterInfo");
                newId = (long) this.MyFunctions.MyDatabase.BaseDb.GetNewId("MeterType");
              }
              row1.MeterInfoID = (int) meterInfoId;
              row1.MeterTypeID = (int) newId;
              row2.MeterTypeID = (int) newId;
              row3.MeterTypeID = (int) newId;
              Table1.AddMeterInfoRow(row1);
              Table3.AddMTypeZelsiusRow(row2);
              Table2.AddMeterTypeRow(row3);
            }
            else
            {
              string SQLCommand4 = "SELECT * FROM MeterType WHERE MeterTypeId = " + meterInfoRow1.MeterTypeID.ToString();
              Table2 = new Schema.MeterTypeDataTable();
              tableBySqlCommand1 = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand4, (DataTable) Table2, TheTransaction);
              string SQLCommand5 = "SELECT * FROM MTypeZelsius WHERE MeterTypeId = " + meterInfoRow1.MeterTypeID.ToString();
              Table3 = new Schema.MTypeZelsiusDataTable();
              tableBySqlCommand2 = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand5, (DataTable) Table3, TheTransaction);
              if (Table3.Count != 1)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing MeterType data");
                break;
              }
              if (Table2.Count != 1)
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Missing MTypeZelsius data");
                break;
              }
              row1 = Table1[0];
              row2 = Table3[0];
              row3 = Table2[0];
            }
            this.MyFunctions.MyMeters.NewWorkMeter("prepare for save type");
            this.WorkMeter = this.MyFunctions.MyMeters.WorkMeter;
            this.WorkMeter.MyParameters.ParameterByName["Con_MeterInfoId"].SetUintValue((uint) row1.MeterInfoID);
            this.WorkMeter.MyParameters.ParameterByName["Con_MeterTypeId"].SetUintValue((uint) row1.MeterTypeID);
            if (sapNumberRequest == SapNumberRequestValues.S3_BASETYPE.ToString() || sapNumberRequest == SapNumberRequestValues.PARAMETER_TYPE.ToString())
            {
              result = 0;
              this.WorkMeter.MyParameters.ParameterByName["Con_BaseTypeId"].SetUintValue((uint) row1.MeterInfoID);
            }
            else if (sapNumberRequest == SapNumberRequestValues.NotDefined.ToString())
              result = 0;
            else if (sapNumberRequest == SapNumberRequestValues.Auto.ToString())
              result = row1.MeterInfoID;
            else if (!Util.TryParse(sapNumberRequest, out result))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "sapNumber not available");
              break;
            }
            this.WorkMeter.MyParameters.ParameterByName["Con_SAP_MaterialNumber"].SetUintValue((uint) result);
            this.WorkMeter.SetTypeTimesAndTypeConstants();
            if (!this.OverwriteFromType.OverwriteWorkWithResetValues(OverwriteOptions.CloneAndCompile))
              return false;
            if (!this.MyFunctions.MyMeters.WorkMeter.GenerateIdentificationChecksum())
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Checksum generation error (Identification block)");
            this.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
            if (hardwareIdentification != null)
            {
              if (!this.MyFunctions.MyDatabase.GetNewestHardwareTypeIdFromHardwareIdentification(hardwareIdentification, ref this.WorkMeter.MyIdentification))
                return false;
              this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].SetUintValue(this.WorkMeter.MyIdentification.HardwareTypeId);
              this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].SetUshortValue((ushort) this.WorkMeter.MyIdentification.HardwareMask);
              if (!this.MyFunctions.MyMeters.WorkMeter.GenerateIdentificationChecksum())
                return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Checksum generation error (Identification block)");
              this.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
            }
            row1.PPSArtikelNr = sapNumberRequest == null ? "NotDefined" : sapNumberRequest;
            row1.HardwareTypeID = (int) this.WorkMeter.MyIdentification.HardwareTypeId;
            row1.MeterHardwareID = !this.WorkMeter.MyIdentification.IsWR4 ? 52 : 53;
            row1.MeterTypeID = row3.MeterTypeID;
            row1.Description = meterInfoDescription;
            string str1 = sapNumberRequest;
            string str2 = SapNumberRequestValues.S3_BASETYPE.ToString();
            row1.DefaultFunctionNr = !(str1 == str2) ? this.WorkMeter.MyIdentification.BaseTypeId.ToString() : "-";
            row2.EEPdata = !this.WorkMeter.MyIdentification.IsWR4 ? this.WorkMeter.MyDeviceMemory.GetPackedByteList(new uint?(this.WorkMeter.MyIdentification.FirmwareVersion)) : this.WorkMeter.MyDeviceMemory.GetPackedByteList();
            if (typeOverrideString != null)
              row2.TypeOverrideString = typeOverrideString;
            else
              row2.SetTypeOverrideStringNull();
            if (this.WorkMeter.TypeCreationString != null)
              row2.TypeCreationString = this.WorkMeter.TypeCreationString;
            else
              row2.SetTypeCreationStringNull();
            row3.MTypeTableName = "MTypeZelsius";
            row3.Typename = "S3_Device";
            Schema.MeterTypeRow meterTypeRow = row3;
            dateTime = DateTime.Now;
            DateTime universalTime = dateTime.ToUniversalTime();
            meterTypeRow.GenerateDate = universalTime;
            row3.Description = meterTypeDescription;
            dbDataAdapter.Update((DataTable) Table1);
            tableBySqlCommand2.Update((DataTable) Table3);
            tableBySqlCommand1.Update((DataTable) Table2);
            flag = true;
            break;
          case SaveOptions.RenameExists:
            if (meterInfoRow1 != null)
            {
              if (meterInfoRow1.IsPPSArtikelNrNull())
              {
                Schema.MeterInfoRow meterInfoRow2 = meterInfoRow1;
                string ppsArtikelNr = meterInfoRow2.PPSArtikelNr;
                dateTime = DateTime.Now.ToUniversalTime();
                dateTime = dateTime.Date;
                string shortDateString = dateTime.ToShortDateString();
                meterInfoRow2.PPSArtikelNr = ppsArtikelNr + "_UsedTo_" + shortDateString;
              }
              else
              {
                Schema.MeterInfoRow meterInfoRow3 = meterInfoRow1;
                string ppsArtikelNr = meterInfoRow3.PPSArtikelNr;
                dateTime = DateTime.Now.ToUniversalTime();
                dateTime = dateTime.Date;
                string shortDateString = dateTime.ToShortDateString();
                meterInfoRow3.PPSArtikelNr = ppsArtikelNr + "_UsedTo_" + shortDateString;
              }
              dbDataAdapter.Update((DataTable) Table1);
              meterInfoRow1 = (Schema.MeterInfoRow) null;
              goto case SaveOptions.OverrideExists;
            }
            else
              goto case SaveOptions.OverrideExists;
          default:
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal save option");
            break;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on save type" + ZR_Constants.SystemNewLine + ex.ToString());
      }
      finally
      {
        if (flag)
        {
          TheTransaction.Commit();
          if (this.TypeMeterCache != null)
          {
            int index = this.TypeMeterCache.IndexOfKey((int) meterInfoId);
            if (index >= 0)
              this.TypeMeterCache.RemoveAt(index);
          }
          this.TypeMeter = (S3_Meter) null;
        }
        else
          TheTransaction.Rollback();
        this.MyFunctions.MyDatabase.DbConn.Close();
      }
label_68:
      return flag;
    }

    internal bool OverwriteWorkFromType()
    {
      return this.OverwriteFromType.OverwriteWorkFromTypeForTypeOverWrite() && this.MyFunctions.MyMeters.WorkMeter.MyMeterScaling.ReadSettingsFromParameter() && this.MyFunctions.MyMeters.WorkMeter.MyMeterScaling.WriteParameterDependencies() && this.MyFunctions.MyMeters.WorkMeter.MyResources.ReloadResources() && this.WorkMeter.Compile();
    }

    internal bool OverwriteWorkFromType(bool[] OverwriteSelection)
    {
      return this.OverwriteFromType.OverwriteRun(this.MyFunctions.MyMeters.TypeMeter, OverwriteSelection, OverwriteOptions.CloneAndCompile);
    }

    public void OverwriteSrcToWork(
      HandlerMeterObjects sourceObject,
      CommonOverwriteGroups[] overwriteGroups)
    {
      S3_Meter sourceMeter;
      switch (sourceObject)
      {
        case HandlerMeterObjects.TypeMeter:
          sourceMeter = this.TypeMeter;
          break;
        case HandlerMeterObjects.BackupMeter:
          sourceMeter = this.DbMeter;
          break;
        default:
          throw new Exception("Source meter not supported: " + sourceObject.ToString());
      }
      if (sourceMeter == null)
        throw new Exception("Source meter not available: " + sourceObject.ToString());
      bool[] overwriteSelection = new bool[21];
      foreach (CommonOverwriteGroups overwriteGroup in overwriteGroups)
      {
        switch (overwriteGroup)
        {
          case CommonOverwriteGroups.TemperatureSettings:
            overwriteSelection[10] = true;
            break;
          case CommonOverwriteGroups.TemperatureLimits:
            overwriteSelection[11] = true;
            break;
          default:
            throw new Exception("Not supported overwrite group: " + overwriteGroup.ToString());
        }
      }
      ZR_ClassLibMessages.ClearErrors();
      if (!this.OverwriteFromType.OverwriteRun(sourceMeter, overwriteSelection, OverwriteOptions.CloneAndCompile))
        throw new Exception("Overwrite error" + Environment.NewLine + ZR_ClassLibMessages.GetLastErrorMessageAndClearError());
    }

    internal bool OverwriteWorkFromType(bool[] OverwriteSelection, OverwriteOptions owOptions)
    {
      return this.OverwriteFromType.OverwriteRun(this.MyFunctions.MyMeters.TypeMeter, OverwriteSelection, owOptions);
    }

    internal bool IsTypeOverwritePossible(bool[] OverwriteSelection)
    {
      return this.OverwriteFromType.IsTypeOverwritePossible(OverwriteSelection);
    }

    internal bool IsTypeOverwritePerfect() => this.OverwriteFromType.IsTypeOverwritePerfect();

    internal bool OpenDeviceLastBackup(int MeterId)
    {
      try
      {
        string SQLCommand = "SELECT * FROM MeterData WHERE ((MeterId = " + MeterId.ToString() + ") and (PValueID = 1))  ORDER BY TimePoint DESC";
        Schema.MeterDataDataTable Table = new Schema.MeterDataDataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table);
        DateTime localTime = Table[0].TimePoint.ToLocalTime();
        return this.OpenDevice(MeterId, localTime);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Read meter data" + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
    }

    internal bool OpenLastBackupFromTimeRange(
      DateTime rangeStartTime,
      DateTime rangeEndTime,
      int MeterID)
    {
      try
      {
        string str = "SELECT MAX(TimePoint) FROM MeterData WHERE ((MeterId = " + MeterID.ToString() + ") AND (PValueID = 1) AND (TimePoint >= @rangeStartTime) AND (TimePoint <= @rangeEndTime))";
        DateTime TimePoint = DateTime.MinValue;
        using (DbConnection newConnection = this.MyFunctions.MyDatabase.BaseDb.GetNewConnection())
        {
          newConnection.Open();
          DbCommand command = newConnection.CreateCommand();
          command.CommandText = str;
          DbParameter parameter1 = command.CreateParameter();
          parameter1.DbType = DbType.DateTime;
          parameter1.ParameterName = "@rangeStartTime";
          parameter1.Value = (object) rangeStartTime.ToUniversalTime();
          command.Parameters.Add((object) parameter1);
          DbParameter parameter2 = command.CreateParameter();
          parameter2.DbType = DbType.DateTime;
          parameter2.ParameterName = "@rangeEndTime";
          parameter2.Value = (object) rangeEndTime.ToUniversalTime();
          command.Parameters.Add((object) parameter2);
          List<int> intList = new List<int>();
          using (DbDataReader dbDataReader = command.ExecuteReader())
          {
            dbDataReader.Read();
            TimePoint = Convert.ToDateTime(dbDataReader[0]).ToLocalTime();
          }
        }
        return !(TimePoint == DateTime.MinValue) && this.OpenDevice(MeterID, TimePoint);
      }
      catch
      {
        return false;
      }
    }

    internal bool OpenDevice(int MeterId, DateTime TimePoint)
    {
      try
      {
        DbDataAdapter dataAdapter = this.MyFunctions.MyDatabase.GetDataAdapter("SELECT * FROM MeterData WHERE ((MeterId = " + MeterId.ToString() + ") and (PValueID = 1)  AND (TimePoint = @TimePoint))");
        DbCommand selectCommand = dataAdapter.SelectCommand;
        IDbDataParameter parameter = (IDbDataParameter) selectCommand.CreateParameter();
        parameter.DbType = DbType.DateTime;
        parameter.ParameterName = "@TimePoint";
        parameter.Value = (object) TimePoint.ToUniversalTime();
        selectCommand.Parameters.Add((object) parameter);
        Schema.MeterDataDataTable meterDataDataTable = new Schema.MeterDataDataTable();
        dataAdapter.Fill((DataTable) meterDataDataTable);
        if (meterDataDataTable.Rows.Count != 1)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Meter data not found.");
          return false;
        }
        byte[] pvalueBinary = meterDataDataTable[0].PValueBinary;
        HardwareTypeTables.HardwareAndFirmwareInfoRow hardwareAndFirmwareInfo = this.MyFunctions.MyDatabase.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo(int.Parse(meterDataDataTable[0].PValue));
        S3_Meter s3Meter = new S3_Meter(this.MyFunctions, pvalueBinary);
        s3Meter.MyIdentification.AddIdsFromTypeData((uint) hardwareAndFirmwareInfo.FirmwareVersion, (uint) hardwareAndFirmwareInfo.HardwareVersion, (uint) hardwareAndFirmwareInfo.HardwareTypeID, (uint) hardwareAndFirmwareInfo.MapID);
        if (!s3Meter.CreateCompleteFromMemory())
          return false;
        if (!s3Meter.MyDeviceMemory.BlockHandlerInfo.CreateFromMemory())
        {
          S3_AllMeters.S3_AllMetersLogger.Error("Read HandlerInfo block!");
          return false;
        }
        s3Meter.CloneFlowName = "DbMeter" + MeterId.ToString();
        this.MyFunctions.MyMeters.DbMeter = s3Meter;
        bool flag = true;
        if (this.WorkMeter == null)
        {
          flag = this.MyFunctions.MyMeters.NewWorkMeter("work from meter backup");
          if (flag)
            flag = this.MyFunctions.MyMeters.WorkMeter.Compile();
        }
        s3Meter.HardwareTypeDescription = hardwareAndFirmwareInfo.Description;
        s3Meter.MeterObjectGeneratedTimeStamp = TimePoint;
        return flag;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Read meter data" + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
    }

    internal bool SaveDevice()
    {
      if (this.WorkMeter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "WorkMeter object not available");
        return false;
      }
      try
      {
        S3_DeviceId identification = (S3_DeviceId) this.MyFunctions.MyMeters.WorkMeter.MyIdentification;
        bool flag = false;
        if (identification.MeterId == 0U)
        {
          identification.MeterId = (uint) this.MyFunctions.MyDatabase.BaseDb.GetNewId("Meter");
          this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName["Con_MeterId"].SetUintValue(identification.MeterId);
          identification.MeterInfoId = 0U;
          this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName["Con_MeterInfoId"].SetUintValue(identification.MeterInfoId);
          flag = true;
        }
        uint num = identification.MeterInfoId;
        string SQLCommand1 = "SELECT * FROM MeterInfo WHERE MeterInfoId = " + identification.MeterInfoId.ToString();
        Schema.MeterInfoDataTable Table1 = new Schema.MeterInfoDataTable();
        DbDataAdapter tableBySqlCommand = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand1, (DataTable) Table1);
        if (Table1.Count != 1)
        {
          string SQLCommand2 = "SELECT * FROM MeterInfo WHERE MeterInfoId = " + identification.BaseTypeId.ToString();
          Schema.MeterInfoDataTable Table2 = new Schema.MeterInfoDataTable();
          tableBySqlCommand = this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand2, (DataTable) Table2);
          if (Table2.Count != 1)
            ZR_ClassLibMessages.AddWarning("BaseType: " + identification.BaseTypeId.ToString() + " not found in database.", S3_AllMeters.S3_AllMetersLogger);
          num = identification.BaseTypeId;
        }
        if (!this.MyFunctions.MyMeters.WorkMeter.GenerateIdentificationChecksum())
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Checksum generation error (Identification block)");
          return false;
        }
        BaseDbConnection baseDb = this.MyFunctions.MyDatabase.BaseDb;
        using (DbConnection newConnection = baseDb.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction transaction = newConnection.BeginTransaction();
          string selectSql = "SELECT * FROM Meter WHERE MeterId = " + identification.MeterId.ToString();
          BaseTables.MeterDataTable meterDataTable = new BaseTables.MeterDataTable();
          DbDataAdapter dataAdapter1 = baseDb.GetDataAdapter(selectSql, newConnection, transaction, out DbCommandBuilder _);
          dataAdapter1.Fill((DataTable) meterDataTable);
          if (meterDataTable.Rows.Count != 1)
          {
            BaseTables.MeterRow meterRow = meterDataTable.NewMeterRow();
            meterRow.MeterID = (int) identification.MeterId;
            meterRow.MeterInfoID = (int) num;
            meterRow.ProductionDate = DateTime.Now;
            DeviceIdentification deviceIdentification = (DeviceIdentification) new S3_CommonDeviceIdentification(this.WorkMeter);
            SortedList<string, S3_Parameter> parameterByName = this.WorkMeter.MyParameters.ParameterByName;
            string key = S3_ParameterNames.PrintedSerialNumber.ToString();
            meterRow.SerialNr = !parameterByName.ContainsKey(key) ? deviceIdentification.FullSerialNumber : deviceIdentification.PrintedSerialNumberAsString;
            if (deviceIdentification.SAP_ProductionOrderNumber != null)
              meterRow.OrderNr = deviceIdentification.SAP_ProductionOrderNumber;
            meterDataTable.AddMeterRow(meterRow);
            MeterChanges.UpdateMeterRowChanges(baseDb, meterRow, dataAdapter1);
          }
          else
          {
            if (flag)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MeterID generation error");
              transaction.Rollback();
              return false;
            }
            meterDataTable[0].MeterInfoID = (int) num;
            DeviceIdentification deviceIdentification = (DeviceIdentification) new S3_CommonDeviceIdentification(this.WorkMeter);
            SortedList<string, S3_Parameter> parameterByName = this.WorkMeter.MyParameters.ParameterByName;
            string key = S3_ParameterNames.PrintedSerialNumber.ToString();
            meterDataTable[0].SerialNr = !parameterByName.ContainsKey(key) ? deviceIdentification.FullSerialNumber : deviceIdentification.PrintedSerialNumberAsString;
            if (deviceIdentification.SAP_ProductionOrderNumber != null)
              meterDataTable[0].OrderNr = deviceIdentification.SAP_ProductionOrderNumber;
            MeterChanges.UpdateMeterRowChanges(baseDb, meterDataTable[0], dataAdapter1);
          }
          DbDataAdapter dataAdapter2 = baseDb.GetDataAdapter("SELECT * FROM MeterData", newConnection, transaction, out DbCommandBuilder _);
          BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
          BaseTables.MeterDataRow row = meterDataDataTable.NewMeterDataRow();
          row.MeterID = (int) identification.MeterId;
          row.PValue = identification.HardwareTypeId.ToString();
          DateTime now = DateTime.Now;
          row.TimePoint = now.AddMilliseconds((double) (now.Millisecond * -1));
          row.PValueID = 1;
          row.PValueBinary = this.WorkMeter.MyDeviceMemory.GetPackedByteList(new uint?(this.WorkMeter.MyIdentification.FirmwareVersion));
          meterDataDataTable.AddMeterDataRow(row);
          dataAdapter2.Update((DataTable) meterDataDataTable);
          transaction.Commit();
          newConnection.Close();
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on save meter" + ZR_Constants.SystemNewLine + ex.ToString());
        return false;
      }
      return true;
    }

    internal bool WriteClone() => this.WriteClone(true);

    internal bool WriteClone(bool ShowMessageWindow)
    {
      if (this.DbMeter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No meter backup loaded", S3_AllMeters.S3_AllMetersLogger);
      try
      {
        this.MeterJobStart(MeterJob.CloneDevice);
        this.ClearWorkLine();
        if (!this.ConnectDevice())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "No connected meter", S3_AllMeters.S3_AllMetersLogger);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("**** Create clone information ****");
        stringBuilder.AppendLine();
        bool flag = true;
        if (!this.MyFunctions.MyDatabase.GetHardwareTypeIdFromVersion((S3_DeviceId) this.ConnectedMeter.MyIdentification))
        {
          stringBuilder.AppendLine("Error on read HardwareType data out of database");
        }
        else
        {
          stringBuilder.AppendLine("Connected meter firmware version: " + this.ConnectedMeter.MyIdentification.FirmwareVersionString);
          stringBuilder.AppendLine("Database backup firmware version: " + this.DbMeter.MyIdentification.FirmwareVersionString);
          stringBuilder.AppendLine();
          if (!this.ConnectedMeter.LoadMapVars())
            stringBuilder.AppendLine("Error on load map vars out of database");
          else if (this.ConnectedMeter.MyIdentification.ReadHardwareIdentification() == null)
            stringBuilder.AppendLine("Error on read device identification data");
        }
        stringBuilder.AppendLine("Connected meter MeterId: " + this.ConnectedMeter.MyIdentification.MeterId.ToString());
        stringBuilder.AppendLine("Database backup MeterId: " + this.DbMeter.MyIdentification.MeterId.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Connected meter serial number: " + this.ConnectedMeter.MyIdentification.FullSerialNumber);
        stringBuilder.AppendLine("Database backup serial number: " + this.DbMeter.MyIdentification.FullSerialNumber);
        stringBuilder.AppendLine();
        if ((int) this.ConnectedMeter.MyIdentification.FirmwareVersion != (int) this.DbMeter.MyIdentification.FirmwareVersion)
        {
          stringBuilder.AppendLine("Clone with different firmware not possible");
          flag = false;
        }
        ulong? nullable = new ulong?();
        if (this.DbMeter.MyIdentification.IsLoRa)
        {
          try
          {
            string str = new S3_CommonDeviceIdentification(this.DbMeter).LoRa_DevEUI.Value.ToString("X16");
            string oneValue = EnterOneValue.GetOneValue("Change DevEUI", "Change DevEUI to ensure the clone device is not registered inside the LoRa server as original device. Original DevEUI = 0x" + str, "04B6488900009900");
            if ((string.IsNullOrEmpty(oneValue) || oneValue == str) && GMM_MessageBox.ShowMessage("CloneInfo", "DevEUI not changed! Write clone stopped.", MessageBoxButtons.OKCancel) != DialogResult.OK)
              return false;
            nullable = new ulong?(ulong.Parse(oneValue, NumberStyles.HexNumber));
          }
          catch (Exception ex)
          {
            ExceptionViewer.Show(ex, "Exception on DevEUI changeing");
            return false;
          }
        }
        if (this.ConnectedMeter.IsWriteProtected)
        {
          stringBuilder.AppendLine("Connected meter has write protection: Clone with write protection not possible");
          flag = false;
        }
        if (flag)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("Create clone?");
          if (ShowMessageWindow && GMM_MessageBox.ShowMessage("CloneInfo", stringBuilder.ToString(), MessageBoxButtons.OKCancel) != DialogResult.OK)
            return true;
          this.ConnectedMeter.MyDeviceMemory.ClearMemory();
          if (!this.MyFunctions.MyCommands.SetEmergencyMode())
            return false;
          if (!this.DbMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.RAM))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (RAM)", S3_AllMeters.S3_AllMetersLogger);
          if (!this.DbMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.MainFlash))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (Flash)", S3_AllMeters.S3_AllMetersLogger);
          if (nullable.HasValue)
          {
            new S3_CommonDeviceIdentification(this.ConnectedMeter).LoRa_DevEUI = new ulong?(nullable.Value);
            this.ConnectedMeter.MyDeviceMemory.WriteDataToConnectedDevice(this.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_lora_deveui_0.ToString()].BlockStartAddress, 8);
          }
          S3_AllMeters.S3_AllMetersLogger.Debug("Run backup (Code 0x08)");
          if (!this.MyFunctions.MyCommands.RunBackup())
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Backup error", S3_AllMeters.S3_AllMetersLogger);
          S3_AllMeters.S3_AllMetersLogger.Debug("ResetDevice");
          if (!this.MyFunctions.MyCommands.ResetDevice())
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Reset error", S3_AllMeters.S3_AllMetersLogger);
          if (((uint) this.DbMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].GetUshortValue() & 4096U) > 0U)
          {
            uint Key;
            MeterDBAccess.ValueTypes ValueType;
            if (!this.MyFunctions.MyDatabase.GetDeviceKeys((int) this.DbMeter.MyIdentification.MeterId, out Key, out ValueType))
              ZR_ClassLibMessages.AddWarning("Key for write protection not fond in database. Clone not protected!");
            if (ValueType == MeterDBAccess.ValueTypes.GovernmentRandomNr)
            {
              uint userKeyChecksum = (uint) UserRights.GlobalUserRights.GetUserKeyChecksum("ZelsiusLockKey");
              if (userKeyChecksum == 0U)
              {
                ZR_ClassLibMessages.AddWarning("ZelsiusLockKey for write protection not fond in database.");
                goto label_49;
              }
              else
                Key ^= userKeyChecksum;
            }
            if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey(Key))
              ZR_ClassLibMessages.AddWarning("MeterKey not cloned! (Different set?)");
            if (!this.DbMeter.SetWriteProtection())
              ZR_ClassLibMessages.AddWarning("SetWriteProtection error");
          }
label_49:
          this.MyFunctions.WriteEnabled = false;
          return true;
        }
        int num = (int) GMM_MessageBox.ShowMessage("CloneInfo", stringBuilder.ToString(), true);
        return false;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
      }
      finally
      {
        this.ConnectedMeter = (S3_Meter) null;
        this.MeterJobFinished(MeterJob.CloneDevice);
      }
      return false;
    }

    internal void MeterJobStart(MeterJob TheJob)
    {
      this.MyFunctions.BreakRequest = false;
      if (!S3_AllMeters.S3_AllMetersLogger.IsInfoEnabled)
        return;
      S3_AllMeters.S3_AllMetersLogger.Info("Start job:" + TheJob.ToString());
    }

    internal void MeterJobFinished(MeterJob JobToFinish)
    {
      if (!S3_AllMeters.S3_AllMetersLogger.IsInfoEnabled)
        return;
      S3_AllMeters.S3_AllMetersLogger.Info("Finished job: " + JobToFinish.ToString());
    }

    internal bool ConnectDevice()
    {
      try
      {
        this.MeterJobStart(MeterJob.ReadFirmwareVersion);
        this.ConnectedMeter = (S3_Meter) null;
        BusDevice selectedDevice = this.MyFunctions.MyCommands.GetSelectedDevice();
        if (selectedDevice != null && !(selectedDevice is Serie3MBus))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "DeviceCollector setup error: Not a series3 device", S3_AllMeters.S3_AllMetersLogger);
        ReadVersionData versionData;
        if (!this.MyFunctions.MyCommands.ReadVersion(out versionData))
          return ZR_ClassLibMessages.AddErrorDescription("Read version error", S3_AllMeters.S3_AllMetersLogger);
        if (versionData.Version < 67309573U)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Old firmware version. Not supported!", S3_AllMeters.S3_AllMetersLogger);
        S3_Meter s3Meter = new S3_Meter(this.MyFunctions, 36864);
        s3Meter.CloneInfo = "Connected device";
        s3Meter.MyIdentification.AddIdsFromVersion(versionData);
        s3Meter.IsWriteProtected = (versionData.HardwareIdentification & 4096U) > 0U;
        this.MyFunctions.MyMeters.ConnectedMeter = s3Meter;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read device identification.", S3_AllMeters.S3_AllMetersLogger);
      }
      finally
      {
        this.MeterJobFinished(MeterJob.ReadDeviceIdentification);
      }
      return true;
    }

    internal bool ReadHardwareIdentification(out S3_DeviceId DeviceId)
    {
      DeviceId = (S3_DeviceId) null;
      bool flag = false;
      try
      {
        this.ClearWorkLine();
        if (this.OnProgress != null)
          this.OnProgress((object) this, 10);
        if (this.ConnectDevice())
        {
          if (this.OnProgress != null)
            this.OnProgress((object) this, 15);
          this.MeterJobStart(MeterJob.ReadDeviceIdentification);
          this.MyFunctions.MyDatabase.GetHardwareTypeIdFromVersion((S3_DeviceId) this.ConnectedMeter.MyIdentification);
          if (this.ConnectedMeter.LoadMapVars())
          {
            DeviceId = this.ConnectedMeter.MyIdentification.ReadHardwareIdentification();
            if (DeviceId != null)
            {
              flag = true;
              if (this.OnProgress != null)
                this.OnProgress((object) this, 18);
            }
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read device identification.");
      }
      finally
      {
        this.MeterJobFinished(MeterJob.ReadDeviceIdentification);
      }
      return flag;
    }

    internal bool ReadConnectedDevice(ReadPartsSelection readPartsSelection = ReadPartsSelection.Dump)
    {
      bool flag = false;
      try
      {
        if (this.OnProgress != null)
          this.OnProgress((object) this, 8);
        if (this.MyFunctions.MyCommands is S3_CommandsCHANGED)
        {
          if (this.MyFunctions.MyCommands.IsSelectedDevice(DeviceTypes.ZR_Serie3))
            flag = this.MyFunctions.MyCommands.ReadVersion();
          return flag;
        }
        if (this.MyFunctions.MyCommands is S3_CommandsDCAC)
        {
          if (!this.MyFunctions.MyCommands.IsSelectedDevice(DeviceTypes.ZR_Serie3))
          {
            this.MyFunctions.Clear();
            if (this.MyFunctions.MyCommands.GetBaseMode() == BusMode.MBusPointToPoint && (UserRights.GlobalUserRights.PackageName == UserRights.Packages.ConfigurationManager.ToString() || UserRights.GlobalUserRights.PackageName == UserRights.Packages.ConfigurationManagerPro.ToString()))
            {
              this.MyFunctions.MyCommands.DeleteBusInfo();
              if (this.MyFunctions.MyCommands.AddDevice(DeviceTypes.ZR_Serie3, 0))
              {
                this.MyFunctions.MyCommands.SingleParameter(CommParameter.Wakeup, WakeupSystem.BaudrateCarrier.ToString());
                goto label_14;
              }
            }
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DeviceNotFound, "NoSerial3Device");
          }
        }
        else
          this.MyFunctions.progress.SplitByReportLoggerTimesString("1;2;2890;229;1;106;1;113;1;114;1;111;118;1;102;1;114;1;115;1;114;1;113;1;112;1;120;1;112;1;114;1;111;1;113;1;114;1;115;1;114;1;114;1;114;1;112;1;115;1;115;1;121;1;113;1;113;1;113;1;113;138;1;103;1;113;1;115;1;113;1;112;1;114;86;1;105;1;113;1;114;1;114;92;1;106;1;114;1;115;1;114;1;113;1;115;1;113;1;112;1;113;1;117;1;114;1;114;1;113;1;113;1;114;1;115;1;114;1;113;1;114;1;112;1;114;1;115;1;114;1;114;1;115;1;113;1;114;1;115;1;113;1;113;1;113;1;117;1;115;1;114;1;113;1;113;1;113;1;113;1;113;1;112;1;112;1;112;1;112;1;114;1;114;1;113;1;113;1;114;1;112;1;112;1;111;1;63;249;1;103;1;114;1;113;1;113;1;112;1;112;1;113;1;112;1;112;1;113;1;112;1;113;1;112;1;113;1;113;211;1;102;1;115");
label_14:
        if (this.ReadHardwareIdentification(out S3_DeviceId _))
        {
          this.MeterJobStart(MeterJob.ReadDevice);
          this.ConnectedMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Read connected device"));
          int index = this.ConnectedMeter.MyParameters.AddressLables.IndexOfKey("SERIE3_CHANGEABLE_CONFIG");
          if (index >= 0)
          {
            int NumberOfBytes1 = ((this.ConnectedMeter.MyParameters.AddressLables.Values[index] - this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress + 512) / 200 + 1) * 200;
            if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress, NumberOfBytes1))
            {
              this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress += NumberOfBytes1;
              if (this.OnProgress != null)
              {
                this.OnProgress((object) this, 20);
                this.MyFunctions.progress.Report(20);
              }
              int num = (int) this.ConnectedMeter.MyParameters.ParameterByName["Con_TransmitTablePtr"].GetUshortValue() - this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress;
              if (num > 0)
              {
                int NumberOfBytes2 = (num / 200 + 1) * 200;
                if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress, NumberOfBytes2))
                  this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress += NumberOfBytes2;
                else
                  goto label_61;
              }
              if (this.OnProgress != null)
              {
                this.OnProgress((object) this, 30);
                this.MyFunctions.progress.Report(30);
              }
            }
            else
              goto label_61;
          }
          if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(6144, 512))
          {
            if (this.OnProgress != null)
            {
              this.OnProgress((object) this, 40);
              this.MyFunctions.progress.Report(40);
            }
            if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(7168, this.ConnectedMeter.MyParameters.AddressLables["CSTACK"] - 7168))
            {
              if (this.OnProgress != null)
              {
                this.MyFunctions.progress.Report(50);
                this.OnProgress((object) this, 50);
              }
              this.ConnectedMeter.MyIdentification.LoadTypeIdFromParameter();
              if (this.ConnectedMeter.CreateToLoggerStructFromMemory())
              {
                if (this.OnProgress != null)
                {
                  this.OnProgress((object) this, 60);
                  this.MyFunctions.progress.Report(60);
                }
                if (this.ConnectedMeter.MyParameters.AddressLables.ContainsKey("SERIE3_CHANGEABLE_CONFIG"))
                {
                  int byteSize = this.ConnectedMeter.MyDeviceMemory.BlockFlashBlock3.ByteSize;
                  if (byteSize > 0)
                  {
                    int NumberOfBytes = (byteSize / 200 + 1) * 200;
                    if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress, NumberOfBytes))
                      this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress += NumberOfBytes;
                    else
                      goto label_61;
                  }
                  if (!this.ConnectedMeter.CreateToLoggerTransmitParameterFromMemory())
                    return false;
                  this.ConnectedMeter.CheckWriteProtectionAndInputs();
                  if (this.OnProgress != null)
                  {
                    this.OnProgress((object) this, 70);
                    this.MyFunctions.progress.Report(70);
                  }
                  int num = this.ConnectedMeter.MyDeviceMemory.BlockFlashBlock3.StartAddressOfNextBlock - this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress;
                  if (num > 0)
                  {
                    int NumberOfBytes = (num / 200 + 1) * 200;
                    if (this.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress, NumberOfBytes))
                      this.ConnectedMeter.MyDeviceMemory.flashNextReadAddress += NumberOfBytes;
                    else
                      goto label_61;
                  }
                  if (!this.ConnectedMeter.MyLoggerManager.CreateStructureObjects())
                  {
                    S3_AllMeters.S3_AllMetersLogger.Error("Create logger structure error!");
                    goto label_61;
                  }
                  else
                  {
                    if (this.OnProgress != null)
                    {
                      this.OnProgress((object) this, 80);
                      this.MyFunctions.progress.Report(80);
                    }
                    if (!this.ConnectedMeter.MyDeviceMemory.BlockHandlerInfo.CreateFromMemory())
                    {
                      S3_AllMeters.S3_AllMetersLogger.Error("Read HandlerInfo block!");
                      goto label_61;
                    }
                  }
                }
                if (!this.ConnectedMeter.MyLinker.MoveEmtyBlocks())
                  return false;
                this.MyFunctions.WriteEnabled = true;
                this.ConnectedMeter.CloneFlowName = "Connected";
                this.NewWorkMeter("auto clone by read");
                this.WorkMeter.MyIdentification.GarantTypeIdConsistent();
                this.WorkMeter.MyIdentification.GarantCloneRules(this.ConnectedMeter);
                this.WorkMeter.Compile();
                if (this.OnProgress != null)
                {
                  this.OnProgress((object) this, 90);
                  this.MyFunctions.progress.Report(90);
                }
                this.MyFunctions.MyMeters.ConnectedMeter.MeterObjectGeneratedTimeStamp = DateTime.Now;
                if (this.MyFunctions._meterBackupOnRead && this.MyFunctions.MyDatabase.DailyAutosave && (!this.MyFunctions._onlyOneReadBackupPerDay || !this.MyFunctions.MyDatabase.ExistsDalyBackup(this.MyFunctions.MyMeters.WorkMeter.MyIdentification.MeterId)))
                  this.SaveDevice();
                if (this.MyFunctions.MyCommands.GetBaseMode() == BusMode.MBus)
                {
                  string SerialNumber = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].GetUintValue().ToString("X8");
                  string meterNumber = this.MyFunctions.MyCommands.GetSelectedDevice().Info.MeterNumber;
                  if (SerialNumber != meterNumber && !this.MyFunctions.MyCommands.SetPhysicalDeviceBySerialNumber(SerialNumber))
                    return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not find the main device of C5 in DeviceCollector! The serial number of main device is: " + SerialNumber, S3_AllMeters.S3_AllMetersLogger);
                }
                flag = true;
              }
            }
          }
        }
label_61:
        if (!flag)
          this.ClearReadLine();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on read from device");
      }
      finally
      {
        this.MeterJobFinished(MeterJob.ReadDevice);
        if (this.MyFunctions.progress != null)
          this.MyFunctions.progress.GetReportLoggerTimesString();
      }
      return flag;
    }

    internal bool CreateWorkMeterFromObjectMemory(S3_Meter srcMeter)
    {
      try
      {
        this.ClearWorkLine();
        S3_Meter s3Meter = srcMeter.Clone(this.MyFunctions);
        s3Meter.CloneInfo = "Created from: " + srcMeter.CloneInfo;
        this.WorkMeter = s3Meter;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on CreateWorkMeterFromObjectMemory.");
      }
      return true;
    }

    internal bool RefreshDynamicParameterFromRAM()
    {
      S3_AllMeters.S3_AllMetersLogger.Debug(nameof (RefreshDynamicParameterFromRAM));
      if (this.ConnectedMeter == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Refresh RAM before read");
        return false;
      }
      try
      {
        if (this.WorkMeter == null && !this.NewWorkMeter("Create for refresh dynamic parameter"))
          return false;
        this.MeterJobStart(MeterJob.RefreshRAM);
        int StartAddress;
        int ReadSize;
        if (!this.ConnectedMeter.MyParameters.ReadDynamicParameterFromRAM(out StartAddress, out ReadSize))
          return false;
        this.WorkMeter.MyDeviceMemory.LoadFromDifferentMemory(this.ConnectedMeter.MyDeviceMemory, StartAddress, ReadSize);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on refresh RAM data");
        return false;
      }
      finally
      {
        this.MeterJobFinished(MeterJob.RefreshRAM);
      }
      S3_AllMeters.S3_AllMetersLogger.Debug("... RefreshDynamicParameterFromRAM done");
      return true;
    }

    internal bool IsConnectedMeterUnchanged()
    {
      if (this.ConnectedMeter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Connected device not available.", S3_AllMeters.S3_AllMetersLogger);
      ByteField MemoryData;
      if (!this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, this.ConnectedMeter.MyDeviceMemory.flashStartAddress, 200, out MemoryData))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read ident block error.", S3_AllMeters.S3_AllMetersLogger);
      for (int index = 0; index < MemoryData.Count; ++index)
      {
        if ((int) MemoryData.Data[index] != (int) this.ConnectedMeter.MyDeviceMemory.MemoryBytes[this.ConnectedMeter.MyDeviceMemory.flashStartAddress + index])
          return false;
      }
      return true;
    }

    internal bool WriteChangesToConnectedDevice()
    {
      try
      {
        this.MeterJobStart(MeterJob.WriteDevice);
        if (!this.MyFunctions.WriteEnabled)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Write not prepared", S3_AllMeters.S3_AllMetersLogger);
        if (this.ConnectedMeter == null)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Connected device not available.", S3_AllMeters.S3_AllMetersLogger);
        this.MyFunctions.progress.SplitByReportLoggerTimesString("1;1;1006;1;113;51;1631;51;3;102;102;1;203;102");
        ByteField MemoryData;
        if (!this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, this.ConnectedMeter.MyDeviceMemory.flashStartAddress, 200, out MemoryData))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read ident block error.", S3_AllMeters.S3_AllMetersLogger);
        for (int index1 = 0; index1 < MemoryData.Count; ++index1)
        {
          if ((int) MemoryData.Data[index1] != (int) this.ConnectedMeter.MyDeviceMemory.MemoryBytes[this.ConnectedMeter.MyDeviceMemory.flashStartAddress + index1])
          {
            for (int index2 = 0; index2 < MemoryData.Count; ++index2)
            {
              if (MemoryData.Data[index2] != byte.MaxValue)
              {
                for (int index3 = 0; index3 < MemoryData.Count; ++index3)
                {
                  if ((int) MemoryData.Data[index3] != (int) this.WorkMeter.MyDeviceMemory.MemoryBytes[this.WorkMeter.MyDeviceMemory.flashStartAddress + index3])
                    return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Identification changed! Device is not readed device.", S3_AllMeters.S3_AllMetersLogger);
                }
              }
            }
          }
        }
        if (this.WorkMeter.TestVolumeSimulationValue.HasValue)
        {
          this.MyFunctions.SimulateVolume(this.WorkMeter.TestVolumeSimulationValue.Value);
          return true;
        }
        this.MyFunctions.MyMeters.NewWorkMeter("prepare for write device");
        this.WorkMeter.AddOverwriteHistoryItem(new OverwriteHistoryItem("Create clone before finalise to write"));
        if (this.ConnectedMeter.IsWriteProtected)
        {
          this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].SetUintValue(this.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].GetUintValue());
          this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].SetUshortValue(this.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].GetUshortValue());
        }
        if (!this.MyFunctions.MyMeters.WorkMeter.GenerateIdentificationChecksum())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Checksum generation error.", S3_AllMeters.S3_AllMetersLogger);
        if (this.ConnectedMeter.MyIdentification.IsUltrasonic && this.ConnectedMeter.MyIdentification.FirmwareVersion > 117542947U && new S3P_Device_Setup_2(this.WorkMeter).UltrasonicFactorMatrixUsed != this.ConnectedMeter.MyIdentification.IsUltrasonicDirect)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Illegal usage of ultrasonic factor matrix", S3_AllMeters.S3_AllMetersLogger);
        if (this.MyFunctions._usePcTime)
          this.WorkMeter.SetPcTime();
        if (this.MyFunctions._meterBackupOnWrite && !this.SaveDevice())
          return false;
        if (this.ConnectedMeter.IsWriteProtected)
        {
          for (ushort address = 0; (int) address < this.WorkMeter.MyDeviceMemory.MemoryBytes.Length; ++address)
          {
            if (this.WorkMeter.MyDeviceMemory.ByteIsDefined[(int) address] && (!this.ConnectedMeter.MyDeviceMemory.ByteIsDefined[(int) address] || (int) this.WorkMeter.MyDeviceMemory.MemoryBytes[(int) address] != (int) this.ConnectedMeter.MyDeviceMemory.MemoryBytes[(int) address]) && this.ConnectedMeter.MyWriteProtTableManager.IsByteProtected(address))
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Try writing to protected address: " + address.ToString("x04"), S3_AllMeters.S3_AllMetersLogger);
          }
        }
        if (!this.MyFunctions.MyCommands.SetEmergencyMode())
          return ZR_ClassLibMessages.AddErrorDescription("Set emergency mode error", S3_AllMeters.S3_AllMetersLogger);
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.RAM))
          return ZR_ClassLibMessages.AddErrorDescription("Write changes error (RAM)", S3_AllMeters.S3_AllMetersLogger);
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.MainFlash))
          return ZR_ClassLibMessages.AddErrorDescription("Write changes error (Flash)", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("Run backup (Code 0x08)");
        if (!this.MyFunctions.MyCommands.RunBackup())
          return ZR_ClassLibMessages.AddErrorDescription("Backup error", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("ResetDevice");
        if (!this.MyFunctions.MyCommands.ResetDevice())
          return ZR_ClassLibMessages.AddErrorDescription("Reset error", S3_AllMeters.S3_AllMetersLogger);
        if (!this.WorkMeter.IsWriteProtected && this.MyFunctions.MyMeters.SetWriteProtectionOnWrite)
        {
          if (this.MyFunctions.MyCommands.DeviceProtectionSetKey(uint.MaxValue))
          {
            if (!UserManager.IsNewLicenseModel() || this.MyFunctions.IsHandlerCompleteEnabled())
            {
              if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey(0U))
                return ZR_ClassLibMessages.AddErrorDescription("SetWriteProtection error by set of developer key", S3_AllMeters.S3_AllMetersLogger);
              ZR_ClassLibMessages.AddWarning("Device not out of production. Set developer write protection key!");
            }
            else
            {
              if (UserManager.CheckPermission(UserManager.Role_Developer) || this.MyFunctions.MyDatabase.BaseDb.ConnectionInfo.DbType == MeterDbTypes.MSSQL)
                return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set WriteProtection! Illegal production state!", S3_AllMeters.S3_AllMetersLogger);
              if (!this.MyFunctions.MyCommands.DeviceProtectionSetKey((uint) new Random((int) DateTime.Now.Ticks).Next()))
                return ZR_ClassLibMessages.AddErrorDescription("SetWriteProtection error by set of random key", S3_AllMeters.S3_AllMetersLogger);
            }
          }
          if (!this.WorkMeter.SetWriteProtection())
            return ZR_ClassLibMessages.AddErrorDescription("SetWriteProtection error", S3_AllMeters.S3_AllMetersLogger);
          if (!this.MyFunctions.MyMeters.SetSleepModeOnWrite && !this.MyFunctions.MyCommands.ResetDevice())
            return ZR_ClassLibMessages.AddErrorDescription("Reset error", S3_AllMeters.S3_AllMetersLogger);
        }
        this.MyFunctions.MyMeters.SetWriteProtectionOnWrite = false;
        this.MyFunctions.MyMeters.SetSleepModeOnWrite = false;
        this.MyFunctions.WriteEnabled = false;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Error on write to device", S3_AllMeters.S3_AllMetersLogger);
      }
      finally
      {
        this.MeterJobFinished(MeterJob.WriteDevice);
        if (this.MyFunctions.progress != null)
          this.MyFunctions.progress.GetReportLoggerTimesString();
      }
    }

    internal bool WriteConnectedMeterToConnectedCompatibledDevice()
    {
      if (this.WorkMeter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No meter data available", S3_AllMeters.S3_AllMetersLogger);
      try
      {
        this.MeterJobStart(MeterJob.WriteCompatibleDevice);
        this.ConnectedMeter = (S3_Meter) null;
        this.MyFunctions.MyCommands.ClearWakeup();
        if (!this.ConnectDevice())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "No connected meter", S3_AllMeters.S3_AllMetersLogger);
        if (!this.MyFunctions.MyDatabase.GetHardwareTypeIdFromVersion((S3_DeviceId) this.ConnectedMeter.MyIdentification))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Error on read HardwareType data out of database", S3_AllMeters.S3_AllMetersLogger);
        StringBuilder stringBuilder = new StringBuilder();
        if ((int) this.ConnectedMeter.MyIdentification.HardwareMask != (int) this.WorkMeter.MyIdentification.HardwareMask)
        {
          stringBuilder.AppendLine("Hardware not compatible !!!!!");
          stringBuilder.AppendLine("Loaded hardware: " + ParameterService.GetHardwareString(this.WorkMeter.MyIdentification.HardwareMask));
          stringBuilder.AppendLine("Connected hardware: " + ParameterService.GetHardwareString(this.ConnectedMeter.MyIdentification.HardwareMask));
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("Would you like to write nevertheless?");
        }
        if (!this.MyFunctions.MyDatabase.IsDeviceHardwareAndFirmwareMapCompatible((S3_DeviceId) this.ConnectedMeter.MyIdentification, (S3_DeviceId) this.WorkMeter.MyIdentification))
          stringBuilder.AppendLine("Map is not compatible");
        if (stringBuilder.Length > 0 && GMM_MessageBox.ShowMessage("S3_Handler", stringBuilder.ToString(), MessageBoxButtons.YesNo) != DialogResult.Yes)
          return false;
        this.ConnectedMeter.MyDeviceMemory.ClearMemory();
        if (this.ConnectedMeter.IsWriteProtected)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Connected meter has write protection. Write all to a protected device not possible.", S3_AllMeters.S3_AllMetersLogger);
        if (!this.MyFunctions.MyCommands.SetEmergencyMode())
          return false;
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.RAM))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (RAM)", S3_AllMeters.S3_AllMetersLogger);
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.MainFlash))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (Flash)", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("Run backup (Code 0x08)");
        if (!this.MyFunctions.MyCommands.RunBackup())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Backup error", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("ResetDevice");
        if (!this.MyFunctions.MyCommands.ResetDevice())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Reset error", S3_AllMeters.S3_AllMetersLogger);
        this.MyFunctions.WriteEnabled = false;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
      }
      finally
      {
        this.ConnectedMeter = (S3_Meter) null;
        this.MeterJobFinished(MeterJob.CloneDevice);
      }
      return false;
    }

    internal bool WriteWorkMeterToConnectedDevice()
    {
      if (this.WorkMeter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "No meter data available", S3_AllMeters.S3_AllMetersLogger);
      try
      {
        this.ConnectedMeter = (S3_Meter) null;
        this.MyFunctions.MyCommands.ClearWakeup();
        if (!this.ConnectDevice())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "No connected meter", S3_AllMeters.S3_AllMetersLogger);
        if (!this.MyFunctions.MyCommands.SetEmergencyMode())
          return false;
        this.ConnectedMeter.MyDeviceMemory.ClearMemory();
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.RAM))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (RAM)", S3_AllMeters.S3_AllMetersLogger);
        if (!this.WorkMeter.MyDeviceMemory.WriteChangesToConnectedDevice(this.ConnectedMeter.MyDeviceMemory, DeviceMemory.ControllerMemoryTypes.MainFlash))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write changes error (Flash)", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("Run backup (Code 0x08)");
        if (!this.MyFunctions.MyCommands.RunBackup())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Backup error", S3_AllMeters.S3_AllMetersLogger);
        S3_AllMeters.S3_AllMetersLogger.Debug("ResetDevice");
        if (!this.MyFunctions.MyCommands.ResetDevice())
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Reset error", S3_AllMeters.S3_AllMetersLogger);
        this.MyFunctions.WriteEnabled = false;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Exception: " + ex.ToString(), S3_AllMeters.S3_AllMetersLogger);
      }
      finally
      {
        this.ConnectedMeter = (S3_Meter) null;
        this.MeterJobFinished(MeterJob.CloneDevice);
      }
      return false;
    }

    internal SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(int SubDevice)
    {
      if (this.WorkMeter == null)
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      SortedList<OverrideID, ConfigurationParameter> baseTypeParameterList = (SortedList<OverrideID, ConfigurationParameter>) null;
      if (this.MyFunctions._useBaseTypeByConfig && !this.MyFunctions.baseTypeEditMode && this.GarantBaseTypeLoaded(out int _))
        baseTypeParameterList = this.TypeMeter.GetConfigurationParameters(SubDevice, (SortedList<OverrideID, ConfigurationParameter>) null, true);
      return this.WorkMeter.GetConfigurationParameters(SubDevice, baseTypeParameterList, true);
    }

    internal bool SetConfigurationParameterNoClone(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      if (SubDevice == 0)
      {
        if (!this.WorkMeter.SetConfigurationParameterHeatMeter(parameterList, (SortedList<OverrideID, ConfigurationParameter>) null))
          return false;
      }
      else if (!this.WorkMeter.SetConfigurationParameterInput(parameterList, (SortedList<OverrideID, ConfigurationParameter>) null, SubDevice - 1))
        return false;
      return this.WorkMeter.RunConfiguratorFunctions();
    }

    internal bool SetConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      SortedList<OverrideID, ConfigurationParameter>[] parameterLists = new SortedList<OverrideID, ConfigurationParameter>[4];
      for (int index = 0; index < 4; ++index)
        parameterLists[index] = SubDevice != index ? sortedList : parameterList;
      return this.SetAllConfigurationParameter(parameterLists, true);
    }

    internal bool SetAllConfigurationParameter(
      SortedList<OverrideID, ConfigurationParameter>[] parameterLists,
      bool useRights)
    {
      try
      {
        SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.WorkMeter.GetConfigurationParameters(0, (SortedList<OverrideID, ConfigurationParameter>) null, useRights);
        SortedList<OverrideID, ConfigurationParameter> configurationParametersInput1 = this.WorkMeter.GetConfigurationParametersInput(0, (SortedList<OverrideID, ConfigurationParameter>) null, useRights);
        SortedList<OverrideID, ConfigurationParameter> configurationParametersInput2 = this.WorkMeter.GetConfigurationParametersInput(1, (SortedList<OverrideID, ConfigurationParameter>) null, useRights);
        SortedList<OverrideID, ConfigurationParameter> configurationParametersInput3 = this.WorkMeter.GetConfigurationParametersInput(2, (SortedList<OverrideID, ConfigurationParameter>) null, useRights);
        S3_Meter workMeter = this.WorkMeter;
        int changedBaseTypeId = -1;
        bool flag = false;
        if (this.MyFunctions._useBaseTypeByConfig && !this.MyFunctions.baseTypeEditMode && this.GarantBaseTypeLoaded(out changedBaseTypeId))
        {
          if (!this.OverwriteFromType.OverwriteWorkFromTypeForConfigurator(this.WorkMeter.IsUseCompactMBusListRequired(parameterLists[0])))
            return false;
          flag = true;
        }
        if (!flag && !this.NewWorkMeter("configuration without base type") || !this.WorkMeter.MyResources.ReloadResources() || !this.WorkMeter.SetConfigurationParameterHeatMeter(parameterLists[0], configurationParameters) || !this.WorkMeter.SetConfigurationParameterInput(parameterLists[1], configurationParametersInput1, 0) || !this.WorkMeter.SetConfigurationParameterInput(parameterLists[2], configurationParametersInput2, 1) || configurationParametersInput3 != null && !this.WorkMeter.SetConfigurationParameterInput(parameterLists[3], configurationParametersInput3, 2))
          return false;
        if (workMeter != this.WorkMeter)
          this.WorkMeter.MyIdentification.GarantCloneRules(workMeter);
        if (!this.WorkMeter.Compile())
          return false;
        if (changedBaseTypeId >= 0)
          this.WorkMeter.MyParameters.ParameterByName["Con_BaseTypeId"].SetUintValue((uint) changedBaseTypeId);
        if (!this.WorkMeter.MyMeterScaling.WriteSettingsToParameter())
          return false;
        for (int inputIndex = 0; inputIndex < 3; ++inputIndex)
        {
          if (!this.WorkMeter.MyIdentification.GarantInputIdentity(inputIndex))
            return false;
        }
        this.WorkMeter.MyIdentification.LoadDeviceIdFromParameter();
        if (!this.WorkMeter.CheckCycleSettings())
          return false;
        this.WorkMeter.CheckMemoryUsing();
        this.WorkMeter.GenerateIdentificationChecksum();
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception("Configuration error", ex);
      }
    }

    internal bool ChangeHardwareType(
      DeviceHardwareIdentification hardwareIdentification)
    {
      S3_DeviceIdentification identification = this.WorkMeter.MyIdentification;
      if (!this.MyFunctions.MyDatabase.GetNewestHardwareTypeIdFromHardwareIdentification(hardwareIdentification, ref this.WorkMeter.MyIdentification))
        return false;
      if (!this.MyFunctions.MyDatabase.IsDeviceHardwareCompatible((S3_DeviceId) this.WorkMeter.MyIdentification, (S3_DeviceId) identification))
      {
        this.WorkMeter.MyIdentification = identification;
        return false;
      }
      this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].SetUintValue(this.WorkMeter.MyIdentification.HardwareTypeId);
      this.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].SetUshortValue((ushort) this.WorkMeter.MyIdentification.HardwareMask);
      return true;
    }

    internal bool OverwriteFromBackup(bool[] overwriteSelection)
    {
      return this.OverwriteFromType.OverwriteRun(this.DbMeter, overwriteSelection, OverwriteOptions.None);
    }

    internal bool CalibrateClock(double error_ppm) => false;

    internal bool CalibrateRadioFrequency(double error_Hz) => false;
  }
}
