// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.NetworkSetupSlaveInfo
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.DTO;
using System;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class NetworkSetupSlaveInfo : DTOBase
  {
    private bool? _hasErrors;
    private string _nodeId;
    private string _signalStrength;
    private DateTime? _lastStartOn;
    private DateTime? _lastRegisteredOn;
    private string _imageLocation;

    public StructureNodeDTO Slave { get; set; }

    public bool? HasErrors
    {
      get => this._hasErrors;
      set
      {
        this._hasErrors = value;
        this.OnPropertyChanged(nameof (HasErrors));
      }
    }

    public string NodeId
    {
      get => this._nodeId;
      set
      {
        this._nodeId = value;
        this.OnPropertyChanged(nameof (NodeId));
      }
    }

    public string SignalStrength
    {
      get => this._signalStrength;
      set
      {
        this._signalStrength = value;
        this.OnPropertyChanged(nameof (SignalStrength));
      }
    }

    public DateTime? LastStartOn
    {
      get => this._lastStartOn;
      set
      {
        this._lastStartOn = value;
        this.OnPropertyChanged(nameof (LastStartOn));
      }
    }

    public DateTime? LastRegisteredOn
    {
      get => this._lastRegisteredOn;
      set
      {
        this._lastRegisteredOn = value;
        this.OnPropertyChanged(nameof (LastRegisteredOn));
      }
    }

    public string ImageLocation
    {
      get => this._imageLocation;
      set
      {
        this._imageLocation = value;
        this.OnPropertyChanged(nameof (ImageLocation));
      }
    }
  }
}
