// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Model.Enterprises.Enterprise
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

#nullable disable
namespace MSS.MDMCommunication.Business.Model.Enterprises
{
  public class Enterprise
  {
    public virtual int Enterprise_ID { get; set; }

    public virtual string Enterprise_Name { get; set; }

    public virtual string Address { get; set; }

    public virtual string Address2 { get; set; }

    public virtual string City { get; set; }

    public virtual string State { get; set; }

    public virtual string Zip { get; set; }

    public virtual string Country { get; set; }

    public virtual string Contact_Person { get; set; }

    public virtual string Contact_Phone { get; set; }

    public virtual string Contact_Fax { get; set; }

    public virtual string Contact_Email { get; set; }

    public virtual string Page_Header { get; set; }

    public virtual string Page_Footer { get; set; }

    public virtual string BroadcastMessage { get; set; }

    public virtual byte Is_HeadEnd_Radio3 { get; set; }

    public virtual int TimeZone_ID { get; set; }

    public virtual byte Apply_DST { get; set; }

    public virtual byte IsActive { get; set; }

    public virtual string Lang_ID { get; set; }
  }
}
