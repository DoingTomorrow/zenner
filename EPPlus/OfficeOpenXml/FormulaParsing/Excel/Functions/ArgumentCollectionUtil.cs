// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.ArgumentCollectionUtil
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class ArgumentCollectionUtil
  {
    private readonly DoubleEnumerableArgConverter _doubleEnumerableArgConverter;
    private readonly ObjectEnumerableArgConverter _objectEnumerableArgConverter;

    public ArgumentCollectionUtil()
      : this(new DoubleEnumerableArgConverter(), new ObjectEnumerableArgConverter())
    {
    }

    public ArgumentCollectionUtil(
      DoubleEnumerableArgConverter doubleEnumerableArgConverter,
      ObjectEnumerableArgConverter objectEnumerableArgConverter)
    {
      this._doubleEnumerableArgConverter = doubleEnumerableArgConverter;
      this._objectEnumerableArgConverter = objectEnumerableArgConverter;
    }

    public virtual IEnumerable<double> ArgsToDoubleEnumerable(
      bool ignoreHidden,
      bool ignoreErrors,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this._doubleEnumerableArgConverter.ConvertArgs(ignoreHidden, ignoreErrors, arguments, context);
    }

    public virtual IEnumerable<object> ArgsToObjectEnumerable(
      bool ignoreHidden,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this._objectEnumerableArgConverter.ConvertArgs(ignoreHidden, arguments, context);
    }

    public virtual double CalculateCollection(
      IEnumerable<FunctionArgument> collection,
      double result,
      Func<FunctionArgument, double, double> action)
    {
      foreach (FunctionArgument functionArgument in collection)
        result = !(functionArgument.Value is IEnumerable<FunctionArgument>) ? action(functionArgument, result) : this.CalculateCollection((IEnumerable<FunctionArgument>) functionArgument.Value, result, action);
      return result;
    }
  }
}
