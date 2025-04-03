// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.IntervalsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Jobs;
using MSS.Core.Utils;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class IntervalsViewModel : ValidationViewModelBase
  {
    private string _timeHours;
    private int _minuteHour;
    private int _secondHour;
    private List<CardinalDayObject> _selectedMonthDaysIntValues = new List<CardinalDayObject>();
    private List<DayObject> _selectedMonthlyWeekDays = new List<DayObject>();
    private List<OrdinalDayObject> _selectedOrdinalDays = new List<OrdinalDayObject>();
    private List<MonthObject> _selectedMonths = new List<MonthObject>();
    private bool _isOneTime;
    private bool _isFixedInterval;
    private bool _isDaily;
    private bool _isWeekly;
    private bool _isMonthly;
    private bool _isDayOfTheMonth;
    private bool _isWeekDay;
    private List<DayObject> _weekDaysList = new List<DayObject>();
    private int _repeatIn;
    private DateTime? _oneTimeDate;
    private DateTime? _startDateTime;
    private DateTime? _endDateTime;
    private DateTime? _selectedTime;
    private string _repeatInterval;
    private List<DayObject> _weekdays;
    private List<MonthObject> _months;
    private List<DayObject> _monthlyWeekDays;
    private List<OrdinalDayObject> _ordinalDay;
    private List<CardinalDayObject> _monthDaysIntValues;
    private ViewModelBase _messageUserControl;

    public IntervalsViewModel(byte[] interval)
    {
      this.WeekDays = new List<DayObject>();
      this.Months = new List<MonthObject>();
      this.MonthlyWeekDays = new List<DayObject>();
      this.OrdinalDay = new List<OrdinalDayObject>();
      this.MonthDaysIntValues = new List<CardinalDayObject>();
      if (DateTimeFormatInfo.CurrentInfo != null)
      {
        string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
        string[] dayNames = DateTimeFormatInfo.CurrentInfo.DayNames;
        Array values = Enum.GetValues(typeof (TaskScheduler.DayOccurrence));
        for (int index = 0; index < monthNames.Length; ++index)
          this.Months.Add(new MonthObject()
          {
            Id = index,
            NameOfTheMonth = monthNames[index]
          });
        for (int index = 0; index < dayNames.Length; ++index)
          this.WeekDays.Add(new DayObject()
          {
            Id = index,
            NameOfTheDay = dayNames[index]
          });
        for (int index = 1; index < dayNames.Length; ++index)
          this.MonthlyWeekDays.Add(new DayObject()
          {
            Id = index,
            NameOfTheDay = dayNames[index]
          });
        for (int index = 0; index < values.Length; ++index)
          this.OrdinalDay.Add(new OrdinalDayObject()
          {
            Id = index,
            OrdinalValue = ((TaskScheduler.DayOccurrence) values.GetValue(index)).GetStringValue()
          });
      }
      int num;
      for (int index = 0; index < 31; num = index++)
      {
        List<CardinalDayObject> monthDaysIntValues = this.MonthDaysIntValues;
        CardinalDayObject cardinalDayObject1 = new CardinalDayObject();
        cardinalDayObject1.Id = index;
        CardinalDayObject cardinalDayObject2 = cardinalDayObject1;
        num = index + 1;
        string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        cardinalDayObject2.CardinalValue = str;
        CardinalDayObject cardinalDayObject3 = cardinalDayObject1;
        monthDaysIntValues.Add(cardinalDayObject3);
      }
      this.MonthDaysIntValues.Add(new CardinalDayObject()
      {
        Id = 31,
        CardinalValue = "Last"
      });
      if (interval == null)
      {
        this.IsOneTimeOnly = true;
        this.IsDayOfTheMonth = true;
        this.TimeHours = "00";
        this.TimeMinutes = 0;
        this.TimeSeconds = 0;
        this.RepeatInterval = "000000";
        this.StartDateTime = new DateTime?(DateTime.Now);
        this.EndDateTime = new DateTime?(new DateTime(2040, 1, 1));
        this.SelectedTime = new DateTime?(DateTime.Now);
      }
      else
      {
        IntervalDto deserializedInterval = JobsManager.DeserializeIntervals(interval);
        this.IsOneTimeOnly = deserializedInterval.IntervalType[IntervalTypeEnum.OnlyOneTime];
        this.IsDaily = deserializedInterval.IntervalType[IntervalTypeEnum.Daily];
        this.IsMonthly = deserializedInterval.IntervalType[IntervalTypeEnum.Monthly];
        this.IsWeekly = deserializedInterval.IntervalType[IntervalTypeEnum.Weekly];
        this.IsFixedInterval = deserializedInterval.IntervalType[IntervalTypeEnum.FixedInterval];
        this.IsWeekDay = deserializedInterval.MonthlyType[MonthlyTypeEnum.IsWeekDay];
        this.IsDayOfTheMonth = deserializedInterval.MonthlyType[MonthlyTypeEnum.IsDayOfTheMonth];
        this.SelectedMonths = this.Months.Where<MonthObject>((Func<MonthObject, bool>) (x => deserializedInterval.MonthlyMonth.Contains(x.Id))).ToList<MonthObject>();
        this.SelectedWeekDaysList = this.WeekDays.Where<DayObject>((Func<DayObject, bool>) (x => deserializedInterval.WeeklyDays.Contains(x.Id))).ToList<DayObject>();
        this.SelectedMonthlyWeekDays = this.MonthlyWeekDays.Where<DayObject>((Func<DayObject, bool>) (x => deserializedInterval.MonthlyWeekDay.Contains(x.Id))).ToList<DayObject>();
        this.SelectedOrdinalDays = this.OrdinalDay.Where<OrdinalDayObject>((Func<OrdinalDayObject, bool>) (x => deserializedInterval.MonthlyOrdinalDay.Contains(x.Id))).ToList<OrdinalDayObject>();
        this.SelectedMonthDaysIntValues = this.MonthDaysIntValues.Where<CardinalDayObject>((Func<CardinalDayObject, bool>) (x => deserializedInterval.MonthlyCardinalDay.Contains(x.Id))).ToList<CardinalDayObject>();
        this.EndDateTime = deserializedInterval.EndDate;
        this.StartDateTime = deserializedInterval.StartDate;
        this.SelectedTime = deserializedInterval.Time;
        this.RepeatIn = deserializedInterval.RepeatIn;
        this.OneTimeDate = deserializedInterval.OneTimeDate;
        this.RepeatInterval = deserializedInterval.FixedInterval.HasValue ? string.Format("{0:HHmmss}", (object) deserializedInterval.FixedInterval) : "000000";
      }
    }

    public string TimeHours
    {
      get => this._timeHours;
      set
      {
        this._timeHours = value;
        this.OnPropertyChanged(nameof (TimeHours));
      }
    }

    public int TimeMinutes
    {
      get => this._minuteHour;
      set
      {
        this._minuteHour = value;
        this.OnPropertyChanged(nameof (TimeMinutes));
      }
    }

    public int TimeSeconds
    {
      get => this._secondHour;
      set
      {
        this._secondHour = value;
        this.OnPropertyChanged(nameof (TimeSeconds));
      }
    }

    public List<CardinalDayObject> SelectedMonthDaysIntValues
    {
      get => this._selectedMonthDaysIntValues;
      set
      {
        this._selectedMonthDaysIntValues = value;
        this.OnPropertyChanged(nameof (SelectedMonthDaysIntValues));
      }
    }

    public List<DayObject> SelectedMonthlyWeekDays
    {
      get => this._selectedMonthlyWeekDays;
      set
      {
        this._selectedMonthlyWeekDays = value;
        this.OnPropertyChanged(nameof (SelectedMonthlyWeekDays));
      }
    }

    public List<OrdinalDayObject> SelectedOrdinalDays
    {
      get => this._selectedOrdinalDays;
      set
      {
        this._selectedOrdinalDays = value;
        this.OnPropertyChanged(nameof (SelectedOrdinalDays));
      }
    }

    public List<MonthObject> SelectedMonths
    {
      get => this._selectedMonths;
      set
      {
        this._selectedMonths = value;
        this.OnPropertyChanged(nameof (SelectedMonths));
      }
    }

    public bool IsOneTimeOnly
    {
      get => this._isOneTime;
      set
      {
        this._isOneTime = value;
        this.OnPropertyChanged(nameof (IsOneTimeOnly));
      }
    }

    public bool IsFixedInterval
    {
      get => this._isFixedInterval;
      set
      {
        this._isFixedInterval = value;
        this.OnPropertyChanged(nameof (IsFixedInterval));
      }
    }

    public bool IsDaily
    {
      get => this._isDaily;
      set
      {
        this._isDaily = value;
        this.OnPropertyChanged(nameof (IsDaily));
      }
    }

    public bool IsWeekly
    {
      get => this._isWeekly;
      set
      {
        this._isWeekly = value;
        this.OnPropertyChanged(nameof (IsWeekly));
      }
    }

    public bool IsMonthly
    {
      get => this._isMonthly;
      set
      {
        this._isMonthly = value;
        this.OnPropertyChanged(nameof (IsMonthly));
      }
    }

    public bool IsDayOfTheMonth
    {
      get => this._isDayOfTheMonth;
      set
      {
        this._isDayOfTheMonth = value;
        this.OnPropertyChanged(nameof (IsDayOfTheMonth));
      }
    }

    public bool IsWeekDay
    {
      get => this._isWeekDay;
      set
      {
        this._isWeekDay = value;
        this.OnPropertyChanged(nameof (IsWeekDay));
      }
    }

    public List<DayObject> SelectedWeekDaysList
    {
      get => this._weekDaysList;
      set
      {
        this._weekDaysList = value;
        this.OnPropertyChanged(nameof (SelectedWeekDaysList));
      }
    }

    public int RepeatIn
    {
      get => this._repeatIn;
      set
      {
        this._repeatIn = value;
        this.OnPropertyChanged(nameof (RepeatIn));
      }
    }

    public DateTime? OneTimeDate
    {
      get => this._oneTimeDate;
      set
      {
        this._oneTimeDate = value;
        this.OnPropertyChanged(nameof (OneTimeDate));
      }
    }

    [Required(ErrorMessage = "MSS_INTERVALS_REQUIRED_START_DATE")]
    public DateTime? StartDateTime
    {
      get => this._startDateTime;
      set
      {
        this._startDateTime = value;
        this.OnPropertyChanged(nameof (StartDateTime));
      }
    }

    [Required(ErrorMessage = "MSS_INTERVALS_REQUIRED_END_DATE")]
    public DateTime? EndDateTime
    {
      get => this._endDateTime;
      set
      {
        this._endDateTime = value;
        this.OnPropertyChanged(nameof (EndDateTime));
      }
    }

    [Required(ErrorMessage = "MSS_INTERVALS_REQUIRED_TIME")]
    public DateTime? SelectedTime
    {
      get => this._selectedTime;
      set
      {
        this._selectedTime = value;
        this.OnPropertyChanged(nameof (SelectedTime));
      }
    }

    public string RepeatInterval
    {
      get => this._repeatInterval;
      set
      {
        this._repeatInterval = value;
        this.OnPropertyChanged(nameof (RepeatInterval));
      }
    }

    public List<DayObject> WeekDays
    {
      get => this._weekdays;
      set
      {
        this._weekdays = value;
        this.OnPropertyChanged(nameof (WeekDays));
      }
    }

    public List<MonthObject> Months
    {
      get => this._months;
      set
      {
        this._months = value;
        this.OnPropertyChanged(nameof (Months));
      }
    }

    public List<DayObject> MonthlyWeekDays
    {
      get => this._monthlyWeekDays;
      set
      {
        this._monthlyWeekDays = value;
        this.OnPropertyChanged(nameof (MonthlyWeekDays));
      }
    }

    public List<OrdinalDayObject> OrdinalDay
    {
      get => this._ordinalDay;
      set
      {
        this._ordinalDay = value;
        this.OnPropertyChanged(nameof (OrdinalDay));
      }
    }

    public List<CardinalDayObject> MonthDaysIntValues
    {
      get => this._monthDaysIntValues;
      set
      {
        this._monthDaysIntValues = value;
        this.OnPropertyChanged(nameof (MonthDaysIntValues));
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

    public ICommand SaveInterval
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsValid)
            return;
          IntervalDto intervalDto1 = new IntervalDto();
          intervalDto1.StartDate = this.StartDateTime;
          intervalDto1.EndDate = this.EndDateTime;
          intervalDto1.Time = this.SelectedTime;
          intervalDto1.OneTimeDate = this.IsOneTimeOnly ? this.OneTimeDate : new DateTime?();
          intervalDto1.RepeatIn = this.IsDaily ? this.RepeatIn : 0;
          intervalDto1.WeeklyDays = this.IsWeekly ? this.SelectedWeekDaysList.Select<DayObject, int>((Func<DayObject, int>) (x => x.Id)).ToList<int>() : (List<int>) null;
          IntervalDto intervalDto2 = intervalDto1;
          SerializableDictionary<IntervalTypeEnum, bool> serializableDictionary1 = new SerializableDictionary<IntervalTypeEnum, bool>()
          {
            {
              IntervalTypeEnum.OnlyOneTime,
              this.IsOneTimeOnly
            },
            {
              IntervalTypeEnum.Daily,
              this.IsDaily
            },
            {
              IntervalTypeEnum.Weekly,
              this.IsWeekly
            },
            {
              IntervalTypeEnum.Monthly,
              this.IsMonthly
            },
            {
              IntervalTypeEnum.FixedInterval,
              this.IsFixedInterval
            }
          };
          intervalDto2.IntervalType = serializableDictionary1;
          intervalDto1.MonthlyMonth = this.IsMonthly ? this.SelectedMonths.Select<MonthObject, int>((Func<MonthObject, int>) (x => x.Id)).ToList<int>() : (List<int>) null;
          intervalDto1.MonthlyOrdinalDay = !this.IsMonthly || !this.IsWeekDay ? (List<int>) null : this.SelectedOrdinalDays.Select<OrdinalDayObject, int>((Func<OrdinalDayObject, int>) (x => x.Id)).ToList<int>();
          intervalDto1.MonthlyWeekDay = !this.IsMonthly || !this.IsWeekDay ? (List<int>) null : this.SelectedMonthlyWeekDays.Select<DayObject, int>((Func<DayObject, int>) (x => x.Id)).ToList<int>();
          intervalDto1.MonthlyCardinalDay = !this.IsMonthly || !this.IsDayOfTheMonth ? (List<int>) null : this.SelectedMonthDaysIntValues.Select<CardinalDayObject, int>((Func<CardinalDayObject, int>) (x => x.Id)).ToList<int>();
          IntervalDto intervalDto3 = intervalDto1;
          SerializableDictionary<MonthlyTypeEnum, bool> serializableDictionary2 = new SerializableDictionary<MonthlyTypeEnum, bool>()
          {
            {
              MonthlyTypeEnum.IsDayOfTheMonth,
              this.IsDayOfTheMonth
            },
            {
              MonthlyTypeEnum.IsWeekDay,
              this.IsWeekDay
            }
          };
          intervalDto3.MonthlyType = serializableDictionary2;
          IntervalDto interval = intervalDto1;
          if (this.IsOneTimeOnly && !this.OneTimeDate.HasValue)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_DATE);
          }
          else
          {
            int hour1;
            int minute1;
            int second1;
            if (this.IsFixedInterval)
            {
              if (string.IsNullOrEmpty(this.RepeatInterval) || this.RepeatInterval.Length != 6)
              {
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_FIXED_INTERVAL);
                return;
              }
              this.Hour(out hour1, out minute1, out second1);
              if (hour1 > 23 || minute1 > 59 || second1 > 59)
              {
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_FIXED_INTERVAL);
                return;
              }
              if ("000000".Equals(this.RepeatInterval))
              {
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_INVALID_INTERVAL);
                return;
              }
            }
            if (this.IsDaily && this.RepeatIn == 0)
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_DAILY);
            else if (this.IsWeekly && !this.SelectedWeekDaysList.Any<DayObject>())
            {
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_WEEKLY);
            }
            else
            {
              if (this.IsMonthly)
              {
                if (!this.SelectedMonths.Any<MonthObject>())
                {
                  this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_MONTHLY);
                  return;
                }
                if (this.IsWeekDay)
                {
                  if (!this.SelectedOrdinalDays.Any<OrdinalDayObject>())
                  {
                    this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_DATE);
                    return;
                  }
                  if (!this.SelectedMonthlyWeekDays.Any<DayObject>())
                  {
                    this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_DATE);
                    return;
                  }
                }
                if (this.IsDayOfTheMonth && !this.SelectedMonthDaysIntValues.Any<CardinalDayObject>())
                {
                  this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_INTERVALS_REQUIRED_DATE);
                  return;
                }
              }
              this.Hour(out hour1, out minute1, out second1);
              DateTime dateTime;
              ref DateTime local = ref dateTime;
              DateTime now = DateTime.Now;
              int year = now.Year;
              now = DateTime.Now;
              int month = now.Month;
              now = DateTime.Now;
              int day = now.Day;
              int hour2 = hour1;
              int minute2 = minute1;
              int second2 = second1;
              local = new DateTime(year, month, day, hour2, minute2, second2);
              interval.FixedInterval = this.IsFixedInterval ? new DateTime?(dateTime) : new DateTime?();
              EventPublisher.Publish<SendSerializedDataEvent>(new SendSerializedDataEvent()
              {
                SerializedObject = JobsManager.SerializeIntervals(interval)
              }, (IViewModel) this);
              this.OnRequestClose(true);
            }
          }
        });
      }
    }

    private void Hour(out int hour, out int minute, out int second)
    {
      hour = int.Parse(this.RepeatInterval.Substring(0, 2));
      minute = int.Parse(this.RepeatInterval.Substring(2, 2));
      second = int.Parse(this.RepeatInterval.Substring(4, 2));
    }
  }
}
