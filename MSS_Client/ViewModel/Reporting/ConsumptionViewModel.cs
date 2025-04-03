// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.ConsumptionViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Criterion;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Data;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class ConsumptionViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private BackgroundWorker _backgroundWorkerSync;
    private RadObservableCollection<ReadingValuesForChartDTO> _getReadingValuesForCharts = new RadObservableCollection<ReadingValuesForChartDTO>();
    private double _maxValue;

    [Inject]
    public ConsumptionViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      MeterDTO selectedMeter,
      DateTime startDate,
      DateTime endDate,
      MSS.Core.Model.DataFilters.Filter filter)
    {
      ConsumptionViewModel consumptionViewModel = this;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.GetReadingValuesForCharts = new RadObservableCollection<ReadingValuesForChartDTO>();
      List<long> valueIds = new List<long>();
      List<ValueIdent.ValueIdPart_PhysicalQuantity> physicalQuantities = new List<ValueIdent.ValueIdPart_PhysicalQuantity>();
      GenericProgressDialogViewModel vm = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.METER_DATA_CHART_GENERATION_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.METER_DATA_CHART_GENERATION));
      this._backgroundWorkerSync = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      this._backgroundWorkerSync.DoWork += (DoWorkEventHandler) ((sender, args) => closure_0.CalculateConsumption(selectedMeter, startDate, endDate, filter, valueIds, physicalQuantities));
      this._backgroundWorkerSync.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) => vm.OnRequestClose(false));
      this._backgroundWorkerSync.RunWorkerAsync();
      this._windowFactory.CreateNewProgressDialog((IViewModel) vm, this._backgroundWorkerSync);
    }

    private void CalculateConsumption(
      MeterDTO selectedMeter,
      DateTime startDate,
      DateTime endDate,
      MSS.Core.Model.DataFilters.Filter filter,
      List<long> valueIds,
      List<ValueIdent.ValueIdPart_PhysicalQuantity> physicalQuantities)
    {
      if (filter != null)
        TypeHelperExtensionMethods.ForEach<Rules>((IEnumerable<Rules>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetById((object) filter.Id).Rules, (Action<Rules>) (x =>
        {
          valueIds.Add(long.Parse(x.ValueId));
          if (physicalQuantities.Contains(x.PhysicalQuantity))
            return;
          physicalQuantities.Add(x.PhysicalQuantity);
        }));
      IList<MeterReadingValue> querySelection = !valueIds.Contains(0L) ? this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Restrictions.Eq("ReadingValues.MeterSerialNumber", (object) selectedMeter.SerialNumber)).Add((ICriterion) Restrictions.In("ReadingValues.ValueId", (ICollection) valueIds)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) startDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) endDate)).List<MeterReadingValue>() : this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Restrictions.Eq("ReadingValues.MeterSerialNumber", (object) selectedMeter.SerialNumber)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) startDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) endDate)).List<MeterReadingValue>();
      if (querySelection.Any<MeterReadingValue>())
        this.MaxValue = querySelection.ToList<MeterReadingValue>().Max<MeterReadingValue>((Func<MeterReadingValue, double>) (x => x.Value));
      querySelection = (IList<MeterReadingValue>) querySelection.OrderBy<MeterReadingValue, DateTime>((Func<MeterReadingValue, DateTime>) (x => x.Date)).ToList<MeterReadingValue>();
      List<DateTime> energyDates = new List<DateTime>();
      List<DateTime> temperatureDates = new List<DateTime>();
      List<DateTime> volumeDates = new List<DateTime>();
      physicalQuantities.ForEach((Action<ValueIdent.ValueIdPart_PhysicalQuantity>) (x =>
      {
        switch ((long) x)
        {
          case 0:
            List<MeterReadingValue> energyRV1 = new List<MeterReadingValue>();
            List<MeterReadingValue> temperatureRV1 = new List<MeterReadingValue>();
            List<MeterReadingValue> volumeRV1 = new List<MeterReadingValue>();
            TypeHelperExtensionMethods.ForEach<MeterReadingValue>((IEnumerable<MeterReadingValue>) querySelection, (Action<MeterReadingValue>) (y =>
            {
              if (y.PhysicalQuantity == 2L && !energyDates.Contains(y.Date.Date))
              {
                energyDates.Add(y.Date.Date);
                energyRV1.Add(y);
              }
              DateTime date5;
              if (y.PhysicalQuantity == 1L && !volumeDates.Contains(y.Date.Date))
              {
                volumeRV1.Add(y);
                List<DateTime> dateTimeList = volumeDates;
                date5 = y.Date;
                DateTime date6 = date5.Date;
                dateTimeList.Add(date6);
              }
              if (y.PhysicalQuantity != 6L)
                return;
              List<DateTime> dateTimeList3 = temperatureDates;
              date5 = y.Date;
              DateTime date7 = date5.Date;
              if (!dateTimeList3.Contains(date7))
              {
                List<DateTime> dateTimeList4 = temperatureDates;
                date5 = y.Date;
                DateTime date8 = date5.Date;
                dateTimeList4.Add(date8);
                temperatureRV1.Add(y);
              }
            }));
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) temperatureRV1, (Brush) Brushes.DarkGoldenrod, (Brush) Brushes.Blue);
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) volumeRV1, (Brush) Brushes.BurlyWood, (Brush) Brushes.Red);
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) energyRV1, (Brush) Brushes.Yellow, (Brush) Brushes.Green);
            break;
          case 1:
            List<MeterReadingValue> volumeRV2 = new List<MeterReadingValue>();
            TypeHelperExtensionMethods.ForEach<MeterReadingValue>((IEnumerable<MeterReadingValue>) querySelection, (Action<MeterReadingValue>) (y =>
            {
              if (y.PhysicalQuantity != 1L || volumeDates.Contains(y.Date.Date))
                return;
              volumeRV2.Add(y);
              volumeDates.Add(y.Date.Date);
            }));
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) volumeRV2, (Brush) Brushes.BurlyWood, (Brush) Brushes.Red);
            break;
          case 2:
            List<MeterReadingValue> energyRV2 = new List<MeterReadingValue>();
            TypeHelperExtensionMethods.ForEach<MeterReadingValue>((IEnumerable<MeterReadingValue>) querySelection, (Action<MeterReadingValue>) (y =>
            {
              if (y.PhysicalQuantity != 2L || energyDates.Contains(y.Date.Date))
                return;
              energyDates.Add(y.Date.Date);
              energyRV2.Add(y);
            }));
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) energyRV2, (Brush) Brushes.Yellow, (Brush) Brushes.Green);
            break;
          case 6:
            List<MeterReadingValue> temperatureRV2 = new List<MeterReadingValue>();
            TypeHelperExtensionMethods.ForEach<MeterReadingValue>((IEnumerable<MeterReadingValue>) querySelection, (Action<MeterReadingValue>) (y =>
            {
              if (y.PhysicalQuantity != 6L || temperatureDates.Contains(y.Date.Date))
                return;
              temperatureDates.Add(y.Date.Date);
              temperatureRV2.Add(y);
            }));
            this.CreateReadingValuesForChart((IList<MeterReadingValue>) temperatureRV2, (Brush) Brushes.DarkGoldenrod, (Brush) Brushes.Blue);
            break;
        }
      }));
      IOrderedEnumerable<ReadingValuesForChartDTO> orderedEnumerable = this.GetReadingValuesForCharts.OrderBy<ReadingValuesForChartDTO, DateTime>((Func<ReadingValuesForChartDTO, DateTime>) (x => x.Date));
      this.GetReadingValuesForCharts = new RadObservableCollection<ReadingValuesForChartDTO>();
      TypeHelperExtensionMethods.ForEach<ReadingValuesForChartDTO>((IEnumerable<ReadingValuesForChartDTO>) orderedEnumerable, (Action<ReadingValuesForChartDTO>) (x =>
      {
        x.ValueId = Math.Round(x.ValueId, 3);
        this.GetReadingValuesForCharts.Add(x);
      }));
    }

    private void CreateReadingValuesForChart(
      IList<MeterReadingValue> readingValuesForSelectedMeter,
      Brush estimationValueColor,
      Brush realValueColor)
    {
      List<MeterReadingValue> readingValuesForFirstDaysOfTheMonth = new List<MeterReadingValue>();
      if (!readingValuesForSelectedMeter.Any<MeterReadingValue>())
        return;
      DateTime dateTime1 = readingValuesForSelectedMeter.First<MeterReadingValue>().Date;
      if (dateTime1.Day != 1)
        readingValuesForFirstDaysOfTheMonth.Add(readingValuesForSelectedMeter.First<MeterReadingValue>());
      TypeHelperExtensionMethods.ForEach<MeterReadingValue>((IEnumerable<MeterReadingValue>) readingValuesForSelectedMeter, (Action<MeterReadingValue>) (x =>
      {
        if (x.Date.Day != 1)
          return;
        readingValuesForFirstDaysOfTheMonth.Add(x);
      }));
      dateTime1 = readingValuesForSelectedMeter.Last<MeterReadingValue>().Date;
      if (dateTime1.Day != 1 && readingValuesForSelectedMeter.Count != 1)
      {
        dateTime1 = readingValuesForSelectedMeter.Last<MeterReadingValue>().Date;
        int month1 = dateTime1.Month;
        dateTime1 = readingValuesForFirstDaysOfTheMonth.Last<MeterReadingValue>().Date;
        int month2 = dateTime1.Month;
        if (month1 != month2)
          readingValuesForFirstDaysOfTheMonth.Add(readingValuesForSelectedMeter.Last<MeterReadingValue>());
      }
      List<ReadingValuesForChartDTO> valuesForChartDtoList1 = new List<ReadingValuesForChartDTO>();
      MeterReadingValue meterReadingValue1 = readingValuesForSelectedMeter.FirstOrDefault<MeterReadingValue>();
      if (meterReadingValue1 != null)
      {
        List<ReadingValuesForChartDTO> valuesForChartDtoList2 = valuesForChartDtoList1;
        ReadingValuesForChartDTO valuesForChartDto1 = new ReadingValuesForChartDTO();
        valuesForChartDto1.Color = realValueColor;
        valuesForChartDto1.IsEstimation = false;
        valuesForChartDto1.ValueId = meterReadingValue1.Value;
        ReadingValuesForChartDTO valuesForChartDto2 = valuesForChartDto1;
        dateTime1 = meterReadingValue1.Date;
        string str1 = dateTime1.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture);
        dateTime1 = meterReadingValue1.Date;
        // ISSUE: variable of a boxed type
        __Boxed<int> year = (System.ValueType) dateTime1.Year;
        string str2 = str1 + " " + (object) year;
        valuesForChartDto2.Month = str2;
        valuesForChartDto1.Date = meterReadingValue1.Date;
        valuesForChartDto1.LastValue = meterReadingValue1.Value;
        ReadingValuesForChartDTO valuesForChartDto3 = valuesForChartDto1;
        valuesForChartDtoList2.Add(valuesForChartDto3);
      }
      for (int index = 0; index < readingValuesForFirstDaysOfTheMonth.Count - 1; ++index)
      {
        dateTime1 = readingValuesForFirstDaysOfTheMonth[index].Date;
        int month = dateTime1.Month;
        dateTime1 = readingValuesForFirstDaysOfTheMonth[index + 1].Date;
        int num = dateTime1.Month - 1;
        if (month != num && valuesForChartDtoList1.Any<ReadingValuesForChartDTO>() && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index].Date, valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date) != 1 && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index].Date, valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date) != 0)
        {
          DateTime rValue = readingValuesForFirstDaysOfTheMonth[index].Date;
          int addedMonths = 0;
          for (; rValue < readingValuesForFirstDaysOfTheMonth[index + 1].Date && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index + 1].Date, rValue) != 1; rValue = rValue.AddMonths(1))
          {
            ++addedMonths;
            this.AddEstimatedReadingValue((IList<MeterReadingValue>) readingValuesForFirstDaysOfTheMonth, index, readingValuesForSelectedMeter, (IList<ReadingValuesForChartDTO>) valuesForChartDtoList1, addedMonths, estimationValueColor);
          }
        }
        else if (valuesForChartDtoList1.Any<ReadingValuesForChartDTO>() && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index].Date, valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date) == 1)
        {
          ConsumptionViewModel.AddRealReadingValue(valuesForChartDtoList1, readingValuesForFirstDaysOfTheMonth, index, true, realValueColor, estimationValueColor);
          DateTime rValue = valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date;
          int addedMonths = 0;
          for (; rValue < readingValuesForFirstDaysOfTheMonth[index + 1].Date && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index + 1].Date, rValue) != 1; rValue = rValue.AddMonths(1))
          {
            ++addedMonths;
            this.AddEstimatedReadingValue((IList<MeterReadingValue>) readingValuesForFirstDaysOfTheMonth, index, readingValuesForSelectedMeter, (IList<ReadingValuesForChartDTO>) valuesForChartDtoList1, addedMonths, estimationValueColor);
          }
        }
        else if (this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index + 1].Date, readingValuesForFirstDaysOfTheMonth[index].Date) != 1)
        {
          DateTime dateTime2 = readingValuesForFirstDaysOfTheMonth[index].Date;
          int addedMonths = 0;
          for (; dateTime2 < readingValuesForFirstDaysOfTheMonth[index + 1].Date && this.MonthDifference(readingValuesForFirstDaysOfTheMonth[index + 1].Date, dateTime2) != 1 && this.MonthDifference(dateTime2, readingValuesForFirstDaysOfTheMonth[index + 1].Date) != 0; dateTime2 = dateTime2.AddMonths(1))
          {
            ++addedMonths;
            this.AddEstimatedReadingValue((IList<MeterReadingValue>) readingValuesForFirstDaysOfTheMonth, index, readingValuesForSelectedMeter, (IList<ReadingValuesForChartDTO>) valuesForChartDtoList1, addedMonths, estimationValueColor);
          }
        }
        else
          ConsumptionViewModel.AddRealReadingValue(valuesForChartDtoList1, readingValuesForFirstDaysOfTheMonth, index, false, realValueColor, estimationValueColor);
      }
      MeterReadingValue meterReadingValue2 = readingValuesForSelectedMeter.LastOrDefault<MeterReadingValue>();
      if (meterReadingValue2 != null && readingValuesForSelectedMeter.Count != 1)
      {
        if (readingValuesForFirstDaysOfTheMonth.Count != 0 && this.MonthDifference(meterReadingValue2.Date, valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date) != 1 && this.MonthDifference(valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date, meterReadingValue2.Date) != 0)
        {
          DateTime lValue = valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date;
          int months = 0;
          for (; lValue < meterReadingValue2.Date && this.MonthDifference(lValue, meterReadingValue2.Date) != 1; lValue = lValue.AddMonths(1))
          {
            ++months;
            double estimation = this.CreateEstimation(lValue.AddMonths(months), readingValuesForSelectedMeter);
            List<ReadingValuesForChartDTO> valuesForChartDtoList3 = valuesForChartDtoList1;
            ReadingValuesForChartDTO valuesForChartDto4 = new ReadingValuesForChartDTO();
            ReadingValuesForChartDTO valuesForChartDto5 = valuesForChartDto4;
            dateTime1 = lValue.AddMonths(1);
            string str3 = dateTime1.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture);
            dateTime1 = lValue.AddMonths(1);
            // ISSUE: variable of a boxed type
            __Boxed<int> year = (System.ValueType) dateTime1.Year;
            string str4 = str3 + " " + (object) year;
            valuesForChartDto5.Month = str4;
            valuesForChartDto4.ValueId = estimation - valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().LastValue;
            valuesForChartDto4.Color = estimationValueColor;
            valuesForChartDto4.IsEstimation = true;
            valuesForChartDto4.Date = lValue.AddMonths(1);
            valuesForChartDto4.LastValue = estimation;
            ReadingValuesForChartDTO valuesForChartDto6 = valuesForChartDto4;
            valuesForChartDtoList3.Add(valuesForChartDto6);
          }
        }
        dateTime1 = meterReadingValue2.Date;
        int month3 = dateTime1.Month;
        dateTime1 = valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().Date;
        int month4 = dateTime1.Month;
        if (month3 != month4)
        {
          List<ReadingValuesForChartDTO> valuesForChartDtoList4 = valuesForChartDtoList1;
          ReadingValuesForChartDTO valuesForChartDto7 = new ReadingValuesForChartDTO();
          valuesForChartDto7.Color = realValueColor;
          valuesForChartDto7.IsEstimation = false;
          valuesForChartDto7.ValueId = meterReadingValue2.Value - valuesForChartDtoList1.Last<ReadingValuesForChartDTO>().LastValue;
          ReadingValuesForChartDTO valuesForChartDto8 = valuesForChartDto7;
          dateTime1 = meterReadingValue2.Date;
          string str5 = dateTime1.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture);
          dateTime1 = meterReadingValue2.Date;
          // ISSUE: variable of a boxed type
          __Boxed<int> year = (System.ValueType) dateTime1.Year;
          string str6 = str5 + " " + (object) year;
          valuesForChartDto8.Month = str6;
          valuesForChartDto7.Date = meterReadingValue2.Date;
          valuesForChartDto7.LastValue = meterReadingValue2.Value;
          ReadingValuesForChartDTO valuesForChartDto9 = valuesForChartDto7;
          valuesForChartDtoList4.Add(valuesForChartDto9);
        }
      }
      valuesForChartDtoList1.ForEach((Action<ReadingValuesForChartDTO>) (x => this.GetReadingValuesForCharts.Add(x)));
    }

    public int MonthDifference(DateTime lValue, DateTime rValue)
    {
      return lValue.Month - rValue.Month + 12 * (lValue.Year - rValue.Year);
    }

    private void AddEstimatedReadingValue(
      IList<MeterReadingValue> readingValuesForFirstDaysOfTheMonth,
      int rv,
      IList<MeterReadingValue> readingValuesForSelectedMeter,
      IList<ReadingValuesForChartDTO> readingValuesWithEstimations,
      int addedMonths,
      Brush estimationValueColor)
    {
      double estimation = this.CreateEstimation(readingValuesForFirstDaysOfTheMonth[rv].Date.AddMonths(addedMonths), readingValuesForSelectedMeter);
      IList<ReadingValuesForChartDTO> valuesForChartDtoList = readingValuesWithEstimations;
      ReadingValuesForChartDTO valuesForChartDto1 = new ReadingValuesForChartDTO();
      ReadingValuesForChartDTO valuesForChartDto2 = valuesForChartDto1;
      DateTime dateTime = readingValuesForFirstDaysOfTheMonth[rv].Date;
      dateTime = dateTime.AddMonths(addedMonths);
      string str1 = dateTime.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture);
      dateTime = readingValuesForFirstDaysOfTheMonth[rv].Date;
      dateTime = dateTime.AddMonths(addedMonths);
      // ISSUE: variable of a boxed type
      __Boxed<int> year = (System.ValueType) dateTime.Year;
      string str2 = str1 + " " + (object) year;
      valuesForChartDto2.Month = str2;
      valuesForChartDto1.ValueId = estimation - readingValuesWithEstimations.Last<ReadingValuesForChartDTO>().LastValue;
      valuesForChartDto1.Color = estimationValueColor;
      valuesForChartDto1.IsEstimation = true;
      valuesForChartDto1.Date = readingValuesForFirstDaysOfTheMonth[rv].Date.AddMonths(addedMonths);
      valuesForChartDto1.LastValue = estimation;
      ReadingValuesForChartDTO valuesForChartDto3 = valuesForChartDto1;
      valuesForChartDtoList.Add(valuesForChartDto3);
    }

    private static void AddRealReadingValue(
      List<ReadingValuesForChartDTO> readingValuesWithEstimations,
      List<MeterReadingValue> readingValuesForFirstDaysOfTheMonth,
      int rv,
      bool isCurrentValue,
      Brush realValueColor,
      Brush estimationValueColor)
    {
      if (isCurrentValue)
        readingValuesWithEstimations.Add(new ReadingValuesForChartDTO()
        {
          Month = readingValuesForFirstDaysOfTheMonth[rv].Date.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture) + " " + (object) readingValuesForFirstDaysOfTheMonth[rv].Date.Year,
          ValueId = readingValuesForFirstDaysOfTheMonth[rv].Value - readingValuesWithEstimations.Last<ReadingValuesForChartDTO>().LastValue,
          Color = readingValuesWithEstimations.Last<ReadingValuesForChartDTO>().IsEstimation ? estimationValueColor : realValueColor,
          IsEstimation = false,
          Date = readingValuesForFirstDaysOfTheMonth[rv].Date,
          LastValue = readingValuesForFirstDaysOfTheMonth[rv].Value
        });
      else
        readingValuesWithEstimations.Add(new ReadingValuesForChartDTO()
        {
          Month = readingValuesForFirstDaysOfTheMonth[rv + 1].Date.ToString("MMM", (IFormatProvider) CultureInfo.InvariantCulture) + " " + (object) readingValuesForFirstDaysOfTheMonth[rv + 1].Date.Year,
          ValueId = readingValuesForFirstDaysOfTheMonth[rv + 1].Value - readingValuesWithEstimations.Last<ReadingValuesForChartDTO>().LastValue,
          Color = readingValuesWithEstimations.Last<ReadingValuesForChartDTO>().IsEstimation ? estimationValueColor : realValueColor,
          IsEstimation = false,
          Date = readingValuesForFirstDaysOfTheMonth[rv + 1].Date,
          LastValue = readingValuesForFirstDaysOfTheMonth[rv + 1].Value
        });
    }

    private double CreateEstimation(
      DateTime target,
      IList<MeterReadingValue> readingValuesForSelectedMeter)
    {
      if (readingValuesForSelectedMeter.Any<MeterReadingValue>())
      {
        List<DateTime> list = readingValuesForSelectedMeter.Select<MeterReadingValue, DateTime>((Func<MeterReadingValue, DateTime>) (x => x.Date)).ToList<DateTime>();
        int index = list.ToList<DateTime>().BinarySearch(target);
        if (index < 0)
          index = ~index;
        if (index >= list.Count)
          --index;
        if (index != 0)
        {
          DateTime date1 = readingValuesForSelectedMeter[index].Date;
          DateTime date2 = date1.Date;
          date1 = readingValuesForSelectedMeter[index - 1].Date;
          DateTime date3 = date1.Date;
          TimeSpan timeSpan = date2 - date3;
          int days = timeSpan.Days;
          double num = readingValuesForSelectedMeter[index].Value - readingValuesForSelectedMeter[index - 1].Value;
          timeSpan = target - readingValuesForSelectedMeter[index - 1].Date;
          return (double) timeSpan.Days / (double) days * num + readingValuesForSelectedMeter[index - 1].Value;
        }
      }
      return 0.0;
    }

    public RadObservableCollection<ReadingValuesForChartDTO> GetReadingValuesForCharts
    {
      get => this._getReadingValuesForCharts;
      set
      {
        this._getReadingValuesForCharts = value;
        this.OnPropertyChanged(nameof (GetReadingValuesForCharts));
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

    public double MaxValue
    {
      get => this._maxValue;
      set
      {
        this._maxValue = value;
        this.OnPropertyChanged(nameof (MaxValue));
      }
    }
  }
}
