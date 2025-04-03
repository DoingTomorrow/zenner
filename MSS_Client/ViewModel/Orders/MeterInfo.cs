// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.MeterInfo
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.Meters;
using MSS.DTO.Meters;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class MeterInfo
  {
    public MeterDTO Meter { get; set; }

    public string MinomatGsmId { get; set; }

    public string TenantName { get; set; }

    public int TenantNumber { get; set; }

    public string Address { get; set; }

    public string FloorPosition { get; set; }

    public MeterStatusEnum? Status { get; set; }

    public bool Received { get; set; }

    public bool Assigned { get; set; }

    public bool Registered { get; set; }
  }
}
