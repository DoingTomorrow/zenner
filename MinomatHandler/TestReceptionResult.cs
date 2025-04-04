// Decompiled with JetBrains decompiler
// Type: MinomatHandler.TestReceptionResult
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public sealed class TestReceptionResult
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestReceptionResult));
    public const string COLUMN_POS = "#";
    public const string COLUMN_MU_ID = "MU-ID";
    public const string COLUMN_RSSI = "RSSI";
    public const string COLUMN_LQI = "LQI";
    private string header;
    private DataTable table;

    public TestReceptionResult(List<RadioDevice> foundDevices)
    {
      this.table = new DataTable(nameof (TestReceptionResult));
      this.table.Columns.Add("#");
      this.table.Columns.Add("MU-ID");
      this.table.Columns.Add("RSSI");
      this.table.Columns.Add("LQI");
      this.header = "".PadRight(44, '-') + "\n " + "#".PadRight(10) + "| " + "MU-ID".PadRight(10) + "| " + "RSSI".PadRight(10) + "| " + "LQI".PadRight(10) + "\n" + "".PadRight(44, '-') + "\n";
      this.FoundDevices = foundDevices;
    }

    public List<RadioDevice> FoundDevices { get; set; }

    public static TestReceptionResult Parse(SCGiFrame frame)
    {
      SCGiPacket scGiPacket1 = frame != null && frame.Count != 0 ? frame[0] : throw new ArgumentNullException("SCGi frame can not be null!");
      if (scGiPacket1.Payload.Length < 2)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.InvalidSCGiPacket, "Invalid SCGi response! " + scGiPacket1?.ToString());
        TestReceptionResult.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      if (scGiPacket1.Payload.Length == 4)
      {
        if (scGiPacket1.Payload[2] == (byte) 0)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.DeviceIsNotInAppropriateMode, "The device is not in the appropriate mode!");
          TestReceptionResult.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        if (scGiPacket1.Payload[2] == byte.MaxValue)
        {
          SCGiError scGiError = new SCGiError(SCGiErrorType.TestReceptionIsNotYetCompleted, "The test reception is not yet completed!");
          TestReceptionResult.logger.Error<SCGiError>(scGiError);
          throw scGiError;
        }
        SCGiError scGiError1 = new SCGiError(SCGiErrorType.UnknownErrorOccured, "CMD: 0x" + Util.ByteArrayToHexString(SCGiCommandManager.GetBytes(SCGiCommand.TestReceptionResult)) + " (TestReceptionResult) received an unknown error! Payload: 0x" + Util.ByteArrayToHexString(scGiPacket1.Payload));
        TestReceptionResult.logger.Error<SCGiError>(scGiError1);
        throw scGiError1;
      }
      List<byte> byteList = new List<byte>();
      for (int index = 0; index < frame.Count; ++index)
      {
        SCGiPacket scGiPacket2 = frame[index];
        if (index == 0)
        {
          byte[] numArray = new byte[scGiPacket2.Payload.Length - 2];
          Buffer.BlockCopy((Array) scGiPacket2.Payload, 2, (Array) numArray, 0, numArray.Length);
          byteList.AddRange((IEnumerable<byte>) numArray);
        }
        else
          byteList.AddRange((IEnumerable<byte>) scGiPacket2.Payload);
      }
      List<RadioDevice> tableRows = TestReceptionResult.ParseTableRows(byteList.ToArray());
      return tableRows == null ? (TestReceptionResult) null : new TestReceptionResult(tableRows);
    }

    private static List<RadioDevice> ParseTableRows(byte[] table)
    {
      if (table.Length % 6 != 0)
      {
        SCGiError scGiError = new SCGiError(SCGiErrorType.ParseError, "Wrong length or test reception table! Expected: length mod 6 == 0");
        TestReceptionResult.logger.Error<SCGiError>(scGiError);
        throw scGiError;
      }
      List<RadioDevice> tableRows = new List<RadioDevice>();
      for (int index = 0; index < table.Length; index += 6)
      {
        uint uint32 = BitConverter.ToUInt32(new byte[4]
        {
          table[index],
          table[index + 1],
          table[index + 2],
          table[index + 3]
        }, 0);
        byte num1 = table[index + 4];
        byte num2 = table[index + 5];
        tableRows.Add(new RadioDevice()
        {
          MUID = uint32,
          RSSI = num1,
          LQI = num2
        });
      }
      return tableRows;
    }

    public DataTable CreateTable()
    {
      this.table.Clear();
      int num = 1;
      foreach (RadioDevice foundDevice in this.FoundDevices)
        this.table.Rows.Add((object) num++, (object) foundDevice.MUID, (object) foundDevice.RSSI, (object) foundDevice.LQI);
      return this.table;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\n");
      stringBuilder.Append(this.header);
      int num = 1;
      foreach (RadioDevice foundDevice in this.FoundDevices)
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(num++.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(foundDevice.MUID.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(foundDevice.RSSI.ToString().PadRight(10));
        stringBuilder.Append("| ");
        stringBuilder.Append(foundDevice.LQI.ToString().PadRight(10));
        stringBuilder.Append("\n");
      }
      return stringBuilder.ToString();
    }
  }
}
