// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.TaskScheduler
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Utils.Utils;
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

#nullable disable
namespace MSS.Business.Utils
{
  public class TaskScheduler
  {
    private TaskScheduler.TriggerItemCollection _triggerItems;
    private int _Interval = 500;
    private bool _Enabled = false;
    private Timer _triggerTimer;

    public TaskScheduler()
    {
      this._triggerItems = new TaskScheduler.TriggerItemCollection();
      this._triggerTimer = new Timer();
      this._triggerTimer.Tick += new EventHandler(this._triggerTimer_Tick);
    }

    public int Interval
    {
      get => this._Interval;
      set => this._Interval = value;
    }

    public bool Enabled
    {
      get => this._Enabled;
      set
      {
        this._Enabled = value;
        if (this._Enabled)
          this.Start();
        else
          this.Stop();
      }
    }

    public TaskScheduler.TriggerItem AddTrigger(TaskScheduler.TriggerItem item)
    {
      return this._triggerItems[this._triggerItems.Add(item)];
    }

    private void Start()
    {
      this._triggerTimer.Interval = this._Interval;
      this._triggerTimer.Start();
    }

    private void Stop() => this._triggerTimer.Stop();

    public TaskScheduler.TriggerItemCollection TriggerItems => this._triggerItems;

    private void _triggerTimer_Tick(object sender, EventArgs e)
    {
      this._triggerTimer.Stop();
      foreach (TaskScheduler.TriggerItem triggerItem in (CollectionBase) this.TriggerItems)
      {
        if (triggerItem.Enabled)
        {
          while (triggerItem.TriggerTime <= DateTime.Now)
            triggerItem.RunCheck(DateTime.Now);
        }
      }
      this._triggerTimer.Start();
    }

    public enum DayOccurrence
    {
      [StringEnum("MSS_INTERVALS_ORDINAL_FIRST")] First,
      [StringEnum("MSS_INTERVALS_ORDINAL_SECOND")] Second,
      [StringEnum("MSS_INTERVALS_ORDINAL_THIRD")] Third,
      [StringEnum("MSS_INTERVALS_ORDINAL_FOURTH")] Fourth,
      [StringEnum("MSS_INTERVALS_ORDINAL_LAST")] Last,
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

    public class TriggerSettingsOneTimeOnly
    {
      private DateTime _date;
      private bool _active;

      [XmlIgnore]
      public DateTime Date
      {
        get => this._date;
        set => this._date = value;
      }

      [XmlElement("Date")]
      public string XMLDate
      {
        get => this._date.ToString("yyyy-MM-dd HH:mm:ss");
        set
        {
          this.Date = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", (IFormatProvider) null).Date;
        }
      }

      public bool Active
      {
        get => this._active;
        set => this._active = value;
      }
    }

    public class TriggerSettingsDaily
    {
      private ushort _Interval;

      public ushort Interval
      {
        get => this._Interval;
        set
        {
          this._Interval = value;
          if (this._Interval >= (ushort) 0)
            return;
          this._Interval = (ushort) 0;
        }
      }
    }

    public class TriggerSettingsWeekly
    {
      private bool[] _DaysOfWeek;

      public bool[] DaysOfWeek
      {
        get => this._DaysOfWeek;
        set => this._DaysOfWeek = value;
      }

      public TriggerSettingsWeekly() => this._DaysOfWeek = new bool[7];
    }

    public class TriggerSettingsMonthlyWeekDay
    {
      private bool[] _WeekNumber;
      private bool[] _DayOfWeek;

      public bool[] WeekNumber
      {
        get => this._WeekNumber;
        set => this._WeekNumber = value;
      }

      public bool[] DayOfWeek
      {
        get => this._DayOfWeek;
        set => this._DayOfWeek = value;
      }

      public TriggerSettingsMonthlyWeekDay()
      {
        this._WeekNumber = new bool[5];
        this._DayOfWeek = new bool[7];
      }
    }

    public class TriggerSettingsMonthly
    {
      private bool[] _Month;
      private bool[] _DaysOfMonth;
      private TaskScheduler.TriggerSettingsMonthlyWeekDay _WeekDay;

      public bool[] Month
      {
        get => this._Month;
        set => this._Month = value;
      }

      public bool[] DaysOfMonth
      {
        get => this._DaysOfMonth;
        set => this._DaysOfMonth = value;
      }

      public TaskScheduler.TriggerSettingsMonthlyWeekDay WeekDay
      {
        get => this._WeekDay;
        set => this._WeekDay = value;
      }

      public TriggerSettingsMonthly()
      {
        this._Month = new bool[12];
        this._DaysOfMonth = new bool[32];
        this._WeekDay = new TaskScheduler.TriggerSettingsMonthlyWeekDay();
      }
    }

    public class TriggerSettings
    {
      private TaskScheduler.TriggerSettingsOneTimeOnly _OneTimeOnly;
      private TaskScheduler.TriggerSettingsDaily _Daily;
      private TaskScheduler.TriggerSettingsWeekly _Weekly;
      private TaskScheduler.TriggerSettingsMonthly _Monthly;

      public TaskScheduler.TriggerSettingsOneTimeOnly OneTimeOnly
      {
        get => this._OneTimeOnly;
        set => this._OneTimeOnly = value;
      }

      public TaskScheduler.TriggerSettingsDaily Daily
      {
        get => this._Daily;
        set => this._Daily = value;
      }

      public TaskScheduler.TriggerSettingsWeekly Weekly
      {
        get => this._Weekly;
        set => this._Weekly = value;
      }

      public TaskScheduler.TriggerSettingsMonthly Monthly
      {
        get => this._Monthly;
        set => this._Monthly = value;
      }

      public TriggerSettings()
      {
        this._OneTimeOnly = new TaskScheduler.TriggerSettingsOneTimeOnly();
        this._Daily = new TaskScheduler.TriggerSettingsDaily();
        this._Weekly = new TaskScheduler.TriggerSettingsWeekly();
        this._Monthly = new TaskScheduler.TriggerSettingsMonthly();
      }
    }

    public class OnTriggerEventArgs : EventArgs
    {
      private TaskScheduler.TriggerItem _item;
      private DateTime _triggerDate;

      public OnTriggerEventArgs(TaskScheduler.TriggerItem item, DateTime triggerDate)
      {
        this._item = item;
        this._triggerDate = triggerDate;
      }

      public TaskScheduler.TriggerItem Item => this._item;

      public DateTime TriggerDate => this._triggerDate;
    }

    public class TriggerItem
    {
      private DateTime _StartDate = DateTime.MinValue;
      private DateTime _EndDate = DateTime.MaxValue;
      private TaskScheduler.TriggerSettings _TriggerSettings;
      private DateTime _TriggerTime;
      private const byte LastDayOfMonthID = 31;
      private object _Tag;
      private bool _Enabled;

      public event TaskScheduler.TriggerItem.OnTriggerEventHandler OnTrigger;

      public TriggerItem() => this._TriggerSettings = new TaskScheduler.TriggerSettings();

      public string ToXML()
      {
        bool enabled = this.Enabled;
        this.Enabled = false;
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (TaskScheduler.TriggerItem));
        TextWriter textWriter = (TextWriter) new StringWriter();
        xmlSerializer.Serialize(textWriter, (object) this);
        textWriter.Close();
        this.Enabled = enabled;
        return textWriter.ToString();
      }

      public static TaskScheduler.TriggerItem FromXML(string Configuration)
      {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof (TaskScheduler.TriggerItem));
        TextReader textReader = (TextReader) new StringReader(Configuration);
        TaskScheduler.TriggerItem triggerItem = (TaskScheduler.TriggerItem) xmlSerializer.Deserialize(textReader);
        textReader.Close();
        return triggerItem;
      }

      public object Tag
      {
        get => this._Tag;
        set => this._Tag = value;
      }

      public bool Enabled
      {
        get => this._Enabled;
        set
        {
          this._Enabled = value;
          if (this._Enabled)
            this._TriggerTime = this.FindNextTriggerDate(this._StartDate);
          else
            this._TriggerTime = new DateTime(1, 1, 1, this._TriggerTime.Hour, this._TriggerTime.Minute, this._TriggerTime.Second);
        }
      }

      [XmlIgnore]
      public DateTime StartDate
      {
        get => this._StartDate;
        set
        {
          this._StartDate = value;
          if (!(this._EndDate < this._StartDate))
            return;
          this._EndDate = this._StartDate;
        }
      }

      [XmlElement("StartDate")]
      public string XMLStartDate
      {
        get => this._StartDate.ToString("yyyy-MM-dd HH:mm:ss");
        set
        {
          this.StartDate = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", (IFormatProvider) null);
        }
      }

      [XmlIgnore]
      public DateTime EndDate
      {
        get => this._EndDate;
        set => this._EndDate = value.Date;
      }

      [XmlElement("EndDate")]
      public string XMLEndDate
      {
        get => this._EndDate.ToString("yyyy-MM-dd HH:mm:ss");
        set
        {
          this.EndDate = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", (IFormatProvider) null);
        }
      }

      [XmlIgnore]
      public DateTime TriggerTime
      {
        get => this._TriggerTime;
        set
        {
          this._TriggerTime = new DateTime(this._TriggerTime.Year, this._TriggerTime.Month, this._TriggerTime.Day, value.Hour, value.Minute, value.Second);
        }
      }

      [XmlElement("TriggerTime")]
      public string XMLTriggerTime
      {
        get => this.TriggerTime.ToString("HH:mm:ss");
        set => this.TriggerTime = DateTime.ParseExact(value, "HH:mm:ss", (IFormatProvider) null);
      }

      public TaskScheduler.TriggerSettings TriggerSettings
      {
        get => this._TriggerSettings;
        set => this._TriggerSettings = value;
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
        return this._TriggerSettings.OneTimeOnly.Active && this._TriggerSettings.OneTimeOnly.Date == date;
      }

      private bool TriggerDaily(DateTime date)
      {
        if (date < this._StartDate.Date || date > this._EndDate.Date || this._TriggerSettings.Daily.Interval == (ushort) 0)
          return false;
        DateTime dateTime = this._StartDate.Date;
        while (dateTime.Date < date)
          dateTime = dateTime.AddDays((double) this._TriggerSettings.Daily.Interval);
        return dateTime == date;
      }

      private bool TriggerWeekly(DateTime date)
      {
        return !(date < this._StartDate.Date) && !(date > this._EndDate.Date) && this._TriggerSettings.Weekly.DaysOfWeek[(int) date.DayOfWeek];
      }

      private bool TriggerMonthly(DateTime date)
      {
        if (date < this._StartDate.Date || date > this._EndDate.Date)
          return false;
        bool flag = false;
        if (this._TriggerSettings.Monthly.Month[date.Month - 1])
        {
          if (this._TriggerSettings.Monthly.DaysOfMonth[31])
            flag = flag || date == this.LastDayOfMonth(date);
          flag = flag || this._TriggerSettings.Monthly.DaysOfMonth[date.Day - 1];
          if (this._TriggerSettings.Monthly.WeekDay.DayOfWeek[(int) date.DayOfWeek])
          {
            if (this._TriggerSettings.Monthly.WeekDay.WeekNumber[4])
              flag = flag || this.IsLastWeekDayInMonth(date);
            flag = flag || this._TriggerSettings.Monthly.WeekDay.WeekNumber[this.WeekDayOccurInMonth(date)];
          }
        }
        return flag;
      }

      public bool CheckDate(DateTime date)
      {
        return this.TriggerOneTimeOnly(date) || this.TriggerDaily(date) || this.TriggerWeekly(date) || this.TriggerMonthly(date);
      }

      public bool RunCheck(DateTime dateTimeToCheck)
      {
        if (!this._Enabled || !(dateTimeToCheck >= this._TriggerTime))
          return false;
        TaskScheduler.OnTriggerEventArgs e = new TaskScheduler.OnTriggerEventArgs(this, this._TriggerTime);
        this._TriggerTime = this.FindNextTriggerDate(this._TriggerTime.AddDays(1.0));
        if (this.OnTrigger != null)
          this.OnTrigger((object) this, e);
        return true;
      }

      private DateTime FindNextTriggerDate(DateTime lastTriggerDate)
      {
        if (!this._Enabled)
          return DateTime.MaxValue;
        for (DateTime dateTime = lastTriggerDate.Date; dateTime <= this._EndDate; dateTime = dateTime.AddDays(1.0))
        {
          if (this.CheckDate(dateTime.Date))
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, this._TriggerTime.Hour, this._TriggerTime.Minute, this._TriggerTime.Second);
        }
        return DateTime.MaxValue;
      }

      public DateTime GetNextTriggerTime() => this._Enabled ? this._TriggerTime : DateTime.MaxValue;

      public delegate void OnTriggerEventHandler(object sender, TaskScheduler.OnTriggerEventArgs e);
    }

    public class TriggerItemCollection : CollectionBase
    {
      public TaskScheduler.TriggerItem this[int index]
      {
        get => (TaskScheduler.TriggerItem) this.List[index];
        set => this.List[index] = (object) value;
      }

      public int Add(TaskScheduler.TriggerItem value) => this.List.Add((object) value);

      public int IndexOf(TaskScheduler.TriggerItem value) => this.List.IndexOf((object) value);

      public void Insert(int index, TaskScheduler.TriggerItem value)
      {
        this.List.Insert(index, (object) value);
      }

      public void Remove(TaskScheduler.TriggerItem value) => this.List.Remove((object) value);

      public bool Contains(TaskScheduler.TriggerItem value) => this.List.Contains((object) value);

      protected override void OnInsert(int index, object value)
      {
      }

      protected override void OnRemove(int index, object value)
      {
      }

      protected override void OnSet(int index, object oldValue, object newValue)
      {
      }

      protected override void OnValidate(object value)
      {
        if (value.GetType() != typeof (TaskScheduler.TriggerItem))
          throw new ArgumentException("Das angegebene Argument ist kein TaskScheduler-Element", nameof (value));
      }
    }
  }
}
