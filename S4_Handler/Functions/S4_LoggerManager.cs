// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_LoggerManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  public class S4_LoggerManager : INotifyPropertyChanged
  {
    private static Logger S4_LoggerManagerLogger = LogManager.GetLogger(nameof (S4_LoggerManager));
    private List<EventLoggerData> EventLoggerDataListBefore;
    private Random LoggerEventRandom;
    private byte LoggerEventRandomMax;

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<LoggerListItem> LoggerList { get; set; }

    public S4_CurrentData CurrentDeviceData { get; set; }

    public List<EventLoggerData> EventLoggerDataList { get; set; }

    public S4_LoggerManager() => this.Clear();

    public void Clear()
    {
      this.LoggerList = new ObservableCollection<LoggerListItem>();
      this.CurrentDeviceData = (S4_CurrentData) null;
    }

    private void WorkException(Exception ex, string additionalMessage)
    {
      if (ex is OperationCanceledException)
        throw ex;
      if (additionalMessage != null)
        throw new Exception(additionalMessage, ex);
      throw ex;
    }

    public async Task<ObservableCollection<LoggerListItem>> ReadLoggerListAsync(
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      try
      {
        S4_LoggerManager.S4_LoggerManagerLogger.Trace("ReadLoggerList");
        this.Clear();
        byte[] result = await cmd.StandardCommandAsync(progress, cancelToken, NfcCommands.GetLoggerList, NfcDeviceCommands.FillData);
        int scanOffset = 2;
        if (result[0] == byte.MaxValue)
          scanOffset = 4;
        while (scanOffset < result.Length - 2)
        {
          LoggerListItem newEntry = new LoggerListItem(result, ref scanOffset);
          LoggerListItem existingItem = this.LoggerList.FirstOrDefault<LoggerListItem>((Func<LoggerListItem, bool>) (x => x.LoggerName == newEntry.LoggerName));
          if (existingItem != null)
          {
            int index = this.LoggerList.IndexOf(existingItem);
            this.LoggerList.RemoveAt(index);
            this.LoggerList.Insert(index, newEntry);
          }
          else
            this.LoggerList.Add(newEntry);
          existingItem = (LoggerListItem) null;
        }
        this.OnPropertyChanged("LoggerList");
        return this.LoggerList;
      }
      catch (Exception ex)
      {
        this.WorkException(ex, "Error by scanning of logger list data");
        return (ObservableCollection<LoggerListItem>) null;
      }
    }

    private async Task<byte[]> ReadLoggerProtocolBytesAsync(
      string loggerName,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken,
      int? maxLoggerBytes = null)
    {
      List<byte> loggerBytes = new List<byte>();
      byte blockNumber = 0;
      while (true)
      {
        byte[] commandData = new byte[loggerName.Length + 2];
        int insertOffset = 0;
        ByteArrayScanner.ScanInString(commandData, loggerName, ref insertOffset);
        ByteArrayScanner.ScanInByte(commandData, blockNumber, ref insertOffset);
        byte[] result = await cmd.StandardCommandAsync(progress, cancelToken, NfcCommands.ReadLogger, commandData);
        if (result.Length >= 5)
        {
          int readOffset = 1;
          if (result[0] == byte.MaxValue)
          {
            if (result.Length >= 7)
              readOffset = 3;
            else
              goto label_5;
          }
          if (result[readOffset++] == (byte) 34)
          {
            while (readOffset < result.Length - 3)
              loggerBytes.Add(result[readOffset++]);
            if (((int) result[readOffset] & (int) sbyte.MaxValue) == (int) blockNumber)
            {
              if (result[readOffset] >= (byte) 128)
              {
                if (!maxLoggerBytes.HasValue || loggerBytes.Count < maxLoggerBytes.Value)
                {
                  ++blockNumber;
                  commandData = (byte[]) null;
                  result = (byte[]) null;
                }
                else
                  goto label_15;
              }
              else
                goto label_18;
            }
            else
              goto label_12;
          }
          else
            goto label_8;
        }
        else
          break;
      }
      throw new Exception("Number of logger result bytes to short.");
label_5:
      throw new Exception("Number of logger result bytes inside a 16Bit resut to short.");
label_8:
      throw new Exception("Illegal command inside the response telegram.");
label_12:
      throw new Exception("Illegal block number received");
label_15:
      progress.Report("Read logger warning for logger " + loggerName + ": ", (object) new ProgressWarning("End block mark to late."));
label_18:
      byte[] array = loggerBytes.ToArray();
      loggerBytes = (List<byte>) null;
      return array;
    }

    internal static async Task AddEventAsync(
      DateTime eventTime,
      LoggerEventTypes newEvent,
      byte eventParam,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] commandData = new byte[6];
      int insertOffset = 0;
      ByteArrayScanner.ScanInDateTime2000(commandData, eventTime, ref insertOffset);
      ByteArrayScanner.ScanInByte(commandData, (byte) newEvent, ref insertOffset);
      ByteArrayScanner.ScanInByte(commandData, eventParam, ref insertOffset);
      byte[] numArray = await cmd.StandardCommandAsync(progress, cancelToken, NfcCommands.SimulateLoggerEvent, commandData);
      commandData = (byte[]) null;
    }

    internal async Task<List<KeyValuePair<string, string>>> ReadLoggerDataAsListAsync(
      string loggerName,
      S4_DeviceCommandsNFC cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
      IEnumerable baseList = await this.ReadLoggerDataAsync(loggerName, cmd, progress, cancelToken);
      foreach (object entry in baseList)
      {
        switch (entry)
        {
          case VolumeLoggerData _:
            VolumeLoggerData volumeData = (VolumeLoggerData) entry;
            List<KeyValuePair<string, string>> keyValuePairList1 = result;
            DateTime loggerTime1 = volumeData.LoggerTime;
            string shortDateString1 = loggerTime1.ToShortDateString();
            loggerTime1 = volumeData.LoggerTime;
            string shortTimeString1 = loggerTime1.ToShortTimeString();
            KeyValuePair<string, string> keyValuePair1 = new KeyValuePair<string, string>(shortDateString1 + " " + shortTimeString1, volumeData.Volume.ToString() + " " + volumeData.Unit);
            keyValuePairList1.Add(keyValuePair1);
            volumeData = (VolumeLoggerData) null;
            break;
          case EventLoggerData _:
            EventLoggerData eventData = (EventLoggerData) entry;
            List<KeyValuePair<string, string>> keyValuePairList2 = result;
            DateTime loggerTime2 = eventData.LoggerTime;
            string shortDateString2 = loggerTime2.ToShortDateString();
            loggerTime2 = eventData.LoggerTime;
            string shortTimeString2 = loggerTime2.ToShortTimeString();
            KeyValuePair<string, string> keyValuePair2 = new KeyValuePair<string, string>(shortDateString2 + " " + shortTimeString2, eventData.Event.ToString() + " " + eventData.EventParameter);
            keyValuePairList2.Add(keyValuePair2);
            eventData = (EventLoggerData) null;
            break;
          case DynamicLoggerEntry _:
            DynamicLoggerEntry dynamicEntry = (DynamicLoggerEntry) entry;
            StringBuilder propertiesList = new StringBuilder();
            DateTime theTime = DateTime.MinValue;
            foreach (KeyValuePair<string, object> dynamicProperty in dynamicEntry.DynamicProperties)
            {
              KeyValuePair<string, object> property = dynamicProperty;
              if (property.Key == "Time")
              {
                theTime = (DateTime) property.Value;
              }
              else
              {
                if (propertiesList.Length > 0)
                  propertiesList.Append(";");
                propertiesList.Append(property.Key + ": " + property.Value.ToString());
                property = new KeyValuePair<string, object>();
              }
            }
            if (theTime > DateTime.MinValue)
              result.Add(new KeyValuePair<string, string>(theTime.ToString() + " " + theTime.ToShortTimeString(), propertiesList.ToString()));
            else
              result.Add(new KeyValuePair<string, string>("Logger entry", propertiesList.ToString()));
            dynamicEntry = (DynamicLoggerEntry) null;
            propertiesList = (StringBuilder) null;
            break;
        }
      }
      List<KeyValuePair<string, string>> keyValuePairList = result;
      result = (List<KeyValuePair<string, string>>) null;
      baseList = (IEnumerable) null;
      return keyValuePairList;
    }

    internal async Task<IEnumerable> ReadLoggerDataAsync(
      string loggerName,
      S4_DeviceCommandsNFC cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      try
      {
        string str1 = loggerName;
        LoggerNames loggerNames = LoggerNames.KeyDate;
        string str2 = loggerNames.ToString();
        int num;
        if (!(str1 == str2))
        {
          string str3 = loggerName;
          loggerNames = LoggerNames.Month;
          string str4 = loggerNames.ToString();
          if (!(str3 == str4))
          {
            string str5 = loggerName;
            loggerNames = LoggerNames.DayVolumes;
            string str6 = loggerNames.ToString();
            num = str5 == str6 ? 1 : 0;
            goto label_5;
          }
        }
        num = 1;
label_5:
        if (num != 0)
        {
          if (this.CurrentDeviceData == null)
          {
            S4_CurrentData s4CurrentData = await cmd.ReadCurrentDataAsync(progress, cancelToken);
            this.CurrentDeviceData = s4CurrentData;
            s4CurrentData = (S4_CurrentData) null;
          }
          byte[] loggerData = await this.ReadLoggerProtocolBytesAsync(loggerName, cmd.CommonNfcCommands, progress, cancelToken);
          List<VolumeLoggerData> monthLoggerDataList = new List<VolumeLoggerData>();
          int number = 1;
          int scanOffset = 0;
          while (scanOffset < loggerData.Length)
          {
            VolumeLoggerData vld = new VolumeLoggerData(loggerData, ref scanOffset, this.CurrentDeviceData.Units, number++);
            monthLoggerDataList.Add(vld);
            vld = (VolumeLoggerData) null;
          }
          return (IEnumerable) monthLoggerDataList;
        }
        string str7 = loggerName;
        loggerNames = LoggerNames.Event;
        string str8 = loggerNames.ToString();
        if (str7 == str8)
        {
          ObservableCollection<LoggerListItem> observableCollection = await this.ReadLoggerListAsync(cmd.CommonNfcCommands, progress, cancelToken);
          LoggerListItem eventLoggerInfos = this.LoggerList.FirstOrDefault<LoggerListItem>((Func<LoggerListItem, bool>) (x => x.LoggerName == LoggerNames.Event.ToString()));
          if (eventLoggerInfos == null)
            throw new Exception("Event logger not available");
          int eventLoggerMaxBytes = (int) eventLoggerInfos.Entries * 8;
          loggerNames = LoggerNames.Event;
          byte[] loggerData = await this.ReadLoggerProtocolBytesAsync(loggerNames.ToString(), cmd.CommonNfcCommands, progress, cancelToken, new int?(eventLoggerMaxBytes));
          this.EventLoggerDataListBefore = this.EventLoggerDataList;
          this.EventLoggerDataList = new List<EventLoggerData>();
          int number = 1;
          int scanOffset = 0;
          while (scanOffset < loggerData.Length)
          {
            EventLoggerData eld = new EventLoggerData(loggerData, ref scanOffset, number++);
            this.EventLoggerDataList.Add(eld);
            eld = (EventLoggerData) null;
          }
          return (IEnumerable) this.EventLoggerDataList;
        }
        if (this.CurrentDeviceData == null)
        {
          S4_CurrentData s4CurrentData = await cmd.ReadCurrentDataAsync(progress, cancelToken);
          this.CurrentDeviceData = s4CurrentData;
          s4CurrentData = (S4_CurrentData) null;
        }
        List<DynamicLoggerEntry> dynamicLoggerDataList = new List<DynamicLoggerEntry>();
        S4_SmartFunctionManager smartFunctionManager = new S4_SmartFunctionManager(cmd.CommonNfcCommands);
        List<SmartFunctionParameter> functionParameters = await smartFunctionManager.GetLoggerParametersFromCodeAsync(progress, cancelToken, loggerName);
        byte[] smartFunctionLoggerData = await this.ReadLoggerProtocolBytesAsync(loggerName, cmd.CommonNfcCommands, progress, cancelToken);
        List<SmartFunctionParameter> loggerParameters = new List<SmartFunctionParameter>();
        foreach (SmartFunctionParameter fp in functionParameters)
        {
          if (fp.LoggerParameterName != null)
            loggerParameters.Add(fp);
        }
        int readOffset = 0;
        while (readOffset < smartFunctionLoggerData.Length)
        {
          DynamicLoggerEntry loggerEntry = new DynamicLoggerEntry();
          DateTime timeStamp = ByteArrayScanner.ScanDateTime(smartFunctionLoggerData, ref readOffset);
          loggerEntry.DynamicProperties.Add("Time", (object) timeStamp);
          foreach (SmartFunctionParameter loggerParameter in loggerParameters)
          {
            object theValue = loggerParameter.ScanValue(smartFunctionLoggerData, ref readOffset);
            loggerEntry.DynamicProperties.Add(loggerParameter.LoggerParameterName, theValue);
            theValue = (object) null;
          }
          dynamicLoggerDataList.Add(loggerEntry);
          loggerEntry = (DynamicLoggerEntry) null;
        }
        return (IEnumerable) dynamicLoggerDataList;
      }
      catch (Exception ex)
      {
        this.WorkException(ex, "Logger not supported: " + loggerName);
        return (IEnumerable) null;
      }
    }

    internal string GetEventLoggerChanges()
    {
      if (this.EventLoggerDataList == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      List<EventLoggerData> source = new List<EventLoggerData>();
      foreach (EventLoggerData eventLoggerData in this.EventLoggerDataListBefore)
        source.Add(eventLoggerData);
      foreach (EventLoggerData eventLoggerData1 in this.EventLoggerDataList)
      {
        EventLoggerData testEvent = eventLoggerData1;
        EventLoggerData eventLoggerData2 = (EventLoggerData) null;
        if (this.EventLoggerDataListBefore != null)
          eventLoggerData2 = source.FirstOrDefault<EventLoggerData>((Func<EventLoggerData, bool>) (x => x.Event == testEvent.Event && x.LoggerTime == testEvent.LoggerTime && x.EventParameter == testEvent.EventParameter));
        if (eventLoggerData2 != null)
          source.Remove(eventLoggerData2);
        else
          stringBuilder.AppendLine("New event: ... " + testEvent.ToString());
      }
      foreach (EventLoggerData eventLoggerData in source)
        stringBuilder.AppendLine("Deleted event: " + eventLoggerData.ToString());
      return stringBuilder.ToString();
    }

    internal static async Task ReadMonthLoggerMemory(
      S4_DeviceMemory theMemory,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange log_data_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_data);
      AddressRange log_index_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_index);
      AddressRange log_nextTime_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_nextTime);
      theMemory.GarantMemoryAvailable(log_data_AddressRange);
      theMemory.GarantMemoryAvailable(log_index_AddressRange);
      theMemory.GarantMemoryAvailable(log_nextTime_AddressRange);
      await cmd.ReadMemoryAsync(log_data_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      await cmd.ReadMemoryAsync(log_index_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      await cmd.ReadMemoryAsync(log_nextTime_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      log_data_AddressRange = (AddressRange) null;
      log_index_AddressRange = (AddressRange) null;
      log_nextTime_AddressRange = (AddressRange) null;
    }

    internal static async Task WriteMonthLoggerMemory(
      S4_DeviceMemory theMemory,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange log_data_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_data);
      AddressRange log_index_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_index);
      AddressRange log_nextTime_AddressRange = theMemory.GetParameterAddressRange(S4_Params.log_nextTime);
      await cmd.WriteMemoryAsync(log_data_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      await cmd.WriteMemoryAsync(log_index_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      await cmd.WriteMemoryAsync(log_nextTime_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      log_data_AddressRange = (AddressRange) null;
      log_index_AddressRange = (AddressRange) null;
      log_nextTime_AddressRange = (AddressRange) null;
    }

    internal static List<VolumeLoggerData> GetMonthLoggerDataListFromMemory(
      S4_DeviceMemory theMemory,
      S4_BaseUnitSupport unitSupport = null)
    {
      try
      {
        AddressRange parameterAddressRange = theMemory.GetParameterAddressRange(S4_Params.log_data);
        theMemory.GetParameterAddressRange(S4_Params.log_index);
        List<VolumeLoggerData> dataListFromMemory = new List<VolumeLoggerData>();
        int startIndex1 = ((int) (parameterAddressRange.ByteSize / 12U) - 1) * 12;
        byte[] data = theMemory.GetData(parameterAddressRange);
        byte num1 = theMemory.GetByte(parameterAddressRange.StartAddress).Value;
        if (BitConverter.ToUInt32(data, startIndex1) == 0U)
          return dataListFromMemory;
        int startIndex2 = startIndex1 - 12;
        while (startIndex2 >= 0 && BitConverter.ToUInt32(data, startIndex2) != 0U)
          startIndex2 -= 12;
        int scanOffset = startIndex2 + 12;
        int num2 = 1;
        while (scanOffset <= startIndex1)
        {
          VolumeLoggerData volumeLoggerData = new VolumeLoggerData(data, ref scanOffset, unitSupport, num2++, VolumeLoggerData.DateTimeFormat.DateAndTimeSec2000);
          dataListFromMemory.Add(volumeLoggerData);
        }
        return dataListFromMemory;
      }
      catch (Exception ex)
      {
        throw new Exception("GetVolumeLoggerDataList", ex);
      }
    }

    internal static string GetMonthLoggerDataTextFromMemory(
      S4_DeviceMemory theMemory,
      S4_BaseUnitSupport unitSupport)
    {
      return S4_LoggerManager.GetMonthLoggerTextFromList(S4_LoggerManager.GetMonthLoggerDataListFromMemory(theMemory, unitSupport));
    }

    internal static string GetMonthLoggerTextFromList(List<VolumeLoggerData> valueList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (VolumeLoggerData volumeLoggerData in valueList)
        stringBuilder.AppendLine(volumeLoggerData.ToString());
      return stringBuilder.ToString();
    }

    internal static void ClearMonthLoggerInMemory(S4_DeviceMemory theMemory)
    {
      try
      {
        AddressRange parameterAddressRange = theMemory.GetParameterAddressRange(S4_Params.log_data);
        theMemory.GetParameterAddressRange(S4_Params.log_index);
        theMemory.GetParameterAddressRange(S4_Params.log_nextTime);
        byte[] data = new byte[(int) parameterAddressRange.ByteSize];
        int theValue1 = data.Length / 12;
        int theValue2 = 0;
        theMemory.SetData(parameterAddressRange.StartAddress, data);
        theMemory.SetParameterValue<byte>(S4_Params.log_index, (byte) theValue1);
        theMemory.SetParameterValue<int>(S4_Params.log_nextTime, theValue2);
      }
      catch (Exception ex)
      {
        throw new Exception("GetVolumeLoggerDataList", ex);
      }
    }

    internal static void AddMonthLoggerEntryToMemory(
      S4_DeviceMemory theMemory,
      DateTime newTimeStamp,
      double value)
    {
      try
      {
        List<VolumeLoggerData> dataListFromMemory = S4_LoggerManager.GetMonthLoggerDataListFromMemory(theMemory);
        AddressRange parameterAddressRange1 = theMemory.GetParameterAddressRange(S4_Params.log_data);
        AddressRange parameterAddressRange2 = theMemory.GetParameterAddressRange(S4_Params.log_index);
        byte[] data = theMemory.GetData(parameterAddressRange1);
        byte num1 = theMemory.GetByte(parameterAddressRange2.StartAddress).Value;
        int offset = ((int) (parameterAddressRange1.ByteSize / 12U) - 1 - dataListFromMemory.Count) * 12;
        int theValue;
        if (offset < 0)
        {
          offset = 0;
          theValue = 0;
          int num2 = data.Length - 1;
          int num3 = num2 - 12;
          while (num3 >= 0)
            data[num2--] = data[num3--];
        }
        else
          theValue = (int) num1 - 1;
        uint meterTime = CalendarBase2000.Cal_GetMeterTime(newTimeStamp);
        ByteArrayScanner.ScanInUInt32(data, meterTime, ref offset);
        ByteArrayScanner.ScanInDouble(data, value, ref offset);
        theMemory.SetData(parameterAddressRange1.StartAddress, data);
        theMemory.SetParameterValue<byte>(S4_Params.log_index, (byte) theValue);
      }
      catch (Exception ex)
      {
        throw new Exception("GetVolumeLoggerDataList", ex);
      }
    }

    internal static void FillMonthLoggerMemory(
      S4_DeviceMemory theMemory,
      DateTime timeStamp,
      double value,
      double valueIncrement)
    {
      try
      {
        AddressRange parameterAddressRange1 = theMemory.GetParameterAddressRange(S4_Params.log_data);
        AddressRange parameterAddressRange2 = theMemory.GetParameterAddressRange(S4_Params.log_index);
        byte[] data = theMemory.GetData(parameterAddressRange1);
        if (data == null)
          throw new Exception("Mont logger map data not available");
        byte num1 = theMemory.GetByte(parameterAddressRange2.StartAddress).Value;
        int num2 = (int) (parameterAddressRange1.ByteSize / 12U);
        int offset = 0;
        for (int index = 0; index < num2; ++index)
        {
          value -= valueIncrement;
          timeStamp = S4_LoggerManager.GetLastMonthTime(timeStamp);
          uint meterTime = CalendarBase2000.Cal_GetMeterTime(timeStamp);
          ByteArrayScanner.ScanInUInt32(data, meterTime, ref offset);
          ByteArrayScanner.ScanInDouble(data, value, ref offset);
        }
        theMemory.SetData(parameterAddressRange1.StartAddress, data);
        theMemory.SetParameterValue<byte>(S4_Params.log_index, (byte) 0);
      }
      catch (Exception ex)
      {
        throw new Exception("GetVolumeLoggerDataList", ex);
      }
    }

    public static DateTime GetLastMonthTime(DateTime lastTime)
    {
      DateTime lastMonthTime;
      if (lastTime.Day >= 16)
      {
        DateTime dateTime = new DateTime(lastTime.Year, lastTime.Month, 16);
        lastMonthTime = !(dateTime != lastTime) ? new DateTime(dateTime.Year, dateTime.Month, 1) : dateTime;
      }
      else
      {
        DateTime dateTime = new DateTime(lastTime.Year, lastTime.Month, 1);
        if (dateTime != lastTime)
        {
          lastMonthTime = dateTime;
        }
        else
        {
          dateTime = dateTime.AddMonths(-1);
          lastMonthTime = new DateTime(dateTime.Year, dateTime.Month, 16);
        }
      }
      return lastMonthTime;
    }

    public static DateTime GetNextMonthTime(DateTime lastTime)
    {
      DateTime nextMonthTime;
      if (lastTime.Day >= 16)
      {
        DateTime dateTime = new DateTime(lastTime.Year, lastTime.Month, 16).AddMonths(1);
        nextMonthTime = new DateTime(dateTime.Year, dateTime.Month, 1);
      }
      else
        nextMonthTime = new DateTime(lastTime.Year, lastTime.Month, 16);
      return nextMonthTime;
    }

    internal LoggerEventTypes GetRandomEvent()
    {
      if (this.LoggerEventRandom == null)
      {
        this.LoggerEventRandom = new Random(DateTime.Now.Millisecond);
        byte[] values = (byte[]) Enum.GetValues(typeof (LoggerEventTypes));
        this.LoggerEventRandomMax = values[values.Length - 1];
      }
      return (LoggerEventTypes) this.LoggerEventRandom.Next(1, (int) this.LoggerEventRandomMax);
    }

    internal byte GetRandomEventParam()
    {
      if (this.LoggerEventRandom == null)
        this.LoggerEventRandom = new Random(DateTime.Now.Millisecond);
      return (byte) this.LoggerEventRandom.Next(0, (int) byte.MaxValue);
    }

    internal static async Task ReadEventLoggerMemory(
      S4_DeviceMemory theMemory,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange log_data_AddressRange = theMemory.GetParameterAddressRange(S4_Params.event_data);
      theMemory.GarantMemoryAvailable(log_data_AddressRange);
      await cmd.ReadMemoryAsync(log_data_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      log_data_AddressRange = (AddressRange) null;
    }

    internal static async Task<string> ReadEventLoggerStateMemory(
      S4_DeviceMemory theMemory,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange state_AddressRange = theMemory.GetParameterAddressRange(S4_Params.event_state);
      AddressRange config_AddressRange = theMemory.GetParameterAddressRange(S4_Params.event_config);
      uint definedLoggerEvents = state_AddressRange.ByteSize / 5U - 1U;
      uint requiredConfigBytes = definedLoggerEvents * 3U;
      if (config_AddressRange.ByteSize < requiredConfigBytes)
        config_AddressRange = new AddressRange(config_AddressRange.StartAddress, requiredConfigBytes);
      theMemory.GarantMemoryAvailable(state_AddressRange);
      theMemory.GarantMemoryAvailable(config_AddressRange);
      await cmd.ReadMemoryAsync(state_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      await cmd.ReadMemoryAsync(config_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      StringBuilder ls = new StringBuilder();
      uint workStateAdr = state_AddressRange.StartAddress;
      uint workConfigAdr = config_AddressRange.StartAddress;
      for (LoggerEventTypes workEvent = LoggerEventTypes.NotDefined; Enum.IsDefined(typeof (LoggerEventTypes), (object) workEvent); ++workEvent)
      {
        if (workEvent != 0)
        {
          if (ls.Length > 0)
            ls.AppendLine();
          ls.AppendLine("Event: " + workEvent.ToString());
          StringBuilder stringBuilder1 = ls;
          byte? nullable = theMemory.GetByte(workConfigAdr);
          string str1 = "PrioFirst: " + nullable.ToString().PadRight(3);
          stringBuilder1.Append(str1);
          StringBuilder stringBuilder2 = ls;
          nullable = theMemory.GetByte(workConfigAdr + 1U);
          string str2 = "; PrioLast: " + nullable.ToString().PadRight(3);
          stringBuilder2.Append(str2);
          StringBuilder stringBuilder3 = ls;
          nullable = theMemory.GetByte(workConfigAdr + 2U);
          string str3 = "; PrioReduce: " + nullable.ToString().PadRight(3);
          stringBuilder3.Append(str3);
          ls.AppendLine();
          StringBuilder stringBuilder4 = ls;
          nullable = theMemory.GetByte(workStateAdr + 1U);
          string str4 = "Entries: " + nullable.ToString().PadRight(3);
          stringBuilder4.Append(str4);
          StringBuilder stringBuilder5 = ls;
          nullable = theMemory.GetByte(workStateAdr + 2U);
          string str5 = "; EntryToDelete: " + nullable.ToString().PadRight(3);
          stringBuilder5.Append(str5);
          StringBuilder stringBuilder6 = ls;
          nullable = theMemory.GetByte(workStateAdr + 3U);
          string str6 = "; PrioMin: " + nullable.ToString().PadRight(3);
          stringBuilder6.Append(str6);
          StringBuilder stringBuilder7 = ls;
          nullable = theMemory.GetByte(workStateAdr + 4U);
          string str7 = "; PrioNext: " + nullable.ToString().PadRight(3);
          stringBuilder7.Append(str7);
          StringBuilder stringBuilder8 = ls;
          nullable = theMemory.GetByte(workStateAdr);
          string str8 = "; Counts: " + nullable.ToString().PadRight(3);
          stringBuilder8.Append(str8);
          ls.AppendLine();
        }
        workStateAdr += 5U;
        workConfigAdr += 3U;
      }
      string str = ls.ToString();
      state_AddressRange = (AddressRange) null;
      config_AddressRange = (AddressRange) null;
      ls = (StringBuilder) null;
      return str;
    }

    internal static async Task WriteEventLoggerMemory(
      S4_DeviceMemory theMemory,
      NfcDeviceCommands cmd,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      AddressRange log_data_AddressRange = theMemory.GetParameterAddressRange(S4_Params.event_data);
      await cmd.WriteMemoryAsync(log_data_AddressRange, (DeviceMemory) theMemory, progress, cancelToken);
      log_data_AddressRange = (AddressRange) null;
    }

    internal static void ClearEventLoggerMemory(S4_DeviceMemory theMemory)
    {
      AddressRange parameterAddressRange = theMemory.GetParameterAddressRange(S4_Params.event_data);
      byte[] data = new byte[(int) parameterAddressRange.ByteSize];
      theMemory.SetData(parameterAddressRange.StartAddress, data);
    }

    internal static List<EventLoggerData> GetEventLoggerDataListFromMemory(S4_DeviceMemory theMemory)
    {
      try
      {
        AddressRange parameterAddressRange = theMemory.GetParameterAddressRange(S4_Params.event_data);
        byte[] data = theMemory.GetData(parameterAddressRange);
        if (data == null)
          throw new Exception("Memory not available");
        List<EventLoggerData> dataListFromMemory = new List<EventLoggerData>();
        int scanOffset = 0;
        int num = 1;
        while (scanOffset + 6 < data.Length && BitConverter.ToUInt32(data, scanOffset) != 0U)
        {
          EventLoggerData eventLoggerData = new EventLoggerData(data, ref scanOffset, num++, true);
          dataListFromMemory.Insert(0, eventLoggerData);
        }
        return dataListFromMemory;
      }
      catch (Exception ex)
      {
        throw new Exception(nameof (GetEventLoggerDataListFromMemory), ex);
      }
    }

    internal static string GetEventLoggerTextFromList(List<EventLoggerData> valueList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("*** Event logger data ***");
      stringBuilder.AppendLine("Number of logger entries: " + valueList.Count.ToString());
      stringBuilder.AppendLine();
      foreach (EventLoggerData eventLoggerData in valueList)
        stringBuilder.AppendLine(eventLoggerData.ToString());
      return stringBuilder.ToString();
    }

    internal static string GetEventLoggerChangeTextFromLists(
      List<EventLoggerData> oldValueList,
      List<EventLoggerData> newValueList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int index1 = 0;
      int index2 = 0;
      while (index1 < oldValueList.Count || index2 < newValueList.Count)
      {
        if (index1 >= oldValueList.Count)
          stringBuilder.AppendLine(newValueList[index2++].ToString() + " => ++++++ ADDED");
        else if (index2 >= newValueList.Count)
          stringBuilder.AppendLine(oldValueList[index1++].ToString() + " => ------ DELETED");
        else if (newValueList[index2].CompareTo(oldValueList[index1]) == 0)
        {
          stringBuilder.AppendLine(newValueList[index2].ToString());
          ++index2;
          ++index1;
        }
        else
        {
          int index3 = index2 + 1;
          while (index3 < newValueList.Count && oldValueList[index1].CompareTo(newValueList[index3]) != 0)
            ++index3;
          if (index3 < newValueList.Count)
          {
            while (index2 < index3)
              stringBuilder.AppendLine(newValueList[index2++].ToString() + " => ++++++ ADDED");
          }
          else if (oldValueList.Count > index1 + 1 && newValueList[index2].CompareTo(oldValueList[index1 + 1]) == 0)
            stringBuilder.AppendLine(oldValueList[index1++].ToString() + " => ------ DELETED");
          else
            stringBuilder.AppendLine(newValueList[index2++].ToString() + " => ???");
        }
      }
      return stringBuilder.ToString();
    }

    public void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
