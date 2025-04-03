// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ReportsForMinomatsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MVVM.ViewModel;
using Remotion.Linq.Collections;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ReportsForMinomatsViewModel : ViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _structureForMinomats;

    public ReportsForMinomatsViewModel(
      ObservableCollection<StructureNodeDTO> structureForMinomats)
    {
      this.StructureForMinomats = structureForMinomats;
    }

    public ObservableCollection<StructureNodeDTO> StructureForMinomats
    {
      get => this._structureForMinomats;
      set
      {
        this._structureForMinomats = value;
        this.OnPropertyChanged(nameof (StructureForMinomats));
      }
    }
  }
}
