// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.ArgumentParsers
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class ArgumentParsers
  {
    private static object _syncRoot = new object();
    private readonly Dictionary<DataType, ArgumentParser> _parsers = new Dictionary<DataType, ArgumentParser>();
    private readonly ArgumentParserFactory _parserFactory;

    public ArgumentParsers()
      : this(new ArgumentParserFactory())
    {
    }

    public ArgumentParsers(ArgumentParserFactory factory)
    {
      Require.That<ArgumentParserFactory>(factory).Named("argumentParserfactory").IsNotNull<ArgumentParserFactory>();
      this._parserFactory = factory;
    }

    public ArgumentParser GetParser(DataType dataType)
    {
      if (!this._parsers.ContainsKey(dataType))
      {
        lock (ArgumentParsers._syncRoot)
        {
          if (!this._parsers.ContainsKey(dataType))
            this._parsers.Add(dataType, this._parserFactory.CreateArgumentParser(dataType));
        }
      }
      return this._parsers[dataType];
    }
  }
}
