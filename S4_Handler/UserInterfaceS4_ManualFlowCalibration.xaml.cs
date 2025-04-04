// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_ManualFlowCalibration
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_ManualFlowCalibration : Window, IComponentConnector
  {
    private S4_HandlerFunctions MyFunctions;
    internal Button ButtonCalibration;
    internal DataGrid DataGridErrorList;
    private bool _contentLoaded;

    public List<FlowCalibrationPoint> CalibrationPoints { get; set; }

    public S4_ManualFlowCalibration(S4_HandlerFunctions myFunctions)
    {
      this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
      this.MyFunctions = myFunctions;
      this.InitializeComponent();
      this.CalibrationPoints = new List<FlowCalibrationPoint>();
      this.DataGridErrorList.ItemsSource = (IEnumerable) this.CalibrationPoints;
    }

    private void ButtonCalibration_Click(object sender, RoutedEventArgs e)
    {
      this.MyFunctions.FlowCalibration(this.CalibrationPoints);
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_manualflowcalibration.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonCalibration = (Button) target;
          this.ButtonCalibration.Click += new RoutedEventHandler(this.ButtonCalibration_Click);
          break;
        case 2:
          this.DataGridErrorList = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
