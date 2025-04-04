// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_InfoWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace S4_Handler
{
  public class S4_InfoWindow : Window, IComponentConnector
  {
    internal GmmCorporateControl gmmCorporateControl1;
    internal GroupBox GroupBoxWork;
    internal TextBox TextBoxApplicationInfo;
    internal Button ButtonClose;
    private bool _contentLoaded;

    public S4_InfoWindow()
    {
      this.InitializeComponent();
      this.SetInformationText();
    }

    private void SetInformationText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string location = Assembly.GetExecutingAssembly().Location;
        string productVersion1 = FileVersionInfo.GetVersionInfo("HandlerLib.dll").ProductVersion;
        string productVersion2 = FileVersionInfo.GetVersionInfo("MBus_Handler.dll").ProductVersion;
        string productVersion3 = FileVersionInfo.GetVersionInfo("CommunicationPort.dll").ProductVersion;
        stringBuilder.AppendLine("Information about this handler and components");
        stringBuilder.AppendLine("------------------------------------------------------------------");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("HandlerLib version: " + productVersion1);
        stringBuilder.AppendLine("MBusHandler version: " + productVersion2);
        stringBuilder.AppendLine("CommunicationPort version: " + productVersion3);
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Info of implemented functions in HandlerLib.dll:");
        stringBuilder.AppendLine("ProtocolSpecification: " + HandlerLib.LibraryInfo.MBusProtocolSpecification);
        stringBuilder.AppendLine("Protocol Date of implementation: " + HandlerLib.LibraryInfo.MBusProtocolDateOfImplementation);
        stringBuilder.AppendLine("Implemented function codes: " + HandlerLib.LibraryInfo.ImplementedFunctionCodes);
        stringBuilder.AppendLine("Implemented extended function codes: " + HandlerLib.LibraryInfo.ImplementedExtendedFunctionCodes);
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("Critical error occured:");
        stringBuilder.AppendLine("Please check if the installation is OK.");
        stringBuilder.AppendLine("-> " + ex.Message);
      }
      this.TextBoxApplicationInfo.Text = stringBuilder.ToString();
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e) => this.Close();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_infowindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.GroupBoxWork = (GroupBox) target;
          break;
        case 3:
          this.TextBoxApplicationInfo = (TextBox) target;
          break;
        case 4:
          this.ButtonClose = (Button) target;
          this.ButtonClose.Click += new RoutedEventHandler(this.ButtonClose_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
