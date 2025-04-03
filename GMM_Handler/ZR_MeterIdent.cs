// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ZR_MeterIdent
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

#nullable disable
namespace GMM_Handler
{
  public class ZR_MeterIdent
  {
    public MeterBasis TheMeterBasis;
    public short MeterClonNumber = 0;
    public int MeterID;
    public int MeterInfoID;
    public int MeterInfoBaseID;
    public int MeterTypeID;
    public int MapId;
    public int LinkerTableID;
    public int MeterHardwareID;
    public int HardwareTypeID;
    public ushort DefaultFunctionNr;
    public string sFirmwareVersion;
    public long lFirmwareVersion;
    public int extEEPSize;
    public int extEEPUsed;
    public string HardwareVersion;
    public string HardwareName;
    public string SerialNr;
    public string MBusSerialNr;
    public string HardwareResource;
    public string MeterInfoDescription;
    public string PPSArtikelNr;
    public string TypeOverrideString = string.Empty;
    public short MBus_Manufacturer;
    public byte MBus_MeterType;
    public byte MBus_Medium;
    public int MBus_SerialNumber;

    public ZR_MeterIdent(MeterBasis TheMeterBasisIn)
    {
      this.Clear();
      this.TheMeterBasis = TheMeterBasisIn;
    }

    public ZR_MeterIdent(bool Init, MeterBasis TheMeterBasisIn)
    {
      if (Init)
        this.Clear();
      this.TheMeterBasis = TheMeterBasisIn;
    }

    public ZR_MeterIdent Clone()
    {
      return new ZR_MeterIdent(false, this.TheMeterBasis)
      {
        MeterClonNumber = this.MeterClonNumber,
        MeterID = this.MeterID,
        MeterInfoID = this.MeterInfoID,
        MeterInfoBaseID = this.MeterInfoBaseID,
        MapId = this.MapId,
        LinkerTableID = this.LinkerTableID,
        MeterTypeID = this.MeterTypeID,
        MeterHardwareID = this.MeterHardwareID,
        HardwareTypeID = this.HardwareTypeID,
        DefaultFunctionNr = this.DefaultFunctionNr,
        sFirmwareVersion = this.sFirmwareVersion,
        lFirmwareVersion = this.lFirmwareVersion,
        extEEPSize = this.extEEPSize,
        extEEPUsed = this.extEEPUsed,
        HardwareVersion = this.HardwareVersion,
        HardwareName = this.HardwareName,
        SerialNr = this.SerialNr,
        MBusSerialNr = this.MBusSerialNr,
        HardwareResource = this.HardwareResource,
        MeterInfoDescription = this.MeterInfoDescription,
        PPSArtikelNr = this.PPSArtikelNr,
        TypeOverrideString = this.TypeOverrideString,
        MBus_Manufacturer = this.MBus_Manufacturer,
        MBus_MeterType = this.MBus_MeterType,
        MBus_Medium = this.MBus_Medium,
        MBus_SerialNumber = this.MBus_SerialNumber
      };
    }

    public int setIdent(ZR_MeterIdent theIdent)
    {
      int num = 0;
      this.MeterID = theIdent.MeterID;
      this.MeterInfoID = theIdent.MeterInfoID;
      this.MeterInfoBaseID = theIdent.MeterInfoBaseID;
      this.DefaultFunctionNr = theIdent.DefaultFunctionNr;
      this.sFirmwareVersion = theIdent.sFirmwareVersion;
      this.lFirmwareVersion = theIdent.lFirmwareVersion;
      this.HardwareVersion = theIdent.HardwareVersion;
      this.HardwareName = theIdent.HardwareName;
      this.SerialNr = theIdent.SerialNr;
      this.MBusSerialNr = theIdent.MBusSerialNr;
      this.HardwareResource = theIdent.HardwareResource;
      this.MeterInfoDescription = theIdent.MeterInfoDescription;
      this.PPSArtikelNr = theIdent.PPSArtikelNr;
      return num;
    }

    public void Clear()
    {
      this.MeterID = 0;
      this.MeterInfoID = 0;
      this.MeterInfoBaseID = 0;
      this.DefaultFunctionNr = (ushort) 0;
      this.sFirmwareVersion = "0";
      this.lFirmwareVersion = 0L;
      this.HardwareVersion = "0";
      this.HardwareName = "-";
      this.SerialNr = "00000000";
      this.MBusSerialNr = "00000000";
      this.HardwareResource = "";
      this.MeterInfoDescription = "";
      this.PPSArtikelNr = "";
    }
  }
}
