// Decompiled with JetBrains decompiler
// Type: MBusLib.CI_Field
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using System.ComponentModel;

#nullable disable
namespace MBusLib
{
  public enum CI_Field : byte
  {
    [Description("Application select/reset. Select is used for logger reading applications. A reset will select the default application.")] ApplicationReset = 80, // 0x50
    [Description("Normal transmission of SND_UD, data send (M-Bus Master to M-Bus Device).")] DataSendMode1 = 81, // 0x51
    [Description("Opening for secondary addressing (selection of M-Bus Device) is required")] SelectionOfDevice = 82, // 0x52
    SelectionOfSlavesMode1 = 82, // 0x52
    SynronizeAction = 84, // 0x54
    DataSendMode2 = 85, // 0x55
    SelectionOfSlavesMode2 = 86, // 0x56
    [Description("COSEM Data sent by the Readout device to the meter with long Transport Layer")] COSEM_SentDeviceToMeterWithLongTPL = 96, // 0x60
    [Description("COSEM Data sent by the Readout device to the meter with short Transport Layer")] COSEM_SentDeviceToMeterWithShortTPL = 97, // 0x61
    [Description("Reserved for OBIS-based Data sent by the Readout device to the meter with long Transport Layer")] OBIS_SentDeviceToMeterLongTPL = 100, // 0x64
    [Description("Reserved for OBIS-based Data sent by the Readout device to the meter with short Transport Layer")] OBIS_SentDeviceToMeterShortTPL = 101, // 0x65
    [Description("EN 13757-3 Application Layer with Format frame and no Transport Layer")] APL_FormatFrame_NoTPL = 105, // 0x69
    [Description("EN 13757-3 Application Layer with Format frame and with short Transport Layer")] APL_FormatFrame_ShortTPL = 106, // 0x6A
    [Description("EN 13757-3 Application Layer with Format frame and with long Transport Layer")] APL_FormatFrame_LongTPL = 107, // 0x6B
    [Description("Clock synchronization (absolute)")] ClockSynchronisationAbsolute = 108, // 0x6C
    [Description("Clock synchronization (relative)")] ClockSynchronisationRelative = 109, // 0x6D
    [Description("Application error from device with short Transport Layer")] ApplicationErrorShortTPL = 110, // 0x6E
    [Description("Application error from device with long Transport Layer")] ApplicationErrorLongTPL = 111, // 0x6F
    [Description("Application error from device without Transport Layer")] ErrorReport = 112, // 0x70
    [Description("Alarm Report")] AlarmReport = 113, // 0x71
    [Description("Long 12 bytes APL header in RSP_UD response from device.")] MBusWithFullHeader = 114, // 0x72
    [Description("EN 13757-3 Application Layer with Compact frame and long Transport Layer")] APL_CompactFrameLongTPL = 115, // 0x73
    [Description("Alarm from device with short Transport Layer")] AlarmShortTPL = 116, // 0x74
    [Description("Alarm from device with long Transport Layer")] AlarmLongTPL = 117, // 0x75
    ResponseWithVariableDataStructure = 118, // 0x76
    Response_76h = 118, // 0x76
    ResponseWithFixDataStructure = 119, // 0x77
    [Description("No APL header in RSP_UD response from device.")] MBusWithoutHeader = 120, // 0x78
    [Description("EN 13757-3 Application Layer with Compact frame and no header")] APL_CompactFrameNoHeader = 121, // 0x79
    [Description("Short 4 bytes APL header in RSP_UD response from device.")] MBusWithShortHeader = 122, // 0x7A
    [Description("EN 13757-3 Application Layer with Compact frame and short header.")] APL_CompactFrameShortHeader = 123, // 0x7B
    [Description("COSEM Application Layer with long Transport Layer")] COSEM_APL_LongTPL = 124, // 0x7C
    [Description("COSEM Application Layer with short Transport Layer")] COSEM_APL_ShortTPL = 125, // 0x7D
    [Description("OBIS-based Application Layer with long Transport Layer")] OBIS_APL_LongTPL = 126, // 0x7E
    [Description("Reserved for OBIS-based Application Layer with short Transport Layer")] OBIS_APL_ShortTPL = 127, // 0x7F
    [Description("EN 13757-3 Transport Layer (long) from other device to the meter.")] TPL_LongFromOtherDeviceToMeter = 128, // 0x80
    [Description("Network Layer data (Repeater)")] Repeater = 129, // 0x81
    ForFutureUse = 130, // 0x82
    [Description("Network Management application")] NetworkManagementApplication = 131, // 0x83
    [Description("EN 13757-3 Transport Layer (short) from the meter to the other device.")] TPL_ShortFromMeterToOtherDevice = 138, // 0x8A
    [Description("EN 13757-3 Transport Layer (long) from the meter to the other device.")] TPL_LongFromMeterToOtherDevice = 139, // 0x8B
    [Description("Extended Link Layer I (2 Byte)")] ShortELLwithoutReceiverAddress = 140, // 0x8C
    [Description("Extended Link Layer II (8 Byte)")] ShortELLwithReceiverAddress = 141, // 0x8D
    LongELLwithReceiverAddress = 142, // 0x8E
    [Description("Authentication and Fragmentation Layer.")] AFL = 144, // 0x90
    [Description("Kamstrup specific from master to slave")] KamstrupMasterToSlave = 160, // 0xA0
    [Description("Kamstrup specific from slave to master")] KamstrupSlaveToMaster = 161, // 0xA1
    RequestReadoutOfCompleteRAMcontent = 177, // 0xB1
    SendUserData = 178, // 0xB2
    InitializeTestCalibrationMode = 179, // 0xB3
    EEPROMread = 180, // 0xB4
    StartSoftwareTest = 182, // 0xB6
    [Description("Baud rate shift to 300 baud.")] SetBaudrateTo300baud = 184, // 0xB8
    [Description("Baud rate shift to 600 baud.")] SetBaudrateTo600baud = 185, // 0xB9
    [Description("Baud rate shift to 1200 baud.")] SetBaudrateTo1200baud = 186, // 0xBA
    [Description("Baud rate shift to 2400 baud.")] SetBaudrateTo2400baud = 187, // 0xBB
    [Description("Baud rate shift to 4800 baud.")] SetBaudrateTo4800baud = 188, // 0xBC
    [Description("Baud rate shift to 9600 baud.")] SetBaudrateTo9600baud = 189, // 0xBD
    [Description("Baud rate shift to 19200 baud.")] SetBaudrateTo19200baud = 190, // 0xBE
    [Description("Baud rate shift to 38400 baud.")] SetBaudrateTo38400baud = 191, // 0xBF
    [Description("Image transfer (Down) TPL long")] CommandImageTransfer = 192, // 0xC0
    [Description("Image transfer (Up) TPL short")] ResponseShortImageTransfer = 192, // 0xC0
    [Description("Image transfer (Up) TPL long")] ResponseLongImageTransfer = 194, // 0xC2
    [Description("Security Information Transport (Down) TPL long")] CommandSecurityInformationTransport = 195, // 0xC3
    [Description("Security Information Transport (Up) TPL short")] ResponseShortSecurityInformationTransport = 196, // 0xC4
    [Description("Security Information Transport (Up) TPL long")] ResponseLongSecurityInformationTransport = 197, // 0xC5
  }
}
