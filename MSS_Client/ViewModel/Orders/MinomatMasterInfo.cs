// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.MinomatMasterInfo
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO;
using MSS.DTO.Structures;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class MinomatMasterInfo : DTOBase
  {
    private string _imageLocation;
    private string _status;
    public int ReceivedSlavesNumber;
    private string _receivedSlavesString;

    public MinomatSerializableDTO MinomatMaster { get; set; }

    public string ImageLocation
    {
      get => this._imageLocation;
      set
      {
        this._imageLocation = value;
        this.OnPropertyChanged(nameof (ImageLocation));
      }
    }

    public string Address { get; set; }

    public string Floor { get; set; }

    public string Status
    {
      get => this._status;
      set
      {
        this._status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }

    public DateTime Date { get; set; }

    public string ReceivedSlavesString
    {
      get => this._receivedSlavesString;
      set
      {
        this._receivedSlavesString = value;
        this.OnPropertyChanged(nameof (ReceivedSlavesString));
      }
    }

    public List<MinomatSlaveInfo> MinomatSlavesList { get; set; }
  }
}
