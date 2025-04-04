// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.CollectionFlattener`1
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public abstract class CollectionFlattener<T>
  {
    public virtual IEnumerable<T> FuncArgsToFlatEnumerable(
      IEnumerable<FunctionArgument> arguments,
      Action<FunctionArgument, IList<T>> convertFunc)
    {
      List<T> argList = new List<T>();
      this.FuncArgsToFlatEnumerable(arguments, argList, convertFunc);
      return (IEnumerable<T>) argList;
    }

    private void FuncArgsToFlatEnumerable(
      IEnumerable<FunctionArgument> arguments,
      List<T> argList,
      Action<FunctionArgument, IList<T>> convertFunc)
    {
      foreach (FunctionArgument functionArgument in arguments)
      {
        if (functionArgument.Value is IEnumerable<FunctionArgument>)
          this.FuncArgsToFlatEnumerable((IEnumerable<FunctionArgument>) functionArgument.Value, argList, convertFunc);
        else
          convertFunc(functionArgument, (IList<T>) argList);
      }
    }
  }
}
