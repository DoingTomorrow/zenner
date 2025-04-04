// Decompiled with JetBrains decompiler
// Type: Excel.IExcelDataReader
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Data;
using System.IO;

#nullable disable
namespace Excel
{
  public interface IExcelDataReader : IDataReader, IDisposable, IDataRecord
  {
    void Initialize(Stream fileStream);

    DataSet AsDataSet();

    DataSet AsDataSet(bool convertOADateTime);

    bool IsValid { get; }

    string ExceptionMessage { get; }

    string Name { get; }

    int ResultsCount { get; }

    bool IsFirstRowAsColumnNames { get; set; }
  }
}
