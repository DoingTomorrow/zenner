// Decompiled with JetBrains decompiler
// Type: GMM_Handler.LoggerInfo
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class LoggerInfo
  {
    public IntervalAndLogger MyLogger;
    public string LoggerName;
    public int StartAddress;
    public int EndAddress;
    public int WriteAddress;
    public bool LoggerFull;
    public int EntrySize;
    public int MaxEntries;
    public int MaxRamBytes;
    public int AvailableEntries;
    public int TimeListIndex;
    public string Intervall = "";
    public DateTime FirstTimePoint;
    public DateTime LastTimePoint;
    public string FirstStoredTimePoint;
    public string LastStoredTimePoint;
    public string EstimatedReadoutTime;
    public int IndexOfTimeParameter;
    public LoggerParameterData[] AllPotentialParameters;
    public LoggerParameterData[] AllParameters;
    public int BytesToRead;
    public int ReadedBytes;
    public SortedList<DateTime, string[]> LoggerData;
    public DataTable LoggerDataTable;

    public LoggerInfo(IntervalAndLogger LoggerIn) => this.MyLogger = LoggerIn;

    public void CheckReadingTimes(ref DateTime FromTime, ref DateTime ToTime)
    {
      if (FromTime < this.FirstTimePoint)
        FromTime = this.FirstTimePoint;
      if (FromTime > this.LastTimePoint)
        FromTime = this.LastTimePoint;
      this.MyLogger.SetTimeToLastInterval(this.MyLogger.NextEventDateTime, ref FromTime);
      if (ToTime < FromTime)
        ToTime = FromTime;
      if (ToTime > this.LastTimePoint)
        ToTime = this.LastTimePoint;
      this.MyLogger.SetTimeToNextInterval(this.MyLogger.NextEventDateTime, 0, ref ToTime);
    }

    public bool ReadLogger(DateTime FromTime, DateTime ToTime)
    {
      bool flag = false;
      if (this.AvailableEntries < 1)
        return false;
      this.CheckReadingTimes(ref FromTime, ref ToTime);
      int num1 = this.GetNumberOfStoragePoints(this.FirstTimePoint, FromTime) - 1;
      int num2 = this.GetNumberOfStoragePoints(FromTime, ToTime);
      if (num2 > this.AvailableEntries)
        num2 = this.AvailableEntries;
      if (num2 < 1)
        num2 = 1;
      int num3 = this.AvailableEntries - num1 - num2;
      if (num3 < 0)
        num3 = 0;
      this.BytesToRead = num2 * this.EntrySize;
      int num4 = 230 / this.MyLogger.EntrySize * this.MyLogger.EntrySize;
      int num5 = this.WriteAddress - this.EntrySize * (1 + num3);
      this.ReadedBytes = 0;
      int num6 = 0;
      this.LoggerData = new SortedList<DateTime, string[]>();
      while (this.ReadedBytes < this.BytesToRead)
      {
        if (num5 < this.StartAddress)
          num5 = num5 + this.EndAddress - this.StartAddress;
        int num7 = num5 >= this.WriteAddress ? num5 - this.WriteAddress + this.EntrySize : num5 - this.StartAddress + this.EntrySize;
        int NumberOfBytes = num4;
        if (NumberOfBytes > num7)
          NumberOfBytes = num7;
        int num8 = this.BytesToRead - this.ReadedBytes;
        if (num8 < NumberOfBytes)
          NumberOfBytes = num8;
        ByteField MemoryData;
        if (this.MyLogger.MyMeter.MyHandler.SerBus.ReadMemory(MemoryLocation.EEPROM, num5 + this.EntrySize - NumberOfBytes, NumberOfBytes, out MemoryData))
        {
          for (int index1 = MemoryData.Count - this.EntrySize; index1 >= 0; index1 -= this.EntrySize)
          {
            string[] strArray = new string[this.IndexOfTimeParameter < 0 ? this.MyLogger.LoggerParameter.Count : this.MyLogger.LoggerParameter.Count - 1];
            int num9 = 0;
            int num10 = 0;
            DateTime key = new DateTime(1980, 1, 1).AddSeconds((double) num6++);
            for (int index2 = 0; index2 < this.MyLogger.LoggerParameter.Count; ++index2)
            {
              int size = this.MyLogger.LoggerParameter[index2].Size;
              long TheTime = 0;
              for (int index3 = size - 1; index3 >= 0; --index3)
                TheTime = (TheTime << 8) + (long) MemoryData.Data[index1 + num9 + index3];
              if (index2 != this.IndexOfTimeParameter)
              {
                StringBuilder PValue = new StringBuilder(TheTime.ToString());
                int unitExponent = this.AllParameters[index2].UnitExponent;
                ParameterService.SetStringExpo(ref PValue, unitExponent);
                strArray[num10++] = PValue.ToString();
              }
              else
                key = ZR_Calendar.Cal_GetDateTime((uint) TheTime);
              num9 += size;
            }
            if (this.LoggerData.IndexOfKey(key) < 0)
              this.LoggerData.Add(key, strArray);
          }
          this.ReadedBytes += NumberOfBytes;
          num5 -= NumberOfBytes;
        }
        else
          goto label_33;
      }
      flag = true;
label_33:
      try
      {
        this.LoggerDataTable = new DataTable();
        this.LoggerDataTable.Columns.Add(new DataColumn("TimePoint")
        {
          DataType = Type.GetType("System.DateTime")
        });
        for (int index = 0; index < this.AllParameters.Length; ++index)
        {
          if (index != this.IndexOfTimeParameter)
          {
            StringBuilder stringBuilder = new StringBuilder(100);
            stringBuilder.Append("ZDF: " + this.AllParameters[index].ZDF_ParameterID + ZR_Constants.SystemNewLine);
            stringBuilder.Append("Id: " + this.AllParameters[index].PValueDescription.PValueID.ToString() + ZR_Constants.SystemNewLine);
            stringBuilder.Append(this.AllParameters[index].PValueDescription.ValueName + ZR_Constants.SystemNewLine);
            stringBuilder.Append("[" + this.AllParameters[index].PValueDescription.Unit + "]");
            this.LoggerDataTable.Columns.Add(new DataColumn(stringBuilder.ToString())
            {
              DataType = this.AllParameters[index].ParameterFormat != Parameter.BaseParameterFormat.DateTime ? Type.GetType("System.String") : Type.GetType("System.DateTime")
            });
          }
        }
        for (int index4 = this.LoggerData.Count - 1; index4 >= 0; --index4)
        {
          DataRow row = this.LoggerDataTable.NewRow();
          row[0] = (object) this.LoggerData.Keys[index4];
          string[] strArray = this.LoggerData.Values[index4];
          for (int index5 = 0; index5 < strArray.Length; ++index5)
          {
            if (this.LoggerDataTable.Columns[index5 + 1].DataType == typeof (DateTime))
            {
              uint TheTime = uint.Parse(strArray[index5]);
              row[index5 + 1] = (object) ZR_Calendar.Cal_GetDateTime(TheTime);
            }
            else
              row[index5 + 1] = (object) strArray[index5].Replace(".", SystemValues.ZRDezimalSeparator);
          }
          this.LoggerDataTable.Rows.Add(row);
        }
      }
      catch
      {
        return false;
      }
      return flag;
    }

    public int GetCodeTimeIndex()
    {
      byte linkByte = ((LinkObj) this.MyLogger.CodeList[0]).LinkByteList[0];
      for (int codeTimeIndex = 0; codeTimeIndex < IntervalAndLogger.CodeIntervalls.Length; ++codeTimeIndex)
      {
        if ((int) IntervalAndLogger.CodeIntervalls[codeTimeIndex].RuntimeCode == (int) linkByte)
          return codeTimeIndex;
      }
      throw new ArgumentOutOfRangeException("Illegal cycle opcode");
    }

    private int GetNumberOfStoragePoints(DateTime FromTime, DateTime ToTime)
    {
      int numberOfStoragePoints = 1;
      if (this.MyLogger.Interval == 31622400U)
      {
        for (DateTime dateTime = FromTime; dateTime < ToTime; dateTime = dateTime.AddYears(1))
          ++numberOfStoragePoints;
      }
      else if (this.MyLogger.Interval == 2678400U)
      {
        for (DateTime dateTime = FromTime; dateTime < ToTime; dateTime = dateTime.AddMonths(1))
          ++numberOfStoragePoints;
      }
      else
        numberOfStoragePoints = (int) ((long) ToTime.Subtract(FromTime).TotalSeconds / (long) this.MyLogger.Interval + 1L);
      return numberOfStoragePoints;
    }

    public bool ChangeLoggerEntries()
    {
      IntervalAndLogger intervalAndLogger1 = (IntervalAndLogger) null;
      if (this.MaxEntries == this.MyLogger.MaxEntries)
        return true;
      Function fullLoadedFunction = (Function) this.MyLogger.MyMeter.MyHandler.MyLoadedFunctions.FullLoadedFunctions[(object) (ushort) this.MyLogger.FunctionNumber];
      Function ReplaceFunction = fullLoadedFunction.Clone(this.MyLogger.MyMeter);
      for (int index = 0; index < fullLoadedFunction.RuntimeCodeBlockList.Count; ++index)
      {
        CodeBlock runtimeCodeBlock = (CodeBlock) fullLoadedFunction.RuntimeCodeBlockList[index];
        if (runtimeCodeBlock is IntervalAndLogger)
        {
          IntervalAndLogger intervalAndLogger2 = (IntervalAndLogger) runtimeCodeBlock;
          if (intervalAndLogger2.CodeSequenceName == this.MyLogger.CodeSequenceName && intervalAndLogger2.CodeSequenceType == this.MyLogger.CodeSequenceType && intervalAndLogger2.CodeList.Count == this.MyLogger.CodeList.Count)
          {
            intervalAndLogger1 = (IntervalAndLogger) ReplaceFunction.RuntimeCodeBlockList[index];
            break;
          }
        }
      }
      if (intervalAndLogger1 == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Logger block not found.");
        return false;
      }
      intervalAndLogger1.EntrySize = this.MyLogger.EntrySize;
      intervalAndLogger1.P_EndAddress.ValueEprom = intervalAndLogger1.P_StartAddress.ValueEprom + (long) (intervalAndLogger1.EntrySize * this.MaxEntries);
      intervalAndLogger1.MaxEntries = this.MaxEntries;
      for (int index = 0; index < intervalAndLogger1.CodeList.Count; ++index)
        this.MyLogger.MyMeter.MyCompiler.CompileCodeObject((CodeObject) intervalAndLogger1.CodeList[index]);
      return this.MyLogger.MyMeter.MyHandler.MyMeters.ChangeConfigLogger(ReplaceFunction);
    }

    public bool CreateNewLogger()
    {
      ArrayList FunctionParameterList = new ArrayList();
      for (int index = 0; index < this.AllParameters.Length; ++index)
      {
        foreach (Parameter parameter in (IEnumerable<Parameter>) this.MyLogger.MyMeter.PotentialLoggerParameters.Values)
        {
          if (parameter.LoggerData.PValueDescription.ValueName == this.AllParameters[index].PValueDescription.ValueName)
          {
            FunctionParameterList.Add((object) parameter);
            break;
          }
        }
      }
      IntervalAndLogger intervalAndLogger = this.MyLogger.Clone(this.MyLogger.MyMeter, this.MyLogger.CodeSequenceType, this.MyLogger.FrameType, this.MyLogger.FunctionNumber, FunctionParameterList);
      intervalAndLogger.Interval = IntervalAndLogger.CodeIntervalls[this.TimeListIndex].Secounds;
      CodeObject codeObject1 = new CodeObject(this.MyLogger.FunctionNumber);
      codeObject1.Size = 1;
      codeObject1.CodeType = CodeObject.CodeTypes.BYTE;
      codeObject1.CodeValue = this.MyLogger.Type != LoggerTypes.ShortCycleLogger ? IntervalAndLogger.CodeIntervalls[this.TimeListIndex].RuntimeCodeString : "RUI_CODE_Interval RUI_TIME_Extern";
      intervalAndLogger.CodeList.Add((object) codeObject1);
      CodeObject codeObject2 = new CodeObject(this.MyLogger.FunctionNumber);
      codeObject2.Size = 2;
      codeObject2.CodeType = CodeObject.CodeTypes.ePTR;
      codeObject2.CodeValue = this.MyLogger.P_NextTimePoint.Name;
      intervalAndLogger.CodeList.Add((object) codeObject2);
      CodeObject codeObject3 = new CodeObject(this.MyLogger.FunctionNumber);
      codeObject3.Size = 1;
      codeObject3.CodeType = CodeObject.CodeTypes.BYTE;
      intervalAndLogger.CodeList.Add((object) codeObject3);
      int num1 = 0;
      int num2 = 0;
      foreach (Parameter parameter in FunctionParameterList)
      {
        CodeObject codeObject4 = new CodeObject(this.MyLogger.FunctionNumber);
        codeObject4.Size = 1;
        ++num1;
        codeObject4.CodeType = CodeObject.CodeTypes.BYTE;
        intervalAndLogger.CodeList.Add((object) codeObject4);
        bool flag = false;
        switch (parameter.FullName)
        {
          case "DefaultFunction.Sta_Secounds":
            codeObject4.CodeValue = "RUI_CODE_Load_SysTime";
            break;
          case "Energ_WaermeEnergDisplay":
            codeObject4.CodeValue = "RUI_CODE_Load_Energie";
            break;
          case "Vol_VolumenDisplay":
            codeObject4.CodeValue = "RUI_CODE_Load_Volume";
            break;
          default:
            flag = true;
            codeObject4.CodeValue = parameter.Size != 4 ? (parameter.Size != 2 ? "RUI_CODE_Load RUI_VAR_1BYTE" : "RUI_CODE_Load RUI_VAR_2BYTE") : "RUI_CODE_Load RUI_VAR_4BYTE";
            if (!parameter.ExistOnCPU)
            {
              codeObject4.CodeValue += " RUI_VAR_EEPROM";
              break;
            }
            break;
        }
        if (flag)
        {
          CodeObject codeObject5 = new CodeObject(this.MyLogger.FunctionNumber);
          codeObject5.Size = 2;
          num1 += 2;
          if (parameter.ExistOnCPU)
          {
            codeObject5.CodeType = CodeObject.CodeTypes.iPTR;
            codeObject5.CodeValue = parameter.FullName;
          }
          else
          {
            codeObject5.CodeType = CodeObject.CodeTypes.ePTR;
            codeObject5.CodeValue = parameter.FullName;
          }
          intervalAndLogger.CodeList.Add((object) codeObject5);
        }
        CodeObject codeObject6 = new CodeObject(this.MyLogger.FunctionNumber);
        codeObject6.Size = 1;
        ++num1;
        codeObject6.CodeType = CodeObject.CodeTypes.BYTE;
        if (parameter.Size == 4)
        {
          codeObject6.CodeValue = "RUI_CODE_LOGGER_STORE_4BYTE";
          num2 += 4;
        }
        else if (parameter.Size == 2)
        {
          codeObject6.CodeValue = "RUI_CODE_LOGGER_STORE_2BYTE";
          num2 += 2;
        }
        else
        {
          codeObject6.CodeValue = "RUI_CODE_LOGGER_STORE_1BYTE";
          ++num2;
        }
        intervalAndLogger.CodeList.Add((object) codeObject6);
      }
      int num3 = num1 + 1;
      codeObject3.CodeValue = num3.ToString();
      Function function = (Function) this.MyLogger.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) this.MyLogger.FunctionNumber];
      Function ReplaceFunction = function.Clone(this.MyLogger.MyMeter);
      for (int index = 0; index < function.RuntimeCodeBlockList.Count; ++index)
      {
        CodeBlock runtimeCodeBlock = (CodeBlock) function.RuntimeCodeBlockList[index];
        if (runtimeCodeBlock is IntervalAndLogger && (IntervalAndLogger) runtimeCodeBlock == this.MyLogger)
        {
          ReplaceFunction.RuntimeCodeBlockList[index] = (object) intervalAndLogger;
          break;
        }
      }
      foreach (Parameter parameter in ReplaceFunction.ParameterList)
      {
        switch (parameter.StoreType)
        {
          case Parameter.ParamStorageType.INTERVALPOINT:
            intervalAndLogger.P_NextTimePoint = parameter;
            intervalAndLogger.InitialiseNextTimePoint();
            intervalAndLogger.P_NextTimePoint.UpdateByteList();
            break;
          case Parameter.ParamStorageType.INTERVAL:
            intervalAndLogger.P_Intervall = parameter;
            intervalAndLogger.P_Intervall.ValueEprom = (long) intervalAndLogger.Interval;
            intervalAndLogger.P_Intervall.UpdateByteList();
            break;
          case Parameter.ParamStorageType.STARTADDRESS:
            intervalAndLogger.P_StartAddress = parameter;
            intervalAndLogger.P_StartAddress.ValueEprom = 0L;
            intervalAndLogger.P_StartAddress.UpdateByteList();
            break;
          case Parameter.ParamStorageType.ENDADDRESS:
            intervalAndLogger.P_EndAddress = parameter;
            intervalAndLogger.P_EndAddress.ValueEprom = (long) (num2 * this.MaxEntries);
            intervalAndLogger.P_EndAddress.UpdateByteList();
            break;
          case Parameter.ParamStorageType.WRITEPTR:
            intervalAndLogger.P_WriteAddress = parameter;
            intervalAndLogger.P_WriteAddress.ValueEprom = 0L;
            intervalAndLogger.P_WriteAddress.UpdateByteList();
            break;
          case Parameter.ParamStorageType.FLAGS:
            intervalAndLogger.P_Flags = parameter;
            intervalAndLogger.P_Flags.ValueEprom = 0L;
            intervalAndLogger.P_WriteAddress.UpdateByteList();
            break;
          default:
            throw new ArgumentOutOfRangeException("Illegal Parameter at configurable logger");
        }
      }
      for (int index = 0; index < intervalAndLogger.CodeList.Count; ++index)
        this.MyLogger.MyMeter.MyCompiler.CompileCodeObject((CodeObject) intervalAndLogger.CodeList[index]);
      return this.MyLogger.MyMeter.MyHandler.MyMeters.ChangeConfigLogger(ReplaceFunction);
    }

    public bool WriteChangedDataToLogger()
    {
      if (this.LoggerDataTable.Rows.Count > this.MaxEntries)
        throw new ArgumentOutOfRangeException("More lines then logger entries!");
      ByteField LoggerData = new ByteField(this.EntrySize * this.LoggerDataTable.Rows.Count);
      int index1 = 0;
      try
      {
        for (index1 = this.LoggerDataTable.Rows.Count - 1; index1 >= 0; --index1)
        {
          int num = 1;
          for (int index2 = 0; index2 < this.MyLogger.LoggerParameter.Count; ++index2)
          {
            int size = this.MyLogger.LoggerParameter[index2].Size;
            long meterTime;
            if (this.MyLogger.LoggerParameter[index2].FullName == "DefaultFunction.Sta_Secounds")
            {
              meterTime = (long) ZR_Calendar.Cal_GetMeterTime(DateTime.Parse(this.LoggerDataTable.Rows[index1][0].ToString()));
              num = 0;
            }
            else
            {
              StringBuilder PValue = new StringBuilder(this.LoggerDataTable.Rows[index1][index2 + num].ToString().Replace(SystemValues.ZRDezimalSeparator, "."));
              int unitExponent = this.AllParameters[index2].UnitExponent;
              ParameterService.ResetStringExpo(ref PValue, unitExponent * -1);
              string s = PValue.ToString();
              int startIndex = s.IndexOf('.');
              if (startIndex >= 0)
                s = s.Remove(startIndex);
              meterTime = long.Parse(s);
            }
            for (int index3 = 0; index3 < this.MyLogger.LoggerParameter[index2].Size; ++index3)
            {
              LoggerData.Add((byte) ((ulong) meterTime & (ulong) byte.MaxValue));
              meterTime >>= 8;
            }
          }
        }
      }
      catch
      {
        int num = (int) GMM_MessageBox.ShowMessage("Write logger data", "Illegal data at line: " + index1.ToString());
        return false;
      }
      if (!this.MyLogger.WriteDataToLogger(LoggerData))
        return false;
      this.AvailableEntries = this.LoggerDataTable.Rows.Count;
      this.WriteAddress = (int) this.MyLogger.P_WriteAddress.ValueEprom;
      return true;
    }

    public bool LoadLoggerDataFromFile(List<string[]> LineList)
    {
      this.LoggerDataTable.Clear();
      for (int index = 0; index < LineList.Count; ++index)
      {
        DataRow row = this.LoggerDataTable.NewRow();
        for (int columnIndex = 0; columnIndex < this.LoggerDataTable.Columns.Count; ++columnIndex)
          row[columnIndex] = columnIndex >= LineList[index].Length ? (object) "0" : (object) LineList[index][columnIndex];
        this.LoggerDataTable.Rows.Add(row);
      }
      return true;
    }
  }
}
