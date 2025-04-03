// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.ExportFileSettingsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using FluentNHibernate.Conventions;
using MSS.Business.Events;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class ExportFileSettingsViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private bool _dataExportLastDaysChoice;
    private int _numberOfDaysToExport;
    private string _countrySelectorHeight = "0";
    private bool _sasExportType;
    private bool _csvFileType;
    private bool _xmlFileType;
    private bool _excelFileType;
    private bool _commaDecimalSeparator;
    private bool _semicolonValueSeparator;
    private RadObservableCollection<Country> _countryList;
    private BackgroundWorker _backgroundWorkerExport;

    [Inject]
    public ExportFileSettingsViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this.CountryList = this.GetCountries();
      this.SelectedCountries = new RadObservableCollection<Country>();
      this.CsvFileType = true;
      this.CommaDecimalSeparator = true;
      this.SemicolonValueSeparator = true;
    }

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

    public string CountrySelectorHeight
    {
      get => this._countrySelectorHeight;
      set
      {
        this._countrySelectorHeight = value;
        this.OnPropertyChanged(nameof (CountrySelectorHeight));
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

    [Required(ErrorMessage = "MSS_Client_Reporting_Dialog_CountriesErrorToolTip")]
    public RadObservableCollection<Country> SelectedCountries { get; set; }

    public Country Country
    {
      set => this.OnPropertyChanged("SelectedCountries");
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

    private void CancelProcess(object sender, EventArgs e)
    {
      this._backgroundWorkerExport.CancelAsync();
    }

    public ICommand ExportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          DeterminateProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<DeterminateProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_Client_Export), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.EXPORT_INSTALLATION_ORDER_TEXT));
          this._backgroundWorkerExport = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          this._backgroundWorkerExport.DoWork += (DoWorkEventHandler) ((sender, args) => new ReportingManager(this._repositoryFactory).ExportAllReadingValues(this.DataExportLastDaysChoice, this.NumberOfDaysToExport, this.SasExportType, this.CsvFileType, this.XmlFileType, this.ExcelFileType, this.CommaDecimalSeparator, this.SemicolonValueSeparator, this._backgroundWorkerExport, args, this.SelectedCountries.ToList<Country>()));
          this._backgroundWorkerExport.ProgressChanged += (ProgressChangedEventHandler) ((sender, e) => EventPublisher.Publish<ProgressBarValueChanged>(new ProgressBarValueChanged()
          {
            Value = e.ProgressPercentage
          }, (IViewModel) this));
          this._backgroundWorkerExport.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            pd.OnRequestClose(false);
            if (args.Cancelled)
              MessageHandlingManager.ShowWarningMessage(Resources.MSS_MessageCodes_Cancel);
            else if (args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            else
            {
              MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_Synchronization_Succedded);
              EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
              {
                Message = new MSS.DTO.Message.Message()
                {
                  MessageType = MessageTypeEnum.Success,
                  MessageText = Resources.MSS_MessageCodes_SuccessOperation
                }
              }, (IViewModel) this);
            }
          });
          this.OnRequestClose(false);
          this._backgroundWorkerExport.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, this._backgroundWorkerExport);
        });
      }
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<RadObservableCollection<Country>>((Expression<Func<RadObservableCollection<Country>>>) (() => this.SelectedCountries));
      if (!(propertyName == propertyName1))
        return new List<string>();
      ICollection<string> source = (ICollection<string>) new Collection<string>();
      if (this.SasExportType && EnumerableExtensionsForConventions.IsEmpty<Country>((IEnumerable<Country>) this.SelectedCountries))
      {
        source.Add(Resources.MSS_Client_Reporting_Dialog_CountriesErrorToolTip);
        this.IsValid = false;
      }
      return source.ToList<string>();
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestClose(false)));
    }
  }
}
