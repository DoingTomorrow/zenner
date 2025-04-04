// Decompiled with JetBrains decompiler
// Type: StartupLib.LicenseWindow
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using PlugInLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;

#nullable disable
namespace StartupLib
{
  public partial class LicenseWindow : Window, IComponentConnector
  {
    internal System.Windows.Controls.Label lCurrentLicense;
    internal System.Windows.Controls.TextBox tbCurrentLicence;
    internal System.Windows.Controls.Label lPCCode;
    internal System.Windows.Controls.TextBox tbPCCode;
    internal System.Windows.Controls.Label lLicenseFile;
    internal System.Windows.Controls.TextBox tbLicenseFile;
    internal System.Windows.Controls.Button bBrowse;
    internal System.Windows.Controls.Button bOk;
    private bool _contentLoaded;

    public LicenseWindow(string currentLicenseFile = null)
    {
      this.InitializeComponent();
      if (string.IsNullOrEmpty(currentLicenseFile))
        return;
      this.tbCurrentLicence.Text = Path.GetFileName(currentLicenseFile);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.tbPCCode.Text = HardwareKeyGenerator.GetHardwareUniqueKey();
    }

    private void bBrowse_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "License files(*.zlf) |*.zlf";
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ZENNER", "GMM", "Settings");
      if (new DirectoryInfo(path).GetFiles("*.zlf").Length != 0)
        openFileDialog.InitialDirectory = path;
      if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;
      this.tbLicenseFile.Text = openFileDialog.FileName;
    }

    private void bOk_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        PlugInLib.LicenseManager.VerifyLicense(this.tbLicenseFile.Text);
        StartupManager.StaticInstance.ChangeLicenseFile(StartupManager.StaticInstance.CopyLicenseFile(this.tbLicenseFile.Text));
        this.DialogResult = new bool?(true);
        this.Close();
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.MessageBox.Show(ex.Message, "Invalid license file");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/StartupLib;component/licensewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.lCurrentLicense = (System.Windows.Controls.Label) target;
          break;
        case 3:
          this.tbCurrentLicence = (System.Windows.Controls.TextBox) target;
          break;
        case 4:
          this.lPCCode = (System.Windows.Controls.Label) target;
          break;
        case 5:
          this.tbPCCode = (System.Windows.Controls.TextBox) target;
          break;
        case 6:
          this.lLicenseFile = (System.Windows.Controls.Label) target;
          break;
        case 7:
          this.tbLicenseFile = (System.Windows.Controls.TextBox) target;
          break;
        case 8:
          this.bBrowse = (System.Windows.Controls.Button) target;
          this.bBrowse.Click += new RoutedEventHandler(this.bBrowse_Click);
          break;
        case 9:
          this.bOk = (System.Windows.Controls.Button) target;
          this.bOk.Click += new RoutedEventHandler(this.bOk_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
