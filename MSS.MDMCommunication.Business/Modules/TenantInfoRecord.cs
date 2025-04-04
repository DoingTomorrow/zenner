// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.TenantInfoRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class TenantInfoRecord
  {
    public string Project_ID { get; set; }

    public string UnitNbr { get; set; }

    public string Tenant_ID { get; set; }

    public DateTime? Valid_From { get; set; }

    public DateTime? Valid_To { get; set; }

    public DateTime? ReferenceDate { get; set; }

    public string Tenant_Name { get; set; }

    public bool IsEmpty { get; set; }

    public bool IsInWork { get; set; }

    public bool TenantChange { get; set; }

    public string Cust_TenantID { get; set; }

    public string NextTenant_ID { get; set; }

    public bool Print { get; set; }

    public string Language { get; set; }

    public string CostForHeat { get; set; }

    public string VATForm { get; set; }

    public string OwnerAddr_ID { get; set; }

    public string SvcSendAddr_ID { get; set; }

    public string SvcRecAddr_ID { get; set; }

    public int NumTenants { get; set; }

    public string Tax_ID { get; set; }

    public bool AutoBill { get; set; }

    public string BillNo { get; set; }

    public bool KZU_Law { get; set; }

    public bool ChangeCharge { get; set; }

    public bool DriveCharge { get; set; }

    public bool BigLetter { get; set; }

    public string BankAccount { get; set; }

    public string Account_Entity { get; set; }

    public string Bill_Unit { get; set; }

    public string SortField { get; set; }

    public string OwnerNum { get; set; }

    public DateTime? Create_Date { get; set; }

    public string Create_User { get; set; }

    public DateTime? Update_Date { get; set; }

    public string Update_User { get; set; }

    public bool IsConfig { get; set; }

    public bool IsActive { get; set; }
  }
}
