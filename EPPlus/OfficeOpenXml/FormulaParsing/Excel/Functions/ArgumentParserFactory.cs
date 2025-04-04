// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.ArgumentParserFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class ArgumentParserFactory
  {
    public virtual ArgumentParser CreateArgumentParser(DataType dataType)
    {
      switch (dataType)
      {
        case DataType.Integer:
          return (ArgumentParser) new IntArgumentParser();
        case DataType.Decimal:
          return (ArgumentParser) new DoubleArgumentParser();
        case DataType.Boolean:
          return (ArgumentParser) new BoolArgumentParser();
        default:
          throw new InvalidOperationException("non supported argument parser type " + dataType.ToString());
      }
    }
  }
}
