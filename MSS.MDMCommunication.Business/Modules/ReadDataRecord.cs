// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.ReadDataRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class ReadDataRecord
  {
    public string DCU_ID { get; set; }

    public string Meter_ID { get; set; }

    public string Slave_ID { get; set; }

    public string Read_Type { get; set; }

    public DateTime Read_Time { get; set; }

    public double MRead { get; set; }

    public string User1 { get; set; }

    public string User2 { get; set; }

    public string User3 { get; set; }

    public string User4 { get; set; }

    public string User5 { get; set; }

    public string User6 { get; set; }

    public DateTime Create_Date { get; set; }

    public string Create_Prog { get; set; }
  }
}
