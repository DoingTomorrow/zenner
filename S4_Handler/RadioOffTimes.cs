// Decompiled with JetBrains decompiler
// Type: S4_Handler.RadioOffTimes
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace S4_Handler
{
  public class RadioOffTimes : Window, IComponentConnector
  {
    private RadioOffTimeManagement TimeManagement;
    internal StackPanel StackPanelButtons;
    internal Button ButtonTypeParameters;
    internal Button ButtonStartSimulation;
    internal StackPanel StackPanalWeek;
    internal CheckBox CheckBoxMonday;
    internal CheckBox CheckBoxTuesday;
    internal CheckBox CheckBoxWednesday;
    internal CheckBox CheckBoxThursday;
    internal CheckBox CheckBoxFriday;
    internal CheckBox CheckBoxSaturday;
    internal CheckBox CheckBoxSunday;
    internal TextBox TextBoxDailyStartTime;
    internal TextBox TextBoxDailyStopTime;
    internal StackPanel StackPanalYear;
    internal CheckBox CheckBoxJanuary;
    internal CheckBox CheckBoxFebruary;
    internal CheckBox CheckBoxMarch;
    internal CheckBox CheckBoxApril;
    internal CheckBox CheckBoxMay;
    internal CheckBox CheckBoxJune;
    internal CheckBox CheckBoxJuly;
    internal CheckBox CheckBoxAugust;
    internal CheckBox CheckBoxSeptember;
    internal CheckBox CheckBoxOctober;
    internal CheckBox CheckBoxNovember;
    internal CheckBox CheckBoxDecember;
    internal TextBox TextBoxMonthlyStartDay;
    internal TextBox TextBoxMonthlyStopDay;
    internal StackPanel StackPanalSimulation;
    internal TextBox TextBoxSimStartTime;
    internal TextBox TextBoxSimEndTime;
    internal TextBox TextBoxOut;
    private bool _contentLoaded;

    public RadioOffTimes()
    {
      this.InitializeComponent();
      this.SetTimeManagement();
    }

    public RadioOffTimes(RadioOffTimeManagement timeManagement)
      : this()
    {
      this.InitializeComponent();
      this.TimeManagement = timeManagement;
      this.SetDayBits(this.TimeManagement.DaySelection);
      this.TextBoxDailyStartTime.Text = this.TimeManagement.DailyOnHour.ToString();
      this.TextBoxDailyStopTime.Text = this.TimeManagement.DailyOffHour.ToString();
      this.SetMonthBits(this.TimeManagement.MonthSelection);
      this.TextBoxMonthlyStartDay.Text = this.TimeManagement.MonthlyOnDay.ToString();
      this.TextBoxMonthlyStopDay.Text = this.TimeManagement.MonthlyOffDay.ToString();
    }

    private void SetTimeManagement()
    {
      this.TimeManagement = new RadioOffTimeManagement(this.GetDayBits(), this.GetDailyOnHour(), this.GetDailyOffHour(), this.GetMonthBits(), this.GetMonthlyOnDay(), this.GetMonthlyOffDay());
    }

    private RadioOffTimeManagement.Day GetDayBits()
    {
      int dayBits = 0;
      int num = 1;
      for (int index = 0; index < 7; ++index)
      {
        if (((ToggleButton) this.StackPanalWeek.Children[index]).IsChecked.Value)
          dayBits |= num;
        num <<= 1;
      }
      return (RadioOffTimeManagement.Day) dayBits;
    }

    private void SetDayBits(RadioOffTimeManagement.Day daySetup)
    {
      int num = 1;
      for (int index = 0; index < 7; ++index)
      {
        ((ToggleButton) this.StackPanalWeek.Children[index]).IsChecked = new bool?((daySetup & (RadioOffTimeManagement.Day) num) > (RadioOffTimeManagement.Day) 0);
        num <<= 1;
      }
    }

    private byte GetDailyOnHour()
    {
      byte result;
      if (byte.TryParse(this.TextBoxDailyStartTime.Text, out result))
        return result;
      throw new Exception("Illegal DailyOnHour");
    }

    private byte GetDailyOffHour()
    {
      byte result;
      if (byte.TryParse(this.TextBoxDailyStopTime.Text, out result))
        return result;
      throw new Exception("Illegal DailyOffHour");
    }

    private byte GetMonthlyOnDay()
    {
      byte result;
      if (byte.TryParse(this.TextBoxMonthlyStartDay.Text, out result))
        return result;
      throw new Exception("Illegal MonthlyOnDay");
    }

    private byte GetMonthlyOffDay()
    {
      byte result;
      if (byte.TryParse(this.TextBoxMonthlyStopDay.Text, out result))
        return result;
      throw new Exception("Illegal MonthlyOffDay");
    }

    private RadioOffTimeManagement.Month GetMonthBits()
    {
      int monthBits = 0;
      int num = 1;
      for (int index = 0; index < 12; ++index)
      {
        if (((ToggleButton) this.StackPanalYear.Children[index]).IsChecked.Value)
          monthBits |= num;
        num <<= 1;
      }
      return (RadioOffTimeManagement.Month) monthBits;
    }

    private void SetMonthBits(RadioOffTimeManagement.Month yearSetup)
    {
      int num = 1;
      for (int index = 0; index < 12; ++index)
      {
        ((ToggleButton) this.StackPanalYear.Children[index]).IsChecked = new bool?((yearSetup & (RadioOffTimeManagement.Month) num) > (RadioOffTimeManagement.Month) 0);
        num <<= 1;
      }
    }

    private void ButtonTypeParameters_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetTimeManagement();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("DaySetup: 0x" + ((byte) this.TimeManagement.DaySelection).ToString("x02"));
        stringBuilder.AppendLine("DailyOnHour: " + this.TimeManagement.DailyOnHour.ToString());
        stringBuilder.AppendLine("DailyOffHour: " + this.TimeManagement.DailyOffHour.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("YearSetup: 0x" + ((ushort) this.TimeManagement.MonthSelection).ToString("x04"));
        stringBuilder.AppendLine("MonthlyOnDay: " + ((ushort) this.TimeManagement.MonthlyOnDay).ToString());
        stringBuilder.AppendLine("MonthlyOffDay: " + ((ushort) this.TimeManagement.MonthlyOffDay).ToString());
        this.TextBoxOut.Text = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonStartSimulation_Click(object sender, RoutedEventArgs e)
    {
      this.SetTimeManagement();
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        DateTime result1;
        if (!DateTime.TryParse(this.TextBoxSimStartTime.Text, out result1))
          throw new Exception("Illegal simulation start time");
        DateTime result2;
        if (!DateTime.TryParse(this.TextBoxSimEndTime.Text, out result2))
          throw new Exception("Illegal simulation end time");
        DateTime dateTime = result1;
        System.Globalization.Calendar calendar = new CultureInfo("de").Calendar;
        int num1 = -1;
        int num2 = -1;
        while (dateTime <= result2)
        {
          dateTime.Month.ToString();
          dateTime = this.TimeManagement.GetNextOnTime(dateTime);
          if (num1 != dateTime.Month)
          {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("***  " + dateTime.ToString("MMMM") + "  ***");
            stringBuilder.AppendLine();
            num1 = dateTime.Month;
          }
          else
          {
            int weekOfYear = calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            if (num2 != weekOfYear)
            {
              if (num2 > 0)
                stringBuilder.AppendLine();
              num2 = weekOfYear;
            }
          }
          stringBuilder.Append("on from: " + dateTime.ToString("ddd dd.MM") + " " + dateTime.ToShortTimeString());
          dateTime = this.TimeManagement.GetNextOffTime(dateTime);
          stringBuilder.AppendLine(" to: " + dateTime.ToString("ddd dd.MM") + " " + dateTime.ToShortTimeString());
        }
        this.TextBoxOut.Text = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        this.TextBoxOut.Text = stringBuilder.ToString();
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/radioofftimes.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 2:
          this.ButtonTypeParameters = (Button) target;
          this.ButtonTypeParameters.Click += new RoutedEventHandler(this.ButtonTypeParameters_Click);
          break;
        case 3:
          this.ButtonStartSimulation = (Button) target;
          this.ButtonStartSimulation.Click += new RoutedEventHandler(this.ButtonStartSimulation_Click);
          break;
        case 4:
          this.StackPanalWeek = (StackPanel) target;
          break;
        case 5:
          this.CheckBoxMonday = (CheckBox) target;
          break;
        case 6:
          this.CheckBoxTuesday = (CheckBox) target;
          break;
        case 7:
          this.CheckBoxWednesday = (CheckBox) target;
          break;
        case 8:
          this.CheckBoxThursday = (CheckBox) target;
          break;
        case 9:
          this.CheckBoxFriday = (CheckBox) target;
          break;
        case 10:
          this.CheckBoxSaturday = (CheckBox) target;
          break;
        case 11:
          this.CheckBoxSunday = (CheckBox) target;
          break;
        case 12:
          this.TextBoxDailyStartTime = (TextBox) target;
          break;
        case 13:
          this.TextBoxDailyStopTime = (TextBox) target;
          break;
        case 14:
          this.StackPanalYear = (StackPanel) target;
          break;
        case 15:
          this.CheckBoxJanuary = (CheckBox) target;
          break;
        case 16:
          this.CheckBoxFebruary = (CheckBox) target;
          break;
        case 17:
          this.CheckBoxMarch = (CheckBox) target;
          break;
        case 18:
          this.CheckBoxApril = (CheckBox) target;
          break;
        case 19:
          this.CheckBoxMay = (CheckBox) target;
          break;
        case 20:
          this.CheckBoxJune = (CheckBox) target;
          break;
        case 21:
          this.CheckBoxJuly = (CheckBox) target;
          break;
        case 22:
          this.CheckBoxAugust = (CheckBox) target;
          break;
        case 23:
          this.CheckBoxSeptember = (CheckBox) target;
          break;
        case 24:
          this.CheckBoxOctober = (CheckBox) target;
          break;
        case 25:
          this.CheckBoxNovember = (CheckBox) target;
          break;
        case 26:
          this.CheckBoxDecember = (CheckBox) target;
          break;
        case 27:
          this.TextBoxMonthlyStartDay = (TextBox) target;
          break;
        case 28:
          this.TextBoxMonthlyStopDay = (TextBox) target;
          break;
        case 29:
          this.StackPanalSimulation = (StackPanel) target;
          break;
        case 30:
          this.TextBoxSimStartTime = (TextBox) target;
          break;
        case 31:
          this.TextBoxSimEndTime = (TextBox) target;
          break;
        case 32:
          this.TextBoxOut = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
