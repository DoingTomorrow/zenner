// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.FirmwareVersion
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace ZENNER.CommonLibrary
{
  [Serializable]
  public struct FirmwareVersion : IComparable
  {
    private static SortedList<ushort, string> TypeStrings = new SortedList<ushort, string>();

    static FirmwareVersion()
    {
      FirmwareVersion.TypeStrings.Add((ushort) 1, "C2 C3");
      FirmwareVersion.TypeStrings.Add((ushort) 2, "C2a");
      FirmwareVersion.TypeStrings.Add((ushort) 3, "Pulse");
      FirmwareVersion.TypeStrings.Add((ushort) 5, "C5");
      FirmwareVersion.TypeStrings.Add((ushort) 6, "IUW");
      FirmwareVersion.TypeStrings.Add((ushort) 7, "NFC_MiCon_Connector");
      FirmwareVersion.TypeStrings.Add((ushort) 8, "WR3");
      FirmwareVersion.TypeStrings.Add((ushort) 9, "WR4");
      FirmwareVersion.TypeStrings.Add((ushort) 14, "Minoprotect II");
      FirmwareVersion.TypeStrings.Add((ushort) 15, "Smoke detector");
      FirmwareVersion.TypeStrings.Add((ushort) 17, "EDC radio");
      FirmwareVersion.TypeStrings.Add((ushort) 18, "EDC M-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 19, "PDC wMBus");
      FirmwareVersion.TypeStrings.Add((ushort) 20, "PDC M-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 21, "PDC Radio3");
      FirmwareVersion.TypeStrings.Add((ushort) 22, "T&H sensor");
      FirmwareVersion.TypeStrings.Add((ushort) 23, "EDC SIGFOX");
      FirmwareVersion.TypeStrings.Add((ushort) 24, "PDC SIGFOX");
      FirmwareVersion.TypeStrings.Add((ushort) 25, "EDC LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 26, "PDC LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 27, "HCA LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 28, "SD LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 29, "micro LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 30, "micro wM-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 31, "EDC wM-Bus ST");
      FirmwareVersion.TypeStrings.Add((ushort) 32, "EDC LoRa 470 MHz");
      FirmwareVersion.TypeStrings.Add((ushort) 33, "EDC ModBus");
      FirmwareVersion.TypeStrings.Add((ushort) 34, "BootLoader");
      FirmwareVersion.TypeStrings.Add((ushort) 35, "C5 LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 36, "WR4 LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 37, "micro LoRa LL");
      FirmwareVersion.TypeStrings.Add((ushort) 38, "NFC SENSUS Connector");
      FirmwareVersion.TypeStrings.Add((ushort) 39, "NFC LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 40, "T&H LoRa");
      FirmwareVersion.TypeStrings.Add((ushort) 41, "EDC LoRa 915 MHz");
      FirmwareVersion.TypeStrings.Add((ushort) 42, "EDC NB-IoT");
      FirmwareVersion.TypeStrings.Add((ushort) 43, "IDU");
      FirmwareVersion.TypeStrings.Add((ushort) 44, "micro wM-Bus LL");
      FirmwareVersion.TypeStrings.Add((ushort) 45, "T&H sensor wMBus");
      FirmwareVersion.TypeStrings.Add((ushort) 46, "PDC LoRa 915 MHz");
      FirmwareVersion.TypeStrings.Add((ushort) 47, "UDC LoRa 915 MHz");
      FirmwareVersion.TypeStrings.Add((ushort) 48, "EDC mBus_Modbus");
      FirmwareVersion.TypeStrings.Add((ushort) 49, "IDU v1");
      FirmwareVersion.TypeStrings.Add((ushort) 50, "ODU");
      FirmwareVersion.TypeStrings.Add((ushort) 51, "EDC mBus_CJ188");
      FirmwareVersion.TypeStrings.Add((ushort) 52, "EDC RS485_Modbus");
      FirmwareVersion.TypeStrings.Add((ushort) 53, "EDC RS485_CJ188");
      FirmwareVersion.TypeStrings.Add((ushort) 54, "EDC NBIoT_LCSW");
      FirmwareVersion.TypeStrings.Add((ushort) 55, "NFC wMBus Connector");
      FirmwareVersion.TypeStrings.Add((ushort) 56, "NFC LoRa & wM-Bus Connector");
      FirmwareVersion.TypeStrings.Add((ushort) 57, "EDC NBIoT_YJSW");
      FirmwareVersion.TypeStrings.Add((ushort) 58, "PDC LoRa SD 868 MHz");
      FirmwareVersion.TypeStrings.Add((ushort) 59, "EDC MBus STM32 Controller");
      FirmwareVersion.TypeStrings.Add((ushort) 60, "EDC RS485 STM32 Controller");
      FirmwareVersion.TypeStrings.Add((ushort) 61, "EDC NBIoT_FSNH");
      FirmwareVersion.TypeStrings.Add((ushort) 62, "EDC NBIoT_XM");
      FirmwareVersion.TypeStrings.Add((ushort) 63, "SD wM-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 64, "CO2 Sensor");
      FirmwareVersion.TypeStrings.Add((ushort) 65, "T&H sensor radio(LoRa & wM-Bus)");
      FirmwareVersion.TypeStrings.Add((ushort) 66, "8E LoRa & wM-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 67, "8E M-Bus");
      FirmwareVersion.TypeStrings.Add((ushort) 68, "EDC NBIoT_Israel");
      FirmwareVersion.TypeStrings.Add((ushort) 69, "EDC NBIoT_TaiWan");
      FirmwareVersion.TypeStrings.Add((ushort) 70, "PDC GAS");
      FirmwareVersion.TypeStrings.Add((ushort) 71, "M7+");
      FirmwareVersion.TypeStrings.Add((ushort) 72, "micro radio3 LL");
      FirmwareVersion.TypeStrings.Add((ushort) 73, "MinoConnect");
      FirmwareVersion.TypeStrings.Add((ushort) 74, "EDC LoRa 868 v3");
      FirmwareVersion.TypeStrings.Add((ushort) 75, "EDC LoRa 915 v2 US");
      FirmwareVersion.TypeStrings.Add((ushort) 76, "EDC LoRa 915 v2 BR");
    }

    public uint Version { get; private set; }

    public byte Major => (byte) this.GetPartValue(VersionMasks.Major);

    public byte Minor => (byte) this.GetPartValue(VersionMasks.Minor);

    public ushort Revision => this.GetPartValue(VersionMasks.Revision);

    public ushort Type => this.GetPartValue(VersionMasks.Type);

    public ushort TypeBSL
    {
      get
      {
        if (!this.IsBootLoader)
          throw new Exception("TypeBSL only available for Bootloader firmware");
        return this.GetPartValue(VersionMasks.BSL_Type);
      }
    }

    public bool IsBootLoader => this.Type == (ushort) 34;

    public bool IsDebugVersion => (this.Version & 2147483648U) > 0U;

    public string TypeString => this.GetTypeString(this.Type);

    public string TypeBSLString => this.GetTypeString(this.TypeBSL);

    public string VersionString
    {
      get
      {
        return string.Format("{0}.{1}.{2}", (object) this.Major, (object) this.Minor, (object) this.Revision);
      }
    }

    private string GetTypeString(ushort uType)
    {
      int index = FirmwareVersion.TypeStrings.IndexOfKey(uType);
      return index >= 0 ? FirmwareVersion.TypeStrings.Values[index] : "Unknown type: " + this.Type.ToString();
    }

    private ushort GetPartValue(VersionMasks partMask)
    {
      uint partValue = (uint) ((VersionMasks) this.Version & partMask);
      for (uint index = (uint) partMask; ((int) index & 1) == 0; index >>= 1)
        partValue >>= 1;
      return (ushort) partValue;
    }

    private void SetPartValue(VersionMasks partMask, ushort theValue)
    {
      uint num = (uint) theValue;
      for (uint index = (uint) partMask; ((int) index & 1) == 0; index >>= 1)
        num <<= 1;
      if (((VersionMasks) num & ~partMask) > ~(VersionMasks.Type | VersionMasks.BSL_Type | VersionMasks.Major | VersionMasks.Debug))
        throw new Exception("Revision part out of range");
      this.Version |= num;
    }

    public override string ToString()
    {
      string str;
      if (this.IsBootLoader)
        str = string.Format("{0} {2} for {1}", (object) this.Major, (object) this.TypeBSLString, (object) this.TypeString);
      else
        str = string.Format("{0}.{1}.{2} {3}", (object) this.Major, (object) this.Minor, (object) this.Revision, (object) this.TypeString);
      if (this.IsDebugVersion)
        str += ":Debug";
      return str;
    }

    public FirmwareVersion(uint versionValue) => this.Version = versionValue;

    public FirmwareVersion(string versionString)
      : this()
    {
      this.NewFromString(versionString);
    }

    private FirmwareVersion(object version)
      : this()
    {
      switch (version)
      {
        case string _:
          this.NewFromString((string) version);
          break;
        case uint num:
          this.Version = num;
          break;
        case FirmwareVersion firmwareVersion:
          this.Version = firmwareVersion.Version;
          break;
        default:
          throw new Exception("Not supported initialisation type");
      }
    }

    private void NewFromString(string versionString)
    {
      versionString = versionString.Trim();
      this.Version = 0U;
      if (versionString.Contains("."))
      {
        try
        {
          string[] strArray = versionString.Split(new char[1]
          {
            '.'
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length == 3)
          {
            int length = strArray[2].IndexOf(' ');
            if (length > 0)
            {
              string s = strArray[2].Substring(0, length);
              int num = strArray[2].IndexOf(":Debug");
              string str;
              if (num > 0)
              {
                this.Version += 2147483648U;
                str = strArray[2].Substring(length + 1, num - length - 1);
              }
              else
                str = strArray[2].Substring(length + 1);
              int index = FirmwareVersion.TypeStrings.IndexOfValue(str);
              if (index >= 0)
              {
                this.SetPartValue(VersionMasks.Major, ushort.Parse(strArray[0]));
                this.SetPartValue(VersionMasks.Minor, ushort.Parse(strArray[1]));
                this.SetPartValue(VersionMasks.Revision, ushort.Parse(s));
                this.SetPartValue(VersionMasks.Type, FirmwareVersion.TypeStrings.Keys[index]);
                return;
              }
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Illegal firmware display string: " + versionString, ex);
        }
        throw new Exception("Illegal firmware display string: " + versionString);
      }
      uint result;
      if (!uint.TryParse(versionString, NumberStyles.HexNumber, (IFormatProvider) null, out result))
        throw new Exception("Illegal hex firmware version string: " + versionString);
      this.Version = result;
    }

    public int CompareTo(object compareObject)
    {
      FirmwareVersion firmwareVersion = new FirmwareVersion(compareObject);
      int num1 = this.TypeString.CompareTo(firmwareVersion.TypeString);
      byte num2;
      if (num1 == 0)
      {
        num2 = this.Major;
        num1 = num2.CompareTo(firmwareVersion.Major);
      }
      if (num1 == 0)
      {
        num2 = this.Minor;
        num1 = num2.CompareTo(firmwareVersion.Minor);
      }
      if (num1 == 0)
        num1 = this.Revision.CompareTo(firmwareVersion.Revision);
      return num1;
    }

    public static bool operator >(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) > 0;
    }

    public static bool operator <(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) < 0;
    }

    public static bool operator >=(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) >= 0;
    }

    public static bool operator <=(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) <= 0;
    }

    public static bool operator ==(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) == 0;
    }

    public static bool operator !=(FirmwareVersion operand1, object operand2object)
    {
      FirmwareVersion compareObject = new FirmwareVersion(operand2object);
      return operand1.CompareTo((object) compareObject) != 0;
    }

    public bool IsInRange(object fromVersion, object toVersion)
    {
      return !(this < (object) new FirmwareVersion(fromVersion)) && !(this > (object) new FirmwareVersion(toVersion));
    }
  }
}
