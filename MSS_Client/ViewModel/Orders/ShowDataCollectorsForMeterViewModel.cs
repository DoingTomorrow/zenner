// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ShowDataCollectorsForMeterViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ShowDataCollectorsForMeterViewModel : ViewModelBase
  {
    private string _title;
    private List<MinomatMeter> _minomatMeters;

    public ShowDataCollectorsForMeterViewModel(
      StructureNodeDTO selectedMeter,
      IRepositoryFactory repositoryFactory)
    {
      this.Title = Resources.MSS_Client_ShowDataCollectorsTitle + " " + (selectedMeter.Entity is MeterDTO entity ? entity.SerialNumber : (string) null);
      this.MinomatMeters = repositoryFactory.GetRepository<MinomatMeter>().Where((Expression<Func<MinomatMeter, bool>>) (item => item.Meter.Id == (selectedMeter.Entity as MeterDTO).Id)).ToList<MinomatMeter>();
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

    public List<MinomatMeter> MinomatMeters
    {
      get => this._minomatMeters;
      set
      {
        this._minomatMeters = value;
        this.OnPropertyChanged(nameof (MinomatMeters));
      }
    }
  }
}
