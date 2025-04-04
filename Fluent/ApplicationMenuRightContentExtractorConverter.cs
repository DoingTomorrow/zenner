// Decompiled with JetBrains decompiler
// Type: Fluent.ApplicationMenuRightContentExtractorConverter
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Fluent
{
  public class ApplicationMenuRightContentExtractorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is ApplicationMenu templatedParent ? (object) (templatedParent.Template.FindName("PART_RightContentPresenter", (FrameworkElement) templatedParent) as ContentPresenter) : value;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value;
    }
  }
}
