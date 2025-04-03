// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.RegisteredDevicesForMinomatViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.ViewModel.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class RegisteredDevicesForMinomatViewModel : StructureViewModelBase
  {
    private StructureNodeDTO _rootNode;
    private string _title;
    private List<MeterInfo> _metersDetails;

    public RegisteredDevicesForMinomatViewModel(
      IRepositoryFactory repositoryFactory,
      StructureNodeDTO selectedMinomatStructureNode,
      StructureNodeDTO rootNode)
    {
      this._rootNode = rootNode;
      MinomatSerializableDTO selectedMinomat = selectedMinomatStructureNode.Entity as MinomatSerializableDTO;
      this.Title = string.Format(Resources.MSS_Client_RegisteredDevicesForMinomat_Title, (object) (selectedMinomat?.RadioId ?? ""));
      if (selectedMinomat == null || !(selectedMinomat.Id != Guid.Empty))
        return;
      List<Guid> list = repositoryFactory.GetRepository<MinomatMeter>().Where((Expression<Func<MinomatMeter, bool>>) (item => item.Minomat.Id == selectedMinomat.Id)).ToList<MinomatMeter>().Select<MinomatMeter, Guid>((Func<MinomatMeter, Guid>) (item => item.Meter.Id)).ToList<Guid>();
      this._metersDetails = new List<MeterInfo>();
      if (list.Any<Guid>())
        this.GetMeterInfos(ref this._metersDetails, list, rootNode);
    }

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public List<MeterInfo> MetersDetails
    {
      get => this._metersDetails;
      set
      {
        this._metersDetails = value;
        this.OnPropertyChanged(nameof (MetersDetails));
      }
    }

    private void GetMeterInfos(
      ref List<MeterInfo> meterInfos,
      List<Guid> metersToDisplay_Ids,
      StructureNodeDTO rootNode)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter" && subNode.Entity is MeterDTO && metersToDisplay_Ids.Contains((subNode.Entity as MeterDTO).Id))
        {
          MeterInfo meterInfo = new MeterInfo();
          if (subNode.Entity is MeterDTO entity1)
            meterInfo.Meter = entity1;
          TenantDTO tenantParent = this.GetTenantParent(subNode);
          if (tenantParent != null)
          {
            meterInfo.TenantNumber = tenantParent.TenantNr;
            meterInfo.TenantName = tenantParent.Name;
            meterInfo.FloorPosition = tenantParent?.FloorNr + "." + tenantParent?.FloorName + " / " + tenantParent?.ApartmentNr + " " + tenantParent?.Direction;
          }
          if (this._rootNode.Entity is LocationDTO)
            meterInfo.Address = (this._rootNode.Entity is LocationDTO entity2 ? entity2.Street : (string) null) + " " + (this._rootNode.Entity is LocationDTO entity3 ? entity3.BuildingNr : (string) null);
          meterInfos.Add(meterInfo);
        }
        this.GetMeterInfos(ref meterInfos, metersToDisplay_Ids, subNode);
      }
    }

    private TenantDTO GetTenantParent(StructureNodeDTO node)
    {
      StructureNodeDTO structureNodeDto = node;
      while (structureNodeDto.ParentNode != null && structureNodeDto.ParentNode.Id != Guid.Empty && structureNodeDto.NodeType.Name != "Tenant")
        structureNodeDto = structureNodeDto.ParentNode;
      return structureNodeDto.Entity as TenantDTO;
    }
  }
}
