// Decompiled with JetBrains decompiler
// Type: HandlerLib.NfcDeviceIdentification
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using HandlerLib.NFC;
using MBusLib;
using System;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  [Serializable]
  public class NfcDeviceIdentification : DeviceIdentification, IPrintable
  {
    protected byte? nfcIdentFrameVersion;
    protected byte? nfcProtocolVersion;
    protected uint? compilerVersion;
    protected byte? numberOfAvailableParameterGroups;
    protected ushort? numberOfAvailableParameters;
    protected byte? numberOfSelectedParameterGroups;
    protected ushort? numberOfSelectedParameters;
    protected ushort? maximumRecordLength;

    public NfcDeviceIdentification()
    {
    }

    public NfcDeviceIdentification(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      this.iD_BCD = new uint?(serialNumberBCD);
      this.manufacturer = new ushort?(manufacturerCode);
      this.generation = new byte?(generation);
      this.medium = new byte?(mediumCode);
    }

    public NfcDeviceIdentification(NfcFrame deviceIdFrame)
    {
      if (deviceIdFrame == null)
        throw new Exception("No deviceIdFrame");
      if (deviceIdFrame.NfcResponseFrame.Length != 35)
        throw new Exception("Illegal deviceID response frame length");
      if (deviceIdFrame.NfcResponseFrame[1] != (byte) 1)
        throw new Exception("Illegal deviceID response command byte");
      this.setIdentification(deviceIdFrame.NfcResponseFrame);
    }

    public NfcDeviceIdentification(byte[] ResponseFrame)
    {
      if (ResponseFrame == null)
        throw new Exception("No response frame");
      if (ResponseFrame.Length < 35)
        throw new ArgumentException("Wrong length of response frame");
      this.setIdentification(ResponseFrame);
    }

    public NfcDeviceIdentification(
      byte[] NfcResponseFrame,
      uint offsetIdentStart = 0,
      uint frameLenght = 36)
    {
      if (NfcResponseFrame == null)
        throw new Exception("No deviceID response frame");
      if ((long) NfcResponseFrame.Length != (long) frameLenght)
        throw new Exception("Illegal deviceID response frame length");
      byte[] numArray = new byte[0];
      Buffer.BlockCopy((Array) NfcResponseFrame, (int) offsetIdentStart, (Array) numArray, 0, (int) ((long) NfcResponseFrame.Length - (long) offsetIdentStart));
      this.setIdentification(numArray);
    }

    private void setIdentification(byte[] ResponseFrame)
    {
      this.nfcIdentFrameVersion = new byte?(ResponseFrame[2]);
      byte? identFrameVersion = this.nfcIdentFrameVersion;
      if (identFrameVersion.HasValue)
      {
        switch (identFrameVersion.GetValueOrDefault())
        {
          case 0:
            this.setIdentificationVersion_0(ResponseFrame);
            return;
          case 1:
            this.setIdentificationVersion_1(ResponseFrame);
            return;
          case 2:
            this.setIdentificationVersion_2(ResponseFrame);
            return;
        }
      }
      throw new Exception("Unknown identification flame version: " + this.nfcIdentFrameVersion.ToString());
    }

    private void setIdentificationVersion_0(byte[] ResponseFrame)
    {
      this.nfcProtocolVersion = new byte?(ResponseFrame[3]);
      this.medium = new byte?(ResponseFrame[4]);
      this.obisMedium = new char?((char) ResponseFrame[5]);
      this.manufacturer = new ushort?(BitConverter.ToUInt16(ResponseFrame, 6));
      this.generation = new byte?(ResponseFrame[8]);
      this.iD_BCD = new uint?(BitConverter.ToUInt32(ResponseFrame, 9));
      this.hardwareID = new uint?((uint) BitConverter.ToUInt16(ResponseFrame, 13));
      this.firmwareVersion = new uint?(BitConverter.ToUInt32(ResponseFrame, 15));
      this.meterID = new uint?(BitConverter.ToUInt32(ResponseFrame, 19));
      this.hardwareTypeID = new uint?(BitConverter.ToUInt32(ResponseFrame, 23));
      this.numberOfAvailableParameterGroups = new byte?(ResponseFrame[27]);
      this.numberOfAvailableParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 28));
      this.numberOfSelectedParameterGroups = new byte?(ResponseFrame[30]);
      this.numberOfSelectedParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 31));
      this.maximumRecordLength = new ushort?((ushort) ResponseFrame[33]);
    }

    private void setIdentificationVersion_1(byte[] ResponseFrame)
    {
      this.nfcProtocolVersion = new byte?(ResponseFrame[3]);
      this.medium = new byte?(ResponseFrame[4]);
      this.obisMedium = new char?((char) ResponseFrame[5]);
      this.manufacturer = new ushort?(BitConverter.ToUInt16(ResponseFrame, 6));
      this.generation = new byte?(ResponseFrame[8]);
      this.iD_BCD = new uint?(BitConverter.ToUInt32(ResponseFrame, 9));
      this.hardwareID = new uint?((uint) BitConverter.ToUInt16(ResponseFrame, 13));
      this.deviceStatusFlags = new uint?((uint) BitConverter.ToUInt16(ResponseFrame, 15));
      this.firmwareVersion = new uint?(BitConverter.ToUInt32(ResponseFrame, 17));
      this.meterID = new uint?(BitConverter.ToUInt32(ResponseFrame, 21));
      this.svnRevision = new uint?(BitConverter.ToUInt32(ResponseFrame, 25));
      byte[] numArray = new byte[4];
      Buffer.BlockCopy((Array) ResponseFrame, 29, (Array) numArray, 0, 4);
      this.buildTime = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(numArray);
      this.compilerVersion = new uint?(BitConverter.ToUInt32(ResponseFrame, 33));
      this.signatur = new ushort?(BitConverter.ToUInt16(ResponseFrame, 37));
      this.numberOfAvailableParameterGroups = new byte?(ResponseFrame[39]);
      this.numberOfAvailableParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 40));
      this.numberOfSelectedParameterGroups = new byte?(ResponseFrame[42]);
      this.numberOfSelectedParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 43));
      this.maximumRecordLength = new ushort?((ushort) ResponseFrame[45]);
    }

    private void setIdentificationVersion_2(byte[] ResponseFrame)
    {
      this.nfcProtocolVersion = new byte?(ResponseFrame[3]);
      this.medium = new byte?(ResponseFrame[4]);
      this.obisMedium = new char?((char) ResponseFrame[5]);
      this.manufacturer = new ushort?(BitConverter.ToUInt16(ResponseFrame, 6));
      this.generation = new byte?(ResponseFrame[8]);
      this.iD_BCD = new uint?(BitConverter.ToUInt32(ResponseFrame, 9));
      this.hardwareID = new uint?((uint) BitConverter.ToUInt16(ResponseFrame, 13));
      this.deviceStatusFlags = new uint?(BitConverter.ToUInt32(ResponseFrame, 15));
      this.firmwareVersion = new uint?(BitConverter.ToUInt32(ResponseFrame, 19));
      this.meterID = new uint?(BitConverter.ToUInt32(ResponseFrame, 23));
      this.svnRevision = new uint?(BitConverter.ToUInt32(ResponseFrame, 27));
      byte[] numArray = new byte[4];
      Buffer.BlockCopy((Array) ResponseFrame, 31, (Array) numArray, 0, 4);
      this.buildTime = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(numArray);
      this.compilerVersion = new uint?(BitConverter.ToUInt32(ResponseFrame, 35));
      this.signatur = new ushort?(BitConverter.ToUInt16(ResponseFrame, 39));
      this.numberOfAvailableParameterGroups = new byte?(ResponseFrame[41]);
      this.numberOfAvailableParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 42));
      this.numberOfSelectedParameterGroups = new byte?(ResponseFrame[44]);
      this.numberOfSelectedParameters = new ushort?(BitConverter.ToUInt16(ResponseFrame, 45));
      this.maximumRecordLength = new ushort?((ushort) ResponseFrame[47]);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Serial number: " + this.FullSerialNumber);
      stringBuilder1.AppendLine("HardwareVersions: " + this.HardwareID.Value.ToString("x08"));
      stringBuilder1.AppendLine("FirmwareVersion: " + this.GetFirmwareVersionString());
      stringBuilder1.AppendLine("Medium: " + this.GetMediumAsText());
      StringBuilder stringBuilder2 = stringBuilder1;
      uint? nullable = this.MeterID;
      string str1 = nullable.ToString();
      nullable = this.hardwareTypeID;
      string str2 = nullable.ToString();
      string str3 = "MeterID: " + str1 + "; HardwareTypeID: " + str2;
      stringBuilder2.AppendLine(str3);
      return stringBuilder1.ToString();
    }

    public new string Print(int spaces = 0) => Utility.PrintObject((object) this);

    public override DeviceIdentification Clone()
    {
      return (DeviceIdentification) (this.MemberwiseClone() as NfcDeviceIdentification);
    }

    public virtual byte? NfcIdentFrameVersion
    {
      get => this.nfcIdentFrameVersion;
      set => throw new Exception("Write to NfcIdentFrameVersion not defined");
    }

    public virtual byte? NfcProtocolVersion
    {
      get => this.nfcProtocolVersion;
      set => throw new Exception("Write to NfcProtocolVersion not defined");
    }

    public virtual uint? CompilerVersion
    {
      get => this.compilerVersion;
      set => throw new Exception("Write to CompilerVersion not defined");
    }

    public virtual byte? NumberOfAvailableParameterGroups
    {
      get => this.numberOfAvailableParameterGroups;
      set => throw new Exception("Write to NumberOfAvailableParameterGroups not defined");
    }

    public virtual ushort? NumberOfAvailableParameters
    {
      get => this.numberOfAvailableParameters;
      set => throw new Exception("Write to NumberOfAvailableParameters not defined");
    }

    public virtual byte? NumberOfSelectedParameterGroups
    {
      get => this.numberOfSelectedParameterGroups;
      set => throw new Exception("Write to NumberOfSelectedParameterGroups not defined");
    }

    public virtual ushort? NumberOfSelectedParameters
    {
      get => this.numberOfSelectedParameters;
      set => throw new Exception("Write to NumberOfSelectedParameters not defined");
    }

    public virtual ushort? MaximumRecordLength
    {
      get => this.maximumRecordLength;
      set => throw new Exception("Write to MaximumRecordLength not defined");
    }
  }
}
