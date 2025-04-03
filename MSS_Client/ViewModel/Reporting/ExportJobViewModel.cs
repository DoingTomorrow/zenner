// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.ExportJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using FluentNHibernate.Conventions;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.MessageHandler;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class ExportJobViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly string _action;
    private readonly ReportingManager _reportingManager;
    private Guid _jobId;
    private ViewModelBase _messageUserControl;
    private RadObservableCollection<Country> _countryList;
    private DateTime _startDateValue;
    private bool _dataExportLastDaysChoice;
    private int _numberOfDaysToExport;
    private bool _sasExportType;
    private bool _csvFileType;
    private bool _xmlFileType;
    private bool _excelFileType;
    private bool _commaDecimalSeparator;
    private bool _semicolonValueSeparator;
    private string _folderPath;
    private string _countrySelectorHeight = "0";

    [Inject]
    public ExportJobViewModel(
      IRepositoryFactory repositoryFactory,
      AutomatedExportJobDTO exportJob,
      string action)
    {
      this._repositoryFactory = repositoryFactory;
      this._action = action;
      this.SelectedCountries = new RadObservableCollection<Country>();
      this._reportingManager = new ReportingManager(this._repositoryFactory);
      this.DialogTitle = this.GetDialogTitle(action);
      this.CommaDescription = ReportingHelper.GetLocalizedCharacterName(',');
      this.DotDescription = ReportingHelper.GetLocalizedCharacterName('.');
      this.SemicolonDescription = ReportingHelper.GetLocalizedCharacterName(';');
      this.InitContext(action, exportJob);
    }

    private string GetDialogTitle(string action)
    {
      switch (action)
      {
        case "create":
          return Resources.MSS_Client_Reporting_AutomatedJobCreateDialog_Title;
        case "edit":
          return Resources.MSS_Client_Reporting_AutomatedJobEditDialog_Title;
        default:
          return string.Empty;
      }
    }

    private void InitContext(string action, AutomatedExportJobDTO exportJob)
    {
      if (action == "create")
      {
        this.StartDateValue = new AutomatedExportJobDTO().StartDate;
        this.CsvFileType = true;
        this.CommaDecimalSeparator = true;
        this.SemicolonValueSeparator = true;
        this.CountryList = this.GetCountries();
      }
      if (!(action == "edit"))
        return;
      this._jobId = exportJob.Id;
      this.StartDateValue = exportJob.StartDate;
      this.SelectedJobPeriodicityId = this.GetJobPeriodicities.First<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (jp => jp.AutomatedExportPeriodicityEnum == exportJob.Periodicity)).Id;
      this.ArchiveAfterExport = exportJob.ArchiveAfterExport;
      this.DeleteAfterExport = exportJob.DeleteAfterExport;
      this.FolderPath = exportJob.ExportPath;
      this.DataExportLastDaysChoice = exportJob.DataToExport.Definition == "FromTheLastXDays";
      this.NumberOfDaysToExport = exportJob.DataToExport.NumberOfDays;
      this.SasExportType = exportJob.ExportFor == "SAS";
      this.CsvFileType = exportJob.ExportedFileType == "CSV";
      this.XmlFileType = exportJob.ExportedFileType == "XML";
      this.ExcelFileType = exportJob.ExportedFileType == "EXCEL";
      this.CommaDecimalSeparator = exportJob.DecimalSeparator == ReportingHelper.GetLocalizedCharacterName(',');
      this.SemicolonValueSeparator = exportJob.ValueSeparator == ReportingHelper.GetLocalizedCharacterName(';');
      this.SelectedCountries = this._reportingManager.GetCountriesForExportJob(exportJob);
      this.CountryList = new RadObservableCollection<Country>(this.GetCountries().Where<Country>((Func<Country, bool>) (c => !this.SelectedCountries.Contains(c))));
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

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<int>((Expression<Func<int>>) (() => this.SelectedJobPeriodicityId));
      string propertyName2 = this.GetPropertyName<RadObservableCollection<Country>>((Expression<Func<RadObservableCollection<Country>>>) (() => this.SelectedCountries));
      if (propertyName == propertyName1)
      {
        ICollection<string> source = (ICollection<string>) new Collection<string>();
        if (this.SelectedJobPeriodicityId == 0)
        {
          source.Add(Resources.MSS_Client_AutomatedExportJob_SelectedJobPeriodicityErrorToolTip);
          this.IsValid = false;
        }
        return source.ToList<string>();
      }
      if (!(propertyName == propertyName2))
        return new List<string>();
      ICollection<string> source1 = (ICollection<string>) new Collection<string>();
      if (this.SasExportType && EnumerableExtensionsForConventions.IsEmpty<Country>((IEnumerable<Country>) this.SelectedCountries))
      {
        source1.Add(Resources.MSS_Client_Reporting_Dialog_CountriesErrorToolTip);
        this.IsValid = false;
      }
      return source1.ToList<string>();
    }

    public RadObservableCollection<Country> GetCountries()
    {
      return new UsersManager(this._repositoryFactory).GetCountries();
    }

    public RadObservableCollection<Country> CountryList
    {
      get => this._countryList;
      set
      {
        this._countryList = value;
        this.OnPropertyChanged(nameof (CountryList));
      }
    }

    private void SaveData()
    {
      if (this._action == "create")
        this.CreateAutomatedJob();
      if (!(this._action == "edit"))
        return;
      this.EditAutomatedJob();
    }

    private void CreateAutomatedJob()
    {
      string str = "CSV";
      char ch1 = this.CommaDecimalSeparator ? ',' : '.';
      char ch2 = this.SemicolonValueSeparator ? ';' : ',';
      if (this.XmlFileType)
      {
        str = "XML";
        ch1 = ch2 = char.MinValue;
      }
      if (this.ExcelFileType)
      {
        str = "EXCEL";
        ch1 = ch2 = char.MinValue;
      }
      AutomatedExportJob automatedExportJob = new AutomatedExportJob()
      {
        Periodicity = this.GetJobPeriodicities.First<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (jp => jp.Id == this.SelectedJobPeriodicityId)).AutomatedExportPeriodicityEnum,
        Type = AutomatedExportJobTypeEnum.ReadingValues,
        ArchiveAfterExport = this.ArchiveAfterExport,
        DeleteAfterExport = this.DeleteAfterExport,
        StartDate = this.StartDateValue,
        LastExecutionTime = new DateTime?(),
        DataToExport = JsonConvert.SerializeObject((object) new DataToExport()
        {
          Definition = (this.DataExportLastDaysChoice ? "FromTheLastXDays" : "NotYetExported"),
          NumberOfDays = (this.DataExportLastDaysChoice ? this.NumberOfDaysToExport : 0)
        }),
        ExportFor = this.SasExportType ? "SAS" : "GMM",
        ExportedFileType = str,
        DecimalSeparator = ch1,
        ValueSeparator = ch2,
        Path = this.FolderPath
      };
      this._reportingManager.CreateExportJob(automatedExportJob);
      if (!this.SasExportType)
        return;
      this._reportingManager.CreateSasJobCountryConnections(automatedExportJob, this.SelectedCountries);
    }

    private void EditAutomatedJob()
    {
      AutomatedExportJob byId = this._repositoryFactory.GetRepository<AutomatedExportJob>().GetById((object) this._jobId);
      byId.Periodicity = (AutomatedExportJobPeriodicityEnum) Enum.Parse(typeof (AutomatedExportJobPeriodicityEnum), this.GetJobPeriodicities.First<AutomatedExportJobPeriodicityDTO>((Func<AutomatedExportJobPeriodicityDTO, bool>) (jp => jp.Id == this.SelectedJobPeriodicityId)).AutomatedExportJobPeriodicity);
      byId.Type = AutomatedExportJobTypeEnum.ReadingValues;
      byId.ArchiveAfterExport = this.ArchiveAfterExport;
      byId.DeleteAfterExport = this.DeleteAfterExport;
      byId.StartDate = this.StartDateValue;
      byId.DataToExport = JsonConvert.SerializeObject((object) new DataToExport()
      {
        Definition = (this.DataExportLastDaysChoice ? "FromTheLastXDays" : "NotYetExported"),
        NumberOfDays = (this.DataExportLastDaysChoice ? this.NumberOfDaysToExport : 0)
      });
      byId.ExportFor = this.SasExportType ? "SAS" : "GMM";
      string str = "CSV";
      if (this.XmlFileType)
        str = "XML";
      if (this.ExcelFileType)
        str = "EXCEL";
      byId.ExportedFileType = str;
      byId.DecimalSeparator = this.CommaDecimalSeparator ? ',' : '.';
      byId.ValueSeparator = this.SemicolonValueSeparator ? ';' : ',';
      this._reportingManager.UpdateExportJob(byId);
      if (!this.SasExportType)
        return;
      this._reportingManager.UpdateSasJobCountryConnections(byId, this.SelectedCountries);
    }

    public ICommand ExportJobOkCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (this.IsValid)
          {
            this.SaveData();
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            };
            EventPublisher.Publish<ActionUpdated>(new ActionUpdated()
            {
              Message = message
            }, (IViewModel) this);
            this.OnRequestClose(true);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = MessageCodes.ValidationError.GetStringValue()
            }.MessageText);
        });
      }
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          EventPublisher.Publish<ActionUpdated>(new ActionUpdated()
          {
            Message = message
          }, (IViewModel) this);
          this.OnRequestClose(false);
        }));
      }
    }

    public IEnumerable<AutomatedExportJobPeriodicityDTO> GetJobPeriodicities
    {
      get
      {
        return MSSHelper.GetListOfObjectsFromEnum<AutomatedExportJobPeriodicityEnum, AutomatedExportJobPeriodicityDTO>();
      }
    }

    public ICommand BrowseWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
          {
            ShowNewFolderButton = true
          };
          if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            return;
          this.FolderPath = folderBrowserDialog.SelectedPath;
        }));
      }
    }

    public string DialogTitle { get; set; }

    [Required(ErrorMessage = "MSS_Client_AutomatedExportJob_SelectedJobPeriodicityErrorToolTip")]
    public int SelectedJobPeriodicityId { get; set; }

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

    public bool ArchiveAfterExport { get; set; }

    public bool DeleteAfterExport { get; set; }

    public bool DataExportLastDaysChoice
    {
      get => this._dataExportLastDaysChoice;
      set
      {
        this._dataExportLastDaysChoice = value;
        this.OnPropertyChanged(nameof (DataExportLastDaysChoice));
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

    public bool SasExportType
    {
      get => this._sasExportType;
      set
      {
        this._sasExportType = value;
        this.CountrySelectorHeight = this._sasExportType ? "3*" : "0";
        this.OnPropertyChanged("SelectedCountries");
        this.OnPropertyChanged(nameof (SasExportType));
      }
    }

    public bool CsvFileType
    {
      get => this._csvFileType;
      set
      {
        this._csvFileType = value;
        this.OnPropertyChanged(nameof (CsvFileType));
      }
    }

    public bool XmlFileType
    {
      get => this._xmlFileType;
      set
      {
        this._xmlFileType = value;
        this.OnPropertyChanged(nameof (XmlFileType));
      }
    }

    public bool ExcelFileType
    {
      get => this._excelFileType;
      set
      {
        this._excelFileType = value;
        this.OnPropertyChanged(nameof (ExcelFileType));
      }
    }

    public bool CommaDecimalSeparator
    {
      get => this._commaDecimalSeparator;
      set
      {
        this._commaDecimalSeparator = value;
        this.OnPropertyChanged(nameof (CommaDecimalSeparator));
      }
    }

    public bool SemicolonValueSeparator
    {
      get => this._semicolonValueSeparator;
      set
      {
        this._semicolonValueSeparator = value;
        this.OnPropertyChanged(nameof (SemicolonValueSeparator));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Reporting_AutomatedJobCreateDialog_NoPathWasSelected_Message")]
    public string FolderPath
    {
      get => this._folderPath;
      set
      {
        this._folderPath = value;
        this.OnPropertyChanged(nameof (FolderPath));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Reporting_Dialog_CountriesErrorToolTip")]
    public RadObservableCollection<Country> SelectedCountries { get; set; }

    public Country Country
    {
      set => this.OnPropertyChanged("SelectedCountries");
    }

    public string CountrySelectorHeight
    {
      get => this._countrySelectorHeight;
      set
      {
        this._countrySelectorHeight = value;
        this.OnPropertyChanged(nameof (CountrySelectorHeight));
      }
    }

    public string CommaDescription { get; set; }

    public string DotDescription { get; set; }

    public string SemicolonDescription { get; set; }
  }
}
