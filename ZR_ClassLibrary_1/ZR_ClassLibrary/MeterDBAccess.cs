// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterDBAccess
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace ZR_ClassLibrary
{
  public class MeterDBAccess
  {
    private Logger DatabaseAccessLogger = LogManager.GetLogger(nameof (DatabaseAccessLogger));
    private ArrayList ID_List = new ArrayList();
    private SortedList<string, IdContainer> CachedIds = new SortedList<string, IdContainer>();
    private int CacheSize = 1;
    private string ErrorText;
    public DbBasis myDB;

    public MeterDBAccess(DbConnectionInfo connectionInfo, out DbBasis db)
    {
      this.DatabaseAccessLogger.Info("Loading driver: " + connectionInfo.DbType.ToString());
      db = DbBasis.getDbObject(connectionInfo);
      this.myDB = db;
    }

    public MeterDBAccess(MeterDbTypes databaseTypeConst, string connectionString, out DbBasis db)
    {
      this.DatabaseAccessLogger.Info("Loading driver: " + databaseTypeConst.ToString());
      db = DbBasis.getDbObject(databaseTypeConst, connectionString);
      this.myDB = db;
    }

    public string GetDatabaseInfo(string LineStartText, bool ShowDatabaseConnection)
    {
      return ShowDatabaseConnection ? this.myDB.BaseDbConnection.GetDatabaseFullInfo(LineStartText) : this.myDB.BaseDbConnection.GetDatabaseInfo(LineStartText);
    }

    public string GetDbShortDescription() => this.myDB.BaseDbConnection.GetDatabaseShortInfo();

    public string GetDatabaseInfo(string LineStartText)
    {
      return this.myDB.BaseDbConnection.GetDatabaseInfo(LineStartText);
    }

    public string GetDatabaseLocationName() => this.myDB.BaseDbConnection.DatabaseLocationName;

    public string GetDatabaseType() => this.myDB.BaseDbConnection.ConnectionInfo.DbType.ToString();

    public string getDBTypeString() => this.GetDbShortDescription();

    [Obsolete]
    public int FillTable(string sSQL, DataTable outTab, out string ErrMsg)
    {
      ErrMsg = "unbekannter Fehler";
      int num;
      try
      {
        using (IDbConnection dbConnection = this.myDB.GetDbConnection())
          this.myDB.ZRDataAdapter(sSQL, dbConnection).Fill(outTab);
        ErrMsg = "";
        num = 0;
      }
      catch (Exception ex)
      {
        num = 2;
        ErrMsg = ex.ToString();
      }
      return num;
    }

    public long GetNewId(string TableName, string FieldName)
    {
      long newID;
      string ErrMsg;
      if (this.getNewID(TableName, FieldName, out newID, out ErrMsg) != 0)
        throw new Exception("Error on GetNewId" + ErrMsg);
      return newID;
    }

    public int getNewID(string TableName, string FieldName, out long newID, out string ErrMsg)
    {
      ErrMsg = ErrMsg = "";
      int index = this.CachedIds.IndexOfKey(TableName);
      if (this.CacheSize > 1)
      {
        IdContainer newIds = this.myDB.BaseDbConnection.GetNewIds(TableName, this.CacheSize);
        if (index >= 0)
          this.CachedIds.Values[index] = newIds;
        else
          this.CachedIds.Add(TableName, newIds);
        newID = (long) newIds.GetNextID();
        this.CacheSize = 1;
      }
      else if (index >= 0)
      {
        IdContainer idContainer = this.CachedIds.Values[index];
        newID = (long) idContainer.GetNextID();
        if (idContainer.NumberOfUnusedIds < 1)
          this.CachedIds.RemoveAt(index);
      }
      else
        newID = (long) this.myDB.BaseDbConnection.GetNewIds(TableName, 1).GetNextID();
      return 0;
    }

    public void setIDCachSize(long theNewCachSize) => this.CacheSize = (int) theNewCachSize;

    private void addErrorText(string errtxt)
    {
      if (!(errtxt != ""))
        return;
      this.ErrorText = this.ErrorText + errtxt + "\r\n";
    }

    public string getErrorText()
    {
      string errorText = this.ErrorText;
      this.ErrorText = "";
      return errorText;
    }

    public enum EHKV_II_Ressources
    {
      EHKV_II,
      TwoSensor,
      Opto,
      Radio,
      RemoteSensor,
    }

    public enum OverWritePIDEHKV
    {
      StartTime,
      EndTime,
      StartMonth,
      SendMonth,
      SendWeeks,
      SendDays,
      SendKey,
      Cycle,
      RDay,
      Sensor,
      Scale,
      Radio,
      Opto,
      Mode,
      UTC,
    }

    public enum OverWritePIDFunkmodule
    {
      InputMode = 1,
      Medium = 2,
      PulseValue = 3,
      Unit = 4,
      RFMode = 5,
      RDay = 6,
      UTC = 7,
      ManipulationTimer = 8,
      Sendemonate = 9,
      Sendetage = 10, // 0x0000000A
      Sendestunden = 11, // 0x0000000B
    }

    public enum ValueTypes
    {
      TestError = -2, // 0xFFFFFFFE
      TestValue = -1, // 0xFFFFFFFF
      Unknown = 0,
      EEPDATA = 1,
      MeterID = 2,
      EndEnergy = 3,
      EnergyNominal = 4,
      EnergyUnit = 5,
      PositionVMT = 6,
      PulseValance = 7,
      SensorType = 8,
      StartEnergy = 9,
      TemperatureFlow = 10, // 0x0000000A
      TemperatureReturn = 11, // 0x0000000B
      Pulses = 12, // 0x0000000C
      Repetitions = 13, // 0x0000000D
      CountsMax = 14, // 0x0000000E
      CountsMin = 15, // 0x0000000F
      CountsNominal = 16, // 0x00000010
      CurrentMax = 17, // 0x00000011
      CurrentMin = 18, // 0x00000012
      CurrentNominal = 19, // 0x00000013
      EnergyErrorMax = 20, // 0x00000014
      EnergyErrorMin = 21, // 0x00000015
      EnergyErrorNominal = 22, // 0x00000016
      ErrorMax = 23, // 0x00000017
      ErrorMin = 24, // 0x00000018
      Flow = 25, // 0x00000019
      Measurements = 26, // 0x0000001A
      MeterInfoID = 27, // 0x0000001B
      TemperatureDifferenzErrorMax = 28, // 0x0000001C
      TemperatureDifferenzErrorMin = 29, // 0x0000001D
      TimeMax = 30, // 0x0000001E
      TimeMin = 31, // 0x0000001F
      TimeNominal = 32, // 0x00000020
      ErrorNominal = 33, // 0x00000021
      VoltageMax = 34, // 0x00000022
      VoltageMin = 35, // 0x00000023
      VoltageNominal = 36, // 0x00000024
      VoltageOff = 37, // 0x00000025
      VoltageOn = 38, // 0x00000026
      VoltageSupply = 39, // 0x00000027
      VolumeErrorMax = 40, // 0x00000028
      VolumeErrorMin = 41, // 0x00000029
      VolumeErrorNominal = 42, // 0x0000002A
      TemperatureDifferenzErrorNominal = 43, // 0x0000002B
      PPSArtikelnummer = 44, // 0x0000002C
      ValueFlowMax = 45, // 0x0000002D
      ValueFlowMin = 46, // 0x0000002E
      ValueFlowNominal = 47, // 0x0000002F
      ValueReturnMax = 48, // 0x00000030
      ValueReturnMin = 49, // 0x00000031
      ValueReturnNominal = 50, // 0x00000032
      VoltageOffMax = 51, // 0x00000033
      VoltageOffMin = 52, // 0x00000034
      VoltageOffNominal = 53, // 0x00000035
      VoltageOnMax = 54, // 0x00000036
      VoltageOnMin = 55, // 0x00000037
      VoltageOnNominal = 56, // 0x00000038
      VoltageErrorOff = 57, // 0x00000039
      VoltageErrorOn = 58, // 0x0000003A
      ErrorAtFlow = 59, // 0x0000003B
      ErrorAtReturn = 60, // 0x0000003C
      VoltageError = 61, // 0x0000003D
      CurrentResetMax = 62, // 0x0000003E
      CurrentResetMin = 63, // 0x0000003F
      CurrentResetNominal = 64, // 0x00000040
      ErrorReset = 65, // 0x00000041
      CurrentPowerDownMax = 66, // 0x00000042
      CurrentPowerDownMin = 67, // 0x00000043
      CurrentPowerDownNominal = 68, // 0x00000044
      ErrorPowerDown = 69, // 0x00000045
      CurrentReset = 70, // 0x00000046
      CurrentPowerDown = 72, // 0x00000048
      ValueAtFlow = 73, // 0x00000049
      ValueAtReturn = 74, // 0x0000004A
      Voltage = 75, // 0x0000004B
      Error = 76, // 0x0000004C
      PulseValueDeviationMax = 77, // 0x0000004D
      TVMinRef = 78, // 0x0000004E
      TVMaxRef = 79, // 0x0000004F
      DeltaTMinRef = 80, // 0x00000050
      DeltaTMaxRef = 81, // 0x00000051
      DeltaTVMin = 82, // 0x00000052
      DeltaTVMax = 83, // 0x00000053
      DeltaDTMin = 84, // 0x00000054
      DeltaDTMax = 85, // 0x00000055
      TVRef = 86, // 0x00000056
      DeltaTRef = 87, // 0x00000057
      ButtonOK = 88, // 0x00000058
      EquipmentID = 89, // 0x00000059
      StateAtFlow1 = 90, // 0x0000005A
      StateAtFlow2 = 91, // 0x0000005B
      StateAtReturn1 = 92, // 0x0000005C
      StateAtReturn2 = 93, // 0x0000005D
      FileName1 = 94, // 0x0000005E
      FileName2 = 95, // 0x0000005F
      GovernmentRandomNr = 96, // 0x00000060
      DisplayValueStart = 97, // 0x00000061
      DisplayValueEnd = 98, // 0x00000062
      SnCounter = 99, // 0x00000063
      SnVolumeCounter = 100, // 0x00000064
      SnTempSensorFlow = 101, // 0x00000065
      SnTempSensorReturn = 102, // 0x00000066
      DatePartlist = 103, // 0x00000067
      KFactorFlow = 104, // 0x00000068
      KFactorReturn = 105, // 0x00000069
      DisplayProgressOK = 106, // 0x0000006A
      ApprovalMark = 107, // 0x0000006B
      ProgramOK = 108, // 0x0000006C
      HydQn = 109, // 0x0000006D
      HydClass = 110, // 0x0000006E
      HydImpulsValue = 111, // 0x0000006F
      HydSnVmt = 112, // 0x00000070
      HydQmin = 113, // 0x00000071
      HydQtrenn = 114, // 0x00000072
      HydQmax = 115, // 0x00000073
      HydVsollQmin = 116, // 0x00000074
      HydVsollQtrenn = 117, // 0x00000075
      HydVsollQmax = 118, // 0x00000076
      HydVistQmin = 119, // 0x00000077
      HydVistQtrenn = 120, // 0x00000078
      HydVistQmax = 121, // 0x00000079
      HydFQmin = 122, // 0x0000007A
      HydFQtrenn = 123, // 0x0000007B
      HydFQmax = 124, // 0x0000007C
      HydFGQminMin = 125, // 0x0000007D
      HydFGQtrennMin = 126, // 0x0000007E
      HydFGQmaxMin = 127, // 0x0000007F
      HydFGQminMax = 128, // 0x00000080
      HydFGQtrennMax = 129, // 0x00000081
      HydFGQmaxMax = 130, // 0x00000082
      HydErgQmin = 131, // 0x00000083
      HydErgQtrenn = 132, // 0x00000084
      HydErgQmax = 133, // 0x00000085
      HydFlag = 134, // 0x00000086
      HydTQmin = 135, // 0x00000087
      HydTQtrenn = 136, // 0x00000088
      HydTQmax = 137, // 0x00000089
      HydProdNr = 138, // 0x0000008A
      Prefix = 139, // 0x0000008B
      SnModule = 140, // 0x0000008C
      VoltageOff1 = 141, // 0x0000008D
      VoltageErrorOff1 = 142, // 0x0000008E
      VoltageOn1 = 143, // 0x0000008F
      VoltageErrorOn1 = 144, // 0x00000090
      VoltageOff2 = 145, // 0x00000091
      VoltageErrorOff2 = 146, // 0x00000092
      VoltageOn2 = 147, // 0x00000093
      VoltageErrorOn2 = 148, // 0x00000094
      Input1Ok = 149, // 0x00000095
      Input2Ok = 150, // 0x00000096
      Voltage1 = 151, // 0x00000097
      VoltageError1 = 152, // 0x00000098
      Voltage2 = 153, // 0x00000099
      VoltageError2 = 154, // 0x0000009A
      OldMeterInfoID = 155, // 0x0000009B
      FrequencyMax = 156, // 0x0000009C
      FrequencyMin = 157, // 0x0000009D
      FrequencyNominal = 158, // 0x0000009E
      HubMax = 159, // 0x0000009F
      HubMin = 160, // 0x000000A0
      HubNominal = 161, // 0x000000A1
      OkMeasurementCount = 162, // 0x000000A2
      MisMeasurementCount = 163, // 0x000000A3
      FrequencyMaxDisplay = 164, // 0x000000A4
      FrequencyMinDisplay = 165, // 0x000000A5
      CurrentLimit = 166, // 0x000000A6
      OverVoltageProtection = 167, // 0x000000A7
      ResistorValue = 168, // 0x000000A8
      CurrentOffMax = 169, // 0x000000A9
      CurrentOffMin = 170, // 0x000000AA
      CurrentOffNominal = 171, // 0x000000AB
      CurrentOffValue = 172, // 0x000000AC
      CurrentOffError = 173, // 0x000000AD
      CurrentOnMax = 174, // 0x000000AE
      CurrentOnMin = 175, // 0x000000AF
      CurrentOnNominal = 176, // 0x000000B0
      CurrentOnValue = 177, // 0x000000B1
      CurrentOnError = 178, // 0x000000B2
      WaitTime1 = 179, // 0x000000B3
      WaitTime2 = 180, // 0x000000B4
      WaitTime3 = 181, // 0x000000B5
      WaitTime4 = 182, // 0x000000B6
      TemperatureMax = 183, // 0x000000B7
      TemperatureMin = 184, // 0x000000B8
      TemperatureNominal = 185, // 0x000000B9
      Temperature = 186, // 0x000000BA
      MeterIDInnotas = 187, // 0x000000BB
      Current = 188, // 0x000000BC
      CapHub = 189, // 0x000000BD
      CapHubMin = 190, // 0x000000BE
      CapHubMax = 191, // 0x000000BF
      PulsesMin = 192, // 0x000000C0
      PulsesMax = 193, // 0x000000C1
      WaveFormError = 194, // 0x000000C2
      HardwaretestOK = 195, // 0x000000C3
      MeterIDRead = 196, // 0x000000C4
      SerialNumberRead = 197, // 0x000000C5
      MaxSamples = 198, // 0x000000C6
      MaxSamplesMin = 199, // 0x000000C7
      MaxSamplesMax = 200, // 0x000000C8
      MaxSamplesNom = 201, // 0x000000C9
      FullWaves = 202, // 0x000000CA
      FullWavesMin = 203, // 0x000000CB
      FullWavesMax = 204, // 0x000000CC
      FullWavesNom = 205, // 0x000000CD
      DCT_ValueMin = 206, // 0x000000CE
      DCT_ValueMax = 207, // 0x000000CF
      DCT_ValueNom = 208, // 0x000000D0
      DCT_Value = 209, // 0x000000D1
      HubUnsymetrieMax = 210, // 0x000000D2
      HubUnsymetrieNom = 211, // 0x000000D3
      HubUnsymetrie = 212, // 0x000000D4
      HubUnsymetrieMin = 213, // 0x000000D5
      FileName3 = 214, // 0x000000D6
      FileName4 = 215, // 0x000000D7
      FileName5 = 216, // 0x000000D8
      MeterKey = 217, // 0x000000D9
      HydPruefstand = 218, // 0x000000DA
      HydEinbaurichtung = 219, // 0x000000DB
      HydPruefer = 220, // 0x000000DC
      HydPruefdatum = 221, // 0x000000DD
      FlowMin = 222, // 0x000000DE
      FlowMax = 223, // 0x000000DF
      FlowNom = 224, // 0x000000E0
      MotherMeterValue = 225, // 0x000000E1
      MeasuringMethod = 226, // 0x000000E2
      BalanceValue = 227, // 0x000000E3
      Volume = 228, // 0x000000E4
      NumberOfMeasuringTestPoints = 229, // 0x000000E5
      Factor = 230, // 0x000000E6
      ErrorMID1 = 231, // 0x000000E7
      ErrorMID2 = 232, // 0x000000E8
      ErrorMID3 = 233, // 0x000000E9
      FileName6 = 234, // 0x000000EA
      Density = 235, // 0x000000EB
      TestMethod = 236, // 0x000000EC
      MID = 237, // 0x000000ED
      Ratio = 238, // 0x000000EE
      Class = 239, // 0x000000EF
      ClassToShow = 240, // 0x000000F0
      Approval = 241, // 0x000000F1
      MountingDirection = 242, // 0x000000F2
      Qn = 243, // 0x000000F3
      TempRange = 244, // 0x000000F4
      MeterType = 245, // 0x000000F5
      TestCourseError = 246, // 0x000000F6
      RssiMin = 247, // 0x000000F7
      RssiMax = 248, // 0x000000F8
      RssiNom = 249, // 0x000000F9
      DeltaRssiMax = 250, // 0x000000FA
      ResistorMinValue = 251, // 0x000000FB
      ResistorMaxValue = 252, // 0x000000FC
      ValueFlowMax_1 = 253, // 0x000000FD
      ValueFlowMin_1 = 254, // 0x000000FE
      ValueFlowNominal_1 = 255, // 0x000000FF
      ValueReturnMax_1 = 256, // 0x00000100
      ValueReturnMin_1 = 257, // 0x00000101
      ValueReturnNominal_1 = 258, // 0x00000102
      Uref1Max = 259, // 0x00000103
      Uref1Min = 260, // 0x00000104
      Uref2Max = 261, // 0x00000105
      Uref2Min = 262, // 0x00000106
      DeltaDTMaxInPercent = 263, // 0x00000107
      DeltaRMaxInOhm = 264, // 0x00000108
      SwitchPosition1 = 265, // 0x00000109
      SwitchPosition2 = 266, // 0x0000010A
      SwitchPosition3 = 267, // 0x0000010B
      SwitchPosition = 268, // 0x0000010C
      counterNRef = 269, // 0x0000010D
      counterNRefMin = 270, // 0x0000010E
      counterNRefMax = 271, // 0x0000010F
      counterNRef2 = 272, // 0x00000110
      counterNRef2Min = 273, // 0x00000111
      counterNRef2Max = 274, // 0x00000112
      counterNVL = 275, // 0x00000113
      counterNVLMin = 276, // 0x00000114
      counterNVLMax = 277, // 0x00000115
      counterNRL = 278, // 0x00000116
      counterNRLMin = 279, // 0x00000117
      counterNRLMax = 280, // 0x00000118
      DeltaDT1InPercent = 281, // 0x00000119
      DeltaDT2InPercent = 282, // 0x0000011A
      DeltaDT3InPercent = 283, // 0x0000011B
      DeltaRV1InOhm = 284, // 0x0000011C
      DeltaRV2InOhm = 285, // 0x0000011D
      DeltaRV3InOhm = 286, // 0x0000011E
      tf_exp_1 = 287, // 0x0000011F
      tf_exp_2 = 288, // 0x00000120
      tf_exp_3 = 289, // 0x00000121
      tf_exp_4 = 290, // 0x00000122
      tf_exp_5 = 291, // 0x00000123
      tf_exp_6 = 292, // 0x00000124
      tf_man_1 = 293, // 0x00000125
      tf_man_2 = 294, // 0x00000126
      tf_man_3 = 295, // 0x00000127
      tf_man_4 = 296, // 0x00000128
      tf_man_5 = 297, // 0x00000129
      tf_man_6 = 298, // 0x0000012A
      n_ref_man_2 = 299, // 0x0000012B
      CurrentAfterReset = 300, // 0x0000012C
      CurrentAfterResetMin = 301, // 0x0000012D
      CurrentAfterResetMax = 302, // 0x0000012E
      CurrentWithKey = 303, // 0x0000012F
      CurrentWithKeyMin = 304, // 0x00000130
      CurrentWithKeyMax = 305, // 0x00000131
      CurrentWithKeyAndCancel = 306, // 0x00000132
      CurrentWithKeyAndCancelMin = 307, // 0x00000133
      CurrentWithKeyAndCancelMax = 308, // 0x00000134
      U_VCC_Bat1 = 309, // 0x00000135
      U_VCC_Bat2 = 310, // 0x00000136
      U_VCC_VCCExt = 311, // 0x00000137
      I_Bat2_Bat1 = 312, // 0x00000138
      I_Bat1_Bat2 = 313, // 0x00000139
      I_Bat2_VCCExt = 314, // 0x0000013A
      U_AlpVCC = 315, // 0x0000013B
      Torzeit1Hz = 316, // 0x0000013C
      Torzeit10Hz = 317, // 0x0000013D
      Torzeit100Hz = 318, // 0x0000013E
      Torzeit1000Hz = 319, // 0x0000013F
      Pulse10HzPassivIN1 = 320, // 0x00000140
      Pulse10HzPassivMin = 321, // 0x00000141
      Pulse10HzPassivMax = 322, // 0x00000142
      Flag10HzPassivIN2 = 323, // 0x00000143
      Pulse100HzPassivIN1 = 324, // 0x00000144
      Pulse100HzPassivMax = 325, // 0x00000145
      Flag100HzPassivIN2 = 326, // 0x00000146
      Pulse100HzAktivIN1 = 327, // 0x00000147
      Pulse100HzAktivMin = 328, // 0x00000148
      Pulse100HzAktivMax = 329, // 0x00000149
      Flag100HzAktivIN2 = 330, // 0x0000014A
      Pulse1000HzAktivIN1 = 331, // 0x0000014B
      Pulse1000HzAktivMax = 332, // 0x0000014C
      Flag1000HzAktivIN2 = 333, // 0x0000014D
      Pulse1HzIo1 = 334, // 0x0000014E
      Pulse1HzIo2 = 335, // 0x0000014F
      Pulse1HzMin = 336, // 0x00000150
      Pulse1HzMax = 337, // 0x00000151
      Pulse10HzIo1 = 338, // 0x00000152
      Pulse10HzIo2 = 339, // 0x00000153
      Pulse10HzMax = 340, // 0x00000154
      Pulse1HzIo1Open = 341, // 0x00000155
      Pulse1HzIo2Open = 342, // 0x00000156
      Pulse10HzIo1Open = 343, // 0x00000157
      Pulse10HzIo2Open = 344, // 0x00000158
      RerunReasonHWTWRT = 345, // 0x00000159
      TempFlowNominal = 346, // 0x0000015A
      TempReturnNominal = 347, // 0x0000015B
      ProductionYear = 348, // 0x0000015C
      CurrentStandbyMin = 349, // 0x0000015D
      CurrentStandbyMax = 350, // 0x0000015E
      CurrentInterfaceMin = 351, // 0x0000015F
      CurrentInterfaceMax = 352, // 0x00000160
      Frequency = 353, // 0x00000161
      Torzeit = 354, // 0x00000162
      FrequencyLow = 355, // 0x00000163
      PulsesLowMin = 356, // 0x00000164
      PulsesLowMax = 357, // 0x00000165
      TorzeitLow = 358, // 0x00000166
      FrequencyHigh = 359, // 0x00000167
      PulsesHighMin = 360, // 0x00000168
      PulsesHighMax = 361, // 0x00000169
      TorzeitHigh = 362, // 0x0000016A
      TV2WireErrTest = 363, // 0x0000016B
      DeltaT2WireErrTest = 364, // 0x0000016C
      TV2WireS2Wire = 365, // 0x0000016D
      DeltaT2WireS2Wire = 366, // 0x0000016E
      TV4WireErrTest = 367, // 0x0000016F
      DeltaT4WireErrTest = 368, // 0x00000170
      TV4WireS4Wire = 369, // 0x00000171
      DeltaT4WireS4Wire = 370, // 0x00000172
      TV4WireS2Wire = 371, // 0x00000173
      DeltaT4WireS2Wire = 372, // 0x00000174
      PulsesLow = 373, // 0x00000175
      PulsesHigh = 374, // 0x00000176
      CurrentStandby = 375, // 0x00000177
      CurrentInterface = 376, // 0x00000178
      Zustand1OnOff = 377, // 0x00000179
      Zustand1OffOn = 378, // 0x0000017A
      Zustand2OnOff = 379, // 0x0000017B
      Zustand2OffOn = 380, // 0x0000017C
      BatterieCapacity = 381, // 0x0000017D
      BatterieCapacityMin = 382, // 0x0000017E
      DeltaDTInPercent = 383, // 0x0000017F
      DeltaRVInOhm = 384, // 0x00000180
      TypeEtikettName = 385, // 0x00000181
      KlemmEtikettNameSmall = 386, // 0x00000182
      KlemmEtikettNameNurIO = 387, // 0x00000183
      KlemmEtikettNameMbusUndIO = 388, // 0x00000184
      KlemmEtikettNameMbusUndIO4Draht = 389, // 0x00000185
      KlemmEtikettNameMbusUndIO4DrahtDR = 390, // 0x00000186
      KlemmEtikettNameRS485UndIO = 391, // 0x00000187
      KlemmEtikettNameRS485UndIO4Draht = 392, // 0x00000188
      KlemmEtikettNameRS485UndIO4DrahtDR = 393, // 0x00000189
      KlemmEtikettNameMbusOhneIO = 394, // 0x0000018A
      KlemmEtikettNameRS232UndIO2Draht = 395, // 0x0000018B
      KartonEtikettNameCometGasmonitor = 396, // 0x0000018C
      TypeEtikettNameCometGasmonitor = 397, // 0x0000018D
      KartonEtikettName = 398, // 0x0000018E
      TypeEtikettNameZRWR3IZM = 399, // 0x0000018F
      KartonEtikettNameZRWR3IZM = 400, // 0x00000190
      KlemmenEtikettNameWR3IZM = 401, // 0x00000191
      DisplayValue = 402, // 0x00000192
      DisplayString = 403, // 0x00000193
      ApprovalMarkCooling = 404, // 0x00000194
      TestMethodCooling = 405, // 0x00000195
      KartonEtikettNameNEUTRAL = 406, // 0x00000196
      TypeEtikettNameCO = 407, // 0x00000197
      KartonEtikettNameMMmultidataWR3MID = 408, // 0x00000198
      KartonEtikettNameZelsiusHSI = 409, // 0x00000199
      KlemmenEtikettNameWR3IZM_RS485 = 410, // 0x0000019A
      NumberOfMinPulsesBeforeStart06 = 411, // 0x0000019B
      NumberOfMinPulsesBeforeStart15 = 412, // 0x0000019C
      NumberOfMinPulsesBeforeStart25 = 413, // 0x0000019D
      NumberOfReachedMinPulsesBeforeStart06 = 414, // 0x0000019E
      NumberOfReachedMinPulsesBeforeStart15 = 415, // 0x0000019F
      NumberOfReachedMinPulsesBeforeStart25 = 416, // 0x000001A0
      NumberOfAllowedFewerTheMinPulses = 417, // 0x000001A1
      KlemmenEtikettNameWR3IZM_RS232 = 418, // 0x000001A2
      PreApproval = 419, // 0x000001A3
      NotEnoughWaves = 420, // 0x000001A4
      VolumeMeterType = 421, // 0x000001A5
      ProtocolNr1 = 422, // 0x000001A6
      ProtocolNr2 = 423, // 0x000001A7
      TestsToCheck = 424, // 0x000001A8
      TypeLabelNameC3 = 425, // 0x000001A9
      BathNumberFlow = 426, // 0x000001AA
      BathNumberReturn = 427, // 0x000001AB
      TemperatureBathFlow = 428, // 0x000001AC
      TemperatureBathReturn = 429, // 0x000001AD
      DisplayStringStart = 430, // 0x000001AE
      DisplayStringEnd = 431, // 0x000001AF
      MaxHighMeasurements = 432, // 0x000001B0
      MaxLowMeasurements = 433, // 0x000001B1
      MaxHoldMeasurements = 434, // 0x000001B2
      BleedingCycles = 435, // 0x000001B3
      StartVolume = 436, // 0x000001B4
      EndVolume = 437, // 0x000001B5
      LastDigit = 438, // 0x000001B6
      MinTestVolume = 439, // 0x000001B7
      DeltaTVInKelvin = 440, // 0x000001B8
      DeltaDTInKelvin = 441, // 0x000001B9
      VoltageOffMinLoad = 442, // 0x000001BA
      VoltageOffMaxLoad = 443, // 0x000001BB
      VoltageOnMinLoad = 444, // 0x000001BC
      VoltageOnMaxLoad = 445, // 0x000001BD
      VoltageOffMinNoLoad = 446, // 0x000001BE
      VoltageOffMaxNoLoad = 447, // 0x000001BF
      VoltageOnMinNoLoad = 448, // 0x000001C0
      VoltageOnMaxNoLoad = 449, // 0x000001C1
      RisingTimeMinLoad = 450, // 0x000001C2
      RisingTimeMaxLoad = 451, // 0x000001C3
      RisingTimeMinNoLoad = 452, // 0x000001C4
      RisingTimeMaxNoLoad = 453, // 0x000001C5
      MinLevelInPercent = 454, // 0x000001C6
      MaxLevelInPercent = 455, // 0x000001C7
      GetLevelFromMaximum = 456, // 0x000001C8
      VoltageOn1Load = 457, // 0x000001C9
      VoltageOn2Load = 458, // 0x000001CA
      VoltageOn3Load = 459, // 0x000001CB
      VoltageOff1Load = 460, // 0x000001CC
      VoltageOff2Load = 461, // 0x000001CD
      VoltageOff3Load = 462, // 0x000001CE
      VoltageOn1NoLoad = 463, // 0x000001CF
      VoltageOn2NoLoad = 464, // 0x000001D0
      VoltageOn3NoLoad = 465, // 0x000001D1
      VoltageOff1NoLoad = 466, // 0x000001D2
      VoltageOff2NoLoad = 467, // 0x000001D3
      VoltageOff3NoLoad = 468, // 0x000001D4
      RisingTime1Load = 469, // 0x000001D5
      RisingTime2Load = 470, // 0x000001D6
      RisingTime3Load = 471, // 0x000001D7
      RisingTime1NoLoad = 472, // 0x000001D8
      RisingTime2NoLoad = 473, // 0x000001D9
      RisingTime3NoLoad = 474, // 0x000001DA
      ResultBytesOK1Load = 475, // 0x000001DB
      ResultBytesOK2Load = 476, // 0x000001DC
      ResultBytesOK3Load = 477, // 0x000001DD
      ResultBytesOK1NoLoad = 478, // 0x000001DE
      ResultBytesOK2NoLoad = 479, // 0x000001DF
      ResultBytesOK3NoLoad = 480, // 0x000001E0
      TriggerReceived1Load = 481, // 0x000001E1
      TriggerReceived2Load = 482, // 0x000001E2
      TriggerReceived3Load = 483, // 0x000001E3
      TriggerReceived1NoLoad = 484, // 0x000001E4
      TriggerReceived2NoLoad = 485, // 0x000001E5
      TriggerReceived3NoLoad = 486, // 0x000001E6
      SnSensorPair = 487, // 0x000001E7
      TemperatureFlowRef = 488, // 0x000001E8
      TemperatureReturnRef = 489, // 0x000001E9
      UBatMin = 490, // 0x000001EA
      UBatMax = 491, // 0x000001EB
      UInMin = 492, // 0x000001EC
      UInMax = 493, // 0x000001ED
      UVCCMin = 494, // 0x000001EE
      UVCCMax = 495, // 0x000001EF
      NominalSize = 496, // 0x000001F0
      CurrentMaxMin = 497, // 0x000001F1
      CurrentMaxMax = 498, // 0x000001F2
      UIn = 499, // 0x000001F3
      UVCC = 500, // 0x000001F4
      UBat1 = 501, // 0x000001F5
      UBat2 = 502, // 0x000001F6
      RisingTime = 503, // 0x000001F7
      CommunicationOK = 504, // 0x000001F8
      DeviceVolume = 505, // 0x000001F9
      InletTemp = 506, // 0x000001FA
      OutletTemp = 507, // 0x000001FB
      InletPressure = 508, // 0x000001FC
      OutletPressure = 509, // 0x000001FD
      Lambda = 510, // 0x000001FE
      ExternalTestName = 511, // 0x000001FF
      FirmwareVersion = 512, // 0x00000200
      AmplitudeMin = 513, // 0x00000201
      AmplitudeMax = 514, // 0x00000202
      Amplitude = 515, // 0x00000203
      CountsCoilAAttenuatedMin = 516, // 0x00000204
      CountsCoilAAttenuatedMax = 517, // 0x00000205
      CountsCoilBAttenuatedMin = 518, // 0x00000206
      CountsCoilBAttenuatedMax = 519, // 0x00000207
      CountsCoilANotAttenuatedMin = 520, // 0x00000208
      CountsCoilANotAttenuatedMax = 521, // 0x00000209
      CountsCoilBNotAttenuatedMin = 522, // 0x0000020A
      CountsCoilBNotAttenuatedMax = 523, // 0x0000020B
      CountsCoilAAttenuated = 524, // 0x0000020C
      CountsCoilBAttenuated = 525, // 0x0000020D
      CountsCoilANotAttenuated = 526, // 0x0000020E
      CountsCoilBNotAttenuated = 527, // 0x0000020F
      SubTestID = 528, // 0x00000210
      DeltaAttenuatedMax = 529, // 0x00000211
      DeltaNotAttenuatedMax = 530, // 0x00000212
      DeltaAttenuated = 531, // 0x00000213
      DeltaNotAttenuated = 532, // 0x00000214
      AESKey = 533, // 0x00000215
      RadioID = 534, // 0x00000216
      RSSI = 535, // 0x00000217
      LQI = 536, // 0x00000218
      LQIMin = 537, // 0x00000219
      LQIMax = 538, // 0x0000021A
      RadioState = 539, // 0x0000021B
      Hub_B = 540, // 0x0000021C
      CurrentMaxUs = 541, // 0x0000021D
      CurrentMinUs = 542, // 0x0000021E
      CurrentMaxRa = 543, // 0x0000021F
      CurrentMinRa = 544, // 0x00000220
      FirstPulseWidthMin = 545, // 0x00000221
      FirstPulseWidthMax = 546, // 0x00000222
      FirstPulseWidthDifferenceMax = 547, // 0x00000223
      TimeCounterMin = 548, // 0x00000224
      TimeCounterMax = 549, // 0x00000225
      TimeCounterDifferenceMax = 550, // 0x00000226
      FirstPulseWidthUp = 551, // 0x00000227
      FirstPulseWidthDown = 552, // 0x00000228
      TimeCounterUp = 553, // 0x00000229
      TimeCounterDown = 554, // 0x0000022A
      State = 555, // 0x0000022B
      TimeMinUltrasonic = 556, // 0x0000022C
      TimeMaxUltrasonic = 557, // 0x0000022D
      DeviceType = 558, // 0x0000022E
      FirstPulseWidthMin06 = 559, // 0x0000022F
      FirstPulseWidthMax06 = 560, // 0x00000230
      FirstPulseWidthDifferenceMax06 = 561, // 0x00000231
      FirstPulseWidthMin15 = 562, // 0x00000232
      FirstPulseWidthMax15 = 563, // 0x00000233
      FirstPulseWidthDifferenceMax15 = 564, // 0x00000234
      FirstPulseWidthMin25 = 565, // 0x00000235
      FirstPulseWidthMax25 = 566, // 0x00000236
      FirstPulseWidthDifferenceMax25 = 567, // 0x00000237
      Offset = 568, // 0x00000238
      RisingTimeMin = 569, // 0x00000239
      RisingTimeMax = 570, // 0x0000023A
      ActiveTimeMin = 571, // 0x0000023B
      ActiveTimeMax = 572, // 0x0000023C
      SwitchOffTimeMin = 573, // 0x0000023D
      SwitchOffTimeMax = 574, // 0x0000023E
      ActiveTime = 575, // 0x0000023F
      SwitchOffTime = 576, // 0x00000240
      FlushCycles = 577, // 0x00000241
      UltrasonicAdjustTemperature = 578, // 0x00000242
      UltrasonicAdjustValue = 579, // 0x00000243
      UltrasonicAdjustResult = 580, // 0x00000244
      IntegralMinValue = 581, // 0x00000245
      IntegralMaxValue = 582, // 0x00000246
      Row = 583, // 0x00000247
      Column = 584, // 0x00000248
      RisingTimeSensor1 = 585, // 0x00000249
      RisingTimeSensor2 = 586, // 0x0000024A
      MeterIDSensor1 = 587, // 0x0000024B
      MeterIDSensor2 = 588, // 0x0000024C
      TestIDSensor1 = 589, // 0x0000024D
      TestIDSensor2 = 590, // 0x0000024E
      QminAdjustTemperature = 591, // 0x0000024F
      QminAdjustValue = 592, // 0x00000250
      QminAdjustResult = 593, // 0x00000251
      QminAdjustFlow = 594, // 0x00000252
      CountsCoilAAttenuatedMin_CC = 595, // 0x00000253
      CountsCoilAAttenuatedMax_CC = 596, // 0x00000254
      CountsCoilBAttenuatedMin_CC = 597, // 0x00000255
      CountsCoilBAttenuatedMax_CC = 598, // 0x00000256
      maxThreshold = 599, // 0x00000257
      minThreshold = 600, // 0x00000258
      maxRotationSpeed = 601, // 0x00000259
      destroyRotationSpeed = 602, // 0x0000025A
      rotationSpeed = 603, // 0x0000025B
      ChargeNumber = 604, // 0x0000025C
      SmokeDensity = 605, // 0x0000025D
      BatteryNumber = 606, // 0x0000025E
      PCBNumber = 607, // 0x0000025F
      FrequencyError = 608, // 0x00000260
      FrequencyErrorAfterCorrection = 609, // 0x00000261
      FrequencyErrorMax = 610, // 0x00000262
      SmokeDensityMin = 611, // 0x00000263
      SmokeDensityMax = 612, // 0x00000264
      Wr3WSKartonEtikettName = 613, // 0x00000265
      RadioEpsilonValue = 614, // 0x00000266
      RadioFrequencyAdjustment = 615, // 0x00000267
      PCBDate = 616, // 0x00000268
      CorrectionRequired = 617, // 0x00000269
      VoltageMinRadio = 618, // 0x0000026A
      RepetitionsRadio = 619, // 0x0000026B
      FirstPulseWidthMin35 = 620, // 0x0000026C
      FirstPulseWidthMax35 = 621, // 0x0000026D
      FirstPulseWidthDifferenceMax35 = 622, // 0x0000026E
      FirstPulseWidthMin60 = 623, // 0x0000026F
      FirstPulseWidthMax60 = 624, // 0x00000270
      FirstPulseWidthDifferenceMax60 = 625, // 0x00000271
      FirstPulseWidthMin100 = 626, // 0x00000272
      FirstPulseWidthMax100 = 627, // 0x00000273
      FirstPulseWidthDifferenceMax100 = 628, // 0x00000274
      DeviceHasError = 629, // 0x00000275
      IsProtected = 630, // 0x00000276
      OrderNumber = 631, // 0x00000277
      BaseType = 632, // 0x00000278
      IdIsValid = 633, // 0x00000279
      HardwareNumber = 634, // 0x0000027A
      MeasurementDate = 635, // 0x0000027B
      StatusByte = 636, // 0x0000027C
      TesterSWVersion = 637, // 0x0000027D
      VibrationLevelDamped = 638, // 0x0000027E
      VibrationLevelUndamped = 639, // 0x0000027F
      StationNumber = 640, // 0x00000280
      DAC1_min = 641, // 0x00000281
      DAC1_cal = 642, // 0x00000282
      DAC0_max = 643, // 0x00000283
      DAC0_cal = 644, // 0x00000284
      Position = 645, // 0x00000285
      Pressure = 646, // 0x00000286
      NominalVolume = 647, // 0x00000287
      PulseCheck = 648, // 0x00000288
      QAdjustTemperature = 649, // 0x00000289
      QAdjustValue = 650, // 0x0000028A
      QAdjustFlow = 651, // 0x0000028B
      MaxPressureDrop = 652, // 0x0000028C
      TestBenchEquipmentIDs = 653, // 0x0000028D
      PressureTest = 654, // 0x0000028E
      UsePassedValue = 655, // 0x0000028F
      ScanOK = 656, // 0x00000290
      SnVolumeCounterOK = 657, // 0x00000291
      TestResultFound = 658, // 0x00000292
      EnergyType = 659, // 0x00000293
      TempFlowDevice1 = 660, // 0x00000294
      TempReturnDevice1 = 661, // 0x00000295
      TempFlowDevice2 = 662, // 0x00000296
      TempReturnDevice2 = 663, // 0x00000297
      TempDeviationLowerLimit = 664, // 0x00000298
      TempDeviationUpperLimit = 665, // 0x00000299
      CountsCoilAttenuatedMin_CC_Retrofit = 666, // 0x0000029A
      CountsCoilAttenuatedMax_CC_Retrofit = 667, // 0x0000029B
      CountsCoilAttenuatedMin_Standard_Retrofit = 668, // 0x0000029C
      CountsCoilAttenuatedMax_Standard_Retrofit = 669, // 0x0000029D
      CountsCoilAttenuatedMin_Bulk_Retrofit = 670, // 0x0000029E
      CountsCoilAttenuatedMax_Bulk_Retrofit = 671, // 0x0000029F
      MaxSignalRise_CC_Retrofit = 672, // 0x000002A0
      MinSignalRise_CC_Retrofit = 673, // 0x000002A1
      MaxSignalRise_Standard_Retrofit = 674, // 0x000002A2
      MinSignalRise_Standard_Retrofit = 675, // 0x000002A3
      MaxSignalRise_Bulk_Retrofit = 676, // 0x000002A4
      MinSignalRise_Bulk_Retrofit = 677, // 0x000002A5
      DifferenceSignalDistance_CC_Retrofit = 678, // 0x000002A6
      DifferenceSignalDistance_Bulk_Retrofit = 679, // 0x000002A7
      DifferenceSignalDistance_Standard_Retrofit = 680, // 0x000002A8
      RetrofitCapable = 681, // 0x000002A9
      Time = 682, // 0x000002AA
      Delay = 683, // 0x000002AB
      HumidityMinValue = 684, // 0x000002AC
      HumidityMaxValue = 685, // 0x000002AD
      CoverPrinterPrepareMode = 686, // 0x000002AE
      PulsesLowDown = 687, // 0x000002AF
      PulsesHighUp = 688, // 0x000002B0
      PulsesHighDown = 689, // 0x000002B1
      PulsesLow2 = 690, // 0x000002B2
      PulsesHigh1 = 691, // 0x000002B3
      PulsesHigh2 = 692, // 0x000002B4
      AMax = 693, // 0x000002B5
      AMin = 694, // 0x000002B6
      BMax = 695, // 0x000002B7
      BMin = 696, // 0x000002B8
      CMax = 697, // 0x000002B9
      CMin = 698, // 0x000002BA
      SingleSensor2DLabelData = 699, // 0x000002BB
      SensorPair2DLabelData = 700, // 0x000002BC
      CurrentMinPT100 = 701, // 0x000002BD
      CurrentMaxPT100 = 702, // 0x000002BE
      CurrentMaxMinPT100 = 703, // 0x000002BF
      CurrentMaxMaxPT100 = 704, // 0x000002C0
      CurrentMinPT500 = 705, // 0x000002C1
      CurrentMaxPT500 = 706, // 0x000002C2
      CurrentMaxMinPT500 = 707, // 0x000002C3
      CurrentMaxMaxPT500 = 708, // 0x000002C4
      CurrentMinPT1000 = 709, // 0x000002C5
      CurrentMaxPT1000 = 710, // 0x000002C6
      CurrentMaxMinPT1000 = 711, // 0x000002C7
      CurrentMaxMaxPT1000 = 712, // 0x000002C8
      PulsesLowUp = 713, // 0x000002C9
      DeltaTmin = 714, // 0x000002CA
      DeltaTmax = 715, // 0x000002CB
      FirmwareUpdate = 716, // 0x000002CC
      FlowResistance = 717, // 0x000002CD
      ReturnResistance = 718, // 0x000002CE
      SmokeDensityFullMin = 719, // 0x000002CF
      SmokeDensityFullMax = 720, // 0x000002D0
      MinTestVolumeQmin = 721, // 0x000002D1
      MinTestVolumeQ = 722, // 0x000002D2
      HardwareUniqueID = 723, // 0x000002D3
      DeltaA = 724, // 0x000002D4
      DeltaB = 725, // 0x000002D5
      DeltaAMax = 726, // 0x000002D6
      DeltaAMin = 727, // 0x000002D7
      DeltaBMax = 728, // 0x000002D8
      DeltaBMin = 729, // 0x000002D9
      DeltaAT5subT3Cal = 730, // 0x000002DA
      DeltaBT5subT3Cal = 731, // 0x000002DB
      DeltaCT5subT3Cal = 732, // 0x000002DC
      DeltaAT5subT2 = 733, // 0x000002DD
      DeltaBT5subT2 = 734, // 0x000002DE
      DelatBT2subT1 = 735, // 0x000002DF
      SmokeDensityLoRaMin = 736, // 0x000002E0
      SmokeDensityLoRaMax = 737, // 0x000002E1
      Type = 738, // 0x000002E2
      TransducerPair1CalibrationOffset = 739, // 0x000002E3
      TransducerPair1Offset = 740, // 0x000002E4
      TransducerPair1OffsetCalibrationError = 741, // 0x000002E5
      TransducerPair2CalibrationOffset = 742, // 0x000002E6
      TransducerPair2Offset = 743, // 0x000002E7
      TransducerPair2OffsetCalibrationError = 744, // 0x000002E8
      QmaxAdjustValue = 746, // 0x000002EA
      QmaxAdjustFlow = 748, // 0x000002EC
      TestIdHardwareTest = 749, // 0x000002ED
      TestIdPottingTest = 750, // 0x000002EE
      TestIdHydraulicTest = 751, // 0x000002EF
      FirmwareVersionMinoconnect = 752, // 0x000002F0
      FirmwareVersionMiconConnector = 753, // 0x000002F1
      PermanentFlowRateQ3 = 754, // 0x000002F2
      MinimumFlowrateQ1 = 755, // 0x000002F3
      IUWPackageLabel = 756, // 0x000002F4
      IUWCoverLabel = 757, // 0x000002F5
      Temperature_Start = 758, // 0x000002F6
      Temperature_Stop = 759, // 0x000002F7
      TemperatureSensorCounts = 760, // 0x000002F8
      ResonatorCalibration = 761, // 0x000002F9
      Receiver1Amplitude = 762, // 0x000002FA
      Receiver2Amplitude = 763, // 0x000002FB
      SUS_ResonatorCalibration = 764, // 0x000002FC
      SUS_Receiver1Amplitude = 765, // 0x000002FD
      SUS_Receiver2Amplitude = 766, // 0x000002FE
      ClockCalibrationError = 767, // 0x000002FF
      ClockCalibrationChecked = 768, // 0x00000300
      RSSI_DeviceReceive = 769, // 0x00000301
      RSSI_DeviceReceiveMin = 770, // 0x00000302
      RSSI_DeviceReceiveMax = 771, // 0x00000303
      RSSI_DeviceTransmit = 772, // 0x00000304
      RSSI_DeviceTransmitMin = 773, // 0x00000305
      RSSI_DeviceTransmitMax = 774, // 0x00000306
      CurrentMaxLoRa = 775, // 0x00000307
      MinPressure = 776, // 0x00000308
      MaxPressure = 777, // 0x00000309
      PressureDrop = 778, // 0x0000030A
      AdjustmentType = 779, // 0x0000030B
      ErrorLimitType = 780, // 0x0000030C
      DiffCounts1 = 781, // 0x0000030D
      DiffCounts2 = 782, // 0x0000030E
      CyclesErrors1 = 783, // 0x0000030F
      CyclesErrors2 = 784, // 0x00000310
      MaximalAdjustment = 785, // 0x00000311
      ZeroFlowAdjustValueMin = 786, // 0x00000312
      ZeroFlowAdjustValueMax = 787, // 0x00000313
      SusUltrasonicUpTime = 788, // 0x00000314
      SusUltrasonicDownTime = 789, // 0x00000315
      SusUltrasonicTimeDiff = 790, // 0x00000316
      UltrasonicUpTime = 791, // 0x00000317
      UltrasonicDownTime = 792, // 0x00000318
      UltrasonicTimeDiff = 793, // 0x00000319
      Amplitude06Min = 794, // 0x0000031A
      Amplitude06Max = 795, // 0x0000031B
      Amplitude15Min = 796, // 0x0000031C
      Amplitude15Max = 797, // 0x0000031D
      Amplitude25Min = 798, // 0x0000031E
      Amplitude25Max = 799, // 0x0000031F
      Amplitude35Min = 800, // 0x00000320
      Amplitude35Max = 801, // 0x00000321
      Amplitude60Min = 802, // 0x00000322
      Amplitude60Max = 803, // 0x00000323
      Amplitude100Min = 804, // 0x00000324
      Amplitude100Max = 805, // 0x00000325
      MeterIDBody = 806, // 0x00000326
      TemperatureChangeMax = 807, // 0x00000327
      TestVoltage06 = 808, // 0x00000328
      TestVoltage15 = 809, // 0x00000329
      TestVoltage25 = 810, // 0x0000032A
      TestVoltage35 = 811, // 0x0000032B
      TestVoltage60 = 812, // 0x0000032C
      TestVoltage100 = 813, // 0x0000032D
      HasLeak = 814, // 0x0000032E
      ZeroFlowAdjustValueMinSmall = 815, // 0x0000032F
      ZeroFlowAdjustValueMaxSmall = 816, // 0x00000330
      ZeroFlowAdjustValueMinBig = 817, // 0x00000331
      ZeroFlowAdjustValueMaxBig = 818, // 0x00000332
      DN = 819, // 0x00000333
      PressureTestComponentSAPNr = 820, // 0x00000334
      VoltageLimitUsed = 821, // 0x00000335
      RequiredReceiveCount = 822, // 0x00000336
      ApprovalEndDate = 823, // 0x00000337
      TestIdLCDTest = 833, // 0x00000341
      DeltaRssiMaxTransmit = 834, // 0x00000342
      DeltaRssiMaxReceive = 835, // 0x00000343
      RadioTransmit_SendCount = 836, // 0x00000344
      RadioTransmit_ReceiveCount = 837, // 0x00000345
      RadioTransmit_TestCount = 838, // 0x00000346
      RadioReceive_SendCount = 839, // 0x00000347
      RadioReceive_ReceiveCount = 840, // 0x00000348
      RadioReceive_TestCount = 841, // 0x00000349
      FNN_MARKENNUMMERMESSEV = 842, // 0x0000034A
      FNN_MARKENNUMMER = 843, // 0x0000034B
      ApprovalRevision = 844, // 0x0000034C
      ReCertification = 845, // 0x0000034D
      CustomerSerialNumber = 846, // 0x0000034E
      MinScore = 847, // 0x0000034F
      MaxScore = 848, // 0x00000350
      Score0 = 849, // 0x00000351
      Score1 = 850, // 0x00000352
      Score2 = 851, // 0x00000353
      Score3 = 852, // 0x00000354
      Score4 = 853, // 0x00000355
      Score5 = 854, // 0x00000356
      Score6 = 855, // 0x00000357
      MeterInstallation = 1000, // 0x000003E8
      MeterRemove = 1001, // 0x000003E9
      MeterReadingError = 1002, // 0x000003EA
      HeatMeterVolumeInput = 1010, // 0x000003F2
      VolumeCounter1Input = 1011, // 0x000003F3
      VolumeCounter2Input = 1012, // 0x000003F4
      InputLineBreak = 1013, // 0x000003F5
      InputLineShortCircuit = 1014, // 0x000003F6
      HeatMeterEnergy = 1100, // 0x0000044C
      HeatMeterVolume = 1101, // 0x0000044D
      HeatMeterFlow = 1102, // 0x0000044E
      HeatMeterUpperTemperature = 1103, // 0x0000044F
      HeatMeterLowerTemperature = 1104, // 0x00000450
      HeatMeterTemperatureDifference = 1105, // 0x00000451
      HeatMeterPower = 1106, // 0x00000452
      HeatMeterNegativeEnergy = 1107, // 0x00000453
      Input_1_frequencyToHigh = 1108, // 0x00000454
      Input_2_frequencyToHigh = 1109, // 0x00000455
      Input_3_frequencyToHigh = 1110, // 0x00000456
      SelfTestError = 1111, // 0x00000457
      ReadingDayDataError = 1112, // 0x00000458
      InternalLoopMemoryError = 1113, // 0x00000459
      ExternalLoopMemoryError = 1115, // 0x0000045B
      ExternalEEPHardwareError = 1116, // 0x0000045C
      InternalEEPHardwareError = 1117, // 0x0000045D
      ResetError = 1118, // 0x0000045E
      ForwardSensorTooSmall = 1120, // 0x00000460
      ForwardSensorTooLarge = 1121, // 0x00000461
      ReturnSensorTooSmall = 1122, // 0x00000462
      ReturnSensorTooLarge = 1123, // 0x00000463
      FlowReturnSensorWrongWay = 1124, // 0x00000464
      BatteryUndervoltage = 1125, // 0x00000465
      ShortCircuitReturnSensor = 1126, // 0x00000466
      BrokenReturnSensor = 1127, // 0x00000467
      ShortCircuitFlowSensor = 1128, // 0x00000468
      BrokenFlowSensor = 1129, // 0x00000469
      OtherTempMessuringError = 1130, // 0x0000046A
      ElectricityEnergy = 1131, // 0x0000046B
      OperationHours = 1132, // 0x0000046C
      ElectricityPower = 1133, // 0x0000046D
      ElectricityMeterError = 1134, // 0x0000046E
      MeterDeinstallation = 1135, // 0x0000046F
      TimePoint = 1136, // 0x00000470
      HeatMeterTarifEnergy0 = 1137, // 0x00000471
      HeatMeterTarifEnergy1 = 1138, // 0x00000472
      DeviceDateTime = 1139, // 0x00000473
      DeviceNextEventDateTime = 1140, // 0x00000474
      FlowThresholdMin = 1200, // 0x000004B0
      FlowThresholdMax = 1201, // 0x000004B1
      dT_ThresholdMin = 1202, // 0x000004B2
      dT_ThresholdMax = 1203, // 0x000004B3
      HeatMeterUpper_MinThreshold = 1204, // 0x000004B4
      HeatMeterUpper_MaxThreshold = 1205, // 0x000004B5
      HeatMeterLower_MinThreshold = 1206, // 0x000004B6
      HeatMeterLower_MaxThreshold = 1207, // 0x000004B7
      WaterMeterVolume = 1208, // 0x000004B8
      WaterMeterVolumeWarmWater = 1209, // 0x000004B9
      WaterMeterVolumeColdWater = 1210, // 0x000004BA
      WaterMeterVolumeHotWater = 1211, // 0x000004BB
      HeatMeterVolumeHeatingSystem = 1212, // 0x000004BC
      HeatMeterVolumeWarmWater = 1213, // 0x000004BD
      WaterMeterVolumeEnergyRated = 1214, // 0x000004BE
      HeatMeterEnergyHeatingSystem = 1215, // 0x000004BF
      HeatMeterEnergyWarmWater = 1216, // 0x000004C0
      WaterMeterEnergyRated = 1217, // 0x000004C1
      WaterMeterVolumeAvgFlow = 1218, // 0x000004C2
      WaterMeterVolumeWarmWaterAvgFlow = 1219, // 0x000004C3
      WaterMeterVolumeColdWaterAvgFlow = 1220, // 0x000004C4
      WaterMeterVolumeHotWaterAvgFlow = 1221, // 0x000004C5
      HeatMeterVolumeAvgFlow = 1222, // 0x000004C6
      HeatMeterVolumeHeatingSystemAvgFlow = 1223, // 0x000004C7
      HeatMeterVolumeWarmWaterAvgFlow = 1224, // 0x000004C8
      WaterMeterVolumeEnergyRatedAvgFlow = 1225, // 0x000004C9
      HeatMeterPowerAvgFlow = 1226, // 0x000004CA
      HeatMeterPowerHeatingSystemAvgFlow = 1227, // 0x000004CB
      HeatMeterPowerWarmWaterAvgFlow = 1228, // 0x000004CC
      WaterMeterPowerEnergyRatedAvgFlow = 1229, // 0x000004CD
      GasMeterVolume = 1230, // 0x000004CE
      HeatMeterCurrentFlow = 1231, // 0x000004CF
      HeatMeterCurrentPower = 1232, // 0x000004D0
      HeatMeterMaxFlow = 1233, // 0x000004D1
      HeatMeterMaxPower = 1234, // 0x000004D2
      ElectricityEnergyDueDate = 1235, // 0x000004D3
      HeatMeterEnergyHeatingSystemDueDate = 1236, // 0x000004D4
      HeatMeterEnergyWarmWaterDueDate = 1237, // 0x000004D5
      WaterMeterEnergyRatedDueDate = 1238, // 0x000004D6
      GasMeterVolumeDueDate = 1239, // 0x000004D7
      WaterMeterVolumeHotWaterDueDate = 1240, // 0x000004D8
      WaterMeterVolumeColdWaterDueDate = 1241, // 0x000004D9
      HeatMeterVolumeHeatingSystemDueDate = 1242, // 0x000004DA
      HeatMeterVolumeWarmWaterDueDate = 1243, // 0x000004DB
      WaterMeterVolumeWarmWaterDueDate = 1244, // 0x000004DC
      WaterMeterVolumeDueDate = 1245, // 0x000004DD
      WaterMeterVolumeEnergyRatedDueDate = 1246, // 0x000004DE
      ElectricityEnergyLogger = 1247, // 0x000004DF
      HeatMeterEnergyHeatingSystemLogger = 1248, // 0x000004E0
      HeatMeterEnergyWarmWaterLogger = 1249, // 0x000004E1
      WaterMeterEnergyRatedLogger = 1250, // 0x000004E2
      GasMeterVolumeLogger = 1251, // 0x000004E3
      WaterMeterVolumeHotWaterLogger = 1252, // 0x000004E4
      WaterMeterVolumeColdWaterLogger = 1253, // 0x000004E5
      HeatMeterVolumeHeatingSystemLogger = 1254, // 0x000004E6
      HeatMeterVolumeWarmWaterLogger = 1255, // 0x000004E7
      WaterMeterVolumeWarmWaterLogger = 1256, // 0x000004E8
      WaterMeterVolumeLogger = 1257, // 0x000004E9
      WaterMeterVolumeEnergyRatedLogger = 1258, // 0x000004EA
      HeatMeterVolumeDueDate = 1259, // 0x000004EB
      HeatMeterVolumeLogger = 1260, // 0x000004EC
      HeatMeterEnergyDueDate = 1261, // 0x000004ED
      HeatMeterEnergyLogger = 1262, // 0x000004EE
      HeatMeterMaxPowerLogger = 1263, // 0x000004EF
      HeatMeterMaxPowerHeatingSystemLogger = 1264, // 0x000004F0
      HeatMeterMaxPowerEnergyRatedLogger = 1265, // 0x000004F1
      HeatMeterNegativeEnergyHeatingSystem = 1266, // 0x000004F2
      HeatMeterNegativeEnergyWarmWater = 1267, // 0x000004F3
      HeatMeterNegativeEnergyLogger = 1268, // 0x000004F4
      HeatMeterNegativeEnergyHeatingSystemLogger = 1269, // 0x000004F5
      HeatMeterNegativeEnergyWarmWaterLogger = 1270, // 0x000004F6
      HeatMeterNegativeEnergyDueDate = 1271, // 0x000004F7
      HeatMeterNegativeEnergyHeatingSystemDueDate = 1272, // 0x000004F8
      HeatMeterNegativeEnergyWarmWaterDueDate = 1273, // 0x000004F9
      SmokeDetectorA = 1274, // 0x000004FA
      SmokeDetectorB = 1275, // 0x000004FB
      SmokeDetectorC = 1276, // 0x000004FC
      NFCEnergyMin_NoLoad = 1277, // 0x000004FD
      NFCEnergyMax_NoLoad = 1278, // 0x000004FE
      NFCEnergyMin_WithLoad = 1279, // 0x000004FF
      NFCEnergyMax_WithLoad = 1280, // 0x00000500
      NFCEnergy_NoLoad = 1281, // 0x00000501
      NFCEnergy_WithLoad = 1282, // 0x00000502
      MicroLVoltageMaxA = 1283, // 0x00000503
      MicroLVoltageMinA = 1284, // 0x00000504
      MicroLVoltageMaxB = 1285, // 0x00000505
      MicroLVoltageMinB = 1286, // 0x00000506
      T2ReadBValue = 1287, // 0x00000507
      BValueStatus = 1288, // 0x00000508
      T2ReadAValue = 1289, // 0x00000509
      SDT5ReadAValue = 1290, // 0x0000050A
      SDT5ReadBValue = 1291, // 0x0000050B
      SDT5ReadCValue = 1292, // 0x0000050C
      APPKey = 1293, // 0x0000050D
      EDCLVoltageMaxA = 1294, // 0x0000050E
      EDCLVoltageMinA = 1295, // 0x0000050F
      EDCLVoltageMaxB = 1296, // 0x00000510
      EDCLVoltageMinB = 1297, // 0x00000511
      ResonatorCalibrationMin = 1298, // 0x00000512
      ResonatorCalibrationMax = 1299, // 0x00000513
      ReceiverAmplitudeMin = 1300, // 0x00000514
      ReceiverAmplitudeMax = 1301, // 0x00000515
      UltrasonicTimeMin = 1302, // 0x00000516
      UltrasonicTimeMax = 1303, // 0x00000517
      UltrasonicTimeDiffMin = 1304, // 0x00000518
      UltrasonicTimeDiffMax = 1305, // 0x00000519
      ResonatorCalibrationPair1 = 1306, // 0x0000051A
      Receiver1AmplitudePair1 = 1307, // 0x0000051B
      Receiver2AmplitudePair1 = 1308, // 0x0000051C
      UltrasonicUpTimePair1 = 1309, // 0x0000051D
      UltrasonicDownTimePair1 = 1310, // 0x0000051E
      UltrasonicTimeDiffPair1 = 1311, // 0x0000051F
      ResonatorCalibrationPair2 = 1312, // 0x00000520
      Receiver1AmplitudePair2 = 1313, // 0x00000521
      Receiver2AmplitudePair2 = 1314, // 0x00000522
      UltrasonicUpTimePair2 = 1315, // 0x00000523
      UltrasonicDownTimePair2 = 1316, // 0x00000524
      UltrasonicTimeDiffPair2 = 1317, // 0x00000525
      CurrentMaxA_CELL = 1318, // 0x00000526
      CurrentMinA_CELL = 1319, // 0x00000527
      CurrentMaxC_CELL = 1320, // 0x00000528
      CurrentMinC_CELL = 1321, // 0x00000529
      JoinEui = 1322, // 0x0000052A
      Text1 = 1323, // 0x0000052B
      Text2 = 1324, // 0x0000052C
      Text3 = 1325, // 0x0000052D
      Text4 = 1326, // 0x0000052E
      Text5 = 1327, // 0x0000052F
      MWMCustomerSN = 1328, // 0x00000530
      HyduploadNominalflow = 1329, // 0x00000531
      HyduploadNominal = 1330, // 0x00000532
      HydraulicTestbenchName = 1331, // 0x00000533
      HydraulicTester = 1332, // 0x00000534
      VolumnMeterSAP = 1333, // 0x00000535
      ActualMarginValue = 1334, // 0x00000536
      SDBasicReadA1Value = 1335, // 0x00000537
      SDBasicReadB1Value = 1336, // 0x00000538
      NBIoT_IMEI = 1337, // 0x00000539
      SIM_IMSI = 1338, // 0x0000053A
      VoltagePOEMin = 1339, // 0x0000053B
      VoltagePOEMax = 1340, // 0x0000053C
      Voltage12VMin = 1341, // 0x0000053D
      Voltage12VMax = 1342, // 0x0000053E
      CurrentPOEMinA = 1343, // 0x0000053F
      CurrentPOEMinB = 1344, // 0x00000540
      CurrentPOEMinC = 1345, // 0x00000541
      CurrentPOEMinD = 1346, // 0x00000542
      CurrentPOEMinE = 1347, // 0x00000543
      CurrentPOEMaxA = 1348, // 0x00000544
      CurrentPOEMaxB = 1349, // 0x00000545
      CurrentPOEMaxC = 1350, // 0x00000546
      CurrentPOEMaxD = 1351, // 0x00000547
      CurrentPOEMaxE = 1352, // 0x00000548
      Current12VMinA = 1353, // 0x00000549
      Current12VMinB = 1354, // 0x0000054A
      Current12VMinC = 1355, // 0x0000054B
      Current12VMinD = 1356, // 0x0000054C
      Current12VMinE = 1357, // 0x0000054D
      Current12VMaxA = 1358, // 0x0000054E
      Current12VMaxB = 1359, // 0x0000054F
      Current12VMaxC = 1360, // 0x00000550
      Current12VMaxD = 1361, // 0x00000551
      Current12VMaxE = 1362, // 0x00000552
      CurrentA = 1363, // 0x00000553
      CurrentB = 1364, // 0x00000554
      CurrentC = 1365, // 0x00000555
      CurrentD = 1366, // 0x00000556
      CurrentE = 1367, // 0x00000557
      VoltagePOEMinA = 1368, // 0x00000558
      VoltagePOEMinB = 1369, // 0x00000559
      VoltagePOEMinC = 1370, // 0x0000055A
      VoltagePOEMinD = 1371, // 0x0000055B
      VoltagePOEMinE = 1372, // 0x0000055C
      VoltagePOEMinF = 1373, // 0x0000055D
      VoltagePOEMaxA = 1374, // 0x0000055E
      VoltagePOEMaxB = 1375, // 0x0000055F
      VoltagePOEMaxC = 1376, // 0x00000560
      VoltagePOEMaxD = 1377, // 0x00000561
      VoltagePOEMaxE = 1378, // 0x00000562
      VoltagePOEMaxF = 1379, // 0x00000563
      Voltage12VMinA = 1380, // 0x00000564
      Voltage12VMinB = 1381, // 0x00000565
      Voltage12VMinC = 1382, // 0x00000566
      Voltage12VMinD = 1383, // 0x00000567
      Voltage12VMinE = 1384, // 0x00000568
      Voltage12VMinF = 1385, // 0x00000569
      Voltage12VMaxA = 1386, // 0x0000056A
      Voltage12VMaxB = 1387, // 0x0000056B
      Voltage12VMaxC = 1388, // 0x0000056C
      Voltage12VMaxD = 1389, // 0x0000056D
      Voltage12VMaxE = 1390, // 0x0000056E
      Voltage12VMaxF = 1391, // 0x0000056F
      VoltageA = 1392, // 0x00000570
      VoltageB = 1393, // 0x00000571
      VoltageC = 1394, // 0x00000572
      VoltageD = 1395, // 0x00000573
      VoltageE = 1396, // 0x00000574
      VoltageF = 1397, // 0x00000575
      LTESerialNr = 1398, // 0x00000576
      SignalStrengthMin = 1399, // 0x00000577
      SignalStrengthMax = 1400, // 0x00000578
      SignalErrorMin = 1401, // 0x00000579
      SignalErrorMax = 1402, // 0x0000057A
      SignalStrength = 1403, // 0x0000057B
      SignalError = 1404, // 0x0000057C
      MWMInternalSN = 1405, // 0x0000057D
      LteModemModel = 1406, // 0x0000057E
      GpioExpander = 1407, // 0x0000057F
      loraGatewayMac = 1408, // 0x00000580
      LoragatewayCCID = 1409, // 0x00000581
      LoragatewayMACID = 1410, // 0x00000582
      LoragatewayMACIDEnd = 1411, // 0x00000583
      MWMTestPointName = 1412, // 0x00000584
      MWMTestTime = 1413, // 0x00000585
      MWMTestTemperature = 1414, // 0x00000586
      MWMHalfFaultsCheck = 1415, // 0x00000587
      MWMDVGWCheck = 1416, // 0x00000588
      MWMImpulse = 1417, // 0x00000589
      MWMPulse = 1418, // 0x0000058A
      MWMTemperatureIn = 1419, // 0x0000058B
      MWMTemperatureOut = 1420, // 0x0000058C
      MWMTemperatureDifference = 1421, // 0x0000058D
      MWMWaterDensityStd = 1422, // 0x0000058E
      MWMWaterDensityCorrection = 1423, // 0x0000058F
      MWMWeight = 1424, // 0x00000590
      MWMBatchNo = 1425, // 0x00000591
      LPTIMState = 1426, // 0x00000592
      SIM_ICCID = 1427, // 0x00000593
      MesEONumber = 1428, // 0x00000594
      TemplateName = 1429, // 0x00000595
      TheFirstSerialNumber = 1430, // 0x00000596
      Totals = 1431, // 0x00000597
      UseActualValue = 1432, // 0x00000598
      TemplateVar1 = 1433, // 0x00000599
      TemplateVar2 = 1434, // 0x0000059A
      TemplateVar3 = 1435, // 0x0000059B
      TemplateVar4 = 1436, // 0x0000059C
      TemplateVar5 = 1437, // 0x0000059D
      TemplateVar6 = 1438, // 0x0000059E
      TemplateVar7 = 1439, // 0x0000059F
      TemplateVar8 = 1440, // 0x000005A0
      TemplateVar9 = 1441, // 0x000005A1
      TemplateVar10 = 1442, // 0x000005A2
      TemplateVar11 = 1443, // 0x000005A3
      TemplateVar12 = 1444, // 0x000005A4
      TemplateVar13 = 1445, // 0x000005A5
      TemplateVar14 = 1446, // 0x000005A6
      TemplateVar15 = 1447, // 0x000005A7
      TemplateVar16 = 1448, // 0x000005A8
      ETRUSerialNumber = 1449, // 0x000005A9
      FirmwareTimestamp = 1450, // 0x000005AA
      PasswordRoot = 1451, // 0x000005AB
      PasswordHTTPD = 1452, // 0x000005AC
      PasswordConfigstation = 1453, // 0x000005AD
      Certificate = 1454, // 0x000005AE
      NET_STATIC_IP = 1455, // 0x000005AF
      NET_STATIC_MASK = 1456, // 0x000005B0
      SMGW_IP = 1457, // 0x000005B1
      SMGW_PORT = 1458, // 0x000005B2
      CLSCENTER_IP = 1459, // 0x000005B3
      CLSCENTER_PORT = 1460, // 0x000005B4
      CLSKey = 1461, // 0x000005B5
      MWMTQ4 = 1462, // 0x000005B6
      HardwareVersion = 1463, // 0x000005B7
      Branding = 1464, // 0x000005B8
      MWMTQ3 = 1465, // 0x000005B9
      DevEUI = 1466, // 0x000005BA
      OEM_UserID = 20000, // 0x00004E20
      OEM_EquipmentID = 20001, // 0x00004E21
      OEM_PrintedSerialNumber = 20002, // 0x00004E22
      OEM_ZennerMaterialNumber = 20003, // 0x00004E23
      OEM_ZennerShortDescription = 20004, // 0x00004E24
      OEM_RadioTechnology = 20005, // 0x00004E25
      OEM_RadioProtocolMode = 20006, // 0x00004E26
      OEM_FirmwareVersion = 20007, // 0x00004E27
      OEM_LoRaWanVersion = 20008, // 0x00004E28
      OEM_JoinEUI = 20009, // 0x00004E29
      OEM_DevEUI = 20010, // 0x00004E2A
      OEM_AppKey = 20011, // 0x00004E2B
      OEM_DeviceActivationMode = 20012, // 0x00004E2C
      OEM_Frequency = 20013, // 0x00004E2D
      OEM_SubDeviceCount = 20014, // 0x00004E2E
      OEM_OrderNr = 20015, // 0x00004E2F
      OEM_MeterSN = 20016, // 0x00004E30
      OEM_IMEI = 20017, // 0x00004E31
      OEM_IMSI = 20018, // 0x00004E32
      OEM_ICCID = 20019, // 0x00004E33
      OEM_MaxMeterValue = 20020, // 0x00004E34
      OEM_HardwareVersion = 20021, // 0x00004E35
      OEM_RecordUploadTime = 20022, // 0x00004E36
      OEM_DeviceAccessAddress = 20023, // 0x00004E37
      OEM_BeginTransmittingTime = 20024, // 0x00004E38
      OEM_EndTransmittingTime = 20025, // 0x00004E39
      OEM_RSSI = 20026, // 0x00004E3A
      OEM_SNR = 20027, // 0x00004E3B
      OEM_MeterValue = 20028, // 0x00004E3C
      OEM_ValveStatus = 20029, // 0x00004E3D
      OEM_PulseValue = 20030, // 0x00004E3E
      OEM_SampleRate = 20031, // 0x00004E3F
      OEM_CalibrationParameter = 20032, // 0x00004E40
      OEM_BaudRate = 20033, // 0x00004E41
      OEM_CheckBit = 20034, // 0x00004E42
      OEM_CommunicationMode = 20035, // 0x00004E43
      OEM_PulseOutputMode = 20036, // 0x00004E44
      OEM_StorageMode = 20037, // 0x00004E45
      OEM_OutputPulseValue = 20038, // 0x00004E46
      OEM_UniqueID = 20039, // 0x00004E47
      MeterBackupBase = 50000, // 0x0000C350
      MeterBackupC5 = 50005, // 0x0000C355
      MeterBackupIUW = 50006, // 0x0000C356
      MeterBackupMax = 54095, // 0x0000D34F
      END = 100000, // 0x000186A0
    }

    public enum EquipmentIdentifier
    {
      Unknown,
      Card1,
      Card2,
      Channel1,
      Channel2,
      Channel3,
      Channel4,
      ReferenceSensor,
      ReferenceSensorChannel,
      RoomTemperatureSensor,
      RoomTemperatureSensorChannel,
      Bath1,
      Bath2,
      Bath3,
      Bath4,
      Bath5,
      Bath6,
      SensorBath1,
      SensorBath2,
      SensorBath3,
      SensorBath4,
      SensorBath5,
      SensorBath6,
      ChannelBath1,
      ChannelBath2,
      ChannelBath3,
      ChannelBath4,
      ChannelBath5,
      ChannelBath6,
      ResistanceEtalon,
    }

    public enum ReadingTypes
    {
      None = 0,
      MBus = 1,
      ZRMBusSerieI = 2,
      ZRMBusSerieII = 3,
      MBusCom = 4,
      RadioReceiver = 5,
      RadioModule = 6,
      ImpulsMeter = 7,
      multidata = 8,
      IZMxxx = 9,
      zelsius = 10, // 0x0000000A
      RDM8xxT = 11, // 0x0000000B
      RDM8xxR = 12, // 0x0000000C
      ZDFFile = 13, // 0x0000000D
      WaveFlowRadio = 14, // 0x0000000E
      WaveFlowDevice = 15, // 0x0000000F
      MBusPtPCom = 17, // 0x00000011
      EnumEND = 18, // 0x00000012
    }

    public enum SQLError
    {
      DuplicateEntry = -2147467259, // 0x80004005
      ConnectionError = -3, // 0xFFFFFFFD
      Unknown = -1, // 0xFFFFFFFF
      noError = 0,
      ConstraintError = 23505, // 0x00005BD1
    }
  }
}
