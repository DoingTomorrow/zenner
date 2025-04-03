// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.SystemSelectionViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Linq;
using System.Windows.Input;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class SystemSelectionViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private int _dueDateDays;
    private int _dueDateHours;
    private int _dueDateMinutes;
    private int _dueDateSeconds;
    private int _monthDays;
    private int _monthHours;
    private int _monthMinutes;
    private int _monthSeconds;
    private int _dayDays;
    private int _dayHours;
    private int _dayMinutes;
    private int _daySeconds;
    private int _quarterHourDays;
    private int _quarterHourHours;
    private int _quarterHourMinutes;
    private int _quarterHourSeconds;

    public SystemSelectionViewModel(
      IRepositoryFactory repositoryFactory,
      DeviceModel selectedSystem,
      bool isUpdate,
      JobDefinitionDto jobDto,
      ChangeableParametersSystem currentParams)
    {
      this._repositoryFactory = repositoryFactory;
      if (!isUpdate)
      {
        ChangeableParameter changeableParameter1 = selectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDueDate"));
        ChangeableParameter changeableParameter2 = selectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationMonth"));
        ChangeableParameter changeableParameter3 = selectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDay"));
        ChangeableParameter changeableParameter4 = selectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationQuarterHour"));
        if (changeableParameter1 == null || changeableParameter2 == null || changeableParameter3 == null || changeableParameter4 == null)
          return;
        this.SetValues(changeableParameter1.Value, changeableParameter2.Value, changeableParameter3.Value, changeableParameter4.Value);
      }
      else
        this.SetValues(currentParams.DueDate.ToString(), currentParams.Month.ToString(), currentParams.Day.ToString(), currentParams.QuarterHour.ToString());
    }

    private void SetValues(
      string dueDateTimeSpan,
      string monthTimeSpan,
      string dayTimeSpan,
      string quarterHourTimeSpan)
    {
      if (dueDateTimeSpan != null)
      {
        string[] strArray = dueDateTimeSpan.Split(':', '.');
        if (strArray.Length == 4)
        {
          this.DueDateDay = int.Parse(strArray[0]);
          this.DueDateHours = int.Parse(strArray[1]);
          this.DueDateMinutes = int.Parse(strArray[2]);
          this.DueDateSeconds = int.Parse(strArray[3]);
        }
        else
        {
          this.DueDateDay = 0;
          this.DueDateHours = int.Parse(strArray[0]);
          this.DueDateMinutes = int.Parse(strArray[1]);
          this.DueDateSeconds = int.Parse(strArray[2]);
        }
      }
      if (monthTimeSpan != null)
      {
        string[] strArray = monthTimeSpan.Split(':', '.');
        if (strArray.Length == 4)
        {
          this.MonthDay = int.Parse(strArray[0]);
          this.MonthHours = int.Parse(strArray[1]);
          this.MonthMinutes = int.Parse(strArray[2]);
          this.MonthSeconds = int.Parse(strArray[3]);
        }
        else
        {
          this.MonthDay = 0;
          this.MonthHours = int.Parse(strArray[0]);
          this.MonthMinutes = int.Parse(strArray[1]);
          this.MonthSeconds = int.Parse(strArray[2]);
        }
      }
      if (dayTimeSpan != null)
      {
        string[] strArray = dayTimeSpan.Split(':', '.');
        if (strArray.Length == 4)
        {
          this.DayDay = int.Parse(strArray[0]);
          this.DayHours = int.Parse(strArray[1]);
          this.DayMinutes = int.Parse(strArray[2]);
          this.DaySeconds = int.Parse(strArray[3]);
        }
        else
        {
          this.DayDay = 0;
          this.DayHours = int.Parse(strArray[0]);
          this.DayMinutes = int.Parse(strArray[1]);
          this.DaySeconds = int.Parse(strArray[2]);
        }
      }
      if (quarterHourTimeSpan == null)
        return;
      string[] strArray1 = quarterHourTimeSpan.Split(':', '.');
      if (strArray1.Length == 4)
      {
        this.QuarterHourDay = int.Parse(strArray1[0]);
        this.QuarterHourHours = int.Parse(strArray1[1]);
        this.QuarterHourMinutes = int.Parse(strArray1[2]);
        this.QuarterHourSeconds = int.Parse(strArray1[3]);
      }
      else
      {
        this.QuarterHourDay = 0;
        this.QuarterHourHours = int.Parse(strArray1[0]);
        this.QuarterHourMinutes = int.Parse(strArray1[1]);
        this.QuarterHourSeconds = int.Parse(strArray1[2]);
      }
    }

    public ICommand SaveSelectedSystemParametersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (this.DueDateDay < 0)
          {
            this.DueDateHours = -this.DueDateHours;
            this.DueDateMinutes = -this.DueDateMinutes;
            this.DueDateSeconds = -this.DueDateSeconds;
          }
          if (this.DayDay < 0)
          {
            this.DayHours = -this.DayHours;
            this.DayMinutes = -this.DayMinutes;
            this.DaySeconds = -this.DaySeconds;
          }
          if (this.MonthDay < 0)
          {
            this.MonthHours = -this.MonthHours;
            this.MonthMinutes = -this.MonthMinutes;
            this.MonthSeconds = -this.MonthSeconds;
          }
          if (this.QuarterHourDay < 0)
          {
            this.QuarterHourHours = -this.QuarterHourHours;
            this.QuarterHourMinutes = -this.QuarterHourMinutes;
            this.QuarterHourSeconds = -this.QuarterHourSeconds;
          }
          ChangeableParametersSystem parametersSystem = new ChangeableParametersSystem()
          {
            DueDate = new TimeSpan?(new TimeSpan(this.DueDateDay, this.DueDateHours, this.MonthMinutes, this.DueDateSeconds)),
            Day = new TimeSpan?(new TimeSpan(this.DayDay, this.DayHours, this.DayMinutes, this.DaySeconds)),
            Month = new TimeSpan?(new TimeSpan(this.MonthDay, this.MonthHours, this.MonthMinutes, this.MonthSeconds)),
            QuarterHour = new TimeSpan?(new TimeSpan(this.QuarterHourDay, this.QuarterHourHours, this.QuarterHourMinutes, this.QuarterHourSeconds))
          };
          EventPublisher.Publish<SetSystemChangeableParamsEvent>(new SetSystemChangeableParamsEvent()
          {
            DueDate = parametersSystem.DueDate,
            Day = parametersSystem.Day,
            Month = parametersSystem.Month,
            QuarterHour = parametersSystem.QuarterHour
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    public int DueDateDay
    {
      get => this._dueDateDays;
      set
      {
        this._dueDateDays = value;
        this.OnPropertyChanged(nameof (DueDateDay));
      }
    }

    public int DueDateHours
    {
      get => this._dueDateHours;
      set
      {
        this._dueDateHours = value;
        this.OnPropertyChanged(nameof (DueDateHours));
      }
    }

    public int DueDateMinutes
    {
      get => this._dueDateMinutes;
      set
      {
        this._dueDateMinutes = value;
        this.OnPropertyChanged(nameof (DueDateMinutes));
      }
    }

    public int DueDateSeconds
    {
      get => this._dueDateSeconds;
      set
      {
        this._dueDateSeconds = value;
        this.OnPropertyChanged(nameof (DueDateSeconds));
      }
    }

    public int MonthDay
    {
      get => this._monthDays;
      set
      {
        this._monthDays = value;
        this.OnPropertyChanged(nameof (MonthDay));
      }
    }

    public int MonthHours
    {
      get => this._monthHours;
      set
      {
        this._monthHours = value;
        this.OnPropertyChanged(nameof (MonthHours));
      }
    }

    public int MonthMinutes
    {
      get => this._monthMinutes;
      set
      {
        this._monthMinutes = value;
        this.OnPropertyChanged(nameof (MonthMinutes));
      }
    }

    public int MonthSeconds
    {
      get => this._monthSeconds;
      set
      {
        this._monthSeconds = value;
        this.OnPropertyChanged(nameof (MonthSeconds));
      }
    }

    public int DayDay
    {
      get => this._dayDays;
      set
      {
        this._dayDays = value;
        this.OnPropertyChanged(nameof (DayDay));
      }
    }

    public int DayHours
    {
      get => this._dayHours;
      set
      {
        this._dayHours = value;
        this.OnPropertyChanged(nameof (DayHours));
      }
    }

    public int DayMinutes
    {
      get => this._dayMinutes;
      set
      {
        this._dayMinutes = value;
        this.OnPropertyChanged(nameof (DayMinutes));
      }
    }

    public int DaySeconds
    {
      get => this._daySeconds;
      set
      {
        this._daySeconds = value;
        this.OnPropertyChanged(nameof (DaySeconds));
      }
    }

    public int QuarterHourDay
    {
      get => this._quarterHourDays;
      set
      {
        this._quarterHourDays = value;
        this.OnPropertyChanged(nameof (QuarterHourDay));
      }
    }

    public int QuarterHourHours
    {
      get => this._quarterHourHours;
      set
      {
        this._quarterHourHours = value;
        this.OnPropertyChanged(nameof (QuarterHourHours));
      }
    }

    public int QuarterHourMinutes
    {
      get => this._quarterHourMinutes;
      set
      {
        this._quarterHourMinutes = value;
        this.OnPropertyChanged(nameof (QuarterHourMinutes));
      }
    }

    public int QuarterHourSeconds
    {
      get => this._quarterHourSeconds;
      set
      {
        this._quarterHourSeconds = value;
        this.OnPropertyChanged(nameof (QuarterHourSeconds));
      }
    }
  }
}
