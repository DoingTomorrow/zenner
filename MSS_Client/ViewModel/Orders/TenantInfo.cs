// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.TenantInfo
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.DTO.Structures;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class TenantInfo
  {
    public TenantDTO Tenant { get; set; }

    public string Status { get; set; }

    public string ImageLocation { get; set; }

    public string Address { get; set; }

    public string FloorPosition { get; set; }

    public int NoOfDevices { get; set; }

    public int ReceivedDevices { get; set; }

    public int AssignedDevices { get; set; }

    public int RegisteredDevices { get; set; }

    public string RecAsRegDevices { get; set; }

    public int OpenDevices { get; set; }

    public string OpenDevicesString { get; set; }

    public ObservableCollection<StructureNodeDTO> SubNodes { get; set; }
  }
}
