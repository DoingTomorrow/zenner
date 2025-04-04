// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Orders.ExecuteOrderStructureNode
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.DTO.Orders
{
  public class ExecuteOrderStructureNode : DTOBase, IComparable
  {
    private ReadingValueStatusEnum? _status;
    private ReadingValueStatusEnum? _statusForDisplay;
    private BitmapImage _image;

    public Guid Id { get; set; }

    public string Name { get; set; }

    public StructureNodeTypeEnum NodeType { get; set; }

    public string SerialNumber { get; set; }

    public DeviceTypeEnum? DeviceType { get; set; }

    public string Room { get; set; }

    public string TenantFloor { get; set; }

    public ReadingValueStatusEnum? Status
    {
      get => this._status;
      set
      {
        this._status = value;
        ReadingValueStatusEnum? status = this._status;
        ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.MissingTranslationRules;
        this.StatusForDisplay = status.GetValueOrDefault() != readingValueStatusEnum || !status.HasValue ? this._status : new ReadingValueStatusEnum?(ReadingValueStatusEnum.nok);
        this.OnPropertyChanged(nameof (Status));
      }
    }

    public ReadingValueStatusEnum? StatusForDisplay
    {
      get => this._statusForDisplay;
      set
      {
        this._statusForDisplay = value;
        this.OnPropertyChanged(nameof (StatusForDisplay));
      }
    }

    public ObservableCollection<ExecuteOrderStructureNode> Children { get; set; }

    public BitmapImage Image
    {
      get => this._image;
      set
      {
        this._image = value;
        this.OnPropertyChanged(nameof (Image));
      }
    }

    public Guid MeterId { get; set; }

    public Brush BackgroundColor { get; set; }

    public Brush ColorState { get; set; }

    public string Manufacturer { get; set; }

    public DeviceMediumEnum? Medium { get; set; }

    public int? PrimaryAddres { get; set; }

    public bool IsMeter()
    {
      return this.NodeType == StructureNodeTypeEnum.Meter || this.NodeType == StructureNodeTypeEnum.RadioMeter;
    }

    public MbusRadioMeter MbusRadioMeter { get; set; }

    public bool ReadingEnabled { get; set; }

    public string ShortDeviceNo { get; set; }

    public string InputNumber { get; set; }

    public string Generation { get; set; }

    public string DeviceInfo { get; set; }

    public int CompareTo(object other)
    {
      int result1;
      int result2;
      return !(other is ExecuteOrderStructureNode orderStructureNode) || this.NodeType != StructureNodeTypeEnum.Tenant || orderStructureNode.NodeType != StructureNodeTypeEnum.Tenant || !int.TryParse(this.Name.Substring(0, this.Name.IndexOf(' ')), out result1) || !int.TryParse(orderStructureNode.Name.Substring(0, orderStructureNode.Name.IndexOf(' ')), out result2) ? 0 : result1.CompareTo(result2);
    }
  }
}
