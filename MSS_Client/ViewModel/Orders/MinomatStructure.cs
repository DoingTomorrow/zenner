// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.MinomatStructure
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Core.Model.DataCollectors;
using MSS.DTO;
using MSS.DTO.Structures;
using MSS.Utils.Utils;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class MinomatStructure : DTOBase
  {
    private string _description;
    private string _location;
    private string _statusDevices;
    private string _statusNetwork;
    private string _gsmStatus;
    private ObservableCollection<MinomatStructure> _minomatStructureSubNodes;
    private string _statusDevicesImageLocation;
    private string _statusNetworkImageLocation;

    public StructureNodeDTO MinomatStructureNode { get; set; }

    public string Description
    {
      get => this._description;
      set
      {
        this._description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }

    public string Location
    {
      get => this._location;
      set
      {
        this._location = value;
        this.OnPropertyChanged(nameof (Location));
      }
    }

    public string StatusDevices
    {
      get => this._statusDevices;
      set
      {
        this._statusDevices = value;
        if (!string.IsNullOrEmpty(value))
          this.StatusDevicesImageLocation = this.StatusDevices != MinomatStatusDevicesEnum.Open.GetStringValue() ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
        this.OnPropertyChanged(nameof (StatusDevices));
      }
    }

    public string StatusNetwork
    {
      get => this._statusNetwork;
      set
      {
        this._statusNetwork = value;
        if (!string.IsNullOrEmpty(value) && this.MinomatStructureNode?.Entity != null)
          this.StatusNetworkImageLocation = !(this.MinomatStructureNode.Entity as MinomatSerializableDTO).IsMaster ? (this.StatusNetwork == MinomatStatusNetworkEnum.SetupStarted.GetStringValue() ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png") : (this.StatusNetwork == MinomatStatusNetworkEnum.NetworkOptimization.GetStringValue() ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png");
        this.OnPropertyChanged(nameof (StatusNetwork));
      }
    }

    public string GSMStatus
    {
      get => this._gsmStatus;
      set
      {
        this._gsmStatus = value;
        this.OnPropertyChanged(nameof (GSMStatus));
      }
    }

    public ObservableCollection<MinomatStructure> MinomatStructureSubNodes
    {
      get => this._minomatStructureSubNodes;
      set
      {
        this._minomatStructureSubNodes = value;
        this.OnPropertyChanged(nameof (MinomatStructureSubNodes));
      }
    }

    public string StatusDevicesImageLocation
    {
      get => this._statusDevicesImageLocation;
      set
      {
        this._statusDevicesImageLocation = value;
        this.OnPropertyChanged(nameof (StatusDevicesImageLocation));
      }
    }

    public string StatusNetworkImageLocation
    {
      get => this._statusNetworkImageLocation;
      set
      {
        this._statusNetworkImageLocation = value;
        this.OnPropertyChanged(nameof (StatusNetworkImageLocation));
      }
    }
  }
}
