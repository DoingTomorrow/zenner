// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.MinomatSlaveInfo
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.Structures;
using System.Collections.Generic;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class MinomatSlaveInfo
  {
    public MinomatSerializableDTO MinomatSlave { get; set; }

    public string ImageLocation { get; set; }

    public string Address { get; set; }

    public string Floor { get; set; }

    public string Status { get; set; }

    public string NodeId { get; set; }

    public string ParentId { get; set; }

    public string HopCount { get; set; }

    public string RSSI { get; set; }

    public List<MinomatSlaveInfo> MinomatSlavesList { get; set; }
  }
}
