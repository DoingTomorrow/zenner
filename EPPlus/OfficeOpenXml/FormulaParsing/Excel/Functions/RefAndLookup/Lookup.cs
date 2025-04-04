// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Lookup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Lookup : LookupFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      return this.HaveTwoRanges(arguments) ? this.HandleTwoRanges(arguments, context) : this.HandleSingleRange(arguments, context);
    }

    private bool HaveTwoRanges(IEnumerable<FunctionArgument> arguments)
    {
      return arguments.Count<FunctionArgument>() != 2 && ExcelAddressUtil.IsValidAddress(arguments.ElementAt<FunctionArgument>(2).Value.ToString());
    }

    private CompileResult HandleSingleRange(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      object searchedValue = arguments.ElementAt<FunctionArgument>(0).Value;
      Require.That<object>(arguments.ElementAt<FunctionArgument>(1).Value).Named("firstAddress").IsNotNull<object>();
      string str = this.ArgToString(arguments, 1);
      RangeAddress rangeAddress = new RangeAddressFactory(context.ExcelDataProvider).Create(str);
      int num1 = rangeAddress.ToRow - rangeAddress.FromRow;
      int num2 = rangeAddress.ToCol - rangeAddress.FromCol;
      int lookupIndex = num2 + 1;
      LookupDirection direction = LookupDirection.Vertical;
      if (num2 > num1)
      {
        lookupIndex = num1 + 1;
        direction = LookupDirection.Horizontal;
      }
      LookupArguments lookupArguments = new LookupArguments(searchedValue, str, lookupIndex, 0, true);
      return this.Lookup(LookupNavigatorFactory.Create(direction, lookupArguments, context), lookupArguments);
    }

    private CompileResult HandleTwoRanges(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      object searchedValue = arguments.ElementAt<FunctionArgument>(0).Value;
      Require.That<object>(arguments.ElementAt<FunctionArgument>(1).Value).Named("firstAddress").IsNotNull<object>();
      Require.That<object>(arguments.ElementAt<FunctionArgument>(2).Value).Named("secondAddress").IsNotNull<object>();
      string str = this.ArgToString(arguments, 1);
      string range = this.ArgToString(arguments, 2);
      RangeAddressFactory rangeAddressFactory = new RangeAddressFactory(context.ExcelDataProvider);
      RangeAddress rangeAddress1 = rangeAddressFactory.Create(str);
      RangeAddress rangeAddress2 = rangeAddressFactory.Create(range);
      int lookupIndex = rangeAddress2.FromCol - rangeAddress1.FromCol + 1;
      int lookupOffset = rangeAddress2.FromRow - rangeAddress1.FromRow;
      LookupDirection lookupDirection = this.GetLookupDirection(rangeAddress1);
      if (lookupDirection == LookupDirection.Horizontal)
      {
        lookupIndex = rangeAddress2.FromRow - rangeAddress1.FromRow + 1;
        lookupOffset = rangeAddress2.FromCol - rangeAddress1.FromCol;
      }
      LookupArguments lookupArguments = new LookupArguments(searchedValue, str, lookupIndex, lookupOffset, true);
      return this.Lookup(LookupNavigatorFactory.Create(lookupDirection, lookupArguments, context), lookupArguments);
    }
  }
}
