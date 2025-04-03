// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.MeterReadingValuesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using Microsoft.Win32;
using MSS.Business.Documents;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.ReadingValues;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Telerik.Windows.Data;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.Model;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class MeterReadingValuesViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private BackgroundWorker _backgroundWorkerExport;
    private ViewModelBase _messageUserControl;
    private bool _isExportVisible;
    private bool _isExportCsvVisible;
    private bool _isExportPdfVisible;

    public MeterReadingValuesViewModel(
      StructureNodeDTO structureNode,
      OrderDTO selectedOrder,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      MeterReadingValuesViewModel readingValuesViewModel = this;
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this.IsExportVisible = true;
      this.IsExportCsvVisible = false;
      this.IsExportPdfVisible = false;
      GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_CLIENT_READING_VALUE_VIEW_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_READING_VALUE_VIEW_MESSAGE));
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
      {
        if (selectedOrder == null)
        {
          StructureReadingValuesInitializer valuesInitializer = new StructureReadingValuesInitializer(structureNode, readingValuesViewModel._repositoryFactory);
          readingValuesViewModel.ReadingValuesTitle = valuesInitializer.SetTitle(structureNode.Name);
          readingValuesViewModel.MeterReadingValuesDTO = valuesInitializer.GetReadingValuesDTO();
        }
        else
        {
          OrderReadingValuesInitializer valuesInitializer = new OrderReadingValuesInitializer(selectedOrder, structureNode, readingValuesViewModel._repositoryFactory);
          MeterDTO entity = structureNode.Entity as MeterDTO;
          readingValuesViewModel.ReadingValuesTitle = entity != null ? valuesInitializer.SetTitle(entity.SerialNumber) : valuesInitializer.SetTitle(selectedOrder.RootNodeName);
          readingValuesViewModel.MeterReadingValuesDTO = selectedOrder.StructureBytes != null ? valuesInitializer.GetReadingValuesDTO() : new ObservableCollection<MeterReadingValueDTO>();
        }
        MeterReadingValuesViewModel.SetRowColors(readingValuesViewModel.MeterReadingValuesDTO);
      });
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        pd.OnRequestClose(false);
        MSS.DTO.Message.Message message = (MSS.DTO.Message.Message) null;
        if (args.Cancelled)
          message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = Resources.MSS_Client_Archivation_Cancelled
          };
        else if (args.Error != null)
        {
          MSS.Business.Errors.MessageHandler.LogException(args.Error);
          MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), closure_0._windowFactory);
        }
        else
          message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Success,
            MessageText = Resources.MSS_Client_Archivation_Succedded
          };
        if (message == null)
          return;
        EventPublisher.Publish<ActionFinished>(new ActionFinished()
        {
          Message = message
        }, (IViewModel) closure_0);
      });
      backgroundWorker.RunWorkerAsync();
      this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
    }

    public MeterReadingValuesViewModel(
      ObservableCollection<MeterReadingValueDTO> readingValues,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.ReadingValuesTitle = Resources.MSS_Client_Structures_ReadingValues;
      this.IsExportVisible = false;
      this.IsExportCsvVisible = true;
      this.IsExportPdfVisible = true;
      this.MeterReadingValuesDTO = readingValues;
      MeterReadingValuesViewModel.SetRowColors(this.MeterReadingValuesDTO);
    }

    public string ReadingValuesTitle { get; set; }

    private IEnumerable<MeterReadingValue> MeterReadingValues { get; set; }

    public ObservableCollection<MeterReadingValueDTO> MeterReadingValuesDTO { get; set; }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public DataItemCollection FilteredRows { get; set; }

    public bool IsExportVisible
    {
      get => this._isExportVisible;
      set
      {
        this._isExportVisible = value;
        this.OnPropertyChanged(nameof (IsExportVisible));
      }
    }

    public bool IsExportCsvVisible
    {
      get => this._isExportCsvVisible;
      set
      {
        this._isExportCsvVisible = value;
        this.OnPropertyChanged(nameof (IsExportCsvVisible));
      }
    }

    public bool IsExportPdfVisible
    {
      get => this._isExportPdfVisible;
      set
      {
        this._isExportPdfVisible = value;
        this.OnPropertyChanged(nameof (IsExportPdfVisible));
      }
    }

    public ICommand SendReadingValuesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
            serviceClient.SendReadingValues(this.MeterReadingValues.ToList<MeterReadingValue>());
        }));
      }
    }

    public ICommand ExportReadingValueCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (x =>
        {
          if (this.FilteredRows.Count == 0)
          {
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_ReadingValuesExport_NoRecord);
          }
          else
          {
            ObservableCollection<MeterReadingValueDTO> filteredReadingValues = new ObservableCollection<MeterReadingValueDTO>();
            foreach (object filteredRow in this.FilteredRows)
              filteredReadingValues.Add(filteredRow as MeterReadingValueDTO);
            SaveFileDialog saveReadingValueDialog = new SaveFileDialog()
            {
              Filter = "CSV Document for GMM|*.csv|CSV Document for SAS|*.csv|CSV Document for GMM DE|*.csv|CSV Document for SAS DE|*.csv",
              Title = "Save reading values to file"
            };
            saveReadingValueDialog.ShowDialog();
            if (saveReadingValueDialog.FileName == string.Empty)
              return;
            DeterminateProgressDialogViewModel vm = DIConfigurator.GetConfigurator().Get<DeterminateProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_Client_Export), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.EXPORT_INSTALLATION_ORDER_TEXT));
            this._backgroundWorkerExport = new BackgroundWorker()
            {
              WorkerReportsProgress = true,
              WorkerSupportsCancellation = true
            };
            this._backgroundWorkerExport.DoWork += (DoWorkEventHandler) ((sender, eventArgs) =>
            {
              bool isSasExport = saveReadingValueDialog.FilterIndex == 2 || saveReadingValueDialog.FilterIndex == 4;
              bool commaDecimalSeparator = saveReadingValueDialog.FilterIndex == 3 || saveReadingValueDialog.FilterIndex == 4;
              new ReportingManager(this._repositoryFactory).ExportReadingValues(isSasExport ? ReportingHelper.FilteredReadingValuesCollection(filteredReadingValues) : filteredReadingValues, isSasExport, commaDecimalSeparator, saveReadingValueDialog.FileName, this._backgroundWorkerExport, eventArgs);
            });
            this._backgroundWorkerExport.ProgressChanged += (ProgressChangedEventHandler) ((sender, e) => EventPublisher.Publish<ProgressBarValueChanged>(new ProgressBarValueChanged()
            {
              Value = e.ProgressPercentage
            }, (IViewModel) this));
            this._backgroundWorkerExport.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
            {
              vm.OnRequestClose(false);
              if (args.Error != null)
              {
                MSS.Business.Errors.MessageHandler.LogException(args.Error);
                MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
              }
              else
                this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation);
            });
            this._backgroundWorkerExport.RunWorkerAsync();
            this._windowFactory.CreateNewProgressDialog((IViewModel) vm, this._backgroundWorkerExport);
          }
        }));
      }
    }

    public ICommand ExportReadingValueToCSVCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter => this.ExportDocument(DocumentTypesEnum.CSV)));
      }
    }

    public ICommand ExportReadingValueToPDFCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter => this.ExportDocument(DocumentTypesEnum.PDF)));
      }
    }

    private void ExportDocument(DocumentTypesEnum docType)
    {
      if (this.MeterReadingValuesDTO == null || this.MeterReadingValuesDTO.ToList<MeterReadingValueDTO>().Count == 0)
        return;
      CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
      Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
      ReportingManager reportingManager = new ReportingManager(this._repositoryFactory);
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      switch (docType)
      {
        case DocumentTypesEnum.CSV:
          saveFileDialog.Filter = "CSV Document|*.csv";
          break;
        case DocumentTypesEnum.PDF:
          saveFileDialog.Filter = "PDF Document|*.pdf";
          break;
        default:
          throw new UnsupportedFileTypeException(docType.ToString());
      }
      saveFileDialog.Title = Resources.MSS_Client_SaveMeterReadingValuesToFile;
      bool? nullable = saveFileDialog.ShowDialog();
      if (saveFileDialog.FileName == string.Empty)
        return;
      List<string[]> forCsvOrPdfExport = reportingManager.CreateReadingValuesListForCsvOrPdfExport(this.MeterReadingValuesDTO.ToList<MeterReadingValueDTO>());
      try
      {
        if (nullable.HasValue && nullable.Value)
        {
          try
          {
            switch (docType)
            {
              case DocumentTypesEnum.CSV:
                List<string[]> nodeList = CSVManager.AddQuatForCSV(forCsvOrPdfExport);
                new CSVManager().WriteToFile(saveFileDialog.FileName, nodeList);
                break;
              case DocumentTypesEnum.PDF:
                RadDocument meterReadingValues = GeneratePdfFromData.GeneratePdfFromMeterReadingValues(this.MeterReadingValuesDTO.First<MeterReadingValueDTO>().MeterSerialNumber, forCsvOrPdfExport);
                IDocumentFormatProvider providerByExtension = DocumentFormatProvidersManager.GetProviderByExtension(Path.GetExtension(saveFileDialog.SafeFileName));
                if (providerByExtension == null)
                {
                  this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_UnsupportedFileFormat);
                  return;
                }
                try
                {
                  using (Stream output = saveFileDialog.OpenFile())
                  {
                    providerByExtension.Export(meterReadingValues, output);
                    break;
                  }
                }
                catch (Exception ex)
                {
                  this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.Error.GetStringValue() + " " + Resources.MSS_Client_UnableToSaveFile + ex.ToString());
                  break;
                }
            }
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
          }
          catch (Exception ex)
          {
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.Error.GetStringValue() + " " + ex.Message);
            MSS.Business.Errors.MessageHandler.LogException(ex);
            MessageHandlingManager.ShowExceptionMessageDialog(ex.Message, this._windowFactory);
          }
        }
        else
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
      }
      finally
      {
        Thread.CurrentThread.CurrentCulture = cultureInfo;
      }
    }

    private static void SetRowColors(
      ObservableCollection<MeterReadingValueDTO> readingValues)
    {
      if (readingValues.Count <= 0)
        return;
      DateTime date = readingValues[0].Date;
      bool flag = true;
      foreach (MeterReadingValueDTO readingValue in (Collection<MeterReadingValueDTO>) readingValues)
      {
        if (readingValue.Date != date)
        {
          date = readingValue.Date;
          flag = !flag;
        }
        readingValue.IsDarkRowColor = flag;
      }
    }
  }
}
