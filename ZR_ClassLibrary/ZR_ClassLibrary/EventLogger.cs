// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.EventLogger
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class EventLogger : EventTime
  {
    public int ActiveLoggerEvents;
    public bool ShowTime;
    public int MaxLoggerEvents;
    private int WriteOffset;
    private int ReadOffset;
    private int ReadSize;
    private bool Overrun;
    private long LastEventTicks;
    private EventLogger.EVENT_STRUCT[] LoggerData;

    public EventLogger()
    {
      this.ShowTime = true;
      this.NewLogger(100);
    }

    public void NewLogger(int MaxEvents)
    {
      this.MaxLoggerEvents = MaxEvents;
      this.LoggerData = new EventLogger.EVENT_STRUCT[this.MaxLoggerEvents];
      for (int index = 0; index < this.MaxLoggerEvents; ++index)
      {
        this.LoggerData[index].Event = EventLogger.LoggerEvent.EmptyEvent;
        this.LoggerData[index].EventTicks = 0L;
        this.LoggerData[index].DataBytes = new byte[0];
      }
      this.ActiveLoggerEvents = 0;
      this.WriteOffset = 0;
      this.Overrun = false;
    }

    public void WriteLoggerEvent(EventLogger.LoggerEvent Event)
    {
      if ((Event & (EventLogger.LoggerEvent) this.ActiveLoggerEvents) == EventLogger.LoggerEvent.EmptyEvent)
        return;
      this.LoggerData[this.WriteOffset].Event = Event;
      this.LoggerData[this.WriteOffset].EventTicks = this.GetTimeTicks();
      this.LoggerData[this.WriteOffset].DataBytes = new byte[0];
      ++this.WriteOffset;
      if (this.WriteOffset < this.MaxLoggerEvents)
        return;
      this.WriteOffset = 0;
      this.Overrun = true;
    }

    public void WriteLoggerInfo(EventLogger.LoggerEvent Event, string info)
    {
      if ((Event & (EventLogger.LoggerEvent) this.ActiveLoggerEvents) == EventLogger.LoggerEvent.EmptyEvent)
        return;
      this.LoggerData[this.WriteOffset].Event = Event;
      this.LoggerData[this.WriteOffset].EventTicks = this.GetTimeTicks();
      this.LoggerData[this.WriteOffset].DataBytes = new byte[0];
      this.LoggerData[this.WriteOffset].EventInfo = info;
      ++this.WriteOffset;
      if (this.WriteOffset < this.MaxLoggerEvents)
        return;
      this.WriteOffset = 0;
      this.Overrun = true;
    }

    public void WriteLoggerData(EventLogger.LoggerEvent Event, ref ByteField data)
    {
      if ((Event & (EventLogger.LoggerEvent) this.ActiveLoggerEvents) == EventLogger.LoggerEvent.EmptyEvent)
        return;
      int count = data.Count;
      byte[] DataOut;
      data.GetOptimalField(out DataOut);
      this.LoggerData[this.WriteOffset].EventTicks = this.GetTimeTicks();
      this.LoggerData[this.WriteOffset].Event = Event;
      this.LoggerData[this.WriteOffset].DataBytes = DataOut;
      ++this.WriteOffset;
      if (this.WriteOffset < this.MaxLoggerEvents)
        return;
      this.WriteOffset = 0;
      this.Overrun = true;
    }

    public void StartReadout()
    {
      if (this.Overrun)
      {
        this.ReadSize = this.MaxLoggerEvents;
        this.ReadOffset = this.WriteOffset + 1;
        if (this.ReadOffset >= this.MaxLoggerEvents)
          this.ReadOffset = 0;
      }
      else
      {
        this.ReadSize = this.WriteOffset;
        this.ReadOffset = 0;
      }
      this.LastEventTicks = 0L;
    }

    public bool GetNextLine(out string EventLine)
    {
      if (this.ReadSize == 0 || this.ReadOffset == this.WriteOffset)
      {
        this.ReadSize = 0;
        EventLine = "";
        return false;
      }
      string str1 = this.LoggerData[this.ReadOffset].Event.ToString() + " ";
      while (str1.Length < 30)
        str1 += ".";
      long eventTicks = this.LoggerData[this.ReadOffset].EventTicks;
      if (this.ShowTime)
        str1 = str1 + " " + this.GetEventTime(eventTicks);
      string str2 = str1 + " " + this.GetEventTimeDifferenc(eventTicks - this.LastEventTicks);
      this.LastEventTicks = eventTicks;
      string str3 = str2 + "| ";
      for (int index = 0; index < this.LoggerData[this.ReadOffset].DataBytes.Length; ++index)
      {
        string str4 = str3 + this.LoggerData[this.ReadOffset].DataBytes[index].ToString("x2");
        str3 = index % 8 != 7 ? (index % 8 != 3 ? str4 + " " : str4 + ".") : str4 + "\r\n                                                            | ";
      }
      if (this.LoggerData[this.ReadOffset].EventInfo != null)
        str3 = str3 + "| info: " + this.LoggerData[this.ReadOffset].EventInfo;
      EventLine = str3 + "\r\n";
      --this.ReadSize;
      ++this.ReadOffset;
      if (this.ReadOffset >= this.MaxLoggerEvents)
        this.ReadOffset = 0;
      return true;
    }

    public enum LoggerEvent
    {
      EmptyEvent = 0,
      TestEvent = 1,
      ComOpenCloseBlockAdr = 256, // 0x00000100
      ComOpen = 257, // 0x00000101
      ComClose = 258, // 0x00000102
      ComOpenMinoConnect = 259, // 0x00000103
      ComOpenMinoHead = 260, // 0x00000104
      ComDataBlockAdr = 512, // 0x00000200
      ComTransmitData = 513, // 0x00000201
      ComReceiveData = 514, // 0x00000202
      ComStateBlockAdr = 1024, // 0x00000400
      ComClearReceiver = 1025, // 0x00000401
      ComTransmitDone = 1026, // 0x00000402
      ComTransmitBreak = 1027, // 0x00000403
      ComEchoOk = 1028, // 0x00000404
      ComWaitRepeatTime = 1029, // 0x00000405
      ComWaitSafeMode = 1030, // 0x00000406
      ComWaitTransmitTimeS = 1031, // 0x00000407
      ComWaitTransmitTimeE = 1032, // 0x00000408
      ComErrorsBlockAdr = 2048, // 0x00000800
      ComReceiveFramingError = 2049, // 0x00000801
      ComReceiveTimeout = 2050, // 0x00000802
      ComEchoError = 2051, // 0x00000803
      ComHardwareOverflow = 2052, // 0x00000804
      ComQueueOverflow = 2053, // 0x00000805
      ComParityError = 2054, // 0x00000806
      ComFramingError = 2055, // 0x00000807
      ComBreakeDetected = 2056, // 0x00000808
      ComUnknownError = 2057, // 0x00000809
      ComNoErrorFlags = 2058, // 0x0000080A
      ComIOException = 2059, // 0x0000080B
      ComPollingBlockAdr = 4096, // 0x00001000
      ComReceiverPoll = 4097, // 0x00001001
      ComSendMinoConnectStatusRequest = 4098, // 0x00001002
      ComReceiveMinoConnectStatus = 4099, // 0x00001003
      BusFunctionsBlockAdr = 8192, // 0x00002000
      BusDeviceReset = 8193, // 0x00002001
      BusSendREQ_UD2 = 8194, // 0x00002002
      BusSendREQ_Version = 8195, // 0x00002003
      BusStartReadMemory = 8196, // 0x00002004
      BusStartWriteMemory = 8197, // 0x00002005
      BusSelectDevice = 8198, // 0x00002006
      BusSendSND_NKE = 8199, // 0x00002007
      BusStartUpdateMemory = 8200, // 0x00002008
      BusApplicationReset = 8201, // 0x00002009
      BusSetAllParameters = 8202, // 0x0000200A
      BusStatusBlockAdr = 16384, // 0x00004000
      BusWorkHeader = 16385, // 0x00004001
      BusWorkData = 16386, // 0x00004002
      BusStartReadBlock = 16387, // 0x00004003
      BusStartWriteBlock = 16388, // 0x00004004
      BusStartWriteBit = 16389, // 0x00004005
      BusReceiveOK = 16390, // 0x00004006
      BusErrorsBlockAdr = 32768, // 0x00008000
      BusChecksumError = 32769, // 0x00008001
      BusReceiveNOK = 32770, // 0x00008002
      BusReceiveTimeout = 32771, // 0x00008003
      BusReceiveDataError = 32772, // 0x00008004
      BusReadWrongBlockLength = 32773, // 0x00008005
      BusErrorSecondAnswer = 32774, // 0x00008006
      BusErrorFramingError = 32775, // 0x00008007
      BusSendSynchronizeAction = 32776, // 0x00008008
      LoggerEvent = 32777, // 0x00008009
      WriteDueDateMonth = 32778, // 0x0000800A
    }

    private struct EVENT_STRUCT
    {
      public EventLogger.LoggerEvent Event;
      public long EventTicks;
      public byte[] DataBytes;
      public string EventInfo;
    }
  }
}
