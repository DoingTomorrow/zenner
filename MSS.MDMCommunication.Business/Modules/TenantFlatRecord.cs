// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.TenantFlatRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class TenantFlatRecord
  {
    public string Project_ID { get; set; }

    public DateTime? Valid_From { get; set; }

    public DateTime? Valid_To { get; set; }

    public string UnitNbr { get; set; }

    public string ResidentId { get; set; }

    public string Cust_UnitID { get; set; }

    public string MasterCustId { get; set; }

    public string User7 { get; set; }

    public string NextUnit { get; set; }

    public string UpperUnit { get; set; }

    public string Floor { get; set; }

    public string Floor_Pos { get; set; }

    public string Address_ID { get; set; }

    public double AreaSize { get; set; }

    public double AreaSize_Warm { get; set; }

    public double AreaSize_B { get; set; }

    public double AreaSize2 { get; set; }

    public double AreaSize2_Warm { get; set; }

    public double AreaSize_Indu { get; set; }

    public double VolumeSize { get; set; }

    public double VolumeSize_Warm { get; set; }

    public double AirFlow { get; set; }

    public double NumTenants { get; set; }

    public double OwnerShare { get; set; }

    public double Percent { get; set; }

    public double Garbage { get; set; }

    public double Interest { get; set; }

    public double GigaCalorie { get; set; }

    public double Joule { get; set; }

    public double Watt_KW { get; set; }

    public double Watt_MW { get; set; }

    public double Watt { get; set; }

    public double Scale { get; set; }

    public string SvcSend_AddrID { get; set; }

    public string SvcRec_AddrID { get; set; }

    public byte InComplete { get; set; }

    public byte IsOpen { get; set; }

    public byte IsAllow { get; set; }

    public string FloorHost { get; set; }

    public byte Analyze { get; set; }

    public double DisKey1 { get; set; }

    public double DisKey2 { get; set; }

    public double DisKey3 { get; set; }

    public double DisKey4 { get; set; }

    public double DisKey5 { get; set; }

    public double DisKey6 { get; set; }

    public double DisKey7 { get; set; }

    public double DisKey8 { get; set; }

    public DateTime? Create_Date { get; set; }

    public string Create_User { get; set; }

    public DateTime? Update_Date { get; set; }

    public string Update_User { get; set; }

    public bool IsConfig { get; set; }

    public bool IsActive { get; set; }
  }
}
