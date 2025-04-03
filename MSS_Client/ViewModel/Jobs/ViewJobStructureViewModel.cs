// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.ViewJobStructureViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MVVM.ViewModel;
using System.Collections.ObjectModel;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class ViewJobStructureViewModel : ViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _structureForJob;

    public ViewJobStructureViewModel(
      IRepositoryFactory repositoryFactory,
      MssReadingJobDto selectedJob)
    {
      StructureNodeDTO structureNodeDto = !(selectedJob.Status == JobStatusEnum.Active.ToString()) ? StructuresHelper.LoadStructureFromRootNodeId(repositoryFactory, selectedJob.StructureNodeId) : JobCyclesCache.Instance.GetJobStructureByJobId(selectedJob.Id) ?? StructuresHelper.LoadStructureFromRootNodeId(repositoryFactory, selectedJob.StructureNodeId);
      this._structureForJob = new ObservableCollection<StructureNodeDTO>();
      this._structureForJob.Add(structureNodeDto);
    }

    public ObservableCollection<StructureNodeDTO> StructureForJob
    {
      get => this._structureForJob;
      set
      {
        this._structureForJob = value;
        this.OnPropertyChanged(nameof (StructureForJob));
      }
    }
  }
}
