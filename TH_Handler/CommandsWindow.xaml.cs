// Decompiled with JetBrains decompiler
// Type: TH_Handler.CommandsWindow
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using GmmDbLib;
using HandlerLib;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public partial class CommandsWindow : Window, IComponentConnector
  {
    private TH CMD;
    internal Button btnOOK;
    internal ComboBox txtRadioMode;
    internal TextBox txtFreqOffset;
    internal TextBox txtTimeout;
    internal Button btnPN9;
    internal Button btnResetToDelivery;
    internal Button btnReadVersion;
    internal Button btnTactileSwitch;
    internal Button btnReadDateTime;
    internal Button btnWriteDateTime;
    internal Button btnCurrentDateTime;
    internal DatePicker txtDate;
    internal TimeControl txtTime;
    internal Button btnResetDevice;
    internal Button btnLCDDisable;
    internal Button btnLCDEnable;
    internal Button btnLCDTest;
    internal ComboBox txtLCDTest;
    internal Button btnRadioDisable;
    internal Button btnRadioTransmit;
    internal Button btnReadSerial;
    internal Button btnReadTemperature;
    internal Button btnReadHumidity;
    internal Button btnSleep;
    internal Button btnWakeUp;
    internal Button btnRadioEnable;
    internal Button btnSaveConfig;
    internal Button btnSND_NKE;
    private bool _contentLoaded;

    private CommandsWindow() => this.InitializeComponent();

    public static void ShowDialog(Window owner, TH cmd)
    {
      if (cmd == null)
        return;
      try
      {
        CommandsWindow commandsWindow = new CommandsWindow();
        commandsWindow.Owner = owner;
        commandsWindow.CMD = cmd;
        commandsWindow.ShowDialog();
      }
      catch (Exception ex)
      {
        string caption = Ot.Gtt(Tg.Handler_UI, "Error", "Error");
        int num = (int) MessageBox.Show(owner, ex.Message, caption, MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    private void btnCurrentDateTime_Click(object sender, RoutedEventArgs e)
    {
      DateTime now = DateTime.Now;
      this.txtDate.SelectedDate = new DateTime?(now);
      this.txtTime.DateTimeValue = new DateTime?(now);
    }

    private void btnOOK_Click(object sender, RoutedEventArgs e)
    {
      RadioMode radioMode = (RadioMode) Enum.Parse(typeof (RadioMode), this.txtRadioMode.Text);
      short offset = Convert.ToInt16(this.txtFreqOffset.Text);
      ushort timeout = Convert.ToUInt16(this.txtTimeout.Text);
      this.Perform<bool>((Func<bool>) (() => this.CMD.RadioOOK(radioMode, offset, timeout)));
    }

    private void btnPN9_Click(object sender, RoutedEventArgs e)
    {
      RadioMode radioMode = (RadioMode) Enum.Parse(typeof (RadioMode), this.txtRadioMode.Text);
      short offset = Convert.ToInt16(this.txtFreqOffset.Text);
      ushort timeout = Convert.ToUInt16(this.txtTimeout.Text);
      this.Perform<bool>((Func<bool>) (() => this.CMD.RadioPN9(radioMode, offset, timeout)));
    }

    private void btnResetToDelivery_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.ResetToDelivery));
    }

    private void btnReadVersion_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<TH_Version>(new Func<TH_Version>(this.CMD.ReadVersion));
    }

    private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.SaveConfig));
    }

    private void btnResetDevice_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.ResetDevice));
    }

    private void btnTactileSwitch_Click(object sender, RoutedEventArgs e)
    {
      int num = (int) this.Perform<TactileSwitchState>(new Func<TactileSwitchState>(this.CMD.TactileSwitch));
    }

    private void btnLCDDisable_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.LCDDisable));
    }

    private void btnLCDEnable_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.LCDEnable));
    }

    private void btnRadioDisable_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.RadioDisable));
    }

    private void btnRadioEnable_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.RadioEnable));
    }

    private void btnReadSerial_Click(object sender, RoutedEventArgs e)
    {
      int num = (int) this.Perform<uint>(new Func<uint>(this.CMD.ReadSerial));
    }

    private void btnRadioTransmit_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.RadioTransmit));
    }

    private void btnReadTemperature_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<double>(new Func<double>(this.CMD.ReadTemperature));
    }

    private void btnReadHumidity_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<double>(new Func<double>(this.CMD.ReadHumidity));
    }

    private void btnSleep_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.Sleep));
    }

    private void btnWakeUp_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.WakeUp));
    }

    private void btnSND_NKE_Click(object sender, RoutedEventArgs e)
    {
      this.Perform<bool>(new Func<bool>(this.CMD.SND_NKE));
    }

    private void btnLCDTest_Click(object sender, RoutedEventArgs e)
    {
      LcdTest lcdTest = (LcdTest) Enum.Parse(typeof (LcdTest), this.txtLCDTest.Text);
      this.Perform<bool>((Func<bool>) (() => this.CMD.LCDTest(lcdTest)));
    }

    private void btnReadDateTime_Click(object sender, RoutedEventArgs e)
    {
      DateTime dateTime = this.Perform<DateTime>(new Func<DateTime>(this.CMD.ReadDateTime));
      this.txtDate.SelectedDate = new DateTime?(dateTime);
      this.txtTime.DateTimeValue = new DateTime?(dateTime);
    }

    private void btnWriteDateTime_Click(object sender, RoutedEventArgs e)
    {
      DateTime? nullable;
      int num;
      if (this.txtDate.SelectedDate.HasValue)
      {
        nullable = this.txtTime.DateTimeValue;
        num = !nullable.HasValue ? 1 : 0;
      }
      else
        num = 1;
      if (num != 0)
        return;
      nullable = this.txtDate.SelectedDate;
      DateTime dateTime1 = nullable.Value;
      nullable = this.txtTime.DateTimeValue;
      DateTime dateTime2 = nullable.Value;
      DateTime dateTime = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, dateTime2.Hour, dateTime2.Minute, dateTime2.Second);
      this.Perform<bool>((Func<bool>) (() => this.CMD.WriteDateTime(dateTime)));
    }

    private T Perform<T>(Func<T> action)
    {
      string name = action.Method.Name;
      try
      {
        object obj1 = (object) action();
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__0.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__0, obj1, (object) null);
        if (target1((CallSite) p1, obj2))
        {
          int num = (int) MessageBox.Show((Window) this, Ot.Gtm(Tg.Handler_UI, "NoResult", "No result"), name, MessageBoxButton.OK, MessageBoxImage.Exclamation);
          return (T) null;
        }
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__2.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__2, obj1);
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p4 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Type, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__3.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__3, obj3, typeof (bool));
        if (target2((CallSite) p4, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (CommandsWindow)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__5.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__5, obj1))
          {
            int num1 = (int) MessageBox.Show((Window) this, Ot.Gtt(Tg.Handler_UI, "Successful", "Successful"), name, MessageBoxButton.OK, MessageBoxImage.Asterisk);
          }
          else
          {
            int num2 = (int) MessageBox.Show((Window) this, Ot.Gtm(Tg.Handler_UI, "Failed", "Failed"), name, MessageBoxButton.OK, MessageBoxImage.Exclamation);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__7 = CallSite<Action<CallSite, Type, CommandsWindow, object, string, MessageBoxButton, MessageBoxImage>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Show", (IEnumerable<Type>) null, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, Type, CommandsWindow, object, string, MessageBoxButton, MessageBoxImage> target3 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, Type, CommandsWindow, object, string, MessageBoxButton, MessageBoxImage>> p7 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__7;
          Type type = typeof (MessageBox);
          // ISSUE: reference to a compiler-generated field
          if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (CommandsWindow), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__6.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__6, obj1);
          string str = name;
          target3((CallSite) p7, type, this, obj5, str, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        // ISSUE: reference to a compiler-generated field
        if (CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, T>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (T), typeof (CommandsWindow)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__8.Target((CallSite) CommandsWindow.\u003C\u003Eo__25<T>.\u003C\u003Ep__8, obj1);
      }
      catch (AggregateException ex)
      {
        string caption = Ot.Gtt(Tg.Handler_UI, "Error", "Error");
        int num = (int) MessageBox.Show((Window) this, name + " " + ex.InnerException.Message, caption, MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      catch (Exception ex)
      {
        string caption = Ot.Gtt(Tg.Handler_UI, "Error", "Error");
        int num = (int) MessageBox.Show((Window) this, name + " " + ex.Message, caption, MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      return default (T);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/TH_Handler;component/commandswindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.btnOOK = (Button) target;
          this.btnOOK.Click += new RoutedEventHandler(this.btnOOK_Click);
          break;
        case 2:
          this.txtRadioMode = (ComboBox) target;
          break;
        case 3:
          this.txtFreqOffset = (TextBox) target;
          break;
        case 4:
          this.txtTimeout = (TextBox) target;
          break;
        case 5:
          this.btnPN9 = (Button) target;
          this.btnPN9.Click += new RoutedEventHandler(this.btnPN9_Click);
          break;
        case 6:
          this.btnResetToDelivery = (Button) target;
          this.btnResetToDelivery.Click += new RoutedEventHandler(this.btnResetToDelivery_Click);
          break;
        case 7:
          this.btnReadVersion = (Button) target;
          this.btnReadVersion.Click += new RoutedEventHandler(this.btnReadVersion_Click);
          break;
        case 8:
          this.btnTactileSwitch = (Button) target;
          this.btnTactileSwitch.Click += new RoutedEventHandler(this.btnTactileSwitch_Click);
          break;
        case 9:
          this.btnReadDateTime = (Button) target;
          this.btnReadDateTime.Click += new RoutedEventHandler(this.btnReadDateTime_Click);
          break;
        case 10:
          this.btnWriteDateTime = (Button) target;
          this.btnWriteDateTime.Click += new RoutedEventHandler(this.btnWriteDateTime_Click);
          break;
        case 11:
          this.btnCurrentDateTime = (Button) target;
          this.btnCurrentDateTime.Click += new RoutedEventHandler(this.btnCurrentDateTime_Click);
          break;
        case 12:
          this.txtDate = (DatePicker) target;
          break;
        case 13:
          this.txtTime = (TimeControl) target;
          break;
        case 14:
          this.btnResetDevice = (Button) target;
          this.btnResetDevice.Click += new RoutedEventHandler(this.btnResetDevice_Click);
          break;
        case 15:
          this.btnLCDDisable = (Button) target;
          this.btnLCDDisable.Click += new RoutedEventHandler(this.btnLCDDisable_Click);
          break;
        case 16:
          this.btnLCDEnable = (Button) target;
          this.btnLCDEnable.Click += new RoutedEventHandler(this.btnLCDEnable_Click);
          break;
        case 17:
          this.btnLCDTest = (Button) target;
          this.btnLCDTest.Click += new RoutedEventHandler(this.btnLCDTest_Click);
          break;
        case 18:
          this.txtLCDTest = (ComboBox) target;
          break;
        case 19:
          this.btnRadioDisable = (Button) target;
          this.btnRadioDisable.Click += new RoutedEventHandler(this.btnRadioDisable_Click);
          break;
        case 20:
          this.btnRadioTransmit = (Button) target;
          this.btnRadioTransmit.Click += new RoutedEventHandler(this.btnRadioTransmit_Click);
          break;
        case 21:
          this.btnReadSerial = (Button) target;
          this.btnReadSerial.Click += new RoutedEventHandler(this.btnReadSerial_Click);
          break;
        case 22:
          this.btnReadTemperature = (Button) target;
          this.btnReadTemperature.Click += new RoutedEventHandler(this.btnReadTemperature_Click);
          break;
        case 23:
          this.btnReadHumidity = (Button) target;
          this.btnReadHumidity.Click += new RoutedEventHandler(this.btnReadHumidity_Click);
          break;
        case 24:
          this.btnSleep = (Button) target;
          this.btnSleep.Click += new RoutedEventHandler(this.btnSleep_Click);
          break;
        case 25:
          this.btnWakeUp = (Button) target;
          this.btnWakeUp.Click += new RoutedEventHandler(this.btnWakeUp_Click);
          break;
        case 26:
          this.btnRadioEnable = (Button) target;
          this.btnRadioEnable.Click += new RoutedEventHandler(this.btnRadioEnable_Click);
          break;
        case 27:
          this.btnSaveConfig = (Button) target;
          this.btnSaveConfig.Click += new RoutedEventHandler(this.btnSaveConfig_Click);
          break;
        case 28:
          this.btnSND_NKE = (Button) target;
          this.btnSND_NKE.Click += new RoutedEventHandler(this.btnSND_NKE_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
