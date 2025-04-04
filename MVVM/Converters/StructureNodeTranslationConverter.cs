// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.StructureNodeTranslationConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MSS.Localisation;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  [ValueConversion(typeof (bool), typeof (Visibility))]
  public class StructureNodeTranslationConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) CultureResources.GetValue("MSS_StructureNode_" + (string) value);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) null;
    }
  }
}
