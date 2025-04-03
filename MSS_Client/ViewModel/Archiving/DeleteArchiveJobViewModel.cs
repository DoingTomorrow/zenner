// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.DeleteArchiveJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Utils;
using MSS.Core.Model.Archiving;
using MSS.Core.Model.Reporting;
using MSS.DTO.Archive;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class DeleteArchiveJobViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<ArchiveJob> _archiveJobRepository;
    private readonly ArchiveJobDTO _archiveJob;
    private int _selectedJobPeriodicityId;
    private DateTime _startDateValue;
    private string _archiveName = string.Empty;
    private int _numberOfDaysToExport;
    private bool _deleteAfterArchive;
    private ViewModelBase _messageUserControl;
    private string _archiveJobDialogTitle;
    private List<ArchiveEntity> _archiveEntityCollection;

    public DeleteArchiveJobViewModel(IRepositoryFactory repositoryFactory, ArchiveJobDTO archiveJob)
    {
      this._repositoryFactory = repositoryFactory;
      this._archiveJobRepository = repositoryFactory.GetRepository<ArchiveJob>();
      this.ArchiveJobDialogTitle = CultureResources.GetValue("MSS_Client_Archiving_DeleteArchiveJob");
      this._archiveJob = archiveJob;
      this.ArchiveName = archiveJob.ArchiveName;
      this.StartDateValue = archiveJob.StartDate;
      this.DeleteAfterArchive = archiveJob.DeleteAfterArchive;
      this.ArchivedEntitiesCollection = archiveJob.ArchivedEntitiesList;
      AutomatedExportJobPeriodicityDTO jobPeriodicityDto = this.GetJobPeriodicities.FirstOrDefault<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (p => p.AutomatedExportPeriodicityEnum == archiveJob.Periodicity));
      if (jobPeriodicityDto == null)
        return;
      this.SelectedJobPeriodicityId = jobPeriodicityDto.Id;
    }

    public IEnumerable<AutomatedExportJobPeriodicityDTO> GetJobPeriodicities
    {
      get
      {
        return MSSHelper.GetListOfObjectsFromEnum<AutomatedExportJobPeriodicityEnum, AutomatedExportJobPeriodicityDTO>();
      }
    }

    public ICommand DeleteArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._archiveJobRepository.Delete(this._archiveJobRepository.FirstOrDefault((Expression<Func<ArchiveJob, bool>>) (aj => aj.Id == this._archiveJob.Id)));
          this.OnRequestClose(true);
        }));
      }
    }

    public int SelectedJobPeriodicityId
    {
      get => this._selectedJobPeriodicityId;
      set
      {
        this._selectedJobPeriodicityId = value;
        this.OnPropertyChanged(nameof (SelectedJobPeriodicityId));
      }
    }

    public DateTime StartDateValue
    {
      get => this._startDateValue;
      set
      {
        this._startDateValue = value;
        this.OnPropertyChanged(nameof (StartDateValue));
      }
    }

    public string ArchiveName
    {
      get => this._archiveName;
      set
      {
        this._archiveName = value;
        this.OnPropertyChanged(nameof (ArchiveName));
      }
    }

    public int NumberOfDaysToExport
    {
      get => this._numberOfDaysToExport;
      set
      {
        this._numberOfDaysToExport = value;
        this.OnPropertyChanged(nameof (NumberOfDaysToExport));
      }
    }

    public bool DeleteAfterArchive
    {
      get => this._deleteAfterArchive;
      set
      {
        this._deleteAfterArchive = value;
        this.OnPropertyChanged(nameof (DeleteAfterArchive));
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public string ArchiveJobDialogTitle
    {
      get => this._archiveJobDialogTitle;
      set
      {
        this._archiveJobDialogTitle = value;
        this.OnPropertyChanged(nameof (ArchiveJobDialogTitle));
      }
    }

    public List<ArchiveEntity> ArchivedEntitiesCollection
    {
      get => this._archiveEntityCollection;
      set
      {
        this._archiveEntityCollection = value;
        this.OnPropertyChanged(nameof (ArchivedEntitiesCollection));
      }
    }
  }
}
