// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.CommonParameterEditor
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class CommonParameterEditor : Window, IComponentConnector
  {
    internal SortedList<string, CommonEditValues> EditValues;
    private int NumberOfSettings;
    private bool FormIsInitialised = false;
    internal GmmCorporateControl gmmCorporateControl1;
    internal StackPanel StackPanelContent;
    internal StackPanel StackPanelOverview;
    internal TextBlock TextBlockNumberOfLists;
    internal System.Windows.Controls.CheckBox CheckBoxShowOnlyDifferent;
    internal StackPanel StackPanelParameterList;
    private bool _contentLoaded;

    public CommonParameterEditor(List<CommonEditValues> editValues, int numberOfSettings)
    {
      this.EditValues = new SortedList<string, CommonEditValues>();
      foreach (CommonEditValues editValue in editValues)
        this.EditValues.Add(editValue.ValueName, editValue);
      this.NumberOfSettings = numberOfSettings;
      this.InitializeComponent();
      this.FormIsInitialised = true;
      this.RefreshForm();
    }

    private void RefreshForm()
    {
      if (!this.FormIsInitialised)
        return;
      this.TextBlockNumberOfLists.Text = this.NumberOfSettings.ToString();
      this.StackPanelParameterList.Children.Clear();
      foreach (CommonEditValues commonEditValues in (IEnumerable<CommonEditValues>) this.EditValues.Values)
      {
        if (!this.CheckBoxShowOnlyDifferent.IsChecked.Value || commonEditValues.valueListAndUsing.Count != 1 || commonEditValues.editByListAndUsing.Count != 1)
        {
          StringBuilder stringBuilder1 = new StringBuilder();
          int num1 = 0;
          foreach (KeyValuePair<string, List<int>> keyValuePair in commonEditValues.valueListAndUsing)
          {
            if (stringBuilder1.Length > 0)
              stringBuilder1.Append("; ");
            num1 += keyValuePair.Value.Count;
            stringBuilder1.Append(keyValuePair.Key + "(" + keyValuePair.Value.Count.ToString() + ")");
          }
          int num2 = this.NumberOfSettings - num1;
          if (num2 > 0)
            stringBuilder1.Append("; null(" + num2.ToString() + ")");
          StringBuilder stringBuilder2 = new StringBuilder();
          int num3 = 0;
          foreach (KeyValuePair<string, List<int>> keyValuePair in commonEditValues.editByListAndUsing)
          {
            if (stringBuilder2.Length > 0)
              stringBuilder2.Append("; ");
            num3 += keyValuePair.Value.Count;
            stringBuilder2.Append(keyValuePair.Key + "(" + keyValuePair.Value.Count.ToString() + ")");
          }
          int num4 = this.NumberOfSettings - num3;
          if (num4 > 0)
            stringBuilder2.Append("; null(" + num4.ToString() + ")");
          Border element1 = new Border();
          element1.BorderBrush = (Brush) Brushes.Blue;
          element1.BorderThickness = new Thickness(2.0);
          element1.Margin = new Thickness(2.0);
          StackPanel stackPanel = new StackPanel();
          stackPanel.Margin = new Thickness(2.0);
          StackPanel element2 = new StackPanel();
          element2.Orientation = System.Windows.Controls.Orientation.Horizontal;
          StackPanel element3 = new StackPanel();
          element3.Orientation = System.Windows.Controls.Orientation.Horizontal;
          element1.Child = (UIElement) stackPanel;
          stackPanel.Children.Add((UIElement) element2);
          stackPanel.Children.Add((UIElement) element3);
          TextBlock element4 = new TextBlock();
          element4.Text = commonEditValues.ValueName;
          element4.Margin = new Thickness(2.0, 2.0, 10.0, 2.0);
          element4.FontSize = 16.0;
          element4.FontWeight = FontWeights.Bold;
          element2.Children.Add((UIElement) element4);
          TextBlock element5 = new TextBlock();
          element5.Text = "Existing values: " + stringBuilder1.ToString();
          element5.Margin = new Thickness(8.0, 2.0, 2.0, 2.0);
          element2.Children.Add((UIElement) element5);
          TextBlock element6 = new TextBlock();
          element6.Text = "Edit by settings: " + stringBuilder2.ToString();
          element6.Margin = new Thickness(8.0, 2.0, 8.0, 2.0);
          element2.Children.Add((UIElement) element6);
          System.Windows.Controls.Button element7 = new System.Windows.Controls.Button();
          element7.Name = "ButtonDetails" + commonEditValues.ValueName;
          element7.Content = (object) "Details";
          element7.Click += new RoutedEventHandler(this.ButtonDetails_Click);
          element2.Children.Add((UIElement) element7);
          TextBlock element8 = new TextBlock();
          element8.Text = "New value:";
          element8.Margin = new Thickness(2.0);
          element3.Children.Add((UIElement) element8);
          System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
          textBox.Margin = new Thickness(8.0, 2.0, 8.0, 2.0);
          textBox.Width = 100.0;
          element3.Children.Add((UIElement) textBox);
          TextBlock element9 = new TextBlock();
          element9.Text = "New edit by setup:";
          element9.Margin = new Thickness(2.0);
          element3.Children.Add((UIElement) element9);
          System.Windows.Controls.ComboBox comboBox = new System.Windows.Controls.ComboBox();
          comboBox.Margin = new Thickness(8.0, 2.0, 8.0, 2.0);
          foreach (string name in Enum.GetNames(typeof (ConnectionSettingsParameterUsing)))
            comboBox.Items.Add((object) name);
          comboBox.SelectedIndex = 0;
          comboBox.Width = 300.0;
          element3.Children.Add((UIElement) comboBox);
          System.Windows.Controls.Button element10 = new System.Windows.Controls.Button();
          element10.Name = "ButtonChangeValues";
          element10.Content = (object) "Change values";
          element10.Click += new RoutedEventHandler(this.ButtonChangeValues_Click);
          element10.Tag = (object) new CommonParameterEditor.EditControls(commonEditValues.ValueName, textBox, comboBox);
          element10.Margin = new Thickness(8.0, 2.0, 8.0, 2.0);
          element3.Children.Add((UIElement) element10);
          System.Windows.Controls.Button element11 = new System.Windows.Controls.Button();
          element11.Name = "ButtonDeleteValues";
          element11.Content = (object) "Delete values";
          element11.Tag = (object) commonEditValues.ValueName;
          element11.Click += new RoutedEventHandler(this.ButtonDeleteValues_Click);
          element11.Margin = new Thickness(8.0, 2.0, 8.0, 2.0);
          element3.Children.Add((UIElement) element11);
          this.StackPanelParameterList.Children.Add((UIElement) element1);
        }
      }
    }

    private void ButtonDetails_Click(object sender, RoutedEventArgs e)
    {
      string key1 = ((FrameworkElement) sender).Name.Substring("ButtonDetails".Length);
      CommonEditValues editValue = this.EditValues[key1];
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Detaild using of setting " + key1);
      foreach (KeyValuePair<string, List<int>> keyValuePair in editValue.valueListAndUsing)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Value '" + keyValuePair.Key + "' used in settings:");
        foreach (int key2 in keyValuePair.Value)
        {
          ConnectionSettings connectionSettings = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[key2];
          stringBuilder.AppendLine("\t" + key2.ToString() + "; " + connectionSettings.Name);
        }
      }
      foreach (KeyValuePair<string, List<int>> keyValuePair in editValue.editByListAndUsing)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Using '" + keyValuePair.Key + "' used in settings:");
        foreach (int key3 in keyValuePair.Value)
        {
          ConnectionSettings connectionSettings = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[key3];
          stringBuilder.AppendLine("\t" + key3.ToString() + "; " + connectionSettings.Name);
        }
      }
      int num = (int) GMM_MessageBox.ShowMessage("Settings values used in settings", stringBuilder.ToString(), MessageBoxButtons.OK);
    }

    private void ButtonChangeValues_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        System.Windows.Controls.Button button = (System.Windows.Controls.Button) sender;
        CommonParameterEditor.EditControls tag = (CommonParameterEditor.EditControls) button.Tag;
        string valueName = tag.ValueName;
        string newValue = tag.ValueText.Text.Trim();
        string text = tag.EditByText.Text;
        if (newValue.Length == 0)
          return;
        CommonEditValues editValue = this.EditValues[valueName];
        List<int> settingIds = new List<int>();
        foreach (KeyValuePair<string, List<int>> keyValuePair in editValue.valueListAndUsing)
        {
          foreach (int num in keyValuePair.Value)
            settingIds.Add(num);
        }
        ReadoutConfigFunctions.DbData.SetCommonSettingsValues(valueName, newValue, text, settingIds);
        foreach (object child in ((System.Windows.Controls.Panel) button.Parent).Children)
        {
          switch (child)
          {
            case System.Windows.Controls.Button _:
              ((UIElement) child).IsEnabled = false;
              break;
            case System.Windows.Controls.TextBox _:
              ((UIElement) child).IsEnabled = false;
              ((System.Windows.Controls.Control) child).Background = (Brush) Brushes.LightGreen;
              break;
            case System.Windows.Controls.ComboBox _:
              ((UIElement) child).IsEnabled = false;
              break;
          }
        }
        int num1 = (int) System.Windows.MessageBox.Show("Values changed");
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.MessageBox.Show(ex.ToString());
      }
    }

    private void ButtonDeleteValues_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        System.Windows.Controls.Button button = (System.Windows.Controls.Button) sender;
        string tag = (string) button.Tag;
        CommonEditValues editValue = this.EditValues[tag];
        List<int> settingIds = new List<int>();
        foreach (KeyValuePair<string, List<int>> keyValuePair in editValue.valueListAndUsing)
        {
          foreach (int num in keyValuePair.Value)
            settingIds.Add(num);
        }
        ReadoutConfigFunctions.DbData.DeleteCommonSettingsValues(tag, settingIds);
        foreach (object child in ((System.Windows.Controls.Panel) button.Parent).Children)
        {
          switch (child)
          {
            case System.Windows.Controls.Button _:
              ((UIElement) child).IsEnabled = false;
              break;
            case System.Windows.Controls.TextBox _:
              ((System.Windows.Controls.TextBox) child).Text = "Deleted";
              ((UIElement) child).IsEnabled = false;
              ((System.Windows.Controls.Control) child).Background = (Brush) Brushes.Red;
              break;
            case System.Windows.Controls.ComboBox _:
              ((UIElement) child).IsEnabled = false;
              break;
          }
        }
        int num1 = (int) System.Windows.MessageBox.Show("Values deleted");
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.MessageBox.Show(ex.ToString());
      }
    }

    private void CheckBoxShowOnlyDifferent_Checked(object sender, RoutedEventArgs e)
    {
      this.RefreshForm();
    }

    private void CheckBoxShowOnlyDifferent_Unchecked(object sender, RoutedEventArgs e)
    {
      this.RefreshForm();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/commonparametereditor.xaml", UriKind.Relative));
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
          this.StackPanelContent = (StackPanel) target;
          break;
        case 3:
          this.StackPanelOverview = (StackPanel) target;
          break;
        case 4:
          this.TextBlockNumberOfLists = (TextBlock) target;
          break;
        case 5:
          this.CheckBoxShowOnlyDifferent = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxShowOnlyDifferent.Checked += new RoutedEventHandler(this.CheckBoxShowOnlyDifferent_Checked);
          this.CheckBoxShowOnlyDifferent.Unchecked += new RoutedEventHandler(this.CheckBoxShowOnlyDifferent_Unchecked);
          break;
        case 6:
          this.StackPanelParameterList = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    internal class EditControls
    {
      internal string ValueName;
      internal System.Windows.Controls.TextBox ValueText;
      internal System.Windows.Controls.ComboBox EditByText;

      internal EditControls(string valueName, System.Windows.Controls.TextBox valueText, System.Windows.Controls.ComboBox editByText)
      {
        this.ValueName = valueName;
        this.ValueText = valueText;
        this.EditByText = editByText;
      }
    }
  }
}
