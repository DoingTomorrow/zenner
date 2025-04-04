// Decompiled with JetBrains decompiler
// Type: Excel.ExcelReaderFactory
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System.IO;

#nullable disable
namespace Excel
{
  public static class ExcelReaderFactory
  {
    public static IExcelDataReader CreateBinaryReader(Stream fileStream)
    {
      IExcelDataReader binaryReader = (IExcelDataReader) new ExcelBinaryReader();
      binaryReader.Initialize(fileStream);
      return binaryReader;
    }

    public static IExcelDataReader CreateBinaryReader(Stream fileStream, ReadOption option)
    {
      IExcelDataReader binaryReader = (IExcelDataReader) new ExcelBinaryReader(option);
      binaryReader.Initialize(fileStream);
      return binaryReader;
    }

    public static IExcelDataReader CreateBinaryReader(Stream fileStream, bool convertOADate)
    {
      IExcelDataReader binaryReader = ExcelReaderFactory.CreateBinaryReader(fileStream);
      ((ExcelBinaryReader) binaryReader).ConvertOaDate = convertOADate;
      return binaryReader;
    }

    public static IExcelDataReader CreateBinaryReader(
      Stream fileStream,
      bool convertOADate,
      ReadOption readOption)
    {
      IExcelDataReader binaryReader = ExcelReaderFactory.CreateBinaryReader(fileStream, readOption);
      ((ExcelBinaryReader) binaryReader).ConvertOaDate = convertOADate;
      return binaryReader;
    }

    public static IExcelDataReader CreateOpenXmlReader(Stream fileStream)
    {
      IExcelDataReader openXmlReader = (IExcelDataReader) new ExcelOpenXmlReader();
      openXmlReader.Initialize(fileStream);
      return openXmlReader;
    }
  }
}
