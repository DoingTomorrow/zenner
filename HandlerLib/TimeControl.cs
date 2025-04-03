// Decompiled with JetBrains decompiler
// Type: HandlerLib.TimeControl
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class TimeControl : UserControl, IComponentConnector
  {
    internal TextBox txtHours;
    internal TextBox txtMinutes;
    internal TextBox txtSeconds;
    private bool _contentLoaded;

    public TimeControl() => this.InitializeComponent();

    public DateTime? DateTimeValue
    {
      get
      {
        DateTime result;
        return DateTime.TryParse(string.Format("{0:00}:{1:00}:{2:00}", (object) this.txtHours.Text, (object) this.txtMinutes.Text, (object) this.txtSeconds.Text), out result) ? new DateTime?(result) : new DateTime?();
      }
      set
      {
        DateTime? nullable = value;
        if (!nullable.HasValue)
          return;
        this.txtHours.Text = nullable.Value.Hour.ToString("00");
        this.txtMinutes.Text = nullable.Value.Minute.ToString("00");
        this.txtSeconds.Text = nullable.Value.Second.ToString("00");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/util/timecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtHours = (TextBox) target;
          break;
        case 2:
          this.txtMinutes = (TextBox) target;
          break;
        case 3:
          this.txtSeconds = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
