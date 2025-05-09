﻿// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.PPS_CacheParmNames
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

#nullable disable
namespace ZR_ClassLibrary
{
  public enum PPS_CacheParmNames
  {
    ShortText = 1,
    EAN_Number = 2,
    MeterTypeSeries = 3,
    HeatCalculatorType = 4,
    VolMeterType = 5,
    TempSensorType = 6,
    HeatEquipmentType = 7,
    FrontDesign = 8,
    MeterColor = 9,
    Menu = 10, // 0x0000000A
    EnergyCalculation = 11, // 0x0000000B
    EnergyMeterUnit = 12, // 0x0000000C
    TempSensorElement = 13, // 0x0000000D
    TempAsymmetry = 14, // 0x0000000E
    TempMeasurement = 15, // 0x0000000F
    VolTempRange = 16, // 0x00000010
    HeatTempRange = 17, // 0x00000011
    HeatADTempRange = 18, // 0x00000012
    SensorTempRange = 19, // 0x00000013
    SensorConnector = 20, // 0x00000014
    TempFittingList = 21, // 0x00000015
    TempInstallationPoint = 22, // 0x00000016
    TempSensorDimensions = 23, // 0x00000017
    TempSensorCable = 24, // 0x00000018
    TempSensorConnector = 25, // 0x00000019
    VolMeterNominalFlow = 26, // 0x0000001A
    VolMeterLength = 27, // 0x0000001B
    VolMeterConnection = 28, // 0x0000001C
    VolMeterSinglePipeConnection = 29, // 0x0000001D
    VolMeterDimensions = 30, // 0x0000001E
    VolumeUnit = 31, // 0x0000001F
    VolMeterPulseValue = 32, // 0x00000020
    VolMeterPosition = 33, // 0x00000021
    VolMeterMaxFrequency = 34, // 0x00000022
    VolMeterInputType = 35, // 0x00000023
    HeatMeterVolInputType = 36, // 0x00000024
    VolOutputType = 37, // 0x00000025
    VolMeterApproval = 38, // 0x00000026
    VolMeterCalibration = 39, // 0x00000027
    InOut1_Hardware = 40, // 0x00000028
    InOut1_Function = 41, // 0x00000029
    InOut1_PulseValue = 42, // 0x0000002A
    InOut1_Unit = 43, // 0x0000002B
    InOut2_Hardware = 44, // 0x0000002C
    InOut2_Function = 45, // 0x0000002D
    InOut2_PulseValue = 46, // 0x0000002E
    InOut2_Unit = 47, // 0x0000002F
    CommunicationInterface = 48, // 0x00000030
    HeatMeterApproval = 49, // 0x00000031
    HeatMeterTestMethod = 50, // 0x00000032
    HeatMeterCalibration = 51, // 0x00000033
    HeatDTLimits = 52, // 0x00000034
    VolumeMeterMID_qs = 53, // 0x00000035
    VolumeMeterMID_qp = 54, // 0x00000036
    VolumeMeterMID_qi = 55, // 0x00000037
    CalibrationTimePeriod = 56, // 0x00000038
    BatteryEndTime = 57, // 0x00000039
    DueDate = 58, // 0x0000003A
    UTC_TimeZone = 59, // 0x0000003B
    CaseOption = 60, // 0x0000003C
    TypeLabelDevice = 61, // 0x0000003D
    TypeLabelCarton = 62, // 0x0000003E
    MBusParameterList = 63, // 0x0000003F
    AdditionalProperties = 64, // 0x00000040
    BaseParameterSet = 65, // 0x00000041
    Manufacturer = 66, // 0x00000042
    Order_Number = 67, // 0x00000043
    Alias_Number = 68, // 0x00000044
    CreationDate = 69, // 0x00000045
    RadioFrequency = 70, // 0x00000046
    WaveFlowDeviceType = 71, // 0x00000047
    WafeFlowDeviceModel = 72, // 0x00000048
    Input1PulseValue = 73, // 0x00000049
    Input2PulseValue = 74, // 0x0000004A
    Input3PulseValue = 75, // 0x0000004B
    Input4PulseValue = 76, // 0x0000004C
    LoggerInterval = 77, // 0x0000004D
    LoggerDay = 78, // 0x0000004E
    LoggerPeriod = 79, // 0x0000004F
    LogOrStartHour = 80, // 0x00000050
    ManipulationDetection = 81, // 0x00000051
    ManipulationTypeInput1 = 82, // 0x00000052
    ManipulationTypeInput2 = 83, // 0x00000053
    ManipulationTypeInput3 = 84, // 0x00000054
    ManipulationTypeInput4 = 85, // 0x00000055
    TransmitterPower = 86, // 0x00000056
    HardwareTypeID = 87, // 0x00000057
    WaterMeterConnection = 88, // 0x00000058
    WaterMeterModel = 89, // 0x00000059
    WaterMeterLength = 90, // 0x0000005A
    WaterMeterTestMethod = 91, // 0x0000005B
    WaterMeterRemoteIndication = 92, // 0x0000005C
    WaterMeterManufacturer = 93, // 0x0000005D
    WaterMeterNominalFlow = 94, // 0x0000005E
    WaterMeterTempRange = 95, // 0x0000005F
    MaterialNumberRDM = 96, // 0x00000060
    MinCyclusTime = 97, // 0x00000061
    MaxCyclusTime = 98, // 0x00000062
    EnergyCalculationAtDTMin = 99, // 0x00000063
    DTMinHeat = 100, // 0x00000064
    DTMinCooling = 101, // 0x00000065
    HeatCoolingSwitchTemperature = 102, // 0x00000066
    CoolingMeterTestMethod = 103, // 0x00000067
    TarifMeterTestMethod = 104, // 0x00000068
    CoolingMeterApproval = 105, // 0x00000069
    TarifMeterApproval = 106, // 0x0000006A
    CustomerArticleNumber1 = 107, // 0x0000006B
    CustomerArticleNumber2 = 108, // 0x0000006C
    VolMeterModel = 109, // 0x0000006D
    VolMeterTestMethod = 110, // 0x0000006E
    VolMeterCableLength = 111, // 0x0000006F
    VolMeterFlowQS = 112, // 0x00000070
    VolMeterFlowQP = 113, // 0x00000071
    VolMeterFlowQI = 114, // 0x00000072
    VolMeterTestPulseValue = 115, // 0x00000073
    VolMeterNominalPressure = 116, // 0x00000074
    InOut3_Hardware = 117, // 0x00000075
    InOut3_Function = 118, // 0x00000076
    InOut3_PulseValue = 119, // 0x00000077
    InOut3_Unit = 120, // 0x00000078
    VolumeMeterType = 121, // 0x00000079
    WaermeTQ = 122, // 0x0000007A
    DtminQminDirect = 123, // 0x0000007B
    DtminQminPocket = 124, // 0x0000007C
    BatteryType = 125, // 0x0000007D
    Branding = 126, // 0x0000007E
    LeakDetection = 127, // 0x0000007F
    BurstDetection = 128, // 0x00000080
    BackflowDetection = 129, // 0x00000081
    StandstillDetection = 130, // 0x00000082
    FrequencyBand = 131, // 0x00000083
    RadioMode = 132, // 0x00000084
    DismantlingDetection = 133, // 0x00000085
    CoilManipulationDetection = 134, // 0x00000086
    MBusHeaderType = 135, // 0x00000087
    Mounting = 136, // 0x00000088
    OutputModes = 137, // 0x00000089
    TransmissionInterval = 138, // 0x0000008A
    UndersizeDetection = 139, // 0x0000008B
    OversizeDetection = 140, // 0x0000008C
    NumberOfRolls = 141, // 0x0000008D
    EdcType = 142, // 0x0000008E
    PressureLevel = 143, // 0x0000008F
    Ratio = 144, // 0x00000090
    WaterMeterType = 145, // 0x00000091
    MaterialNumberEDCHardware = 146, // 0x00000092
    UserID = 147, // 0x00000093
    EdcHardwareType = 148, // 0x00000094
    AesKey = 149, // 0x00000095
    Model = 150, // 0x00000096
    VolMeterMaximumPressure = 151, // 0x00000097
    InputConfiguration = 152, // 0x00000098
    PulseValueInputA = 153, // 0x00000099
    PulseValueInputB = 154, // 0x0000009A
    HcaModel = 155, // 0x0000009B
    MountingForm = 156, // 0x0000009C
    ProductionStage = 157, // 0x0000009D
    Radio = 158, // 0x0000009E
    Principle = 159, // 0x0000009F
    Type = 160, // 0x000000A0
    CentralCaptureSeries = 161, // 0x000000A1
    BarcodeType = 162, // 0x000000A2
    CombiCableLength = 163, // 0x000000A3
    PulseInputType = 164, // 0x000000A4
    ReedInputA = 165, // 0x000000A5
    ReedInputB = 166, // 0x000000A6
    Senario = 167, // 0x000000A7
    CableProtection = 168, // 0x000000A8
    StartChannel = 169, // 0x000000A9
    UpStreamChannelQuantity = 170, // 0x000000AA
    ActivationMode = 171, // 0x000000AB
    DeliveryMode = 172, // 0x000000AC
    UpDownChannelScenario = 173, // 0x000000AD
    AppSKey = 174, // 0x000000AE
    NwkSKey = 175, // 0x000000AF
    AppKey = 176, // 0x000000B0
    AntennaType = 177, // 0x000000B1
    AntennaCableLength = 178, // 0x000000B2
    Sensor = 179, // 0x000000B3
    JoinEui = 180, // 0x000000B4
    AdaptiveDateRate = 181, // 0x000000B5
    SerialNumberFormat = 182, // 0x000000B6
    SummerTimeCountingStart = 183, // 0x000000B7
    SummerTimeCountingStop = 184, // 0x000000B8
    SummerTimeCountingSuppression = 185, // 0x000000B9
    DinDeviceIdentification = 186, // 0x000000BA
    SerialNumberGenerator = 187, // 0x000000BB
    RatioPulseRotation = 188, // 0x000000BC
    UpLinkStartChannelIndex = 189, // 0x000000BD
    Rx2DownChannelIndex = 190, // 0x000000BE
    TestMethod = 191, // 0x000000BF
    Approval = 192, // 0x000000C0
    SamplingRate = 193, // 0x000000C1
    VolumeResolution = 194, // 0x000000C2
    PermanentFlowRateQ3 = 195, // 0x000000C3
    RadioActiveTime = 196, // 0x000000C4
    MinimumFlowrateQ1 = 197, // 0x000000C5
    WaterMeterAccuracyClass = 198, // 0x000000C6
    PressureTestComponentSapNumber = 199, // 0x000000C7
    MaterialNumberVolumeMeter = 200, // 0x000000C8
    PackingInstruction = 201, // 0x000000C9
    RadioScenario = 202, // 0x000000CA
    RadioTechnologie = 203, // 0x000000CB
    BatteryCapacity = 204, // 0x000000CC
    IncludedBatterys = 205, // 0x000000CD
    TelecomOperator = 206, // 0x000000CE
    RadioProtocolMode = 207, // 0x000000CF
    Channels = 208, // 0x000000D0
    NetType = 209, // 0x000000D1
    GlobalNavigationSatelliteSystem = 210, // 0x000000D2
    PowerType = 211, // 0x000000D3
    SimCard = 213, // 0x000000D5
    PdcType = 214, // 0x000000D6
    VolumeUnitVif = 215, // 0x000000D7
    DN = 216, // 0x000000D8
    PcbaSapNumber = 217, // 0x000000D9
    WmModuleSapNumber = 218, // 0x000000DA
    PartNumberGenerator = 219, // 0x000000DB
    Baudrate = 220, // 0x000000DC
    Parity = 221, // 0x000000DD
    SmartFunctions = 222, // 0x000000DE
    RadioTelegramType = 223, // 0x000000DF
    WaterMeterPulseValue = 224, // 0x000000E0
    WaterMeterTestPulseValue = 225, // 0x000000E1
    Accessories = 226, // 0x000000E2
    IndicatorNumbers = 227, // 0x000000E3
    Released = 228, // 0x000000E4
    ReleasedChangedDate = 229, // 0x000000E5
    ReleasedChanger = 230, // 0x000000E6
    TransducerAmplitudeLimit = 231, // 0x000000E7
    CustomerNumberGenerator = 232, // 0x000000E8
    CustomerNumberFormat = 233, // 0x000000E9
    Code = 234, // 0x000000EA
    BatteryWarningMonths = 235, // 0x000000EB
    Lns = 236, // 0x000000EC
    CellularModem = 237, // 0x000000ED
    LabelType = 238, // 0x000000EE
    PrintedDeviceName = 239, // 0x000000EF
    CounterType = 240, // 0x000000F0
    TransitionalFlowrateQ2 = 241, // 0x000000F1
    PlatformAccessAddress = 242, // 0x000000F2
    NbTestRegister = 243, // 0x000000F3
    DeviceAccessAddress = 244, // 0x000000F4
    DeviceAccessDns = 245, // 0x000000F5
    Environment = 255, // 0x000000FF
    FnnBrandNumber = 256, // 0x00000100
    FnnBrandNumberMessEv = 257, // 0x00000101
    FnnDomesticApproval = 258, // 0x00000102
    FnnMidApproval = 259, // 0x00000103
    ReligionHolidayCounting = 260, // 0x00000104
    FnnNetStaticIP = 261, // 0x00000105
    FnnNetStaticMask = 262, // 0x00000106
    FnnSMGWIP = 263, // 0x00000107
    FnnSMGWPort = 264, // 0x00000108
    FnnCLSCenterIP = 265, // 0x00000109
    FnnCLSCenterPort = 266, // 0x0000010A
    FnnPWRoute = 267, // 0x0000010B
    FnnPWSRV = 268, // 0x0000010C
    SensorType = 269, // 0x0000010D
    Battery = 270, // 0x0000010E
    HazardousMaterialNumber = 271, // 0x0000010F
    DeviceType = 272, // 0x00000110
    SmartFunctionsConfiguration = 273, // 0x00000111
    TypeLabelCartonMultiple = 274, // 0x00000112
    PressureLossClass = 275, // 0x00000113
    SmartFunctionsGroup = 276, // 0x00000114
    ConfigurationKey = 277, // 0x00000115
    CommunicationScenarioConfig = 278, // 0x00000116
  }
}
