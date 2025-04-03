// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.CreateEditArchiveJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.Archiving;
using MSS.Business.Utils;
using MSS.Core.Model.Archiving;
using MSS.Core.Model.Reporting;
using MSS.DTO.Archive;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class CreateEditArchiveJobViewModel : ValidationViewModelBase
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

    public CreateEditArchiveJobViewModel(
      IRepositoryFactory repositoryFactory,
      ArchiveJobDTO archiveJob)
    {
      this._repositoryFactory = repositoryFactory;
      this._archiveJobRepository = repositoryFactory.GetRepository<ArchiveJob>();
      this.ArchiveJobDialogTitle = archiveJob == null ? CultureResources.GetValue("MSS_Client_Archiving_CreateArchiveJob") : CultureResources.GetValue("MSS_Client_Archiving_EditArchiveJob");
      this._archiveJob = archiveJob;
      bool flag = archiveJob != null;
      this.IsAddArchiveJobButtonVisible = !flag;
      this.IsEditArchiveJobButtonVisible = flag;
      if (flag)
      {
        this.ArchiveName = archiveJob.ArchiveName;
        this.StartDateValue = archiveJob.StartDate;
        this.NumberOfDaysToExport = archiveJob.NumberOfDaysToExport;
        this.DeleteAfterArchive = archiveJob.DeleteAfterArchive;
        this.ArchivedEntitiesCollection = archiveJob.ArchivedEntitiesList;
        AutomatedExportJobPeriodicityDTO jobPeriodicityDto = this.GetJobPeriodicities.FirstOrDefault<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (p => p.AutomatedExportPeriodicityEnum == archiveJob.Periodicity));
        if (jobPeriodicityDto == null)
          return;
        this.SelectedJobPeriodicityId = jobPeriodicityDto.Id;
      }
      else
      {
        this.StartDateValue = DateTime.Now;
        this.ArchivedEntitiesCollection = ((IEnumerable<ArchivedEntitiesEnum>) Enum.GetValues(typeof (ArchivedEntitiesEnum))).Where<ArchivedEntitiesEnum>((Func<ArchivedEntitiesEnum, bool>) (x => x == ArchivedEntitiesEnum.Logs || x == ArchivedEntitiesEnum.ReadingData)).Select<ArchivedEntitiesEnum, ArchiveEntity>((Func<ArchivedEntitiesEnum, ArchiveEntity>) (archivedEntitiesEnum => new ArchiveEntity()
        {
          IsChecked = false,
          Name = archivedEntitiesEnum.GetStringValue(),
          ArchivedEntityEnum = archivedEntitiesEnum
        })).ToList<ArchiveEntity>();
      }
    }

    public IEnumerable<AutomatedExportJobPeriodicityDTO> GetJobPeriodicities
    {
      get
      {
        return MSSHelper.GetListOfObjectsFromEnum<AutomatedExportJobPeriodicityEnum, AutomatedExportJobPeriodicityDTO>();
      }
    }

    public ICommand CreateArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsValid)
            return;
          AutomatedExportJobPeriodicityDTO jobPeriodicityDto = this.GetJobPeriodicities.FirstOrDefault<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (jp => jp.Id == this.SelectedJobPeriodicityId));
          if (jobPeriodicityDto != null)
            this._archiveJobRepository.Insert(new ArchiveJob()
            {
              ArchiveName = this.ArchiveName,
              CreatedOn = DateTime.Now,
              NumberOfDaysToExport = this.NumberOfDaysToExport,
              DeleteAfterArchive = this.DeleteAfterArchive,
              ArchivedEntities = ArchivingHelper.SerializeArchivedEntities(this.ArchivedEntitiesCollection),
              StartDate = this.StartDateValue,
              Periodicity = jobPeriodicityDto.AutomatedExportPeriodicityEnum
            });
          this.OnRequestClose(true);
        });
      }
    }

    public ICommand EditArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsValid)
            return;
          ArchiveJob entity = this._archiveJobRepository.FirstOrDefault((Expression<Func<ArchiveJob, bool>>) (aj => aj.Id == this._archiveJob.Id));
          AutomatedExportJobPeriodicityDTO jobPeriodicityDto = this.GetJobPeriodicities.FirstOrDefault<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (jp => jp.Id == this.SelectedJobPeriodicityId));
          entity.ArchiveName = this.ArchiveName;
          entity.CreatedOn = DateTime.Now;
          entity.NumberOfDaysToExport = this.NumberOfDaysToExport;
          entity.DeleteAfterArchive = this.DeleteAfterArchive;
          entity.ArchivedEntities = ArchivingHelper.SerializeArchivedEntities(this.ArchivedEntitiesCollection);
          entity.StartDate = this.StartDateValue;
          if (jobPeriodicityDto != null)
            entity.Periodicity = jobPeriodicityDto.AutomatedExportPeriodicityEnum;
          this._archiveJobRepository.Update(entity);
          this.OnRequestClose(true);
        });
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

    [Required(ErrorMessage = "MSS_Client_AutomatedExportJob_StartDateErrorToolTip")]
    public DateTime StartDateValue
    {
      get => this._startDateValue;
      set
      {
        this._startDateValue = value;
        this.OnPropertyChanged(nameof (StartDateValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Archive_ArchiveNameValidationMessage")]
    public string ArchiveName
    {
      get => this._archiveName;
      set
      {
        this._archiveName = value;
        this.OnPropertyChanged(nameof (ArchiveName));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Archive_NumberOfDaysToExportErrorToolTip")]
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

    public bool IsAddArchiveJobButtonVisible { get; set; }

    public bool IsEditArchiveJobButtonVisible { get; set; }

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
