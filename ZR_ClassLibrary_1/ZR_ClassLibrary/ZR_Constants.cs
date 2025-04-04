// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_Constants
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_Constants
  {
    public const string GMM_MainThreadName = "GMM main";
    public static string SystemNewLine = Environment.NewLine;
    public const int EEPADR_SERIALNR = 6;
    public const int EEPADR_MBUSSERIALNR = 10;
    public const int EEPADR_MBUSMANUFACTURER = 14;
    public const int EEPADR_MBUSMETERTYPE = 16;
    public const int EEPADR_MBUSMEDIUM = 17;
    public const int EEPADR_METERID = 18;
    public const int EEPADR_METERINFOID = 22;
    public const int EEPADR_STATICCHECKSUM = 54;
    public const int EEPADR_HEADERCHECKSUM = 56;
    public const int EEPADR_BACKUPCHECKSUM = 58;
    public const int HEADERSIZE = 76;
    public const int HEADERVARCOUNT = 7;
    public string[] HeaderVarNames = new string[7];
    public int[,] HeaderVarsAddr = new int[7, 2];
    public const byte ZR_FUNCTIONTABLE_VERSION = 3;
    public const int MAXEEPLENGTH = 8196;
    public const int MAXFUTABLELEN = 500;
    public const string NewLine = "<r>";
    public const string FixTextLine = "<C>";
    public const int ZR_PM_EPROMVARS = 1;
    public const int ZR_PM_PARAMETER = 2;
    public const int ZR_PM_RUNTIMEVARS = 3;
    public const int ZR_PM_RAMPARAMETER = 4;
    public const int ZR_PM_BACKUP = 5;
    public const int ZR_PM_STATIC = 6;
    public const int ZR_PM_FIXEDPARAM = 7;
    public const int ZR_PM_MBUS = 8;
    public const int ZR_PM_DATALOGGER = 9;
    public const int ZR_PM_WRITEPERMTABLE = 10;
    public const int ZR_PM_DISPLAYCODE = 11;
    public const int ZR_PM_RUNTIMECODE = 12;
    public const int ZR_PM_EPROMRUNTIME = 13;
    public const int ZR_PM_FUNCTIONTABLE = 14;
    public const int ZR_PM_LOGGERSTORE = 15;
    public const int ZR_PM_RESETRUNTIMECODE = 16;
    public const int ZR_PM_INTERVALRUNTIMECODE = 17;
    public const int ZR_PM_MESUREMENTCODE = 18;
    public const int ZR_PM_MBUSCODE = 19;
    public const int ZR_PM_EEPINTERVALRUNTIMECODE = 20;
    public const int ZR_PM_EVENTRUNTIMECODE = 21;
    public const int ZR_PM_STATIC_BACKUP = 22;
    public const int Bl_HEADER = 0;
    public const int Bl_RAMPARAMETER = 4;
    public const int Bl_PARAMETER = 2;
    public const int Bl_EPROMVARS = 1;
    public const int Bl_RUNTIMEVARS = 3;
    public const int Bl_BACKUP = 5;
    public const int Bl_STATIC = 6;
    public const int Bl_FIXEDPARAM = 7;
    public const int Bl_MBUS = 8;
    public const int Bl_DATALOGGER = 9;
    public const int Bl_WRITEPERMTABLE = 10;
    public const int Bl_DISPLAYCODE = 11;
    public const int Bl_RUNTIMECODE = 12;
    public const int Bl_EPROMRUNTIME = 13;
    public const int Bl_FUNCTIONTABLE = 14;
    public const int Bl_LOGGERSTORE = 15;
    public const int Bl_RESETRUNTIMECODE = 16;
    public const int BL_INTERVALRUNTIMECODE = 17;
    public const int Bl_MESUREMENTRUNTIMECODE = 18;
    public const int Bl_MBUSRUNTIMECODE = 19;
    public const int Bl_EEPINTERVALRUNTIMECODE = 20;
    public const int Bl_EVENTRUNTIMECODE = 21;
    public const int Bl_MBUSMINLIST = 22;
    public const int Bl_MBUSEXLIST = 23;
    public const int Bl_MBUS_DATALOGGER_MINLIST = 24;
    public const int Bl_MBUS_DATALOGGER_EXLIST = 25;
    public const byte P_MBUSENABLE = 4;
    public const byte P_MBUSLONGLIST = 1;
    public const byte P_MBUSSHORTLIST = 2;
    public const int ZR_FU_NORMAL = 1;
    public const int ZR_FU_FIRST = 2;
    public const int ZR_FU_MAIN = 3;
    public const int ZR_FU_INVISIBLE = 4;
    public const int ZR_FU_SYSTEM = 9;
    public const int ZR_ACC_PASSWORDLENGTH = 5;
    public const int ZR_FU_LOCKED = 1;
    public const int ZR_FU_UNLOCKED = 2;
    public const int MAX_MENU_ROW = 40;
    public const int MAX_MENU_COLUMN = 3;
    public const int SYSTEM_MENU_COLUMN = 3;
    public const int ZR_P_VALUE = 0;
    public const int ZR_P_PREPAIDFILE = 1;
    public const int ZR_P_INTERVALPOINT = 2;
    public const int ZR_P_INTERVAL = 3;
    public const int ZR_P_INTERVALOFFSET = 4;
    public const int ZR_P_STARTADDRESS = 5;
    public const int ZR_P_ENDADDRESS = 6;
    public const int ZR_P_WRITEPTR = 7;
    public const int ZR_P_FLAGS = 8;
    public const int ZR_P_COUNTERVAR = 9;
    public const int ZR_P_BYTEARRAY = 10;
    public const int ZR_P_TIMEPOINT = 11;
    public const int ZR_P_LENGTH = 20;
    public const int ZR_LOGGER_0 = 0;
    public const int ZR_LOGGER_1 = 1;
    public const int ZR_LOGGER_2 = 2;
    public const int ZR_LOGGER_3 = 3;
    public const int ZR_LOGGER_4 = 4;
    public const int ZR_LOGGER_5 = 5;
    public const int PA_INIT_PCINIT = 1;
    public const int PA_INIT_TYPEINIT = 2;
    public const int PA_INIT_PCACTUAL = 4;
    public const int PA_INIT_IDENT = 8;
    public const int PA_INIT_CALIB = 16;
    public const int PA_INIT_DV_LOAD = 32;
    public const int PA_INIT_UNCHANGED = 64;
    public const int PA_INIT_DV_LOADBASIC = 128;
    public const int PA_INIT_CHECKEQUAL = 256;
    public const int PA_INIT_ALL = -1;
    public const int ZR_NO_CONNECTIONERROR = 0;
    public const int ZR_COUNTER_CONNECTION = 1;
    public const int ZR_DB_CONNECTION = 2;
    public const int ZR_NOACTION = 3;
    public const int ZR_NEWHARDWARE = 4;
    public const int ZR_NEWMETERTYPE = 5;
    public const int ZR_METERTYPEEDIT = 6;
    public const int ZR_COUNTER_CONNECTION_TO_METERTYPE = 7;
    public const int ZR_COUNTER_CONNECTION_FOR_METERTYPE_PROGRAMMING = 8;
    public const int ZR_COUNTER_CONNECTION_SELECTIVE = 9;
    public const int ZR_COUNTER_ONLYCOMPILE = 10;
    public const byte ZR_Comp_NOTYPE = 99;
    public const byte ZR_Comp_BYTE = 0;
    public const byte ZR_Comp_WORD = 1;
    public const byte ZR_Comp_LONG = 2;
    public const byte ZR_Comp_PPTR = 3;
    public const byte ZR_Comp_EPTR = 4;
    public const byte ZR_Comp_MPTR = 5;
    public const byte ZR_Comp_SPTR = 6;
    public const byte ZR_Comp_CPTR = 7;
    public const byte ZR_Comp_ePTR = 8;
    public const byte ZR_Comp_iPTR = 9;
    public const string ZR_Comp_POINTER = "ePTR,iPTR";
    public const string ZR_Comp_INTTYPES = "BYTE;INT;LONG";
    public const string Serie3Resource = ";Serie3;";
    public const int MAXSYNCROWS = 5000;
    public const int ERRNO = 0;
    public const int ERRCONNECT = 1;
    public const int ERRDB = 2;
    public const int ERRSYSVAR = 4;
    public const int ERRNOTAVAILABLE = 8;
    public const int ERRNODATA = 16;
    public const int ERRNOTALLOWED = 32;
    public const int ERRCHECKSUM = 64;
    public const int ERRVERIFY = 128;
    public const int ERREOFFUTAB = 256;
    public const int ERRIDENT = 512;
    public const int ERRNOTFOUND = 1024;
    public const int ERRCONVERT = 2048;
    public const int ERRPROG = 4096;
    public const int ERREICHBIT = 8192;
    public const int ERROUTOFRANGE = 16384;
    public const int ERRREAD = 32768;
    public const int ERRNORESOURCES = 65536;
    public const int ERRINTERVALTIME = 131072;
    public const int ERREMPTYSTRING = 262144;
    public const int ERRUNKNOWNMENU = 67108864;
    public const int ERRNOALLOWEDPARAM = 134217728;
    public const int ERRNOPARAM = 268435456;
    public const int ERRLASTPARAM = 536870912;
    public const int ERRUNKNOWN = 1073741824;

    public ZR_Constants()
    {
      this.HeaderVarNames[0] = "SerialNr";
      this.HeaderVarNames[1] = "MBusSerialNr";
      this.HeaderVarNames[2] = "MBusManufacturer";
      this.HeaderVarNames[3] = "MBusMeterType";
      this.HeaderVarNames[4] = "MBusMedium";
      this.HeaderVarNames[5] = "MeterID";
      this.HeaderVarNames[6] = "MeterInfoID";
      this.HeaderVarsAddr[0, 0] = 6;
      this.HeaderVarsAddr[1, 0] = 10;
      this.HeaderVarsAddr[2, 0] = 14;
      this.HeaderVarsAddr[3, 0] = 16;
      this.HeaderVarsAddr[4, 0] = 17;
      this.HeaderVarsAddr[5, 0] = 18;
      this.HeaderVarsAddr[6, 0] = 22;
      this.HeaderVarsAddr[0, 1] = 4;
      this.HeaderVarsAddr[1, 1] = 4;
      this.HeaderVarsAddr[2, 1] = 2;
      this.HeaderVarsAddr[3, 1] = 1;
      this.HeaderVarsAddr[4, 1] = 1;
      this.HeaderVarsAddr[5, 1] = 4;
      this.HeaderVarsAddr[6, 1] = 4;
    }

    public enum DataState
    {
      NoData,
      Connect,
      DeviceIdent,
      DeviceOK,
      DeviceMenuChanged,
      DeviceMenuChangedOK,
      MeterLoad,
      MeterIdent,
      Type,
      TypeOK,
      TypeDeviceConnect,
      TypeDeviceIdent,
      TypeDeviceIdentAssigned,
      TypeDeviceOK,
      TypeDevMenuChanged,
      TypeDeviceMenuChangedOK,
      TypeMeterLoad,
      TypeMeterIdent,
      TypeLoad,
      TypeIdent,
      TypeDevice,
      TypeMenuChanged,
      TypeMenuChangedOK,
      DTypeDevice,
      DBOpen,
      DBDeviceLoad,
      DBDeviceIdent,
      DBOK,
      DBMenuChanged,
      DBMenuChangedOK,
      DBMeterLoad,
      DBMeterIdent,
      DBDevice,
      DBDevDevice,
      Device,
      DTypeSaved,
      DBDevDeviceSaved,
      DBDeviceSaved,
      TypeSaved,
      DevParamChanged,
      DTypeParamChanged,
      DBDevParamChanged,
      DBParamChanged,
      TypeParamChanged,
      DeviceDMChanged,
      DeviceDPMChanged,
      DeviceDPChanged,
      TypeDPChanged,
      TypeDMChanged,
      TypeDPMChanged,
    }

    public enum FrameNames
    {
      VolumeFrame = 0,
      FlowFrame = 2,
      EnergyFrame = 3,
      PowerFrame = 6,
      FrameCode = 7,
      Input1Frame = 8,
      Input2Frame = 9,
      Input1ImpValFrame = 10, // 0x0000000A
      Input2ImpValFrame = 11, // 0x0000000B
      BCFrame = 12, // 0x0000000C
      ImpulsValueFrame = 13, // 0x0000000D
      FrameNamesSize = 14, // 0x0000000E
    }
  }
}
