// Decompiled with JetBrains decompiler
// Type: MVVM.Converters.NodeToIsMinomatMasterConverter
// Assembly: MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5D33A3C4-E333-437E-9AB2-FFDB9D6F32F8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MVVM.dll

using MSS.Business.DTO;
using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace MVVM.Converters
{
  public class NodeToIsMinomatMasterConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is StructureNodeDTO && ((StructureNodeDTO) value).NodeType.Name == "MinomatMaster" ? (object) true : (object) false;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new InvalidOperationException("IsNodeMinomatMasterConverter can only be used OneWay.");
    }
  }
}
