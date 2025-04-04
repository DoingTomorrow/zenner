// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.Utils.DynamicGridControl
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.Events;
using MSS.Business.Modules.AppParametersManagement;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.CustomControls;
using MSS.Client.UI.Common.ValidationRules;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Interfaces;
using MSS.Utils.Controls.CheckableComboBox;
using MSS.Utils.Utils;
using MVVM.Converters;
using Styles.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Client.UI.Common.Utils
{
  public static class DynamicGridControl
  {
    public static string searchText;
    private static double _controlWidth;
    private const double CONTROL_HEIGHT = 25.0;
    private static bool? _isTabletMode;
    private static TextBlock _oldTextBlock;
    private static Brush _oldTextBlockBackgroundColor;
    private static bool _hasDescriptionTextBoxes;
    private static SolidColorBrush BACKGROUND_DISABLED_COLOR = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 237, (byte) 237, (byte) 237));
    private static SolidColorBrush BACKGROUND_ENABLED_COLOR = Brushes.White;
    private static SolidColorBrush FOREGROUND_COLOR = Brushes.Gray;
    private static SolidColorBrush HIGHLIGHT_COLOR = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 253, (byte) 243, (byte) 86));
    private static SolidColorBrush CONTROL_ENABLED_COLOR = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 165, (byte) 165, (byte) 165));
    private static SolidColorBrush CONTROL_DISABLED_COLOR = Brushes.Transparent;
    private static SolidColorBrush IS_FUNCTION_COLOR = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 183, (byte) 215, (byte) 247));
    private static SolidColorBrush GRID_LINES_COLOR = Brushes.DarkGray;
    public static SolidColorBrush SELECTED_PARAMETER_BACKGROUND_COLOR = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 183, (byte) 215, (byte) 247));
    private static SolidColorBrush FOUND_SEARCHED_TEXT_BACKGROUND_COLOR = Brushes.LightGreen;
    private static int TABLET_FONT_SIZE = 20;

    public static short CreateDynamicGrid(
      IList<Config> parameters,
      out GridControl dynamicGrid,
      ICommand comboboxCommand = null,
      double gridWidth = 650.0,
      double firstColumnPercentage = 50.0,
      bool? isTabletMode = null,
      bool hasDescriptionTextBoxes = false,
      bool addColumnForMeasureUnit = false,
      double thirdColumnWidth = 0.0,
      bool isValidationEnabled = false,
      ChangeableParameterUsings? type = null)
    {
      DynamicGridControl._hasDescriptionTextBoxes = hasDescriptionTextBoxes;
      parameters = parameters == null || !(parameters[0].Parameter is KeyValuePair<OverrideID, ConfigurationParameter>) ? (parameters != null ? (IList<Config>) parameters.OrderByDescending<Config, bool>((Func<Config, bool>) (_ => _.IsReadOnly)).ThenBy<Config, string>((Func<Config, string>) (_ => _.PropertyName)).ToList<Config>() : (IList<Config>) null) : (parameters != null ? (IList<Config>) parameters.OrderByDescending<Config, bool>((Func<Config, bool>) (_ => _.IsReadOnly)).ThenBy<Config, bool>((Func<Config, bool>) (_ => ((KeyValuePair<OverrideID, ConfigurationParameter>) _.Parameter).Value.IsFunction)).ThenBy<Config, string>((Func<Config, string>) (_ => _.PropertyName)).ToList<Config>() : (IList<Config>) null);
      dynamicGrid = new GridControl();
      DynamicGridControl._isTabletMode = isTabletMode;
      DynamicGridControl._controlWidth = gridWidth * ((100.0 - firstColumnPercentage) / 100.0) - 10.0;
      ColumnDefinition columnDefinition1 = new ColumnDefinition()
      {
        Width = new GridLength(firstColumnPercentage * (gridWidth - thirdColumnWidth) / 100.0)
      };
      ColumnDefinition columnDefinition2 = new ColumnDefinition()
      {
        Width = new GridLength((100.0 - firstColumnPercentage) * (gridWidth - thirdColumnWidth) / 100.0)
      };
      dynamicGrid.ColumnDefinitions.Add(columnDefinition1);
      dynamicGrid.ColumnDefinitions.Add(columnDefinition2);
      if (addColumnForMeasureUnit)
      {
        ColumnDefinition columnDefinition3 = new ColumnDefinition()
        {
          Width = new GridLength(thirdColumnWidth)
        };
        dynamicGrid.ColumnDefinitions.Add(columnDefinition3);
      }
      dynamicGrid.Width = gridWidth;
      dynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
      dynamicGrid.VerticalAlignment = VerticalAlignment.Top;
      if (parameters == null)
        return 0;
      short count = 0;
      foreach (Config parameter1 in (IEnumerable<Config>) parameters)
      {
        ChangeableParameter parameter2 = parameter1.Parameter as ChangeableParameter;
        if (!type.HasValue || parameter2 != null && parameter2.ParameterUsing == type.Value)
        {
          ViewObjectTypeEnum viewObjectTypeEnum = (ViewObjectTypeEnum) Enum.Parse(typeof (ViewObjectTypeEnum), parameter1.Type);
          new Canvas().Background = (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR;
          switch (viewObjectTypeEnum)
          {
            case ViewObjectTypeEnum.TextBox:
              RowDefinition rowDefinition1 = new RowDefinition();
              Border row1 = DynamicGridControl.CreateRow(parameter1, count, rowDefinition1, (Grid) dynamicGrid, parameter1.PropertyName);
              Border element1 = DynamicGridControl.ControlDefinition((Control) DynamicGridControl.SetBindingTextbox(parameter1, isValidationEnabled));
              Grid.SetRow((UIElement) element1, (int) count);
              Grid.SetColumn((UIElement) element1, 1);
              dynamicGrid.Children.Add((UIElement) row1);
              dynamicGrid.Children.Add((UIElement) element1);
              DynamicGridControl.AddColumnForMeasureUnit(addColumnForMeasureUnit, parameter1, count, rowDefinition1, dynamicGrid);
              ++count;
              break;
            case ViewObjectTypeEnum.CheckBox:
              RowDefinition rowDefinition2 = new RowDefinition();
              Border row2 = DynamicGridControl.CreateRow(parameter1, count, rowDefinition2, (Grid) dynamicGrid, parameter1.PropertyName);
              Border element2 = DynamicGridControl.SetBindingCheckBox(parameter1, addColumnForMeasureUnit);
              Grid.SetRow((UIElement) element2, (int) count);
              Grid.SetColumn((UIElement) element2, 1);
              dynamicGrid.Children.Add((UIElement) row2);
              dynamicGrid.Children.Add((UIElement) element2);
              DynamicGridControl.AddColumnForMeasureUnit(addColumnForMeasureUnit, parameter1, count, rowDefinition2, dynamicGrid);
              ++count;
              break;
            case ViewObjectTypeEnum.Numeric:
              RowDefinition rowDefinition3 = new RowDefinition();
              Border row3 = DynamicGridControl.CreateRow(parameter1, count, rowDefinition3, (Grid) dynamicGrid, parameter1.PropertyName);
              TextBox textBox = DynamicGridControl.SetBindingTextbox(parameter1, isValidationEnabled);
              textBox.PreviewTextInput += (TextCompositionEventHandler) ((sender, args) => args.Handled = DynamicGridControl.IsTextNumeric(args.Text));
              Border element3 = DynamicGridControl.ControlDefinition((Control) textBox);
              Grid.SetRow((UIElement) element3, (int) count);
              Grid.SetColumn((UIElement) element3, 1);
              dynamicGrid.Children.Add((UIElement) row3);
              dynamicGrid.Children.Add((UIElement) element3);
              DynamicGridControl.AddColumnForMeasureUnit(addColumnForMeasureUnit, parameter1, count, rowDefinition3, dynamicGrid);
              ++count;
              break;
            case ViewObjectTypeEnum.ComboBox:
              RowDefinition rowDefinition4 = new RowDefinition();
              Border row4 = DynamicGridControl.CreateRow(parameter1, count, rowDefinition4, (Grid) dynamicGrid, parameter1.PropertyName);
              RadComboBox radComboBox = DynamicGridControl.SetBindingRadComboBox(parameter1, comboboxCommand, isValidationEnabled);
              radComboBox.SelectionChanged += new SelectionChangedEventHandler(DynamicGridControl.combobox_SelectionChanged);
              Border element4 = DynamicGridControl.ControlDefinition((Control) radComboBox);
              Grid.SetRow((UIElement) element4, (int) count);
              Grid.SetColumn((UIElement) element4, 1);
              dynamicGrid.Children.Add((UIElement) row4);
              dynamicGrid.Children.Add((UIElement) element4);
              DynamicGridControl.AddColumnForMeasureUnit(addColumnForMeasureUnit, parameter1, count, rowDefinition4, dynamicGrid);
              ++count;
              break;
            case ViewObjectTypeEnum.MultiSelectionComboBox:
              RowDefinition rowDefinition5 = new RowDefinition();
              Border row5 = DynamicGridControl.CreateRow(parameter1, count, rowDefinition5, (Grid) dynamicGrid, parameter1.PropertyName);
              Border element5 = DynamicGridControl.ControlDefinition((Control) DynamicGridControl.SetBindingMultiSelectionRadComboBox(parameter1, comboboxCommand, isValidationEnabled));
              Grid.SetRow((UIElement) element5, (int) count);
              Grid.SetColumn((UIElement) element5, 1);
              dynamicGrid.Children.Add((UIElement) row5);
              dynamicGrid.Children.Add((UIElement) element5);
              DynamicGridControl.AddColumnForMeasureUnit(addColumnForMeasureUnit, parameter1, count, rowDefinition5, dynamicGrid);
              ++count;
              break;
          }
        }
      }
      DynamicGridControl._oldTextBlock = (TextBlock) null;
      DynamicGridControl._oldTextBlockBackgroundColor = (Brush) null;
      return count;
    }

    private static void AddColumnForMeasureUnit(
      bool addColumnForMeasureUnit,
      Config item,
      short count,
      RowDefinition row,
      GridControl dynamicGrid)
    {
      if (!addColumnForMeasureUnit)
        return;
      KeyValuePair<OverrideID, ConfigurationParameter> parameter = (KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter;
      Border row1 = DynamicGridControl.CreateRow(item, count, row, (Grid) dynamicGrid, parameter.Value.Unit, true);
      Grid.SetColumn((UIElement) row1, 2);
      dynamicGrid.Children.Add((UIElement) row1);
    }

    private static void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RadComboBox radComboBox = sender as RadComboBox;
      radComboBox.Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
      ((Config) radComboBox.Tag).PropertyValue = radComboBox.SelectedItem is ConfigurationPropertyDTO selectedItem ? selectedItem.Value : (string) null;
    }

    private static RadComboBox SetBindingRadComboBox(
      Config item,
      ICommand comboboxCommand,
      bool isValidationEnabled = false)
    {
      List<ConfigurationPropertyDTO> source = DynamicGridControl.SetSourceItemsForRadComboBox(item);
      RadComboBox radComboBox1;
      if (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value)
      {
        RadComboBox radComboBox2 = new RadComboBox();
        radComboBox2.ItemsSource = (IEnumerable) source;
        radComboBox2.Padding = new Thickness(20.0, 0.0, 0.0, 0.0);
        radComboBox2.IsReadOnly = item.IsReadOnly;
        radComboBox2.IsEnabled = !item.IsReadOnly;
        radComboBox2.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR)) : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR);
        radComboBox2.Tag = (object) item;
        radComboBox2.DisplayMemberPath = "DisplayName";
        radComboBox2.SelectedValuePath = "Value";
        radComboBox2.VerticalContentAlignment = VerticalAlignment.Center;
        radComboBox2.VerticalAlignment = VerticalAlignment.Center;
        radComboBox1 = radComboBox2;
      }
      else
      {
        RadComboBox radComboBox3 = new RadComboBox();
        radComboBox3.ItemsSource = (IEnumerable) source;
        radComboBox3.Padding = new Thickness(20.0, 0.0, 0.0, 0.0);
        radComboBox3.IsReadOnly = item.IsReadOnly;
        radComboBox3.IsEnabled = !item.IsReadOnly;
        radComboBox3.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR)) : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR);
        radComboBox3.Tag = (object) item;
        radComboBox3.DisplayMemberPath = "DisplayName";
        radComboBox3.SelectedValuePath = "Value";
        radComboBox3.VerticalContentAlignment = VerticalAlignment.Center;
        radComboBox1 = radComboBox3;
      }
      RadComboBox target = radComboBox1;
      if (source != null && source.Count > 0)
        target.SelectedItem = (object) source.FirstOrDefault<ConfigurationPropertyDTO>((Func<ConfigurationPropertyDTO, bool>) (x => x.Value == item.PropertyValue));
      else
        target.SelectedItem = (object) null;
      if (item.Parameter is ChangeableParameter parameter && parameter.Key == "COMserver")
        target.ItemContainerStyle = Application.Current.Resources[(object) "Triggers"] as Style;
      if (comboboxCommand != null)
      {
        target.Command = comboboxCommand;
        target.CommandParameter = (object) new CustomMultiBindingConverter();
      }
      if (isValidationEnabled && !item.IsReadOnly)
      {
        Binding binding = new Binding()
        {
          Source = (object) item,
          Path = new PropertyPath("PropertyValue", Array.Empty<object>()),
          NotifyOnValidationError = true,
          NotifyOnSourceUpdated = true,
          UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };
        Collection<ValidationRule> validationRules = binding.ValidationRules;
        RequiredValidationRule requiredValidationRule1 = new RequiredValidationRule();
        requiredValidationRule1.ValidatesOnTargetUpdated = true;
        requiredValidationRule1.ValidationStep = ValidationStep.ConvertedProposedValue;
        RequiredValidationRule requiredValidationRule2 = requiredValidationRule1;
        validationRules.Add((ValidationRule) requiredValidationRule2);
        BindingOperations.SetBinding((DependencyObject) target, Selector.SelectedValueProperty, (BindingBase) binding);
      }
      target.Style = Application.Current?.Resources[(object) "RadComboBoxErrorStyle"] as Style;
      return target;
    }

    private static List<ConfigurationPropertyDTO> SetSourceItemsForRadComboBox(Config item)
    {
      List<ConfigurationPropertyDTO> sourceItems = new List<ConfigurationPropertyDTO>();
      if (item.ProperListValues != null)
      {
        if (!(item.Parameter is ChangeableParameter parameter) || parameter.Key != "COMserver")
        {
          foreach (ConfigurationPropertyValue properListValue in item.ProperListValues)
            sourceItems.Add(new ConfigurationPropertyDTO()
            {
              DisplayName = properListValue.DisplayName,
              Value = properListValue.Value,
              IsOnline = true
            });
        }
        else
        {
          item.PropertyValue = item.PropertyValue != "-" ? item.PropertyValue : string.Empty;
          parameter.AvailableValues?.ForEach((Action<ValueItem>) (availableValue => sourceItems.Add(EquipmentHelper.GetFormattedCOMServerItemForDropdown(availableValue))));
          sourceItems = sourceItems.OrderByDescending<ConfigurationPropertyDTO, bool>((Func<ConfigurationPropertyDTO, bool>) (t => t.IsOnline)).ThenBy<ConfigurationPropertyDTO, string>((Func<ConfigurationPropertyDTO, string>) (t => t.DisplayName)).ToList<ConfigurationPropertyDTO>();
        }
      }
      return sourceItems;
    }

    private static MultiSelectionComboBoxUserControl SetBindingMultiSelectionRadComboBox(
      Config item,
      ICommand comboboxCommand,
      bool isValidationEnabled = false)
    {
      MultiSelectionComboBoxViewModel comboBoxViewModel = DynamicGridControl.SetViewModelForMultiSelectionRadComboBox(item);
      MultiSelectionComboBoxUserControl comboBoxUserControl = new MultiSelectionComboBoxUserControl();
      comboBoxUserControl.DataContext = (object) comboBoxViewModel;
      comboBoxUserControl.Padding = new Thickness(20.0, 0.0, 0.0, 0.0);
      comboBoxUserControl.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR)) : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR : (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR);
      comboBoxUserControl.Tag = (object) item;
      return comboBoxUserControl;
    }

    private static MultiSelectionComboBoxViewModel SetViewModelForMultiSelectionRadComboBox(
      Config item)
    {
      MultiSelectionComboBoxViewModel viewModel = new MultiSelectionComboBoxViewModel()
      {
        ItemsList = (IList<CheckableComboBoxItem>) new List<CheckableComboBoxItem>()
      };
      if (item.ProperListValues == null)
        throw new ArgumentException();
      ConfigurationParameter configurationParameter = ((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value;
      string[] source = configurationParameter != null ? configurationParameter.AllowedValues : throw new ArgumentException();
      if (source != null)
        ((IEnumerable<string>) source).ToList<string>().OrderBy<string, string>((Func<string, string>) (t => t)).ToList<string>().ForEach((Action<string>) (allowedValue => viewModel.ItemsList.Add(new CheckableComboBoxItem()
        {
          Text = allowedValue
        })));
      foreach (string str in ((IEnumerable<string>) (string[]) configurationParameter.ParameterValue).ToList<string>())
      {
        string currentListItem = str;
        CheckableComboBoxItem checkableComboBoxItem = viewModel.ItemsList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (_ => _.Text == currentListItem));
        checkableComboBoxItem.IsChecked = checkableComboBoxItem != null;
      }
      viewModel.RefreshTextAfterInitialization();
      return viewModel;
    }

    private static Border SetBindingCheckBox(Config item, bool addColumnForMeasureUnit)
    {
      bool result = false;
      CheckBox checkBox1 = new CheckBox();
      checkBox1.IsChecked = new bool?(bool.TryParse(item.PropertyValue, out result) & result);
      checkBox1.Foreground = (Brush) DynamicGridControl.FOREGROUND_COLOR;
      checkBox1.VerticalAlignment = VerticalAlignment.Center;
      checkBox1.HorizontalAlignment = HorizontalAlignment.Left;
      checkBox1.Padding = new Thickness(20.0, 5.0, 20.0, 0.0);
      checkBox1.IsEnabled = !item.IsReadOnly;
      checkBox1.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR : (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR)) : (!item.IsReadOnly ? (Brush) DynamicGridControl.CONTROL_ENABLED_COLOR : (Brush) DynamicGridControl.CONTROL_DISABLED_COLOR);
      checkBox1.Tag = (object) item;
      checkBox1.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
      CheckBox checkBox2 = checkBox1;
      Border border1 = new Border();
      border1.BorderBrush = (Brush) DynamicGridControl.GRID_LINES_COLOR;
      border1.Background = (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR;
      border1.BorderThickness = new Thickness(0.7);
      border1.Width = DynamicGridControl._controlWidth;
      border1.Margin = new Thickness(addColumnForMeasureUnit ? 0.0 : -10.0, 0.0, 0.0, 0.0);
      Border border2 = border1;
      border2.Child = (UIElement) checkBox2;
      checkBox2.Checked += (RoutedEventHandler) ((s, e) =>
      {
        CheckBox checkBox3 = s as CheckBox;
        checkBox3.Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
        (checkBox3.Parent as Border).Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
        ((Config) checkBox3.Tag).PropertyValue = "True";
      });
      checkBox2.Unchecked += (RoutedEventHandler) ((s, e) =>
      {
        CheckBox checkBox4 = s as CheckBox;
        checkBox4.Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
        (checkBox4.Parent as Border).Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
        ((Config) checkBox4.Tag).PropertyValue = "False";
      });
      return border2;
    }

    private static TextBox SetBindingTextbox(Config item, bool isValidationEnabled = false)
    {
      TextBox textBox1;
      if (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value)
      {
        TextBox textBox2 = new TextBox();
        textBox2.HorizontalAlignment = HorizontalAlignment.Stretch;
        textBox2.VerticalAlignment = VerticalAlignment.Stretch;
        textBox2.VerticalContentAlignment = VerticalAlignment.Center;
        textBox2.Foreground = (Brush) DynamicGridControl.FOREGROUND_COLOR;
        textBox2.Text = item.PropertyValue;
        textBox2.IsReadOnly = item.IsReadOnly;
        textBox2.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR)) : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR);
        textBox2.Tag = (object) item;
        textBox2.BorderBrush = (Brush) DynamicGridControl.GRID_LINES_COLOR;
        textBox1 = textBox2;
      }
      else
      {
        TextBox textBox3 = new TextBox();
        textBox3.HorizontalAlignment = HorizontalAlignment.Left;
        textBox3.VerticalContentAlignment = VerticalAlignment.Center;
        textBox3.Foreground = (Brush) DynamicGridControl.FOREGROUND_COLOR;
        textBox3.Text = item.PropertyValue;
        textBox3.FontSize = (double) DynamicGridControl.TABLET_FONT_SIZE;
        textBox3.IsReadOnly = item.IsReadOnly;
        textBox3.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR)) : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR);
        textBox3.Tag = (object) item;
        textBox1 = textBox3;
      }
      TextBox textbox = textBox1;
      textbox.TextChanged += (TextChangedEventHandler) ((s, e) =>
      {
        TextBox textBox4 = s as TextBox;
        textBox4.Background = (Brush) DynamicGridControl.HIGHLIGHT_COLOR;
        ((Config) textBox4.Tag).PropertyValue = textBox4.Text;
      });
      if (textbox.IsReadOnly)
      {
        ToolTip toolTip1 = new ToolTip();
        toolTip1.Content = (object) item.PropertyValue;
        ToolTip toolTip2 = toolTip1;
        textbox.ToolTip = (object) toolTip2;
      }
      else if (isValidationEnabled)
        DynamicGridControl.BindValidationRulesToTextbox(item, textbox);
      textbox.Style = Application.Current?.Resources[(object) "TextBoxDynamicGridErrorStyle"] as Style;
      return textbox;
    }

    private static void BindValidationRulesToTextbox(Config item, TextBox textbox)
    {
      Binding binding = new Binding()
      {
        Source = (object) item,
        Path = new PropertyPath("PropertyValue", Array.Empty<object>()),
        NotifyOnValidationError = true,
        NotifyOnSourceUpdated = true,
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
      };
      Collection<ValidationRule> validationRules = binding.ValidationRules;
      RequiredValidationRule requiredValidationRule1 = new RequiredValidationRule();
      requiredValidationRule1.ValidatesOnTargetUpdated = true;
      requiredValidationRule1.ValidationStep = ValidationStep.ConvertedProposedValue;
      RequiredValidationRule requiredValidationRule2 = requiredValidationRule1;
      validationRules.Add((ValidationRule) requiredValidationRule2);
      DynamicGridControl.BindRangeValidationRule(item, binding);
      BindingOperations.SetBinding((DependencyObject) textbox, TextBox.TextProperty, (BindingBase) binding);
    }

    private static void BindRangeValidationRule(Config item, Binding binding)
    {
      if (!(item.Parameter is ChangeableParameter parameter))
        return;
      ValidationRule rangeValidationRule = new RangeValidationRuleGenerator().GetRangeValidationRule(parameter.Type.Name, parameter.ValueMin, parameter.ValueMax);
      if (rangeValidationRule != null)
        binding.ValidationRules.Add(rangeValidationRule);
    }

    private static Border ControlDefinition(Control control)
    {
      control.Foreground = (Brush) DynamicGridControl.FOREGROUND_COLOR;
      control.Width = DynamicGridControl._controlWidth;
      control.Height = 25.0 + (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value ? 1.0 : 11.0);
      control.VerticalAlignment = VerticalAlignment.Center;
      control.HorizontalAlignment = HorizontalAlignment.Stretch;
      control.HorizontalContentAlignment = HorizontalAlignment.Left;
      control.Padding = new Thickness(5.0, 0.0, 5.0, 0.0);
      control.Margin = new Thickness(-10.0, 0.0, 0.0, 0.0);
      Border border1 = new Border();
      border1.BorderBrush = (Brush) DynamicGridControl.GRID_LINES_COLOR;
      border1.Background = control.Background;
      border1.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
      border1.Width = control.Width;
      Border border2 = border1;
      border2.Child = (UIElement) control;
      return border2;
    }

    private static Border CreateRow(
      Config item,
      short count,
      RowDefinition textBlockRow,
      Grid dynamicGrid,
      string text,
      bool isForUnitColumn = false)
    {
      textBlockRow.MinHeight = 25.0 + (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value ? 1.0 : 11.0);
      textBlockRow.MaxHeight = 25.0 + (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value ? 1.0 : 11.0);
      if (!isForUnitColumn)
        dynamicGrid.RowDefinitions.Add(textBlockRow);
      TextBlock textBlock1;
      if (!DynamicGridControl._isTabletMode.HasValue || !DynamicGridControl._isTabletMode.Value)
      {
        TextBlock textBlock2 = new TextBlock();
        textBlock2.VerticalAlignment = VerticalAlignment.Center;
        textBlock2.HorizontalAlignment = HorizontalAlignment.Stretch;
        textBlock2.Padding = new Thickness(5.0, 0.0, 20.0, 0.0);
        textBlock2.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
        textBlock2.Text = text;
        textBlock2.TextAlignment = TextAlignment.Right;
        textBlock2.Tag = (object) item;
        textBlock2.Foreground = (Brush) Brushes.Gray;
        textBlock2.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR)) : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR);
        textBlock1 = textBlock2;
      }
      else
      {
        TextBlock textBlock3 = new TextBlock();
        textBlock3.VerticalAlignment = VerticalAlignment.Center;
        textBlock3.HorizontalAlignment = HorizontalAlignment.Stretch;
        textBlock3.Padding = new Thickness(0.0, 0.0, 20.0, 0.0);
        textBlock3.Text = text;
        textBlock3.TextAlignment = TextAlignment.Right;
        textBlock3.FontSize = (double) DynamicGridControl.TABLET_FONT_SIZE;
        textBlock3.Tag = (object) item;
        textBlock3.Foreground = (Brush) Brushes.Gray;
        textBlock3.Background = item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction ? (Brush) DynamicGridControl.IS_FUNCTION_COLOR : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR)) : (item.IsReadOnly ? (Brush) DynamicGridControl.BACKGROUND_DISABLED_COLOR : (Brush) DynamicGridControl.BACKGROUND_ENABLED_COLOR);
        textBlock1 = textBlock3;
      }
      TextBlock textBlock4 = textBlock1;
      Border element = new Border()
      {
        BorderBrush = (Brush) DynamicGridControl.GRID_LINES_COLOR,
        Background = textBlock4.Background,
        BorderThickness = new Thickness(0.7)
      };
      element.Child = (UIElement) textBlock4;
      if (DynamicGridControl._hasDescriptionTextBoxes)
      {
        element.MouseLeftButtonUp += new MouseButtonEventHandler(DynamicGridControl.Border_DisplayPropertyDetails_OnMouseLeftButtonUp);
        element.TouchUp += new EventHandler<TouchEventArgs>(DynamicGridControl.Border_DisplayPropertyDetails_OnTouchUp);
      }
      Grid.SetRow((UIElement) element, (int) count);
      Grid.SetColumn((UIElement) element, 0);
      return element;
    }

    private static void Border_DisplayPropertyDetails_OnMouseLeftButtonUp(
      object sender,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      DynamicGridControl.ResetBackgroundForTextBlocks(sender);
      DynamicGridControl.PublishConfigurationParameterDescription(((sender is Border border ? border.Child : (UIElement) null) is TextBlock child ? child.Tag : (object) null) is Config tag ? tag.Description : (string) null);
    }

    private static void Border_DisplayPropertyDetails_OnTouchUp(
      object sender,
      TouchEventArgs touchEventArgs)
    {
      DynamicGridControl.ResetBackgroundForTextBlocks(sender);
      DynamicGridControl.PublishConfigurationParameterDescription(((sender is Border border ? border.Child : (UIElement) null) is TextBlock child ? child.Tag : (object) null) is Config tag ? tag.Description : (string) null);
    }

    public static SolidColorBrush GetBackgroundColor(Config item)
    {
      if (item.Parameter is KeyValuePair<OverrideID, ConfigurationParameter>)
      {
        if (((KeyValuePair<OverrideID, ConfigurationParameter>) item.Parameter).Value.IsFunction)
          return DynamicGridControl.IS_FUNCTION_COLOR;
        return item.IsReadOnly ? DynamicGridControl.BACKGROUND_DISABLED_COLOR : DynamicGridControl.BACKGROUND_ENABLED_COLOR;
      }
      return item.IsReadOnly ? DynamicGridControl.BACKGROUND_DISABLED_COLOR : DynamicGridControl.BACKGROUND_ENABLED_COLOR;
    }

    private static void ResetBackgroundForTextBlocks(object sender)
    {
      if (DynamicGridControl._oldTextBlock != null)
      {
        Config tag = DynamicGridControl._oldTextBlock.Tag as Config;
        DynamicGridControl._oldTextBlock.Background = DynamicGridControl._oldTextBlockBackgroundColor == null || !DynamicGridControl._oldTextBlockBackgroundColor.Equals((object) DynamicGridControl.FOUND_SEARCHED_TEXT_BACKGROUND_COLOR) ? (Brush) DynamicGridControl.GetBackgroundColor(tag) : (Brush) DynamicGridControl.FOUND_SEARCHED_TEXT_BACKGROUND_COLOR;
        DynamicGridControl._oldTextBlock.FontWeight = FontWeights.Normal;
        if (DynamicGridControl._oldTextBlock?.Parent is Border parent)
          parent.Background = DynamicGridControl._oldTextBlock.Background;
      }
      TextBlock child = (sender is Border border ? border.Child : (UIElement) null) as TextBlock;
      DynamicGridControl._oldTextBlock = child;
      DynamicGridControl._oldTextBlockBackgroundColor = child?.Background;
      if (child == null)
        return;
      child.Background = (Brush) DynamicGridControl.SELECTED_PARAMETER_BACKGROUND_COLOR;
      child.FontWeight = FontWeights.Bold;
      if (child.Parent is Border parent1)
        parent1.Background = (Brush) DynamicGridControl.SELECTED_PARAMETER_BACKGROUND_COLOR;
    }

    private static void PublishConfigurationParameterDescription(string description)
    {
      EventPublisher.Publish<ConfigurationParameterClicked>(new ConfigurationParameterClicked()
      {
        Description = description
      }, (IViewModel) null);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    public static void UpdateCheckBoxInDynamicGrid(
      this List<Config> dynamiConfigsGrid,
      GridControl grid)
    {
      foreach (KeyValuePair<UIElement, UIElement> keyValuePair in Enumerable.Cast<UIElement>(grid.Children).ToList<UIElement>().AsPairsSafe<UIElement>().Select<Tuple<UIElement, UIElement, UIElement>, Tuple<UIElement, UIElement, UIElement>>((Func<Tuple<UIElement, UIElement, UIElement>, Tuple<UIElement, UIElement, UIElement>>) (_ => _)).Where<Tuple<UIElement, UIElement, UIElement>>((Func<Tuple<UIElement, UIElement, UIElement>, bool>) (_ => !(_.Item1 is Border) || !(_.Item2 is Border) ? _.Item1 is TextBlock && !(_.Item2 is Canvas) : _.Item1.ChildrenOfType<UIElement>().FirstOrDefault<UIElement>() is TextBlock && !(_.Item2.ChildrenOfType<UIElement>().FirstOrDefault<UIElement>() is Canvas))).ToDictionary<Tuple<UIElement, UIElement, UIElement>, UIElement, UIElement>((Func<Tuple<UIElement, UIElement, UIElement>, UIElement>) (_ => !(_.Item1 is Border) ? _.Item1 : _.Item1.ChildrenOfType<UIElement>().FirstOrDefault<UIElement>()), (Func<Tuple<UIElement, UIElement, UIElement>, UIElement>) (_ => !(_.Item2 is Border) ? _.Item2 : _.Item2.ChildrenOfType<UIElement>().FirstOrDefault<UIElement>())))
      {
        foreach (Config config1 in dynamiConfigsGrid)
        {
          if (config1.PropertyName == keyValuePair.Key.SafeCast<TextBlock>().Text)
          {
            if (config1.Type == ViewObjectTypeEnum.MultiSelectionComboBox.GetStringName())
            {
              Config config2 = config1;
              object dataContext = keyValuePair.Value.SafeCast<MultiSelectionComboBoxUserControl>().DataContext;
              string selectedItemText = dataContext != null ? dataContext.SafeCast<MultiSelectionComboBoxViewModel>().SelectedItemText : (string) null;
              config2.PropertyValue = selectedItemText;
            }
            if (config1.Type == "CheckBox")
              config1.PropertyValue = keyValuePair.Value.SafeCast<CheckBox>().IsChecked.ToString();
            if (config1.Type == "TextBox")
              config1.PropertyValue = keyValuePair.Value.SafeCast<TextBox>().Text;
            if (config1.Type == "ComboBox")
            {
              Config config3 = config1;
              object selectedValue = keyValuePair.Value.SafeCast<RadComboBox>().SelectedValue;
              string str = selectedValue != null ? selectedValue.SafeCast<string>() : (string) null;
              config3.PropertyValue = str;
            }
          }
        }
      }
    }

    public static SortedList<OverrideID, ConfigurationParameter> SetConfigurationParameters(
      object parameter,
      List<Config> dynamicGridTag)
    {
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      GridControl grid = (parameter as UIElementCollection)[0] as GridControl;
      dynamicGridTag.UpdateCheckBoxInDynamicGrid(grid);
      return sortedList;
    }
  }
}
