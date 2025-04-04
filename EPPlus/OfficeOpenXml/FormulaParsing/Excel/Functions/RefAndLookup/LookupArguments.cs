// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.LookupArguments
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class LookupArguments
  {
    private readonly ArgumentParsers _argumentParsers;

    public LookupArguments(IEnumerable<FunctionArgument> arguments)
      : this(arguments, new ArgumentParsers())
    {
    }

    public LookupArguments(IEnumerable<FunctionArgument> arguments, ArgumentParsers argumentParsers)
    {
      this._argumentParsers = argumentParsers;
      this.SearchedValue = arguments.ElementAt<FunctionArgument>(0).Value;
      object obj = arguments.ElementAt<FunctionArgument>(1).Value;
      switch (obj)
      {
        case IEnumerable<FunctionArgument> functionArguments:
          this.DataArray = functionArguments;
          this.ArgumentDataType = LookupArguments.LookupArgumentDataType.DataArray;
          break;
        case ExcelDataProvider.IRangeInfo rangeInfo:
          this.RangeAddress = string.IsNullOrEmpty(rangeInfo.Address.WorkSheet) ? rangeInfo.Address.Address : "'" + rangeInfo.Address.WorkSheet + "'!" + rangeInfo.Address.Address;
          this.RangeInfo = rangeInfo;
          this.ArgumentDataType = LookupArguments.LookupArgumentDataType.ExcelRange;
          break;
        default:
          this.RangeAddress = obj.ToString();
          this.ArgumentDataType = LookupArguments.LookupArgumentDataType.ExcelRange;
          break;
      }
      this.LookupIndex = (int) this._argumentParsers.GetParser(DataType.Integer).Parse(arguments.ElementAt<FunctionArgument>(2).Value);
      if (arguments.Count<FunctionArgument>() > 3)
        this.RangeLookup = (bool) this._argumentParsers.GetParser(DataType.Boolean).Parse(arguments.ElementAt<FunctionArgument>(3).Value);
      else
        this.RangeLookup = true;
    }

    public LookupArguments(
      object searchedValue,
      string rangeAddress,
      int lookupIndex,
      int lookupOffset,
      bool rangeLookup)
    {
      this.SearchedValue = searchedValue;
      this.RangeAddress = rangeAddress;
      this.LookupIndex = lookupIndex;
      this.LookupOffset = lookupOffset;
      this.RangeLookup = rangeLookup;
    }

    public object SearchedValue { get; private set; }

    public string RangeAddress { get; private set; }

    public int LookupIndex { get; private set; }

    public int LookupOffset { get; private set; }

    public bool RangeLookup { get; private set; }

    public IEnumerable<FunctionArgument> DataArray { get; private set; }

    public ExcelDataProvider.IRangeInfo RangeInfo { get; private set; }

    public LookupArguments.LookupArgumentDataType ArgumentDataType { get; private set; }

    public enum LookupArgumentDataType
    {
      ExcelRange,
      DataArray,
    }
  }
}
