// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.LookupNavigatorFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public static class LookupNavigatorFactory
  {
    public static LookupNavigator Create(
      LookupDirection direction,
      LookupArguments args,
      ParsingContext parsingContext)
    {
      if (args.ArgumentDataType == LookupArguments.LookupArgumentDataType.ExcelRange)
        return (LookupNavigator) new ExcelLookupNavigator(direction, args, parsingContext);
      if (args.ArgumentDataType == LookupArguments.LookupArgumentDataType.DataArray)
        return (LookupNavigator) new ArrayLookupNavigator(direction, args, parsingContext);
      throw new NotSupportedException("Invalid argument datatype");
    }
  }
}
