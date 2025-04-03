// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.SearchLogsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Utils;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Reporting;
using MVVM.ViewModel;
using Ninject;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class SearchLogsViewModel : ViewModelBase
  {
    public VirtualQueryableCollectionView<ArchiveJobLogs> _archiveJobLogsDTO;

    private ArchiveInformation ArchiveInformation { get; set; }

    [Inject]
    public SearchLogsViewModel() => this.IntializeCollection();

    private void IntializeCollection()
    {
      this.ArchiveJobLogsDTO = MSSHelper.LoadCollection<ArchiveJobLogs>();
    }

    public VirtualQueryableCollectionView<ArchiveJobLogs> ArchiveJobLogsDTO
    {
      get => this._archiveJobLogsDTO;
      set
      {
        this._archiveJobLogsDTO = value;
        this.OnPropertyChanged(nameof (ArchiveJobLogsDTO));
      }
    }
  }
}
