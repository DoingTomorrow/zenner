// Decompiled with JetBrains decompiler
// Type: HandlerLib.InfoWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class InfoWindow : Window, IComponentConnector
  {
    internal TextBox TextBoxApplicationInfo;
    private bool _contentLoaded;

    public InfoWindow() => this.InitializeComponent();

    public static void ShowDialog(Window owner, string[,] additionalInfo = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        AssemblyName name = Assembly.GetCallingAssembly().GetName();
        string str1 = name.Name + ".dll";
        string str2 = File.Exists("HandlerLib.dll") ? FileVersionInfo.GetVersionInfo("HandlerLib.dll").ProductVersion : "n/a";
        string str3 = File.Exists("MBus_Handler.dll") ? FileVersionInfo.GetVersionInfo("MBus_Handler.dll").ProductVersion : "n/a";
        string str4 = File.Exists("MBusLib.dll") ? FileVersionInfo.GetVersionInfo("MBusLib.dll").ProductVersion : "n/a";
        string str5 = File.Exists("CommunicationPort.dll") ? FileVersionInfo.GetVersionInfo("CommunicationPort.dll").ProductVersion : "n/a";
        string str6 = File.Exists(str1) ? FileVersionInfo.GetVersionInfo(str1).FileVersion : "n/a";
        stringBuilder.AppendLine("Information about this handler and components");
        stringBuilder.AppendLine("------------------------------------------------------------------");
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("Application path: " + directoryName);
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("HandlerLib version: " + str2);
        stringBuilder.AppendLine("MBusHandler version: " + str3);
        stringBuilder.AppendLine("MBusLib version: " + str4);
        stringBuilder.AppendLine("CommunicationPort version: " + str5);
        stringBuilder.AppendLine(name.Name + " version: " + str6);
        stringBuilder.AppendLine("");
        stringBuilder.AppendLine("");
        if (!str2.Contains("n/a"))
        {
          stringBuilder.AppendLine("Info of implemented functions in HandlerLib.dll:");
          stringBuilder.AppendLine("ProtocolSpecification: " + LibraryInfo.MBusProtocolSpecification);
          stringBuilder.AppendLine("Protocol Date of implementation: " + LibraryInfo.MBusProtocolDateOfImplementation);
          stringBuilder.AppendLine("Implemented function codes: " + LibraryInfo.ImplementedFunctionCodes);
          stringBuilder.AppendLine("Implemented extended function codes: " + LibraryInfo.ImplementedExtendedFunctionCodes);
          stringBuilder.AppendLine("");
          stringBuilder.AppendLine("");
        }
        if (additionalInfo != null)
        {
          for (int index = 0; index < additionalInfo.GetLength(0); ++index)
          {
            if (!string.IsNullOrEmpty(additionalInfo[index, 0]))
              stringBuilder.AppendLine(additionalInfo[index, 0] + ": " + additionalInfo[index, 1]);
          }
        }
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("\n\nCritical error occurred:");
        stringBuilder.AppendLine("Please check if the installation is OK.");
        stringBuilder.AppendLine("-> " + ex.Message);
      }
      InfoWindow infoWindow = new InfoWindow();
      infoWindow.Owner = owner;
      infoWindow.TextBoxApplicationInfo.Text = stringBuilder.ToString();
      if (infoWindow.ShowDialog().Value)
        ;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/infowindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.TextBoxApplicationInfo = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
