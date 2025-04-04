// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.DataGridNumericUpDownColumn
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class DataGridNumericUpDownColumn : DataGridBoundColumn
  {
    private static Style _defaultEditingElementStyle;
    private static Style _defaultElementStyle;
    private double minimum = (double) NumericUpDown.MinimumProperty.DefaultMetadata.DefaultValue;
    private double maximum = (double) NumericUpDown.MaximumProperty.DefaultMetadata.DefaultValue;
    private double interval = (double) NumericUpDown.IntervalProperty.DefaultMetadata.DefaultValue;
    private string stringFormat = (string) NumericUpDown.StringFormatProperty.DefaultMetadata.DefaultValue;
    private bool hideUpDownButtons = (bool) NumericUpDown.HideUpDownButtonsProperty.DefaultMetadata.DefaultValue;
    private double upDownButtonsWidth = (double) NumericUpDown.UpDownButtonsWidthProperty.DefaultMetadata.DefaultValue;
    public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
    public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
    public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
    public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));
    public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridNumericUpDownColumn.NotifyPropertyChangeForRefreshContent)));

    static DataGridNumericUpDownColumn()
    {
      DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) DataGridNumericUpDownColumn.DefaultElementStyle));
      DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof (DataGridNumericUpDownColumn), (PropertyMetadata) new FrameworkPropertyMetadata((object) DataGridNumericUpDownColumn.DefaultEditingElementStyle));
    }

    public static Style DefaultEditingElementStyle
    {
      get
      {
        if (DataGridNumericUpDownColumn._defaultEditingElementStyle == null)
        {
          Style style = new Style(typeof (NumericUpDown));
          style.Setters.Add((SetterBase) new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0)));
          style.Setters.Add((SetterBase) new Setter(Control.PaddingProperty, (object) new Thickness(0.0)));
          style.Setters.Add((SetterBase) new Setter(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Top));
          style.Setters.Add((SetterBase) new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, (object) ScrollBarVisibility.Disabled));
          style.Setters.Add((SetterBase) new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, (object) ScrollBarVisibility.Disabled));
          style.Setters.Add((SetterBase) new Setter(Control.VerticalContentAlignmentProperty, (object) VerticalAlignment.Center));
          style.Setters.Add((SetterBase) new Setter(FrameworkElement.MinHeightProperty, (object) 0.0));
          style.Seal();
          DataGridNumericUpDownColumn._defaultEditingElementStyle = style;
        }
        return DataGridNumericUpDownColumn._defaultEditingElementStyle;
      }
    }

    public static Style DefaultElementStyle
    {
      get
      {
        if (DataGridNumericUpDownColumn._defaultElementStyle == null)
        {
          Style style = new Style(typeof (NumericUpDown));
          style.Setters.Add((SetterBase) new Setter(Control.BorderThicknessProperty, (object) new Thickness(0.0)));
          style.Setters.Add((SetterBase) new Setter(FrameworkElement.VerticalAlignmentProperty, (object) VerticalAlignment.Top));
          style.Setters.Add((SetterBase) new Setter(UIElement.IsHitTestVisibleProperty, (object) false));
          style.Setters.Add((SetterBase) new Setter(UIElement.FocusableProperty, (object) false));
          style.Setters.Add((SetterBase) new Setter(NumericUpDown.HideUpDownButtonsProperty, (object) true));
          style.Setters.Add((SetterBase) new Setter(Control.BackgroundProperty, (object) Brushes.Transparent));
          style.Setters.Add((SetterBase) new Setter(ScrollViewer.HorizontalScrollBarVisibilityProperty, (object) ScrollBarVisibility.Disabled));
          style.Setters.Add((SetterBase) new Setter(ScrollViewer.VerticalScrollBarVisibilityProperty, (object) ScrollBarVisibility.Disabled));
          style.Setters.Add((SetterBase) new Setter(Control.VerticalContentAlignmentProperty, (object) VerticalAlignment.Center));
          style.Setters.Add((SetterBase) new Setter(FrameworkElement.MinHeightProperty, (object) 0.0));
          style.Setters.Add((SetterBase) new Setter(ControlsHelper.DisabledVisualElementVisibilityProperty, (object) Visibility.Collapsed));
          style.Seal();
          DataGridNumericUpDownColumn._defaultElementStyle = style;
        }
        return DataGridNumericUpDownColumn._defaultElementStyle;
      }
    }

    private static void ApplyBinding(
      BindingBase binding,
      DependencyObject target,
      DependencyProperty property)
    {
      if (binding != null)
        BindingOperations.SetBinding(target, property, binding);
      else
        BindingOperations.ClearBinding(target, property);
    }

    private new void ApplyStyle(
      bool isEditing,
      bool defaultToElementStyle,
      FrameworkElement element)
    {
      Style style = this.PickStyle(isEditing, defaultToElementStyle);
      if (style == null)
        return;
      element.Style = style;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
      return (FrameworkElement) this.GenerateNumericUpDown(true, cell);
    }

    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
      NumericUpDown numericUpDown = this.GenerateNumericUpDown(false, cell);
      numericUpDown.HideUpDownButtons = true;
      return (FrameworkElement) numericUpDown;
    }

    private NumericUpDown GenerateNumericUpDown(bool isEditing, DataGridCell cell)
    {
      NumericUpDown numericUpDown = (cell != null ? cell.Content as NumericUpDown : (NumericUpDown) null) ?? new NumericUpDown();
      this.SyncProperties((FrameworkElement) numericUpDown);
      this.ApplyStyle(isEditing, true, (FrameworkElement) numericUpDown);
      DataGridNumericUpDownColumn.ApplyBinding(this.Binding, (DependencyObject) numericUpDown, NumericUpDown.ValueProperty);
      numericUpDown.Minimum = this.Minimum;
      numericUpDown.Maximum = this.Maximum;
      numericUpDown.StringFormat = this.StringFormat;
      numericUpDown.Interval = this.Interval;
      numericUpDown.InterceptArrowKeys = true;
      numericUpDown.InterceptMouseWheel = true;
      numericUpDown.Speedup = true;
      numericUpDown.HideUpDownButtons = this.HideUpDownButtons;
      numericUpDown.UpDownButtonsWidth = this.UpDownButtonsWidth;
      return numericUpDown;
    }

    private void SyncProperties(FrameworkElement e)
    {
      DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) e, TextElement.FontFamilyProperty, DataGridNumericUpDownColumn.FontFamilyProperty);
      DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) e, TextElement.FontSizeProperty, DataGridNumericUpDownColumn.FontSizeProperty);
      DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) e, TextElement.FontStyleProperty, DataGridNumericUpDownColumn.FontStyleProperty);
      DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) e, TextElement.FontWeightProperty, DataGridNumericUpDownColumn.FontWeightProperty);
      DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) e, TextElement.ForegroundProperty, DataGridNumericUpDownColumn.ForegroundProperty);
    }

    protected override void RefreshCellContent(FrameworkElement element, string propertyName)
    {
      if (element is DataGridCell dataGridCell && dataGridCell.Content is FrameworkElement content)
      {
        switch (propertyName)
        {
          case "FontFamily":
            DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) content, TextElement.FontFamilyProperty, DataGridNumericUpDownColumn.FontFamilyProperty);
            break;
          case "FontSize":
            DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) content, TextElement.FontSizeProperty, DataGridNumericUpDownColumn.FontSizeProperty);
            break;
          case "FontStyle":
            DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) content, TextElement.FontStyleProperty, DataGridNumericUpDownColumn.FontStyleProperty);
            break;
          case "FontWeight":
            DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) content, TextElement.FontWeightProperty, DataGridNumericUpDownColumn.FontWeightProperty);
            break;
          case "Foreground":
            DataGridNumericUpDownColumn.SyncColumnProperty((DependencyObject) this, (DependencyObject) content, TextElement.ForegroundProperty, DataGridNumericUpDownColumn.ForegroundProperty);
            break;
        }
      }
      base.RefreshCellContent(element, propertyName);
    }

    protected override object PrepareCellForEdit(
      FrameworkElement editingElement,
      RoutedEventArgs editingEventArgs)
    {
      if (!(editingElement is NumericUpDown numericUpDown))
        return (object) null;
      numericUpDown.Focus();
      numericUpDown.SelectAll();
      return (object) numericUpDown.Value;
    }

    private static void SyncColumnProperty(
      DependencyObject column,
      DependencyObject content,
      DependencyProperty contentProperty,
      DependencyProperty columnProperty)
    {
      if (DataGridNumericUpDownColumn.IsDefaultValue(column, columnProperty))
        content.ClearValue(contentProperty);
      else
        content.SetValue(contentProperty, column.GetValue(columnProperty));
    }

    private static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
    {
      return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
    }

    private Style PickStyle(bool isEditing, bool defaultToElementStyle)
    {
      Style style = isEditing ? this.EditingElementStyle : this.ElementStyle;
      if (isEditing & defaultToElementStyle && style == null)
        style = this.ElementStyle;
      return style;
    }

    public double Minimum
    {
      get => this.minimum;
      set => this.minimum = value;
    }

    public double Maximum
    {
      get => this.maximum;
      set => this.maximum = value;
    }

    public double Interval
    {
      get => this.interval;
      set => this.interval = value;
    }

    public string StringFormat
    {
      get => this.stringFormat;
      set => this.stringFormat = value;
    }

    public bool HideUpDownButtons
    {
      get => this.hideUpDownButtons;
      set => this.hideUpDownButtons = value;
    }

    public double UpDownButtonsWidth
    {
      get => this.upDownButtonsWidth;
      set => this.upDownButtonsWidth = value;
    }

    public FontFamily FontFamily
    {
      get => (FontFamily) this.GetValue(DataGridNumericUpDownColumn.FontFamilyProperty);
      set => this.SetValue(DataGridNumericUpDownColumn.FontFamilyProperty, (object) value);
    }

    [TypeConverter(typeof (FontSizeConverter))]
    [Localizability(LocalizationCategory.None)]
    public double FontSize
    {
      get => (double) this.GetValue(DataGridNumericUpDownColumn.FontSizeProperty);
      set => this.SetValue(DataGridNumericUpDownColumn.FontSizeProperty, (object) value);
    }

    public FontStyle FontStyle
    {
      get => (FontStyle) this.GetValue(DataGridNumericUpDownColumn.FontStyleProperty);
      set => this.SetValue(DataGridNumericUpDownColumn.FontStyleProperty, (object) value);
    }

    public FontWeight FontWeight
    {
      get => (FontWeight) this.GetValue(DataGridNumericUpDownColumn.FontWeightProperty);
      set => this.SetValue(DataGridNumericUpDownColumn.FontWeightProperty, (object) value);
    }

    public Brush Foreground
    {
      get => (Brush) this.GetValue(DataGridNumericUpDownColumn.ForegroundProperty);
      set => this.SetValue(DataGridNumericUpDownColumn.ForegroundProperty, (object) value);
    }

    private new static void NotifyPropertyChangeForRefreshContent(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((DataGridColumn) d).NotifyPropertyChanged(e.Property.Name);
    }
  }
}
