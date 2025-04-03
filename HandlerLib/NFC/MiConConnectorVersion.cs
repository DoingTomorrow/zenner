// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.MiConConnectorVersion
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using System;

#nullable disable
namespace HandlerLib.NFC
{
  public class MiConConnectorVersion : NfcDeviceIdentification
  {
    public MiConConnectorVersion()
    {
    }

    public MiConConnectorVersion(byte[] buffer)
    {
      int index1 = 3;
      this.nfcIdentFrameVersion = new byte?(buffer[index1]);
      int index2 = index1 + 1;
      byte? identFrameVersion1 = this.nfcIdentFrameVersion;
      int? nullable = identFrameVersion1.HasValue ? new int?((int) identFrameVersion1.GetValueOrDefault()) : new int?();
      int num1 = 0;
      if (nullable.GetValueOrDefault() == num1 & nullable.HasValue)
      {
        this.nfcProtocolVersion = new byte?(buffer[index2]);
        int index3 = index2 + 1;
        this.generation = new byte?(buffer[index3]);
        int startIndex1 = index3 + 1;
        this.iD_BCD = new uint?(BitConverter.ToUInt32(buffer, startIndex1));
        int startIndex2 = startIndex1 + 4;
        this.hardwareID = new uint?(BitConverter.ToUInt32(buffer, startIndex2));
        int startIndex3 = startIndex2 + 4;
        uint uint32 = BitConverter.ToUInt32(buffer, startIndex3);
        int startIndex4 = startIndex3 + 4;
        this.firmwareVersion = new uint?(uint32);
        this.svnRevision = new uint?(BitConverter.ToUInt32(buffer, startIndex4));
        int srcOffset1 = startIndex4 + 4;
        byte[] numArray = new byte[4];
        Buffer.BlockCopy((Array) buffer, srcOffset1, (Array) numArray, 0, 4);
        int startIndex5 = srcOffset1 + 4;
        this.buildTime = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(numArray);
        this.compilerVersion = new uint?(BitConverter.ToUInt32(buffer, startIndex5));
        int srcOffset2 = startIndex5 + 4;
        this.unique_ID = new byte[12];
        Buffer.BlockCopy((Array) buffer, srcOffset2, (Array) this.unique_ID, 0, 12);
      }
      else
      {
        byte? identFrameVersion2 = this.nfcIdentFrameVersion;
        nullable = identFrameVersion2.HasValue ? new int?((int) identFrameVersion2.GetValueOrDefault()) : new int?();
        int num2 = 1;
        int num3;
        if (!(nullable.GetValueOrDefault() == num2 & nullable.HasValue))
        {
          identFrameVersion2 = this.nfcIdentFrameVersion;
          nullable = identFrameVersion2.HasValue ? new int?((int) identFrameVersion2.GetValueOrDefault()) : new int?();
          int num4 = 2;
          num3 = nullable.GetValueOrDefault() == num4 & nullable.HasValue ? 1 : 0;
        }
        else
          num3 = 1;
        if (num3 == 0)
          return;
        this.nfcProtocolVersion = new byte?(buffer[index2]);
        int startIndex6 = index2 + 1;
        this.iD_BCD = new uint?(BitConverter.ToUInt32(buffer, startIndex6));
        int index4 = startIndex6 + 4;
        this.generation = new byte?(buffer[index4]);
        int startIndex7 = index4 + 1;
        this.manufacturer = new ushort?(BitConverter.ToUInt16(buffer, startIndex7));
        int index5 = startIndex7 + 2;
        this.medium = new byte?(buffer[index5]);
        int index6 = index5 + 1;
        this.obisMedium = new char?((char) buffer[index6]);
        int startIndex8 = index6 + 1;
        this.sAP_MaterialNumber = new uint?(BitConverter.ToUInt32(buffer, startIndex8));
        int num5 = startIndex8 + 4;
        this.sAP_ProductionOrderNumber = (string) null;
        int startIndex9 = num5 + 4;
        this.meterID = new uint?(BitConverter.ToUInt32(buffer, startIndex9));
        int startIndex10 = startIndex9 + 4 + 4;
        this.hardwareID = new uint?(BitConverter.ToUInt32(buffer, startIndex10));
        int startIndex11 = startIndex10 + 4;
        uint uint32_1 = BitConverter.ToUInt32(buffer, startIndex11);
        int startIndex12 = startIndex11 + 4;
        this.firmwareVersion = new uint?(uint32_1);
        this.svnRevision = new uint?(BitConverter.ToUInt32(buffer, startIndex12));
        int srcOffset3 = startIndex12 + 4;
        byte[] numArray = new byte[4];
        Buffer.BlockCopy((Array) buffer, srcOffset3, (Array) numArray, 0, 4);
        int startIndex13 = srcOffset3 + 4;
        this.buildTime = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(numArray);
        this.compilerVersion = new uint?(BitConverter.ToUInt32(buffer, startIndex13));
        int srcOffset4 = startIndex13 + 4;
        this.unique_ID = new byte[12];
        Buffer.BlockCopy((Array) buffer, srcOffset4, (Array) this.unique_ID, 0, 12);
        int startIndex14 = srcOffset4 + 12;
        identFrameVersion2 = this.nfcIdentFrameVersion;
        nullable = identFrameVersion2.HasValue ? new int?((int) identFrameVersion2.GetValueOrDefault()) : new int?();
        int num6 = 2;
        if (nullable.GetValueOrDefault() == num6 & nullable.HasValue)
        {
          uint uint32_2 = BitConverter.ToUInt32(buffer, startIndex14);
          int num7 = startIndex14 + 4;
          this.NDC_Lib_Version = new uint?(uint32_2);
        }
      }
    }

    public Version GetFirmwareVersion()
    {
      return new Version(new ZENNER.CommonLibrary.FirmwareVersion(this.firmwareVersion.Value).VersionString + "." + this.svnRevision.Value.ToString());
    }
  }
}
