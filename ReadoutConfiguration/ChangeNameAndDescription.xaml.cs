// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ChangeNameAndDescription
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class ChangeNameAndDescription : Window, IComponentConnector
  {
    public string ConfigItemType;
    public string ConfigItemName;
    public string ConfigItemDescription = "";
    public string ConfigItemTypeClassification = "";
    internal GmmCorporateControl gmmCorporateControl1;
    internal Button ButtonOk;
    internal TextBox TextBoxName;
    internal TextBox TextBoxTypeClassification;
    internal TextBox TextBoxDescription;
    internal Label LableItemType;
    private bool _contentLoaded;

    public ChangeNameAndDescription() => this.InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.LableItemType.Content = (object) this.ConfigItemType;
      this.TextBoxName.Text = this.ConfigItemName;
      this.TextBoxDescription.Text = this.ConfigItemDescription;
      this.TextBoxTypeClassification.Text = this.ConfigItemTypeClassification;
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      this.ConfigItemName = this.TextBoxName.Text;
      this.ConfigItemDescription = this.TextBoxDescription.Text;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/changenameanddescription.xaml", UriKind.Relative));
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
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 3:
          this.ButtonOk = (Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 4:
          this.TextBoxName = (TextBox) target;
          break;
        case 5:
          this.TextBoxTypeClassification = (TextBox) target;
          break;
        case 6:
          this.TextBoxDescription = (TextBox) target;
          break;
        case 7:
          this.LableItemType = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
