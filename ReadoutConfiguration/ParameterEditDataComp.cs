// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ParameterEditDataComp
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using System.Collections.Generic;

#nullable disable
namespace ReadoutConfiguration
{
  public class ParameterEditDataComp : IComparer<ParameterEditData>
  {
    public int Compare(ParameterEditData x, ParameterEditData y)
    {
      int num1 = int.Parse(x.SelectedGroup).CompareTo(int.Parse(y.SelectedGroup));
      if (num1 != 0)
        return num1;
      if (x.SelectedType == null || y.SelectedType == null)
        return 0;
      int num2 = x.SelectedType.CompareTo(y.SelectedType);
      if (num2 != 0)
        return num2;
      return x.ParameterValue == null || y.ParameterValue == null ? 0 : x.ParameterValue.CompareTo(y.ParameterValue);
    }
  }
}
