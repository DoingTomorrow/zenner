// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.CustomFilterBehavior
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.Utils
{
  public class CustomFilterBehavior : MVVM.ViewModel.ViewModelBase
  {
    private readonly RadGridView gridView;
    private readonly TextBox tb;
    private CustomFilterDescriptor _customFilterDescriptor;
    public static readonly DependencyProperty TextBoxProperty = DependencyProperty.RegisterAttached("TextBox", typeof (TextBox), typeof (CustomFilterBehavior), new PropertyMetadata(new PropertyChangedCallback(CustomFilterBehavior.OnTextBoxPropertyChanged)));

    public CustomFilterDescriptor CustomFilterDescriptor
    {
      get
      {
        if (this._customFilterDescriptor != null)
          return this._customFilterDescriptor;
        this._customFilterDescriptor = new CustomFilterDescriptor(this.gridView.Columns.OfType<Telerik.Windows.Controls.GridViewColumn>());
        this.gridView.FilterDescriptors.Add((IFilterDescriptor) this._customFilterDescriptor);
        return this._customFilterDescriptor;
      }
    }

    public static void SetTextBox(DependencyObject dependencyObject, TextBox tb)
    {
      dependencyObject.SetValue(CustomFilterBehavior.TextBoxProperty, (object) tb);
    }

    public static TextBox GetTextBox(DependencyObject dependencyObject)
    {
      return (TextBox) dependencyObject.GetValue(CustomFilterBehavior.TextBoxProperty);
    }

    public static void OnTextBoxPropertyChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      RadGridView gridView = dependencyObject as RadGridView;
      TextBox newValue = e.NewValue as TextBox;
      if (gridView == null || newValue == null)
        return;
      CustomFilterBehavior customFilterBehavior = new CustomFilterBehavior(gridView, newValue);
    }

    public CustomFilterBehavior(RadGridView gridView, TextBox tb)
    {
      this.gridView = gridView;
      this.tb = tb;
      this.tb.TextChanged -= new TextChangedEventHandler(this.FilterValue_TextChanged);
      this.tb.TextChanged += new TextChangedEventHandler(this.FilterValue_TextChanged);
    }

    private void FilterValue_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.CustomFilterDescriptor.FilterValue = this.tb.Text;
      this.tb.Focus();
    }
  }
}
