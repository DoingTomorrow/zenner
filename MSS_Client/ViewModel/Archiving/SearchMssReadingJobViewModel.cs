// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.SearchMssReadingJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Utils;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Jobs;
using MVVM.ViewModel;
using Ninject;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  public class SearchMssReadingJobViewModel : ViewModelBase
  {
    public VirtualQueryableCollectionView<ArchiveMssReadingJob> _archiveMssReadingJobDTO;

    private ArchiveInformation ArchiveInformation { get; set; }

    [Inject]
    public SearchMssReadingJobViewModel(ArchiveInformation archiveInformation)
    {
      this.ArchiveInformation = archiveInformation;
    }

    private void IntializeCollection()
    {
      this.ArchiveMssReadingJobDTO = MSSHelper.LoadCollection<ArchiveMssReadingJob>();
    }

    public VirtualQueryableCollectionView<ArchiveMssReadingJob> ArchiveMssReadingJobDTO
    {
      get => this._archiveMssReadingJobDTO;
      set
      {
        this._archiveMssReadingJobDTO = value;
        this.OnPropertyChanged(nameof (ArchiveMssReadingJobDTO));
      }
    }
  }
}
