// Decompiled with JetBrains decompiler
// Type: PDC_Handler.LoggerManager
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace PDC_Handler
{
  internal static class LoggerManager
  {
    public const ushort ADDR_LOGAREA_START = 32768;
    public const ushort ADDR_LOGAREA_END = 37759;
    public const ushort ADDR_HALF_MONTH_START = 32768;
    public const ushort ADDR_HALF_MONTH_END = 35071;
    public const ushort ADDR_MONTH_START = 35072;
    public const ushort ADDR_MONTH_END = 37375;
    public const ushort ADDR_YEAR_START = 37376;
    public const ushort ADDR_YEAR_END = 37759;
    public static ushort RAM_LOGGER_DATA_SIZE = 1856;
    public static ushort RAM_LOGGER_SIZE = 1926;

    internal static DataTable CreateFlashLoggerTable(byte[] memory)
    {
      if (memory == null)
        return (DataTable) null;
      List<FlashLoggerEntry> flashLogger = LoggerManager.ParseFlashLogger(memory);
      if (flashLogger == null)
        return (DataTable) null;
      DataTable flashLoggerTable = new DataTable();
      Type type1 = typeof (string);
      Type type2 = typeof (uint);
      Type type3 = typeof (bool);
      flashLoggerTable.Columns.Add("Type", type1);
      flashLoggerTable.Columns.Add("Timepoint", type1);
      flashLoggerTable.Columns.Add("ValueInputA", type2);
      flashLoggerTable.Columns.Add("ValueInputB", type2);
      flashLoggerTable.Columns.Add("IsCrcOK", type3);
      flashLoggerTable.Columns.Add("Address", type1);
      foreach (FlashLoggerEntry flashLoggerEntry in flashLogger)
        flashLoggerTable.Rows.Add((object) flashLoggerEntry.Type.ToString(), (object) flashLoggerEntry.Timepoint, (object) flashLoggerEntry.ValueInputA, (object) flashLoggerEntry.ValueInputB, (object) flashLoggerEntry.IsCrcOK, (object) ("0x" + flashLoggerEntry.Address.ToString("X4")));
      return flashLoggerTable;
    }

    internal static List<FlashLoggerEntry> ParseFlashLogger(byte[] memory)
    {
      if (memory == null)
        throw new ArgumentNullException(nameof (memory));
      List<FlashLoggerEntry> flashLogger = new List<FlashLoggerEntry>();
      flashLogger.AddRange((IEnumerable<FlashLoggerEntry>) LoggerManager.ParseFlashLogger(memory, FlashLoggerType.Halfmonth, (ushort) 32768, (ushort) 35071));
      flashLogger.AddRange((IEnumerable<FlashLoggerEntry>) LoggerManager.ParseFlashLogger(memory, FlashLoggerType.Fullmonth, (ushort) 35072, (ushort) 37375));
      flashLogger.AddRange((IEnumerable<FlashLoggerEntry>) LoggerManager.ParseFlashLogger(memory, FlashLoggerType.Year, (ushort) 37376, (ushort) 37759));
      return flashLogger;
    }

    private static List<FlashLoggerEntry> ParseFlashLogger(
      byte[] memory,
      FlashLoggerType type,
      ushort startAddr,
      ushort endAddr)
    {
      List<FlashLoggerEntry> flashLogger = new List<FlashLoggerEntry>();
      for (ushort address = startAddr; (int) address < (int) endAddr; address += (ushort) 12)
      {
        FlashLoggerEntry flashLoggerEntry = FlashLoggerEntry.Parse(memory, address, type);
        if (flashLoggerEntry != null)
          flashLogger.Add(flashLoggerEntry);
      }
      return flashLogger;
    }

    internal static bool IsLoggerDataAvailable(PDC_Meter meter)
    {
      return meter != null && meter.Map.AreBytesDefined((ushort) 32768, 12);
    }

    internal static List<RamLogger> ParseRamLogger(PDC_Meter meter)
    {
      ushort address = meter != null ? LoggerManager.GetStartAddressOfRamLogger(meter.Version) : throw new NullReferenceException(nameof (meter));
      if (!meter.Map.AreBytesDefined(address, (int) LoggerManager.RAM_LOGGER_SIZE))
        throw new Exception("The memory for RAM logger was not read!");
      return LoggerManager.ParseRamLogger(meter.Map.MemoryBytes, address);
    }

    internal static List<RamLogger> ParseRamLogger(byte[] buffer, ushort address)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      ushort offset = address;
      List<ReadValue> ramLoggerData1 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 96);
      List<ReadValue> ramLoggerData2 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 96);
      List<ReadValue> ramLoggerData3 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 18);
      List<ReadValue> ramLoggerData4 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 18);
      List<ReadValue> ramLoggerData5 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 4);
      RamLoggerHeader ramLoggerHeader1 = RamLoggerHeader.Parse(buffer, ref offset);
      RamLoggerHeader ramLoggerHeader2 = RamLoggerHeader.Parse(buffer, ref offset);
      RamLoggerHeader ramLoggerHeader3 = RamLoggerHeader.Parse(buffer, ref offset);
      RamLoggerHeader ramLoggerHeader4 = RamLoggerHeader.Parse(buffer, ref offset);
      RamLoggerHeader ramLoggerHeader5 = RamLoggerHeader.Parse(buffer, ref offset);
      return new List<RamLogger>()
      {
        new RamLogger()
        {
          Type = RamLoggerType.QuarterHour,
          Header = ramLoggerHeader1,
          Data = ramLoggerData1
        },
        new RamLogger()
        {
          Type = RamLoggerType.Daily,
          Header = ramLoggerHeader2,
          Data = ramLoggerData2
        },
        new RamLogger()
        {
          Type = RamLoggerType.Halfmonth,
          Header = ramLoggerHeader3,
          Data = ramLoggerData3
        },
        new RamLogger()
        {
          Type = RamLoggerType.Fullmonth,
          Header = ramLoggerHeader4,
          Data = ramLoggerData4
        },
        new RamLogger()
        {
          Type = RamLoggerType.DueDate,
          Header = ramLoggerHeader5,
          Data = ramLoggerData5
        }
      };
    }

    private static List<ReadValue> ParseRamLoggerData(byte[] buffer, ref ushort offset, int count)
    {
      int num = count * 8 + (int) offset;
      List<ReadValue> ramLoggerData = new List<ReadValue>();
      for (int startIndex = (int) offset; startIndex < num; startIndex += 8)
      {
        ReadValue readValue;
        readValue.A = BitConverter.ToUInt32(buffer, startIndex);
        readValue.B = BitConverter.ToUInt32(buffer, startIndex + 4);
        ramLoggerData.Add(readValue);
      }
      offset = (ushort) num;
      return ramLoggerData;
    }

    internal static ushort GetStartAddressOfRamLogger(DeviceVersion version)
    {
      return (ushort) ((uint) PDC_MemoryMap.GetParameter(version, "log_fifos").Address - (uint) LoggerManager.RAM_LOGGER_DATA_SIZE);
    }
  }
}
