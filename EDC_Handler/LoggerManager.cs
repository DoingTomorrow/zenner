// Decompiled with JetBrains decompiler
// Type: EDC_Handler.LoggerManager
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;
using System.Collections.Generic;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public static class LoggerManager
  {
    internal static DataTable CreateFlashLoggerDataTable(EDC_Meter meter)
    {
      if (meter == null)
        return (DataTable) null;
      List<FlashLoggerEntry> flashLogger = LoggerManager.ParseFlashLogger(meter);
      if (flashLogger == null)
        return (DataTable) null;
      DataTable flashLoggerDataTable = new DataTable();
      Type type1 = typeof (string);
      Type type2 = typeof (int);
      Type type3 = typeof (bool);
      flashLoggerDataTable.Columns.Add("Type", type1);
      flashLoggerDataTable.Columns.Add("Index", type2);
      flashLoggerDataTable.Columns.Add("Value", type1);
      flashLoggerDataTable.Columns.Add("Timepoint", type1);
      flashLoggerDataTable.Columns.Add("Month", type1);
      flashLoggerDataTable.Columns.Add("Reserved", type1);
      flashLoggerDataTable.Columns.Add("IsCrcOK", type3);
      flashLoggerDataTable.Columns.Add("Address", type1);
      flashLoggerDataTable.Columns.Add("Buffer", type1);
      foreach (FlashLoggerEntry flashLoggerEntry in flashLogger)
        flashLoggerDataTable.Rows.Add((object) flashLoggerEntry.Type.ToString(), (object) flashLoggerEntry.Index, (object) flashLoggerEntry.Value, (object) flashLoggerEntry.Timepoint, (object) flashLoggerEntry.Month, (object) flashLoggerEntry.Reserved_Byte5, (object) flashLoggerEntry.IsCrcOK, (object) ("0x" + flashLoggerEntry.Address.ToString("X4")), (object) Util.ByteArrayToHexString(flashLoggerEntry.Buffer));
      return flashLoggerDataTable;
    }

    public static List<FlashLoggerEntry> ParseFlashLogger(EDC_Meter meter)
    {
      if (meter == null)
        throw new ArgumentNullException(nameof (meter));
      Addresses addresses = LoggerManager.IsLoggerDataAvailable(meter) ? Addresses.Get(meter) : throw new Exception("The logger data is not available! Please read logger data too.");
      ushort num1 = addresses != null ? addresses.LOG_ADDRESS_HALFMONTH_START : throw new ArgumentNullException("Logger addresses ca not be null!");
      int num2 = (int) addresses.LOG_ADDRESS_STICHTAG_END + 8 - (int) addresses.LOG_ADDRESS_HALFMONTH_START;
      List<FlashLoggerEntry> flashLogger1 = new List<FlashLoggerEntry>();
      DateTime? systemTime = meter.GetSystemTime();
      if (!systemTime.HasValue)
        return (List<FlashLoggerEntry>) null;
      ref DateTime? local1 = ref systemTime;
      DateTime dateTime1 = systemTime.Value;
      int year1 = dateTime1.Year;
      dateTime1 = systemTime.Value;
      int month1 = dateTime1.Month;
      dateTime1 = systemTime.Value;
      int day1 = dateTime1.Day;
      DateTime dateTime2 = new DateTime(year1, month1, day1);
      local1 = new DateTime?(dateTime2);
      if (addresses.NextIndexHalfmonth > 0)
      {
        DateTime startLogDate;
        ref DateTime local2 = ref startLogDate;
        dateTime1 = systemTime.Value;
        int year2 = dateTime1.Year;
        dateTime1 = systemTime.Value;
        int month2 = dateTime1.Month;
        local2 = new DateTime(year2, month2, 16);
        dateTime1 = systemTime.Value;
        if (dateTime1.Day < 16)
          startLogDate = startLogDate.AddMonths(-1);
        startLogDate = startLogDate.AddMonths(-(addresses.NextIndexHalfmonth - 1));
        byte[] memoryBytes = meter.Map.GetMemoryBytes(addresses.LOG_ADDRESS_HALFMONTH_START, (int) addresses.LOG_ADDRESS_HALFMONTH_END - (int) addresses.LOG_ADDRESS_HALFMONTH_START);
        List<FlashLoggerEntry> flashLogger2 = LoggerManager.ParseFlashLogger(startLogDate, FlashLoggerType.Halfmonth, memoryBytes, addresses.LOG_ADDRESS_HALFMONTH_START);
        flashLogger1.AddRange((IEnumerable<FlashLoggerEntry>) flashLogger2);
      }
      if (addresses.NextIndexFullmonth > 0)
      {
        DateTime startLogDate;
        ref DateTime local3 = ref startLogDate;
        dateTime1 = systemTime.Value;
        int year3 = dateTime1.Year;
        dateTime1 = systemTime.Value;
        int month3 = dateTime1.Month;
        local3 = new DateTime(year3, month3, 1);
        startLogDate = startLogDate.AddMonths(-(addresses.NextIndexFullmonth - 1));
        byte[] memoryBytes = meter.Map.GetMemoryBytes(addresses.LOG_ADDRESS_FULLMONTH_START, (int) addresses.LOG_ADDRESS_FULLMONTH_END - (int) addresses.LOG_ADDRESS_FULLMONTH_START);
        List<FlashLoggerEntry> flashLogger3 = LoggerManager.ParseFlashLogger(startLogDate, FlashLoggerType.Fullmonth, memoryBytes, addresses.LOG_ADDRESS_FULLMONTH_START);
        flashLogger1.AddRange((IEnumerable<FlashLoggerEntry>) flashLogger3);
      }
      if (addresses.NextIndexStichtag > 0)
      {
        DateTime dateTime3 = meter.GetDueDate().Value;
        DateTime dateTime4;
        ref DateTime local4 = ref dateTime4;
        dateTime1 = systemTime.Value;
        int year4 = dateTime1.Year;
        int month4 = dateTime3.Month;
        int day2 = dateTime3.Day;
        local4 = new DateTime(year4, month4, day2);
        List<FlashLoggerEntry> flashLogger4 = LoggerManager.ParseFlashLogger(dateTime4.AddYears(-(addresses.NextIndexStichtag - 1)), FlashLoggerType.Year, meter.Map.GetMemoryBytes(addresses.LOG_ADDRESS_STICHTAG_START, (int) addresses.LOG_ADDRESS_STICHTAG_END - (int) addresses.LOG_ADDRESS_STICHTAG_START), addresses.LOG_ADDRESS_STICHTAG_START);
        flashLogger1.AddRange((IEnumerable<FlashLoggerEntry>) flashLogger4);
      }
      return flashLogger1;
    }

    private static List<FlashLoggerEntry> ParseFlashLogger(
      DateTime startLogDate,
      FlashLoggerType type,
      byte[] memory,
      ushort startAddress)
    {
      if (memory == null)
        throw new ArgumentNullException("The memory of " + type.ToString() + " logger does not exist.");
      if (memory.Length % 8 != 0)
        throw new ArgumentOutOfRangeException("The flash memory is not % 8!");
      List<FlashLoggerEntry> flashLogger = new List<FlashLoggerEntry>();
      int num1 = 0;
      ushort num2 = startAddress;
      DateTime dateTime = DateTime.MinValue;
      switch (type)
      {
        case FlashLoggerType.Halfmonth:
          dateTime = new DateTime(startLogDate.Year, startLogDate.Month, 16);
          break;
        case FlashLoggerType.Fullmonth:
          dateTime = new DateTime(startLogDate.Year, startLogDate.Month, 1);
          break;
        case FlashLoggerType.Year:
          dateTime = new DateTime(startLogDate.Year, startLogDate.Month, startLogDate.Day);
          break;
        default:
          throw new NotImplementedException(type.ToString());
      }
      int num3 = memory.Length - 128;
      for (int index = 0; index < memory.Length; index += 8)
      {
        byte[] numArray = new byte[6];
        byte[] dst = new byte[8];
        Buffer.BlockCopy((Array) memory, index, (Array) numArray, 0, numArray.Length);
        Buffer.BlockCopy((Array) memory, index, (Array) dst, 0, dst.Length);
        ushort num4 = Util.CalculatesCRC16_CC430(numArray);
        bool flag = (int) BitConverter.ToUInt16(memory, index + 6) == (int) num4;
        int? nullable1 = new int?();
        byte? nullable2 = new byte?();
        byte? nullable3 = new byte?();
        if (flag)
        {
          nullable1 = new int?(BitConverter.ToInt32(memory, index));
          nullable2 = new byte?(numArray[4]);
          nullable3 = new byte?(numArray[5]);
        }
        flashLogger.Add(new FlashLoggerEntry()
        {
          Type = type,
          Timepoint = dateTime,
          Value = nullable1,
          Month = nullable2,
          Reserved_Byte5 = nullable3,
          Index = num1++,
          Buffer = dst,
          Address = num2,
          IsCrcOK = flag
        });
        num2 += (ushort) 8;
        switch (type)
        {
          case FlashLoggerType.Halfmonth:
            dateTime = dateTime.AddMonths(1);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, 16);
            break;
          case FlashLoggerType.Fullmonth:
            dateTime = dateTime.AddMonths(1);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, 1);
            break;
          case FlashLoggerType.Year:
            if (index < num3)
            {
              dateTime = dateTime.AddYears(1);
              break;
            }
            goto label_20;
          default:
            throw new NotImplementedException(type.ToString());
        }
      }
label_20:
      return flashLogger;
    }

    public static List<RamLogger> ParseRamLogger(EDC_Meter meter)
    {
      if (meter == null)
        throw new ArgumentNullException(nameof (meter));
      if (!LoggerManager.IsLoggerDataAvailable(meter))
        throw new Exception("The logger data is not available! Please read logger data too.");
      ushort startRamLogger;
      ushort endRamLogger;
      Addresses.GetRamLogger(meter.Version, out startRamLogger, out endRamLogger);
      return LoggerManager.ParseRamLogger(meter.Map.GetMemoryBytes(startRamLogger, (int) endRamLogger - (int) startRamLogger));
    }

    public static List<RamLogger> ParseRamLogger(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      int offset = 0;
      List<uint> ramLoggerData1 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 96);
      List<uint> ramLoggerData2 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 96);
      List<uint> ramLoggerData3 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 18);
      List<uint> ramLoggerData4 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 18);
      List<uint> ramLoggerData5 = LoggerManager.ParseRamLoggerData(buffer, ref offset, 16);
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

    private static List<uint> ParseRamLoggerData(byte[] buffer, ref int offset, int count)
    {
      int num = count * 4 + offset;
      List<uint> ramLoggerData = new List<uint>();
      for (int startIndex = offset; startIndex < num; startIndex += 4)
        ramLoggerData.Add(BitConverter.ToUInt32(buffer, startIndex));
      offset = num;
      return ramLoggerData;
    }

    public static bool IsLoggerDataAvailable(EDC_Meter meter)
    {
      Addresses addresses = meter != null ? Addresses.Get(meter) : throw new ArgumentNullException(nameof (meter));
      ushort address = addresses != null ? addresses.LOG_ADDRESS_HALFMONTH_START : throw new ArgumentNullException("Logger addresses ca not be null!");
      int size = (int) addresses.LOG_ADDRESS_STICHTAG_END + 8 - (int) addresses.LOG_ADDRESS_HALFMONTH_START;
      return meter.Map.AreBytesDefined(address, size);
    }
  }
}
