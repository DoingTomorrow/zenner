// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.GSMStatusToStringConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MSS.Localisation;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  public class GSMStatusToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value.ToString().Equals("NotStarted") || value.ToString().Equals("NotStartet"))
        return (object) Resources.GSMTestReceptionState_NotStarted;
      if (value.ToString().Equals("NotComplete"))
        return (object) Resources.GSMTestReceptionState_NotComplete;
      if (value.ToString().Equals("Successful"))
        return (object) Resources.GSMTestReceptionState_Successful;
      return value.ToString().Equals("Failed") ? (object) Resources.GSMTestReceptionState_Failed : (object) "";
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
