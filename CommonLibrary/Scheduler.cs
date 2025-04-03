// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Scheduler
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Timers;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public sealed class Scheduler : IDisposable
  {
    private List<Scheduler.TriggerItem> _triggerItems;
    private Timer _triggerTimer;
    private static Logger logger = LogManager.GetLogger(nameof (Scheduler));

    public double TriggerTimerInterval
    {
      get => this._triggerTimer.Interval;
      set => this._triggerTimer.Interval = value;
    }

    public Scheduler()
      : this(1000.0)
    {
    }

    public Scheduler(double interval)
    {
      this._triggerItems = new List<Scheduler.TriggerItem>();
      this._triggerTimer = new Timer();
      this._triggerTimer.Interval = interval;
      this._triggerTimer.Elapsed += new ElapsedEventHandler(this._triggerTimer_Elapsed);
    }

    public List<Scheduler.TriggerItem> TriggerItems => this._triggerItems;

    public void AddTrigger(Scheduler.TriggerItem item)
    {
      this._triggerItems.Add(item);
      if (this._triggerItems.Count != 1)
        return;
      this.Start();
    }

    public void RemoveTrigger(Scheduler.TriggerItem item)
    {
      Scheduler.logger.Info("[GMM] RemoveTrigger: " + item?.ToString());
      this._triggerItems.Remove(item);
      if (this._triggerItems.Count != 0)
        return;
      this.Stop();
    }

    private void Start()
    {
      Scheduler.logger.Info("[GMM] triggerTimer.Start");
      this._triggerTimer.Start();
    }

    private void Stop()
    {
      Scheduler.logger.Info("[GMM] triggerTimer.Stop");
      this._triggerTimer.Stop();
    }

    private void _triggerTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      this._triggerTimer.Stop();
      if (this.TriggerItems.Count == 0)
        return;
      foreach (Scheduler.TriggerItem triggerItem in this.TriggerItems)
      {
        if (triggerItem.Enabled)
        {
          while (triggerItem.TriggerTime <= DateTime.Now)
            triggerItem.RunCheck(DateTime.Now);
        }
      }
      this._triggerTimer.Start();
    }

    public void Dispose()
    {
      Scheduler.logger.Info("[GMM] Dispose _triggerTimer");
      this._triggerTimer.Stop();
      this._triggerTimer.Dispose();
      this._triggerItems.Clear();
    }

    public enum DayOccurrence
    {
      First,
      Second,
      Third,
      Fourth,
      Last,
    }

    public enum MonthOfTheYeay
    {
      January,
      February,
      March,
      April,
      May,
      June,
      July,
      August,
      September,
      October,
      November,
      December,
    }

    public sealed class TriggerSettingsOneTimeOnly
    {
      public DateTime? Date { get; set; }
    }

    public sealed class TriggerSettingsFixedInterval
    {
      public TimeSpan Interval { get; set; }

      public TriggerSettingsFixedInterval() => this.Interval = TimeSpan.Zero;
    }

    public sealed class TriggerSettingsDaily
    {
      public ushort Interval { get; set; }
    }

    public sealed class TriggerSettingsWeekly
    {
      public bool[] DaysOfWeek { get; set; }

      public TriggerSettingsWeekly() => this.DaysOfWeek = new bool[7];
    }

    public sealed class TriggerSettingsMonthlyWeekDay
    {
      public bool[] WeekNumber { get; set; }

      public bool[] DayOfWeek { get; set; }

      public TriggerSettingsMonthlyWeekDay()
      {
        this.WeekNumber = new bool[5];
        this.DayOfWeek = new bool[7];
      }
    }

    public sealed class TriggerSettingsMonthly
    {
      public bool[] Month { get; set; }

      public bool[] DaysOfMonth { get; set; }

      public Scheduler.TriggerSettingsMonthlyWeekDay WeekDay { get; set; }

      public TriggerSettingsMonthly()
      {
        this.Month = new bool[12];
        this.DaysOfMonth = new bool[32];
        this.WeekDay = new Scheduler.TriggerSettingsMonthlyWeekDay();
      }
    }

    public sealed class TriggerSettings
    {
      public Scheduler.TriggerSettingsOneTimeOnly OneTimeOnly { get; set; }

      public Scheduler.TriggerSettingsFixedInterval FixedInterval { get; set; }

      public Scheduler.TriggerSettingsDaily Daily { get; set; }

      public Scheduler.TriggerSettingsWeekly Weekly { get; set; }

      public Scheduler.TriggerSettingsMonthly Monthly { get; set; }

      public TriggerSettings()
      {
        this.OneTimeOnly = new Scheduler.TriggerSettingsOneTimeOnly();
        this.FixedInterval = new Scheduler.TriggerSettingsFixedInterval();
        this.Daily = new Scheduler.TriggerSettingsDaily();
        this.Weekly = new Scheduler.TriggerSettingsWeekly();
        this.Monthly = new Scheduler.TriggerSettingsMonthly();
      }
    }

    public sealed class OnTriggerEventArgs : EventArgs
    {
      public OnTriggerEventArgs(Scheduler.TriggerItem item, DateTime triggerDate)
      {
        this.Item = item;
        this.TriggerDate = triggerDate;
      }

      public Scheduler.TriggerItem Item { get; private set; }

      public DateTime TriggerDate { get; private set; }
    }

    public sealed class TriggerItem
    {
      private static Logger logger = LogManager.GetLogger(nameof (TriggerItem));
      private const byte LastDayOfMonthID = 31;
      private DateTime startDate = DateTime.MinValue;
      private DateTime endDate = DateTime.MaxValue;
      private DateTime triggerTime;
      private DateTime? lastTriggerTime;
      private bool enabled;

      public event Scheduler.TriggerItem.OnTriggerEventHandler OnTrigger;

      public TriggerItem() => this.TriggerSettings = new Scheduler.TriggerSettings();

      public Scheduler.TriggerSettings TriggerSettings { get; set; }

      public object Tag { get; set; }

      public bool Enabled
      {
        get => this.enabled;
        set
        {
          this.enabled = value;
          if (this.enabled)
          {
            if (this.TriggerSettings.FixedInterval.Interval != TimeSpan.Zero)
            {
              this.startDate = new DateTime(this.startDate.Year, this.startDate.Month, this.startDate.Day, this.triggerTime.Hour, this.triggerTime.Minute, this.triggerTime.Second);
              if (this.startDate.Add(this.TriggerSettings.FixedInterval.Interval) < DateTime.Now)
              {
                while (this.startDate.Add(this.TriggerSettings.FixedInterval.Interval) < DateTime.Now)
                  this.startDate = this.startDate.Add(this.TriggerSettings.FixedInterval.Interval);
                this.startDate = this.startDate.Add(this.TriggerSettings.FixedInterval.Interval);
              }
            }
            else if (!this.TriggerSettings.OneTimeOnly.Date.HasValue && this.startDate.AddDays(1.0) < DateTime.Now)
            {
              while (this.startDate.AddDays(1.0) < DateTime.Now)
                this.startDate = this.startDate.AddDays(1.0);
            }
            this.triggerTime = this.FindNextTriggerDate(this.startDate);
          }
          else
            this.triggerTime = new DateTime(1, 1, 1, this.triggerTime.Hour, this.triggerTime.Minute, this.triggerTime.Second);
        }
      }

      public DateTime StartDate
      {
        get => this.startDate;
        set
        {
          this.startDate = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
          if (!(this.endDate < this.startDate))
            return;
          this.endDate = this.startDate;
        }
      }

      public DateTime EndDate
      {
        get => this.endDate;
        set => this.endDate = value.Date;
      }

      public DateTime TriggerTime
      {
        get => this.triggerTime;
        set
        {
          this.triggerTime = new DateTime(this.triggerTime.Year, this.triggerTime.Month, this.triggerTime.Day, value.Hour, value.Minute, value.Second);
        }
      }

      private DateTime LastDayOfMonth(DateTime date)
      {
        DateTime dateTime = new DateTime(date.Year, date.Month, 1);
        dateTime = dateTime.AddMonths(1);
        return dateTime.AddDays(-1.0);
      }

      private int WeekDayOccurInMonth(DateTime date)
      {
        byte num = 0;
        for (int day = 1; day <= date.Day; ++day)
        {
          if (new DateTime(date.Year, date.Month, day).DayOfWeek == date.DayOfWeek)
            ++num;
        }
        return (int) num - 1;
      }

      private bool IsLastWeekDayInMonth(DateTime date)
      {
        return this.WeekDayOccurInMonth(date.AddDays(7.0)) == 0;
      }

      private bool TriggerOneTimeOnly(DateTime date)
      {
        int num;
        if (this.TriggerSettings.OneTimeOnly.Date.HasValue)
        {
          DateTime? date1 = this.TriggerSettings.OneTimeOnly.Date;
          DateTime dateTime = date;
          num = date1.HasValue ? (date1.HasValue ? (date1.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0;
        }
        else
          num = 0;
        return num != 0;
      }

      private bool TriggerFixInterval(DateTime lastTriggerDate)
      {
        if (lastTriggerDate < this.startDate || lastTriggerDate > this.endDate || this.TriggerSettings.FixedInterval.Interval == TimeSpan.Zero)
          return false;
        DateTime dateTime = this.startDate;
        while (dateTime < lastTriggerDate)
          dateTime = dateTime.Add(this.TriggerSettings.FixedInterval.Interval);
        return dateTime == lastTriggerDate;
      }

      private bool TriggerDaily(DateTime date)
      {
        if (date < this.startDate.Date || date > this.endDate.Date || this.TriggerSettings.Daily.Interval == (ushort) 0)
          return false;
        DateTime dateTime = this.startDate.Date;
        while (dateTime.Date < date)
          dateTime = dateTime.AddDays((double) this.TriggerSettings.Daily.Interval);
        return dateTime == date;
      }

      private bool TriggerWeekly(DateTime date)
      {
        return !(date < this.startDate.Date) && !(date > this.endDate.Date) && this.TriggerSettings.Weekly.DaysOfWeek[(int) date.DayOfWeek];
      }

      private bool TriggerMonthly(DateTime date)
      {
        if (date < this.startDate.Date || date > this.endDate.Date)
          return false;
        bool flag = false;
        if (this.TriggerSettings.Monthly.Month[date.Month - 1])
        {
          if (this.TriggerSettings.Monthly.DaysOfMonth[31])
            flag = flag || date == this.LastDayOfMonth(date);
          flag = flag || this.TriggerSettings.Monthly.DaysOfMonth[date.Day - 1];
          if (this.TriggerSettings.Monthly.WeekDay.DayOfWeek[(int) date.DayOfWeek])
          {
            if (this.TriggerSettings.Monthly.WeekDay.WeekNumber[4])
              flag = flag || this.IsLastWeekDayInMonth(date);
            flag = flag || this.TriggerSettings.Monthly.WeekDay.WeekNumber[this.WeekDayOccurInMonth(date)];
          }
        }
        return flag;
      }

      public bool CheckDate(DateTime lastTriggerDate)
      {
        return this.TriggerOneTimeOnly(lastTriggerDate) || this.TriggerFixInterval(lastTriggerDate) || this.TriggerDaily(lastTriggerDate) || this.TriggerWeekly(lastTriggerDate) || this.TriggerMonthly(lastTriggerDate);
      }

      public bool RunCheck(DateTime dateTimeToCheck)
      {
        if (this.enabled && dateTimeToCheck >= this.triggerTime)
        {
          this.lastTriggerTime = new DateTime?(this.triggerTime);
          Scheduler.OnTriggerEventArgs e = new Scheduler.OnTriggerEventArgs(this, this.triggerTime);
          this.triggerTime = !(this.TriggerSettings.FixedInterval.Interval != TimeSpan.Zero) ? this.FindNextTriggerDate(this.triggerTime.AddDays(1.0)) : this.FindNextTriggerDate(this.triggerTime.Add(this.TriggerSettings.FixedInterval.Interval));
          if (this.OnTrigger != null)
            this.OnTrigger((object) this, e);
          Scheduler.TriggerItem.logger.Info("[GMM] RunCheck == true, triggerTime = " + this.triggerTime.ToString("G"));
          return true;
        }
        Scheduler.TriggerItem.logger.Info("[GMM] RunCheck == false, triggerTime = " + this.triggerTime.ToString("G"));
        return false;
      }

      public void RevertToLastTriggerTime()
      {
        if (!this.lastTriggerTime.HasValue)
          return;
        this.triggerTime = this.lastTriggerTime.Value;
        Scheduler.TriggerItem.logger.Info("[GMM] RevertToLastTriggerTime, triggerTime = " + this.triggerTime.ToString("G") + "lastTriggerTime: " + this.lastTriggerTime.ToString());
      }

      private DateTime FindNextTriggerDate(DateTime lastTriggerDate)
      {
        if (!this.enabled)
          return DateTime.MaxValue;
        if (this.TriggerSettings.FixedInterval.Interval != TimeSpan.Zero)
        {
          for (DateTime lastTriggerDate1 = lastTriggerDate; lastTriggerDate1 <= this.endDate; lastTriggerDate1 = lastTriggerDate1.Add(this.TriggerSettings.FixedInterval.Interval))
          {
            if (this.CheckDate(lastTriggerDate1))
              return lastTriggerDate1;
          }
          return DateTime.MaxValue;
        }
        for (DateTime dateTime = lastTriggerDate.Date; dateTime <= this.endDate; dateTime = dateTime.AddDays(1.0))
        {
          if (this.CheckDate(dateTime.Date))
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, this.triggerTime.Hour, this.triggerTime.Minute, this.triggerTime.Second);
        }
        return DateTime.MaxValue;
      }

      public DateTime GetNextTriggerTime() => this.enabled ? this.triggerTime : DateTime.MaxValue;

      public delegate void OnTriggerEventHandler(object sender, Scheduler.OnTriggerEventArgs e);
    }
  }
}
