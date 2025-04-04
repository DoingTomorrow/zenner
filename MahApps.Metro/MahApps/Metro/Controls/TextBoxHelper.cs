// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.TextBoxHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class TextBoxHelper
  {
    public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(TextBoxHelper.OnIsMonitoringChanged)));
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", typeof (string), typeof (TextBoxHelper), (PropertyMetadata) new UIPropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty UseFloatingWatermarkProperty = DependencyProperty.RegisterAttached("UseFloatingWatermark", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
    public static readonly DependencyProperty TextLengthProperty = DependencyProperty.RegisterAttached("TextLength", typeof (int), typeof (TextBoxHelper), (PropertyMetadata) new UIPropertyMetadata((object) 0));
    public static readonly DependencyProperty ClearTextButtonProperty = DependencyProperty.RegisterAttached("ClearTextButton", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
    public static readonly DependencyProperty ButtonsAlignmentProperty = DependencyProperty.RegisterAttached("ButtonsAlignment", typeof (ButtonsAlignment), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
    public static readonly DependencyProperty IsClearTextButtonBehaviorEnabledProperty = DependencyProperty.RegisterAttached("IsClearTextButtonBehaviorEnabled", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(TextBoxHelper.IsClearTextButtonBehaviorEnabledChanged)));
    public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.RegisterAttached("ButtonCommand", typeof (ICommand), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(TextBoxHelper.ButtonCommandOrClearTextChanged)));
    public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.RegisterAttached("ButtonCommandParameter", typeof (object), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.RegisterAttached("ButtonContent", typeof (object), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) "r"));
    public static readonly DependencyProperty ButtonTemplateProperty = DependencyProperty.RegisterAttached("ButtonTemplate", typeof (ControlTemplate), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ButtonFontFamilyProperty = DependencyProperty.RegisterAttached("ButtonFontFamily", typeof (FontFamily), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata(new FontFamilyConverter().ConvertFromString("Marlett")));
    public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    public static readonly DependencyProperty IsWaitingForDataProperty = DependencyProperty.RegisterAttached("IsWaitingForData", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty HasTextProperty = DependencyProperty.RegisterAttached("HasText", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty IsSpellCheckContextMenuEnabledProperty = DependencyProperty.RegisterAttached("IsSpellCheckContextMenuEnabled", typeof (bool), typeof (TextBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(TextBoxHelper.UseSpellCheckContextMenuChanged)));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    public static bool GetIsSpellCheckContextMenuEnabled(UIElement element)
    {
      return (bool) element.GetValue(TextBoxHelper.IsSpellCheckContextMenuEnabledProperty);
    }

    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    public static void SetIsSpellCheckContextMenuEnabled(UIElement element, bool value)
    {
      element.SetValue(TextBoxHelper.IsSpellCheckContextMenuEnabledProperty, (object) value);
    }

    private static void UseSpellCheckContextMenuChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is TextBoxBase textBoxBase))
        throw new InvalidOperationException("The property 'IsSpellCheckContextMenuEnabled' may only be set on TextBoxBase elements.");
      if ((bool) e.NewValue)
      {
        textBoxBase.SetValue(SpellCheck.IsEnabledProperty, (object) true);
        textBoxBase.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
        textBoxBase.ContextMenuOpening += new ContextMenuEventHandler(TextBoxHelper.TextBoxBaseContextMenuOpening);
      }
      else
      {
        textBoxBase.SetValue(SpellCheck.IsEnabledProperty, (object) false);
        textBoxBase.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
        textBoxBase.ContextMenuOpening -= new ContextMenuEventHandler(TextBoxHelper.TextBoxBaseContextMenuOpening);
      }
    }

    private static void TextBoxBaseContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      TextBoxBase textBoxBase = (TextBoxBase) sender;
      TextBox textBox = textBoxBase as TextBox;
      RichTextBox richTextBox = textBoxBase as RichTextBox;
      textBoxBase.ContextMenu = TextBoxHelper.GetDefaultTextBoxBaseContextMenu();
      int insertIndex1 = 0;
      SpellingError spellingError = textBox != null ? textBox.GetSpellingError(textBox.CaretIndex) : richTextBox?.GetSpellingError(richTextBox.CaretPosition);
      if (spellingError == null)
        return;
      IEnumerable<string> suggestions = spellingError.Suggestions;
      if (suggestions.Any<string>())
      {
        foreach (string str in suggestions)
        {
          MenuItem insertItem = new MenuItem();
          insertItem.Header = (object) str;
          insertItem.FontWeight = FontWeights.Bold;
          insertItem.Command = (ICommand) EditingCommands.CorrectSpellingError;
          insertItem.CommandParameter = (object) str;
          insertItem.CommandTarget = (IInputElement) textBoxBase;
          insertItem.SetResourceReference(FrameworkElement.StyleProperty, (object) "MetroMenuItem");
          textBoxBase.ContextMenu.Items.Insert(insertIndex1, (object) insertItem);
          ++insertIndex1;
        }
        textBoxBase.ContextMenu.Items.Insert(insertIndex1, (object) new Separator());
        ++insertIndex1;
      }
      MenuItem insertItem1 = new MenuItem();
      insertItem1.Header = (object) "Ignore All";
      insertItem1.Command = (ICommand) EditingCommands.IgnoreSpellingError;
      insertItem1.CommandTarget = (IInputElement) textBoxBase;
      insertItem1.SetResourceReference(FrameworkElement.StyleProperty, (object) "MetroMenuItem");
      textBoxBase.ContextMenu.Items.Insert(insertIndex1, (object) insertItem1);
      int insertIndex2 = insertIndex1 + 1;
      Separator insertItem2 = new Separator();
      textBoxBase.ContextMenu.Items.Insert(insertIndex2, (object) insertItem2);
    }

    private static ContextMenu GetDefaultTextBoxBaseContextMenu()
    {
      ContextMenu boxBaseContextMenu = new ContextMenu();
      MenuItem newItem1 = new MenuItem()
      {
        Command = (ICommand) ApplicationCommands.Cut
      };
      newItem1.SetResourceReference(FrameworkElement.StyleProperty, (object) "MetroMenuItem");
      MenuItem newItem2 = new MenuItem()
      {
        Command = (ICommand) ApplicationCommands.Copy
      };
      newItem2.SetResourceReference(FrameworkElement.StyleProperty, (object) "MetroMenuItem");
      MenuItem newItem3 = new MenuItem()
      {
        Command = (ICommand) ApplicationCommands.Paste
      };
      newItem3.SetResourceReference(FrameworkElement.StyleProperty, (object) "MetroMenuItem");
      boxBaseContextMenu.Items.Add((object) newItem1);
      boxBaseContextMenu.Items.Add((object) newItem2);
      boxBaseContextMenu.Items.Add((object) newItem3);
      return boxBaseContextMenu;
    }

    public static void SetIsWaitingForData(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.IsWaitingForDataProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    public static bool GetIsWaitingForData(DependencyObject obj)
    {
      return (bool) obj.GetValue(TextBoxHelper.IsWaitingForDataProperty);
    }

    public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.SelectAllOnFocusProperty, (object) value);
    }

    public static bool GetSelectAllOnFocus(DependencyObject obj)
    {
      return (bool) obj.GetValue(TextBoxHelper.SelectAllOnFocusProperty);
    }

    public static void SetIsMonitoring(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.IsMonitoringProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    [AttachedPropertyBrowsableForType(typeof (DatePicker))]
    public static string GetWatermark(DependencyObject obj)
    {
      return (string) obj.GetValue(TextBoxHelper.WatermarkProperty);
    }

    public static void SetWatermark(DependencyObject obj, string value)
    {
      obj.SetValue(TextBoxHelper.WatermarkProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    public static bool GetUseFloatingWatermark(DependencyObject obj)
    {
      return (bool) obj.GetValue(TextBoxHelper.UseFloatingWatermarkProperty);
    }

    public static void SetUseFloatingWatermark(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.UseFloatingWatermarkProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    [AttachedPropertyBrowsableForType(typeof (DatePicker))]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    public static bool GetHasText(DependencyObject obj)
    {
      return (bool) obj.GetValue(TextBoxHelper.HasTextProperty);
    }

    public static void SetHasText(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.HasTextProperty, (object) value);
    }

    private static void OnIsMonitoringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      switch (d)
      {
        case TextBox _:
          TextBox txtBox = d as TextBox;
          if ((bool) e.NewValue)
          {
            txtBox.TextChanged += new TextChangedEventHandler(TextBoxHelper.TextChanged);
            txtBox.GotFocus += new RoutedEventHandler(TextBoxHelper.TextBoxGotFocus);
            txtBox.Dispatcher.BeginInvoke((Delegate) (() => TextBoxHelper.TextChanged((object) txtBox, (RoutedEventArgs) new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None))));
            break;
          }
          txtBox.TextChanged -= new TextChangedEventHandler(TextBoxHelper.TextChanged);
          txtBox.GotFocus -= new RoutedEventHandler(TextBoxHelper.TextBoxGotFocus);
          break;
        case PasswordBox _:
          PasswordBox passBox = d as PasswordBox;
          if ((bool) e.NewValue)
          {
            passBox.PasswordChanged += new RoutedEventHandler(TextBoxHelper.PasswordChanged);
            passBox.GotFocus += new RoutedEventHandler(TextBoxHelper.PasswordGotFocus);
            passBox.Dispatcher.BeginInvoke((Delegate) (() => TextBoxHelper.PasswordChanged((object) passBox, new RoutedEventArgs(PasswordBox.PasswordChangedEvent, (object) passBox))));
            break;
          }
          passBox.PasswordChanged -= new RoutedEventHandler(TextBoxHelper.PasswordChanged);
          passBox.GotFocus -= new RoutedEventHandler(TextBoxHelper.PasswordGotFocus);
          break;
        case NumericUpDown _:
          NumericUpDown numericUpDown = d as NumericUpDown;
          numericUpDown.SelectAllOnFocus = (bool) e.NewValue;
          if ((bool) e.NewValue)
          {
            numericUpDown.ValueChanged += new RoutedPropertyChangedEventHandler<double?>(TextBoxHelper.OnNumericUpDownValueChaged);
            numericUpDown.GotFocus += new RoutedEventHandler(TextBoxHelper.NumericUpDownGotFocus);
            break;
          }
          numericUpDown.ValueChanged -= new RoutedPropertyChangedEventHandler<double?>(TextBoxHelper.OnNumericUpDownValueChaged);
          numericUpDown.GotFocus -= new RoutedEventHandler(TextBoxHelper.NumericUpDownGotFocus);
          break;
      }
    }

    private static void SetTextLength<TDependencyObject>(
      TDependencyObject sender,
      Func<TDependencyObject, int> funcTextLength)
      where TDependencyObject : DependencyObject
    {
      if ((object) sender == null)
        return;
      int num = funcTextLength(sender);
      sender.SetValue(TextBoxHelper.TextLengthProperty, (object) num);
      sender.SetValue(TextBoxHelper.HasTextProperty, (object) (num >= 1));
    }

    private static void TextChanged(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.SetTextLength<TextBox>(sender as TextBox, (Func<TextBox, int>) (textBox => textBox.Text.Length));
    }

    private static void OnNumericUpDownValueChaged(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.SetTextLength<NumericUpDown>(sender as NumericUpDown, (Func<NumericUpDown, int>) (numericUpDown => !numericUpDown.Value.HasValue ? 0 : 1));
    }

    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.SetTextLength<PasswordBox>(sender as PasswordBox, (Func<PasswordBox, int>) (passwordBox => passwordBox.Password.Length));
    }

    private static void TextBoxGotFocus(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.ControlGotFocus<TextBox>(sender as TextBox, (Action<TextBox>) (textBox => textBox.SelectAll()));
    }

    private static void NumericUpDownGotFocus(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.ControlGotFocus<NumericUpDown>(sender as NumericUpDown, (Action<NumericUpDown>) (numericUpDown => numericUpDown.SelectAll()));
    }

    private static void PasswordGotFocus(object sender, RoutedEventArgs e)
    {
      TextBoxHelper.ControlGotFocus<PasswordBox>(sender as PasswordBox, (Action<PasswordBox>) (passwordBox => passwordBox.SelectAll()));
    }

    private static void ControlGotFocus<TDependencyObject>(
      TDependencyObject sender,
      Action<TDependencyObject> action)
      where TDependencyObject : DependencyObject
    {
      if ((object) sender == null || !TextBoxHelper.GetSelectAllOnFocus((DependencyObject) sender))
        return;
      sender.Dispatcher.BeginInvoke((Delegate) action, (object) sender);
    }

    [Category("MahApps.Metro")]
    public static bool GetClearTextButton(DependencyObject d)
    {
      return (bool) d.GetValue(TextBoxHelper.ClearTextButtonProperty);
    }

    public static void SetClearTextButton(DependencyObject obj, bool value)
    {
      obj.SetValue(TextBoxHelper.ClearTextButtonProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static ButtonsAlignment GetButtonsAlignment(DependencyObject d)
    {
      return (ButtonsAlignment) d.GetValue(TextBoxHelper.ButtonsAlignmentProperty);
    }

    public static void SetButtonsAlignment(DependencyObject obj, ButtonsAlignment value)
    {
      obj.SetValue(TextBoxHelper.ButtonsAlignmentProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Button))]
    public static bool GetIsClearTextButtonBehaviorEnabled(Button d)
    {
      return (bool) d.GetValue(TextBoxHelper.IsClearTextButtonBehaviorEnabledProperty);
    }

    [AttachedPropertyBrowsableForType(typeof (Button))]
    public static void SetIsClearTextButtonBehaviorEnabled(Button obj, bool value)
    {
      obj.SetValue(TextBoxHelper.IsClearTextButtonBehaviorEnabledProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static ICommand GetButtonCommand(DependencyObject d)
    {
      return (ICommand) d.GetValue(TextBoxHelper.ButtonCommandProperty);
    }

    public static void SetButtonCommand(DependencyObject obj, ICommand value)
    {
      obj.SetValue(TextBoxHelper.ButtonCommandProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static object GetButtonCommandParameter(DependencyObject d)
    {
      return d.GetValue(TextBoxHelper.ButtonCommandParameterProperty);
    }

    public static void SetButtonCommandParameter(DependencyObject obj, object value)
    {
      obj.SetValue(TextBoxHelper.ButtonCommandParameterProperty, value);
    }

    [Category("MahApps.Metro")]
    public static object GetButtonContent(DependencyObject d)
    {
      return d.GetValue(TextBoxHelper.ButtonContentProperty);
    }

    public static void SetButtonContent(DependencyObject obj, object value)
    {
      obj.SetValue(TextBoxHelper.ButtonContentProperty, value);
    }

    [Category("MahApps.Metro")]
    public static ControlTemplate GetButtonTemplate(DependencyObject d)
    {
      return (ControlTemplate) d.GetValue(TextBoxHelper.ButtonTemplateProperty);
    }

    public static void SetButtonTemplate(DependencyObject obj, ControlTemplate value)
    {
      obj.SetValue(TextBoxHelper.ButtonTemplateProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static FontFamily GetButtonFontFamily(DependencyObject d)
    {
      return (FontFamily) d.GetValue(TextBoxHelper.ButtonFontFamilyProperty);
    }

    public static void SetButtonFontFamily(DependencyObject obj, FontFamily value)
    {
      obj.SetValue(TextBoxHelper.ButtonFontFamilyProperty, (object) value);
    }

    private static void IsClearTextButtonBehaviorEnabledChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Button button = d as Button;
      if (e.OldValue == e.NewValue || button == null)
        return;
      button.Click -= new RoutedEventHandler(TextBoxHelper.ButtonClicked);
      if (!(bool) e.NewValue)
        return;
      button.Click += new RoutedEventHandler(TextBoxHelper.ButtonClicked);
    }

    public static void ButtonClicked(object sender, RoutedEventArgs e)
    {
      DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) sender);
      while (true)
      {
        switch (parent)
        {
          case TextBox _:
          case PasswordBox _:
          case ComboBox _:
            goto label_3;
          default:
            parent = VisualTreeHelper.GetParent(parent);
            continue;
        }
      }
label_3:
      ICommand buttonCommand = TextBoxHelper.GetButtonCommand(parent);
      if (buttonCommand != null && buttonCommand.CanExecute((object) parent))
      {
        object commandParameter = TextBoxHelper.GetButtonCommandParameter(parent);
        buttonCommand.Execute(commandParameter ?? (object) parent);
      }
      if (!TextBoxHelper.GetClearTextButton(parent))
        return;
      switch (parent)
      {
        case TextBox _:
          ((TextBox) parent).Clear();
          break;
        case PasswordBox _:
          ((PasswordBox) parent).Clear();
          break;
        case ComboBox _:
          if (((ComboBox) parent).IsEditable)
            ((ComboBox) parent).Text = string.Empty;
          ((Selector) parent).SelectedItem = (object) null;
          break;
      }
    }

    private static void ButtonCommandOrClearTextChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (d is TextBox sender1)
      {
        sender1.Loaded -= new RoutedEventHandler(TextBoxHelper.TextChanged);
        sender1.Loaded += new RoutedEventHandler(TextBoxHelper.TextChanged);
        if (sender1.IsLoaded)
          TextBoxHelper.TextChanged((object) sender1, new RoutedEventArgs());
      }
      if (d is PasswordBox sender2)
      {
        sender2.Loaded -= new RoutedEventHandler(TextBoxHelper.PasswordChanged);
        sender2.Loaded += new RoutedEventHandler(TextBoxHelper.PasswordChanged);
        if (sender2.IsLoaded)
          TextBoxHelper.PasswordChanged((object) sender2, new RoutedEventArgs());
      }
      if (!(d is ComboBox sender3))
        return;
      sender3.Loaded -= new RoutedEventHandler(TextBoxHelper.ComboBoxLoaded);
      sender3.Loaded += new RoutedEventHandler(TextBoxHelper.ComboBoxLoaded);
      if (!sender3.IsLoaded)
        return;
      TextBoxHelper.ComboBoxLoaded((object) sender3, new RoutedEventArgs());
    }

    private static void ComboBoxLoaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is ComboBox comboBox))
        return;
      comboBox.SetValue(TextBoxHelper.HasTextProperty, (object) (bool) (!string.IsNullOrWhiteSpace(comboBox.Text) ? 1 : (comboBox.SelectedItem != null ? 1 : 0)));
    }
  }
}
