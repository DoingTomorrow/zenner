// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_DeviceIdentification
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using DeviceCollector;
using GmmDbLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public sealed class TH_DeviceIdentification
  {
    public uint? MeterID { get; set; }

    public uint? HardwareTypeID { get; set; }

    public uint? MeterInfoID { get; set; }

    public uint? BaseTypeID { get; set; }

    public uint? MeterTypeID { get; set; }

    public uint? SapMaterialNumber { get; set; }

    public uint? SapProductionOrderNumber { get; set; }

    public byte[] Con_fullserialnumber { get; set; }

    public byte[] Con_fullserialnumberA { get; set; }

    public byte[] Con_fullserialnumberB { get; set; }

    public string Fullserialnumber
    {
      get => this.DecodeSN(this.Con_fullserialnumber);
      set => this.Con_fullserialnumber = this.EncodeSN(value);
    }

    public string FullserialnumberA
    {
      get => this.DecodeSN(this.Con_fullserialnumberA);
      set => this.Con_fullserialnumberA = this.EncodeSN(value);
    }

    public string FullserialnumberB
    {
      get => this.DecodeSN(this.Con_fullserialnumberB);
      set => this.Con_fullserialnumberB = this.EncodeSN(value);
    }

    internal ushort Con_IdentificationChecksum => this.CalculateChecksum();

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("MeterID: ".PadRight(spaces)).AppendLine(this.MeterID.ToString());
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces)).AppendLine(this.HardwareTypeID.ToString());
      stringBuilder.Append("MeterInfoID: ".PadRight(spaces)).AppendLine(this.MeterInfoID.ToString());
      stringBuilder.Append("BaseTypeID: ".PadRight(spaces)).AppendLine(this.BaseTypeID.ToString());
      stringBuilder.Append("MeterTypeID: ".PadRight(spaces)).AppendLine(this.MeterTypeID.ToString());
      stringBuilder.Append("SAP_MaterialNumber: ".PadRight(spaces)).AppendLine(this.SapMaterialNumber.ToString());
      stringBuilder.Append("SAP_ProductionOrderNumber: ".PadRight(spaces)).AppendLine(this.SapProductionOrderNumber.ToString());
      stringBuilder.Append("Fullserialnumber: ".PadRight(spaces)).AppendLine(this.Fullserialnumber);
      stringBuilder.Append("FullserialnumberA: ".PadRight(spaces)).AppendLine(this.FullserialnumberA);
      stringBuilder.Append("FullserialnumberB: ".PadRight(spaces)).AppendLine(this.FullserialnumberB);
      return stringBuilder.ToString();
    }

    internal static TH_DeviceIdentification Parse(
      uint meterID,
      uint hardwareTypeID,
      uint meterInfoID,
      uint baseTypeID,
      uint meterTypeID,
      uint sapMaterialNumber,
      uint sapProductionOrderNumber,
      byte[] con_fullserialnumber,
      byte[] con_fullserialnumberA,
      byte[] con_fullserialnumberB,
      ushort con_IdentificationChecksum)
    {
      if ((int) TH_DeviceIdentification.CalculateChecksum(TH_DeviceIdentification.GetBuffer(new uint?(meterID), new uint?(hardwareTypeID), new uint?(meterInfoID), new uint?(baseTypeID), new uint?(meterTypeID), new uint?(sapMaterialNumber), new uint?(sapProductionOrderNumber), con_fullserialnumber, con_fullserialnumberA, con_fullserialnumberB)) != (int) con_IdentificationChecksum)
        return (TH_DeviceIdentification) null;
      return new TH_DeviceIdentification()
      {
        MeterID = meterID == uint.MaxValue ? new uint?() : new uint?(meterID),
        HardwareTypeID = hardwareTypeID == uint.MaxValue ? new uint?() : new uint?(hardwareTypeID),
        MeterInfoID = meterInfoID == uint.MaxValue ? new uint?() : new uint?(meterInfoID),
        BaseTypeID = baseTypeID == uint.MaxValue ? new uint?() : new uint?(baseTypeID),
        MeterTypeID = meterTypeID == uint.MaxValue ? new uint?() : new uint?(meterTypeID),
        SapMaterialNumber = sapMaterialNumber == uint.MaxValue ? new uint?() : new uint?(sapMaterialNumber),
        SapProductionOrderNumber = sapProductionOrderNumber == uint.MaxValue ? new uint?() : new uint?(sapProductionOrderNumber),
        Con_fullserialnumber = con_fullserialnumber,
        Con_fullserialnumberA = con_fullserialnumberA,
        Con_fullserialnumberB = con_fullserialnumberB
      };
    }

    private static byte[] GetBuffer(
      uint? meterID,
      uint? hardwareTypeID,
      uint? meterInfoID,
      uint? baseTypeID,
      uint? meterTypeID,
      uint? sapMaterialNumber,
      uint? sapProductionOrderNumber,
      byte[] con_fullserialnumber,
      byte[] con_fullserialnumberA,
      byte[] con_fullserialnumberB)
    {
      List<byte> byteList1 = new List<byte>();
      List<byte> byteList2 = byteList1;
      byte[] collection1;
      if (!meterID.HasValue)
        collection1 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection1 = BitConverter.GetBytes(meterID.Value);
      byteList2.AddRange((IEnumerable<byte>) collection1);
      List<byte> byteList3 = byteList1;
      byte[] collection2;
      if (!hardwareTypeID.HasValue)
        collection2 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection2 = BitConverter.GetBytes(hardwareTypeID.Value);
      byteList3.AddRange((IEnumerable<byte>) collection2);
      List<byte> byteList4 = byteList1;
      byte[] collection3;
      if (!meterInfoID.HasValue)
        collection3 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection3 = BitConverter.GetBytes(meterInfoID.Value);
      byteList4.AddRange((IEnumerable<byte>) collection3);
      List<byte> byteList5 = byteList1;
      byte[] collection4;
      if (!baseTypeID.HasValue)
        collection4 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection4 = BitConverter.GetBytes(baseTypeID.Value);
      byteList5.AddRange((IEnumerable<byte>) collection4);
      List<byte> byteList6 = byteList1;
      byte[] collection5;
      if (!meterTypeID.HasValue)
        collection5 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection5 = BitConverter.GetBytes(meterTypeID.Value);
      byteList6.AddRange((IEnumerable<byte>) collection5);
      List<byte> byteList7 = byteList1;
      byte[] collection6;
      if (!sapMaterialNumber.HasValue)
        collection6 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection6 = BitConverter.GetBytes(sapMaterialNumber.Value);
      byteList7.AddRange((IEnumerable<byte>) collection6);
      List<byte> byteList8 = byteList1;
      byte[] collection7;
      if (!sapProductionOrderNumber.HasValue)
        collection7 = new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection7 = BitConverter.GetBytes(sapProductionOrderNumber.Value);
      byteList8.AddRange((IEnumerable<byte>) collection7);
      List<byte> byteList9 = byteList1;
      byte[] collection8;
      if (con_fullserialnumber == null)
        collection8 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection8 = con_fullserialnumber;
      byteList9.AddRange((IEnumerable<byte>) collection8);
      List<byte> byteList10 = byteList1;
      byte[] collection9;
      if (con_fullserialnumberA == null)
        collection9 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection9 = con_fullserialnumberA;
      byteList10.AddRange((IEnumerable<byte>) collection9);
      List<byte> byteList11 = byteList1;
      byte[] collection10;
      if (con_fullserialnumberB == null)
        collection10 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        collection10 = con_fullserialnumberB;
      byteList11.AddRange((IEnumerable<byte>) collection10);
      return byteList1.ToArray();
    }

    private ushort CalculateChecksum()
    {
      return TH_DeviceIdentification.CalculateChecksum(TH_DeviceIdentification.GetBuffer(this.MeterID, this.HardwareTypeID, this.MeterInfoID, this.BaseTypeID, this.MeterTypeID, this.SapMaterialNumber, this.SapProductionOrderNumber, this.Con_fullserialnumber, this.Con_fullserialnumberA, this.Con_fullserialnumberB));
    }

    private static ushort CalculateChecksum(byte[] buffer)
    {
      ushort checksum = 0;
      for (int index = 0; index < buffer.Length; ++index)
        checksum += (ushort) buffer[index];
      return checksum;
    }

    private string DecodeSN(byte[] buffer)
    {
      if (buffer == null || buffer[0] == byte.MaxValue && buffer[1] == byte.MaxValue && buffer[2] == byte.MaxValue && buffer[3] == byte.MaxValue && buffer[4] == byte.MaxValue && buffer[5] == byte.MaxValue && buffer[6] == byte.MaxValue && buffer[7] == byte.MaxValue)
        return string.Empty;
      if (buffer[0] == (byte) 0 && buffer[1] == (byte) 0 && buffer[2] == (byte) 0 && buffer[3] == (byte) 0 && buffer[4] == (byte) 0 && buffer[5] == (byte) 0 && buffer[6] == (byte) 0 && buffer[7] == (byte) 0)
        return string.Empty;
      try
      {
        string str = Encoding.ASCII.GetString(buffer, 0, 1);
        string manufacturer = MBusDevice.GetManufacturer(BitConverter.ToInt16(buffer, 1));
        byte num = buffer[3];
        uint uint32 = BitConverter.ToUInt32(buffer, 4);
        return string.Format("{0}{1}{2}{3}", (object) str, (object) manufacturer, (object) num.ToString("X2"), (object) uint32.ToString("X8"));
      }
      catch
      {
        return string.Empty;
      }
    }

    private byte[] EncodeSN(string value)
    {
      if (string.IsNullOrEmpty(value))
        return (byte[]) null;
      if (!value.StartsWith("FZRIF"))
        throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "ProductionNumberStartsWithWrongChars", "The production number starts with invalid chars."));
      string s = value.Length == 14 ? value.Substring(0, 1) : throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "ProductionNumberHasInvalidLength", "The production number has wrong length. Expected 14 chars."));
      string Manufacturer = value.Substring(1, 3);
      byte num = byte.Parse(value.Substring(4, 2), NumberStyles.HexNumber);
      string str = value.Substring(6);
      if (!Util.IsValidBCD(str))
        throw new ArgumentException(Ot.Gtm(Tg.HandlerLogic, "ProductionNumberHasInvalidSerialnumber", "The production number has invalid serialnumber."));
      List<byte> byteList = new List<byte>(14);
      byteList.AddRange((IEnumerable<byte>) Encoding.ASCII.GetBytes(s));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(MBusDevice.GetManufacturerCode(Manufacturer)));
      byteList.Add(num);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(Util.ConvertUnt32ToBcdUInt32(uint.Parse(str))));
      return byteList.Count == 8 ? byteList.ToArray() : throw new ArgumentOutOfRangeException("Wrong full serial number was generated!");
    }

    public static TH_DeviceIdentification Parse(
      string meterId,
      string hardwareTypeId,
      string meterInfoId,
      string baseTypeId,
      string meterTypeId,
      string sAP_MaterialNumber,
      string sAP_ProductionOrderNumber,
      string fullserialnumber,
      string fullserialnumberA,
      string fullserialnumberB)
    {
      TH_DeviceIdentification deviceIdentification = new TH_DeviceIdentification();
      uint result;
      if (uint.TryParse(meterId, out result) && result != uint.MaxValue)
        deviceIdentification.MeterID = new uint?(result);
      if (uint.TryParse(hardwareTypeId, out result) && result != uint.MaxValue)
        deviceIdentification.HardwareTypeID = new uint?(result);
      if (uint.TryParse(meterInfoId, out result) && result != uint.MaxValue)
        deviceIdentification.MeterInfoID = new uint?(result);
      if (uint.TryParse(baseTypeId, out result) && result != uint.MaxValue)
        deviceIdentification.BaseTypeID = new uint?(result);
      if (uint.TryParse(meterTypeId, out result) && result != uint.MaxValue)
        deviceIdentification.MeterTypeID = new uint?(result);
      if (uint.TryParse(sAP_MaterialNumber, out result) && result != uint.MaxValue)
        deviceIdentification.SapMaterialNumber = new uint?(result);
      if (uint.TryParse(sAP_ProductionOrderNumber, out result) && result != uint.MaxValue)
        deviceIdentification.SapProductionOrderNumber = new uint?(result);
      deviceIdentification.Fullserialnumber = fullserialnumber;
      deviceIdentification.FullserialnumberA = fullserialnumberA;
      deviceIdentification.FullserialnumberB = fullserialnumberB;
      return deviceIdentification;
    }
  }
}
