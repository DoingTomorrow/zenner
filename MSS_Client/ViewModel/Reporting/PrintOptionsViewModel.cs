// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.PrintOptionsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DIConfiguration;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class PrintOptionsViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private IWindowFactory _windowFactory;
    private IEnumerable<MeterReadingValueDTO> _readingValues;
    private ObservableCollection<ColumnToPrint> _columnsToPrint;
    private bool _isPrintAllChecked;
    private bool _isPrintSelectedChecked;
    private bool _isFitToPageWidthChecked;
    private string _titleOfPrint;

    public PrintOptionsViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      IEnumerable<MeterReadingValueDTO> readingValues)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._readingValues = readingValues;
      this._isPrintSelectedChecked = false;
      this._isPrintAllChecked = !this._isPrintSelectedChecked;
      this._isFitToPageWidthChecked = true;
      this.InitListBoxItems();
    }

    public ICommand PrintPreviewCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          PrintDialog printDialog = new PrintDialog();
          if (!printDialog.ShowDialog().GetValueOrDefault())
            return;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ReadingValuesPrintPreviewViewModel>((IParameter) new ConstructorArgument("readingValues", (object) this._readingValues), (IParameter) new ConstructorArgument("columnsToPrint", (object) this.ColumnsToPrint.ToList<ColumnToPrint>()), (IParameter) new ConstructorArgument("printDialog", (object) printDialog), (IParameter) new ConstructorArgument("title", (object) this.TitleOfPrint)));
        }));
      }
    }

    public ObservableCollection<ColumnToPrint> ColumnsToPrint
    {
      get => this._columnsToPrint;
      set
      {
        this._columnsToPrint = value;
        this.OnPropertyChanged(nameof (ColumnsToPrint));
      }
    }

    public bool IsPrintAllChecked
    {
      get => this._isPrintAllChecked;
      set
      {
        this._isPrintAllChecked = value;
        this.OnPropertyChanged(nameof (IsPrintAllChecked));
      }
    }

    public bool IsPrintSelectedChecked
    {
      get => this._isPrintSelectedChecked;
      set
      {
        this._isPrintAllChecked = value;
        this.OnPropertyChanged(nameof (IsPrintSelectedChecked));
      }
    }

    public bool IsFitToPageWidthChecked
    {
      get => this._isFitToPageWidthChecked;
      set
      {
        this._isFitToPageWidthChecked = value;
        this.OnPropertyChanged(nameof (IsFitToPageWidthChecked));
      }
    }

    public string TitleOfPrint
    {
      get => this._titleOfPrint;
      set
      {
        this._titleOfPrint = value;
        this.OnPropertyChanged(nameof (TitleOfPrint));
      }
    }

    private void InitListBoxItems()
    {
      this._columnsToPrint = new ObservableCollection<ColumnToPrint>();
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_ReadingValues_Date,
        IsChecked = true,
        BoundFieldName = "Date"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_Structures_Header_SerialNumber,
        IsChecked = true,
        BoundFieldName = "MeterSerialNumber"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_ReadingValues_Value,
        IsChecked = true,
        BoundFieldName = "Value"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_PhysicalQuantity,
        IsChecked = true,
        BoundFieldName = "PhysicalQuantity"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_MeterType,
        IsChecked = true,
        BoundFieldName = "MeterType"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_Calculation,
        IsChecked = true,
        BoundFieldName = "Calculation"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_CalculationStart,
        IsChecked = true,
        BoundFieldName = "CalculationStart"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_StorageInterval,
        IsChecked = true,
        BoundFieldName = "StorageInterval"
      });
      this._columnsToPrint.Add(new ColumnToPrint()
      {
        Header = Resources.MSS_Client_DataFilters_Creation,
        IsChecked = true,
        BoundFieldName = "Creation"
      });
    }
  }
}
