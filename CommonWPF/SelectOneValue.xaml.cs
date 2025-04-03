// Decompiled with JetBrains decompiler
// Type: CommonWPF.SelectOneValue
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommonWPF
{
  public partial class SelectOneValue : Window, IComponentConnector
  {
    private string returnValue = (string) null;
    internal Button ButtonOk;
    internal TextBlock TextBlockInfo;
    internal ComboBox ComboBoxValues;
    private bool _contentLoaded;

    private SelectOneValue(
      string title,
      string info,
      string[] selectList,
      string defaultValue,
      bool AllowInput)
    {
      this.InitializeComponent();
      this.ComboBoxValues.IsEditable = AllowInput;
      this.Title = title;
      this.TextBlockInfo.Text = info;
      this.ComboBoxValues.ItemsSource = (IEnumerable) selectList;
      if (defaultValue == null)
        return;
      for (int index = 0; index < selectList.Length; ++index)
      {
        if (this.ComboBoxValues.Items[index].ToString() == defaultValue)
        {
          this.ComboBoxValues.SelectedIndex = index;
          break;
        }
      }
    }

    public static string GetSelectedValue(
      string title,
      string info,
      string[] selectList,
      string defaultValue = null)
    {
      SelectOneValue selectOneValue = new SelectOneValue(title, info, selectList, defaultValue, false);
      selectOneValue.ShowDialog();
      return selectOneValue.returnValue;
    }

    public static string GetSelectedOrEnteredValue(
      string title,
      string info,
      string[] selectList,
      string defaultValue = null)
    {
      SelectOneValue selectOneValue = new SelectOneValue(title, info, selectList, defaultValue, true);
      selectOneValue.ShowDialog();
      return selectOneValue.returnValue;
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      if (this.ComboBoxValues.IsEditable)
        this.returnValue = this.ComboBoxValues.Text.Trim();
      else if (this.ComboBoxValues.SelectedIndex >= 0)
        this.returnValue = this.ComboBoxValues.SelectedItem.ToString();
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/selectonevalue.xaml", UriKind.Relative));
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
          this.ButtonOk = (Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 2:
          this.TextBlockInfo = (TextBlock) target;
          break;
        case 3:
          this.ComboBoxValues = (ComboBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
