// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.DeviceClass
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

#nullable disable
namespace InTheHand.Net.Bluetooth
{
  public enum DeviceClass
  {
    Miscellaneous = 0,
    Computer = 256, // 0x00000100
    DesktopComputer = 260, // 0x00000104
    ServerComputer = 264, // 0x00000108
    LaptopComputer = 268, // 0x0000010C
    HandheldComputer = 272, // 0x00000110
    PdaComputer = 276, // 0x00000114
    WearableComputer = 280, // 0x00000118
    Phone = 512, // 0x00000200
    CellPhone = 516, // 0x00000204
    CordlessPhone = 520, // 0x00000208
    SmartPhone = 524, // 0x0000020C
    WiredPhone = 528, // 0x00000210
    IsdnAccess = 532, // 0x00000214
    AccessPointAvailable = 768, // 0x00000300
    AccessPoint1To17 = 800, // 0x00000320
    AccessPoint17To33 = 832, // 0x00000340
    AccessPoint33To50 = 864, // 0x00000360
    AccessPoint50To67 = 896, // 0x00000380
    AccessPoint67To83 = 928, // 0x000003A0
    AccessPoint83To99 = 960, // 0x000003C0
    AccessPointNoService = 992, // 0x000003E0
    AudioVideoUnclassified = 1024, // 0x00000400
    AudioVideoHeadset = 1028, // 0x00000404
    AudioVideoHandsFree = 1032, // 0x00000408
    AudioVideoMicrophone = 1040, // 0x00000410
    AudioVideoLoudSpeaker = 1044, // 0x00000414
    AudioVideoHeadphones = 1048, // 0x00000418
    AudioVideoPortable = 1052, // 0x0000041C
    AudioVideoCar = 1056, // 0x00000420
    AudioVideoSetTopBox = 1060, // 0x00000424
    AudioVideoHiFi = 1064, // 0x00000428
    AudioVideoVcr = 1068, // 0x0000042C
    AudioVideoVideoCamera = 1072, // 0x00000430
    AudioVideoCamcorder = 1076, // 0x00000434
    AudioVideoMonitor = 1080, // 0x00000438
    AudioVideoDisplayLoudSpeaker = 1084, // 0x0000043C
    AudioVideoVideoConferencing = 1088, // 0x00000440
    AudioVideoGaming = 1096, // 0x00000448
    Peripheral = 1280, // 0x00000500
    PeripheralJoystick = 1284, // 0x00000504
    PeripheralGamepad = 1288, // 0x00000508
    PeripheralRemoteControl = 1292, // 0x0000050C
    PeripheralSensingDevice = 1296, // 0x00000510
    PeripheralDigitizerTablet = 1300, // 0x00000514
    PeripheralCardReader = 1304, // 0x00000518
    PeripheralKeyboard = 1344, // 0x00000540
    PeripheralPointingDevice = 1408, // 0x00000580
    PeripheralCombinedKeyboardPointingDevice = 1472, // 0x000005C0
    Imaging = 1536, // 0x00000600
    ImagingDisplay = 1552, // 0x00000610
    ImagingCamera = 1568, // 0x00000620
    ImagingScanner = 1600, // 0x00000640
    ImagingPrinter = 1664, // 0x00000680
    Wearable = 1792, // 0x00000700
    WearableWristWatch = 1796, // 0x00000704
    WearablePager = 1800, // 0x00000708
    WearableJacket = 1804, // 0x0000070C
    WearableHelmet = 1808, // 0x00000710
    WearableGlasses = 1812, // 0x00000714
    Toy = 2048, // 0x00000800
    ToyRobot = 2052, // 0x00000804
    ToyVehicle = 2056, // 0x00000808
    ToyFigure = 2058, // 0x0000080A
    ToyController = 2060, // 0x0000080C
    ToyGame = 2064, // 0x00000810
    Medical = 2304, // 0x00000900
    MedicalBloodPressureMonitor = 2308, // 0x00000904
    MedicalThermometer = 2312, // 0x00000908
    MedicalWeighingScale = 2314, // 0x0000090A
    MedicalGlucoseMeter = 2316, // 0x0000090C
    MedicalPulseOximeter = 2320, // 0x00000910
    MedicalHeartPulseRateMonitor = 2324, // 0x00000914
    MedicalDataDisplay = 2328, // 0x00000918
    Uncategorized = 7936, // 0x00001F00
  }
}
