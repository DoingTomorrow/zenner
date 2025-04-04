// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.DeviceInfoRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class DeviceInfoRecord
  {
    public string Project_ID { get; set; }

    public string MTU_ID { get; set; }

    public DateTime? Valid_From { get; set; }

    public DateTime? Valid_To { get; set; }

    public string Meter_ID { get; set; }

    public string UnitNbr { get; set; }

    public string KeyRoom { get; set; }

    public string Location { get; set; }

    public string InstallPosition { get; set; }

    public string InstallPoint { get; set; }

    public string MeaPoint_Minol { get; set; }

    public string Channel { get; set; }

    public string ImpulseValue { get; set; }

    public string EstimateCode { get; set; }

    public double EstimateValue { get; set; }

    public bool CartridgeHeater { get; set; }

    public float Factor { get; set; }

    public float Factor2 { get; set; }

    public string DeviceState { get; set; }

    public DateTime? InstallDate { get; set; }

    public string DeviceID_Rep { get; set; }

    public double FinalScore_Rep { get; set; }

    public string Contract_No { get; set; }

    public int Contract_Position { get; set; }

    public string Rent_Contract { get; set; }

    public int Rent_Contract_Pos { get; set; }

    public char Type_Host { get; set; }

    public string User1 { get; set; }

    public string User2 { get; set; }

    public string User3 { get; set; }

    public string User4 { get; set; }

    public string User5 { get; set; }

    public double RadioData { get; set; }

    public int UnityImpulse { get; set; }

    public int ImpulseNum { get; set; }

    public double StartImpulse { get; set; }

    public bool ModuleUsed { get; set; }

    public string Module_ID { get; set; }

    public bool IsHeatMeter { get; set; }

    public string RegMode1 { get; set; }

    public string RegMode2 { get; set; }

    public string RegMode3 { get; set; }

    public int DgReg1 { get; set; }

    public int DgReg2 { get; set; }

    public int DgReg3 { get; set; }

    public int RegSignal1 { get; set; }

    public int RegSignal2 { get; set; }

    public int RegSignal3 { get; set; }

    public string DCU_ID1 { get; set; }

    public string DCU_ID2 { get; set; }

    public string DCU_ID3 { get; set; }

    public string DiagnosticMsg { get; set; }

    public int ConfigFlag { get; set; }

    public int DgScenario { get; set; }

    public string DgMeasureArea { get; set; }

    public string Full_DeviceID { get; set; }

    public string Short_ID { get; set; }

    public DateTime? Create_Date { get; set; }

    public string Create_User { get; set; }

    public DateTime? Update_Date { get; set; }

    public string Update_User { get; set; }

    public bool IsConfig { get; set; }

    public bool IsActive { get; set; }

    public int Device_Type { get; set; }

    public string Device_Product { get; set; }
  }
}
