// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.CustomMultiBindingConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  public class CustomMultiBindingConverter : IMultiValueConverter
  {
    public object Convert(
      object[] Values,
      Type Target_Type,
      object Parameter,
      CultureInfo culture)
    {
      return (object) new CustomMultiBindingConverter.FindCommandParameters()
      {
        Property0 = (((IEnumerable<object>) Values).Any<object>() ? Values[0] : (object) null),
        Property1 = (((IEnumerable<object>) Values).Count<object>() > 1 ? Values[1] : (object) null),
        Property2 = (((IEnumerable<object>) Values).Count<object>() > 2 ? Values[2] : (object) null),
        Property3 = (((IEnumerable<object>) Values).Count<object>() > 3 ? Values[3] : (object) null)
      };
    }

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public class FindCommandParameters
    {
      public object Property0 { get; set; }

      public object Property1 { get; set; }

      public object Property2 { get; set; }

      public object Property3 { get; set; }
    }
  }
}
