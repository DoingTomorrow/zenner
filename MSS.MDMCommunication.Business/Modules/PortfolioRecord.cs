// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Modules.PortfolioRecord
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;

#nullable disable
namespace MSS.MDMCommunication.Business.Modules
{
  public class PortfolioRecord
  {
    public string Portfolio_ID { get; set; }

    public DateTime? Create_Date { get; set; }

    public string Create_User { get; set; }

    public string Name { get; set; }

    public string Address_ID { get; set; }

    public string Country { get; set; }

    public string Office { get; set; }

    public bool IsActive { get; set; }
  }
}
