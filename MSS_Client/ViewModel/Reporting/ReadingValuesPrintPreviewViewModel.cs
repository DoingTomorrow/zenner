// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.ReadingValuesPrintPreviewViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Utils;
using MSS.DTO.Meters;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class ReadingValuesPrintPreviewViewModel : ViewModelBase
  {
    private PrintDialog _printDialog;
    private Thickness _margins;
    private const double LEFT_MARGIN = 25.0;
    private const double TOP_MARGIN = 25.0;
    private const double RIGHT_MARGIN = 25.0;
    private const double BOTTOM_MARGIN = 25.0;
    private string _title;
    private double _gridHeight;
    private double _gridWidth;
    private IEnumerable<MeterReadingValueDTO> _meterReadingValues;
    private bool _isDateChecked;
    private bool _isMeterSerialNumberChecked;
    private bool _isValueChecked;
    private bool _isPhysicalQuantityChecked;
    private bool _isMeterTypeChecked;
    private bool _isCalculationChecked;
    private bool _isCalculationStartChecked;
    private bool _isStorageIntervalChecked;
    private bool _isCreationChecked;

    public ReadingValuesPrintPreviewViewModel(
      IEnumerable<MeterReadingValueDTO> readingValues,
      List<ColumnToPrint> columnsToPrint,
      PrintDialog printDialog,
      string title)
    {
      this._meterReadingValues = readingValues;
      this._printDialog = printDialog;
      this._title = title;
      foreach (ColumnToPrint columnToPrint in columnsToPrint)
        this.GetType().GetProperty("Is" + columnToPrint.BoundFieldName + "Checked").SetValue((object) this, (object) columnToPrint.IsChecked);
      this._margins = new Thickness(25.0, 25.0, 25.0, 25.0);
      PrintCapabilities printCapabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
      this._gridWidth = printCapabilities.PageImageableArea.ExtentWidth - this._margins.Left - this._margins.Right;
      this._gridHeight = printCapabilities.PageImageableArea.ExtentHeight - this._margins.Top - this._margins.Bottom;
    }

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public bool IsTitleVisible => !string.IsNullOrEmpty(this.Title);

    public double PageHeight
    {
      get => this._gridHeight;
      set
      {
        this._gridHeight = value;
        this.OnPropertyChanged("GridHeight");
      }
    }

    public double GridWidth
    {
      get => this._gridWidth;
      set
      {
        this._gridWidth = value;
        this.OnPropertyChanged(nameof (GridWidth));
      }
    }

    public IEnumerable<MeterReadingValueDTO> MeterReadingValues
    {
      get => this._meterReadingValues;
      set
      {
        this._meterReadingValues = value;
        this.OnPropertyChanged(nameof (MeterReadingValues));
      }
    }

    public bool IsDateChecked
    {
      get => this._isDateChecked;
      set
      {
        this._isDateChecked = value;
        this.OnPropertyChanged(nameof (IsDateChecked));
      }
    }

    public bool IsMeterSerialNumberChecked
    {
      get => this._isMeterSerialNumberChecked;
      set
      {
        this._isMeterSerialNumberChecked = value;
        this.OnPropertyChanged(nameof (IsMeterSerialNumberChecked));
      }
    }

    public bool IsValueChecked
    {
      get => this._isValueChecked;
      set
      {
        this._isValueChecked = value;
        this.OnPropertyChanged(nameof (IsValueChecked));
      }
    }

    public bool IsPhysicalQuantityChecked
    {
      get => this._isPhysicalQuantityChecked;
      set
      {
        this._isPhysicalQuantityChecked = value;
        this.OnPropertyChanged(nameof (IsPhysicalQuantityChecked));
      }
    }

    public bool IsMeterTypeChecked
    {
      get => this._isMeterTypeChecked;
      set
      {
        this._isMeterTypeChecked = value;
        this.OnPropertyChanged(nameof (IsMeterTypeChecked));
      }
    }

    public bool IsCalculationChecked
    {
      get => this._isCalculationChecked;
      set
      {
        this._isCalculationChecked = value;
        this.OnPropertyChanged(nameof (IsCalculationChecked));
      }
    }

    public bool IsCalculationStartChecked
    {
      get => this._isCalculationStartChecked;
      set
      {
        this._isCalculationStartChecked = value;
        this.OnPropertyChanged(nameof (IsCalculationStartChecked));
      }
    }

    public bool IsStorageIntervalChecked
    {
      get => this._isStorageIntervalChecked;
      set
      {
        this._isStorageIntervalChecked = value;
        this.OnPropertyChanged(nameof (IsStorageIntervalChecked));
      }
    }

    public bool IsCreationChecked
    {
      get => this._isCreationChecked;
      set
      {
        this._isCreationChecked = value;
        this.OnPropertyChanged(nameof (IsCreationChecked));
      }
    }

    public ICommand PrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          try
          {
            new UIPrinter().Print(parameter as Grid, this._printDialog, this._margins);
            this.OnRequestClose(false);
          }
          catch (PrintAborted ex)
          {
          }
        }));
      }
    }
  }
}
